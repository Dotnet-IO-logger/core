using Diol.Aspnet;

var appOptions = new WebApplicationOptions() 
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args
};

var builder = WebApplication.CreateBuilder(appOptions);

builder.Services.AddDiol();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseDiol();
app.MapRazorPages();

app.Run();
