using Diol.applications.PlaygroundApi;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

// add named http client and httplogger
builder.Services.AddHttpClient("bingClient", httpClient =>
{
    httpClient.DefaultRequestHeaders.Add("my-named-header", "value-1");
});

builder.Services.AddHttpClient("multipleClient", httpClient =>
{
    httpClient.DefaultRequestHeaders.Add("my-named-header-1", "value-1");
    httpClient.DefaultRequestHeaders.Add("my-named-header-2", "value-2");
    httpClient.DefaultRequestHeaders.Add("my-named-header-3", "value-3");
});

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.MapGet("/api/default-client/bing", async (HttpClient httpClient) =>
{
    httpClient.DefaultRequestHeaders.Add("my-default-header", "my-value");

    var response = await httpClient
        .GetAsync("https://www.bing.com")
        .ConfigureAwait(false);

    var content = await response.Content
        .ReadAsStringAsync()
        .ConfigureAwait(false);

    return response.StatusCode;
});


app.MapGet("/api/default-client/bing-and-google", async (HttpClient httpClient) =>
{
    httpClient.DefaultRequestHeaders.Add("my-default-header-1", "value-1");
    httpClient.DefaultRequestHeaders.Add("my-default-header-2", "value-2");
    httpClient.DefaultRequestHeaders.Add("my-default-header-3", "value-3");

    var urls = new string[] { "https://www.bing.com", "https://www.google.com" };

    var tasks = urls.Select(url => Task.Run(async () =>
    {
        var response = await httpClient
        .GetAsync(url)
        .ConfigureAwait(false);

        var content = await response.Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        return response.StatusCode;
    }));

    var results = await Task.WhenAll(tasks)
        .ConfigureAwait(false);

    return results;
});

app.MapGet("/api/named-client/bing", async (IHttpClientFactory factory) =>
{
    var httpClient = factory.CreateClient("bingClient");

    var response = await httpClient
        .GetAsync("https://www.bing.com")
        .ConfigureAwait(false);

    var content = await response.Content
        .ReadAsStringAsync()
        .ConfigureAwait(false);

    return response.StatusCode;
});


app.MapGet("/api/named-client/bing-and-google", async (IHttpClientFactory factory) =>
{
    var httpClient = factory.CreateClient("multipleClient");

    var urls = new string[] { "https://www.bing.com", "https://www.google.com" };

    var tasks = urls.Select(url => Task.Run(async () =>
    {
        var response = await httpClient
        .GetAsync(url)
        .ConfigureAwait(false);

        var content = await response.Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        return response.StatusCode;
    }));

    var results = await Task.WhenAll(tasks)
        .ConfigureAwait(false);

    return results;
});

app.MapGet("/api/process-id", () =>
{
    return Process.GetCurrentProcess().Id;
});

app.MapGet("/api/with-headers", ([FromHeader] int myHeader) =>
{
    return myHeader;
});

app.MapGet("/api/with-route-param/{routeAttribute}", ([FromRoute] int routeAttribute) =>
{
    return routeAttribute;
});

app.MapGet("/api/with-query-param", ([FromQuery] int queryParam) =>
{
    return queryParam;
});

app.MapPost("api/with-body-param", ([FromBody] DummyModel bodyAttribute) =>
{
    return bodyAttribute;
});

app.MapGet("/api/with-response-headers", (HttpRequest request) =>
{
    request.HttpContext.Response.Headers.Add("my-response-header-1", "value-1");
    request.HttpContext.Response.Headers.Add("my-response-header-2", "value-2");

    return Results.Ok();
});

app.MapDelete("/api/fail-if-negative/{id}", ([FromRoute] int id) =>
{
    if (id < 0)
    {
        throw new Exception("Negative value");
    }

    return Results.Ok(id);
});

app.Run();
