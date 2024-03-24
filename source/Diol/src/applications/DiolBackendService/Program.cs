using Diol.Aspnet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddWindowsService();

builder.Services.AddDiolWeb();
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapRazorPages();
app.UseDiol();

app.Run();
