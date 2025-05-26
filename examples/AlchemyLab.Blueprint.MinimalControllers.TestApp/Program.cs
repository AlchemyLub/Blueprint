WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGeneratedControllers();

RouteGroupBuilder group = app
    .MapGroup("/products")
    .WithTags("Products")
    .WithDescription("Контроллер с продуктами")
    .RequireAuthorization(new Microsoft.AspNetCore.Authorization.AuthorizeAttribute()
    {
        Policy = "ProductPolicy",
        Roles = "Admin",
        AuthenticationSchemes = "AuthScheme"
    });

group.MapGet("/", AlchemyLab.Blueprint.MinimalControllers.TestApp.Controllers.ProductsController.GetAll)
    .WithOpenApi(opt =>
    {
        opt.OperationId = "Products.AlchemyLab.Blueprint.MinimalControllers.TestApp.Controllers.ProductsController.GetAll1";
        opt.Description = "Получить все продукты 1";
        opt.Summary = "Получить все продукты 2";
        opt.Parameters.Add(new()
        {
            Name = "id",
            In = ParameterLocation.Path,
            Required = true,
            Description = "Идентификатор продукта"
        });
        opt.Deprecated = true;

        return opt;
    })
    .WithDescription("Описание эндпоинта 1")
    .WithDisplayName("Отображаемое имя 1")
    .WithOrder(3)
    .WithRequestTimeout(TimeSpan.FromHours(1));

app.Run();
