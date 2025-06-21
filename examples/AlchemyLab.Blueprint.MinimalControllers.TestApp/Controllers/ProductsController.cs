using Asp.Versioning;

namespace AlchemyLab.Blueprint.MinimalControllers.TestApp.Controllers;

/// <summary>
/// Контроллер с продуктами
/// </summary>
[MinimalController]
[Route("/api/v{version:apiVersion}/products")]
[ApiVersion(1.0, Deprecated = true)]
[ApiVersion(2.0, "RC")]
public class ProductsController
{
    public record TestResponse(string Name);

    /// <summary>
    /// Получить все продукты
    /// </summary>
    [HttpGet]
    [MapToApiVersion(1.0)]
    [MapToApiVersion(2.0)]
    [MapToApiVersion(3.0)]
    public static Results<Ok<TestResponse>, NotFound<TestResponse>> GetAll()
    {
        Random random = new();

        if (1 == random.Next(0, 2))
        {
            return TypedResults.NotFound(new TestResponse(string.Empty));
        }

        return TypedResults.Ok(new TestResponse("All products"));
    }

    /// <summary>
    /// Получить продукт по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор продукта</param>
    /// <param name="service"><see cref="IProductService"/></param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = "ProductGet", Order = 2)]
    public static string Get(int id, [FromServices] IProductService service) => $"Product {id}";

    /// <summary>
    /// Создать новый продукт
    /// </summary>
    /// <param name="name">Имя нового продукта</param>
    [HttpPost]
    public static string Create(string name) => $"Created {name}";

    /// <summary>
    /// Удалить продукт
    /// </summary>
    /// <param name="id">Идентификатор продукта</param>
    [HttpDelete]
    public static void Delete(int id) {}
}

public interface IProductService { }

public class ProductService : IProductService
{
}
