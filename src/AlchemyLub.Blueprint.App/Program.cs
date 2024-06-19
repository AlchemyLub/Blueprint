WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllLayers();

// Нужно прописать в appsettings работу с Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

WebApplication app = builder.Build();

app.UseAllLayers();

await app.RunAsync();

public partial class Program;
