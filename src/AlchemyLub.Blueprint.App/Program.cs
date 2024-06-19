WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllLayers();

WebApplication app = builder.Build();

// Нужно прописать в appsettings работу с Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

app.UseAllLayers();

await app.RunAsync();

public static partial class Program;
