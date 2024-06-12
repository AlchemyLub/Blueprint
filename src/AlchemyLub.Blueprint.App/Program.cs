WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllLayers();

WebApplication app = builder.Build();

app.UseAllLayers();

await app.RunAsync();

public partial class Program;
