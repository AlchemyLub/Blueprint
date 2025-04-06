namespace AlchemyLab.Blueprint.Endpoints.MinimalApi;

public static class ProductEndpoints
{
    /// <summary>
    /// Регистрирует все сгенерированные эндпоинты Minimal API.
    /// </summary>
    /// <param name="app">Экземпляр WebApplication.</param>
    /// <returns>Экземпляр WebApplication для цепочки вызовов.</returns>
    public static WebApplication MapGeneratedControllers(this WebApplication app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Generated APIs")
            .WithDescription("Сгенерированные API эндпоинты")
            .WithGroupName("Generated");

        group.MapProductsApi();
        group.MapUsersApi(); // Пример для других контроллеров

        return app;
    }

    private static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/products", GetAllProducts)
            .WithName("GetAllProducts")
            .WithDescription("Возвращает список всех продуктов");

        group.MapGet("/products/{id}", GetProduct)
            .WithName("GetProduct")
            .WithDescription("Возвращает продукт по ID");

        group.MapPost("/products", CreateProduct)
            .WithName("CreateProduct")
            .WithDescription("Создает новый продукт")
            .Accepts<Product>("application/json")
            .Produces<Product>(StatusCodes.Status201Created);

        group.MapPut("/products/{id}", UpdateProduct)
            .WithName("UpdateProduct")
            .WithDescription("Обновляет продукт по ID")
            .Accepts<Product>("application/json");

        group.MapDelete("/products/{id}", DeleteProduct)
            .WithName("DeleteProduct")
            .WithDescription("Удаляет продукт по ID");

        return group;
    }

    private static RouteGroupBuilder MapUsersApi(this RouteGroupBuilder group)
    {
        // Аналогично для других контроллеров
        return group;
    }

    // Обработчики (будут сгенерированы на основе классов контроллеров)
    private static IResult GetAllProducts([FromServices] IProductService service)
    {
        var products = service.GetAll();
        return Results.Ok(products);
    }

    private static IResult GetProduct(int id, [FromServices] IProductService service)
    {
        var product = service.GetById(id);
        return product == null ? Results.NotFound() : Results.Ok(product);
    }

    private static IResult CreateProduct(Product product, [FromServices] IProductService service)
    {
        var createdProduct = service.Create(product);
        return Results.Created($"/api/products/{createdProduct.Id}", createdProduct);
    }

    private static IResult UpdateProduct(int id, Product product, [FromServices] IProductService service)
    {
        if (id != product.Id) return Results.BadRequest("ID in URL does not match body ID");
        var updated = service.Update(product);
        return updated ? Results.NoContent() : Results.NotFound();
    }

    private static IResult DeleteProduct(int id, [FromServices] IProductService service)
    {
        var deleted = service.Delete(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    }
}

// Модель продукта (может быть сгенерирована или взята из существующего кода)
public record Product(int Id, string Name, decimal Price);

// Интерфейс сервиса (должен существовать в проекте)
public interface IProductService
{
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product Create(Product product);
    bool Update(Product product);
    bool Delete(int id);
}
