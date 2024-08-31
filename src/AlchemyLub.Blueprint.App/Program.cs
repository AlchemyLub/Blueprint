WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, false)
    .AddEnvironmentVariables();

builder.Services.AddAllLayers(builder.Configuration);
builder.Services.AddObservability(builder.Environment.ApplicationName);

builder.Logging.AddObservability();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

WebApplication app = builder.Build();

app.UseAllLayers();

await app.RunAsync();

public partial class Program;
