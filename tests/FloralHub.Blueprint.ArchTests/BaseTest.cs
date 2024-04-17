namespace FloralHub.Blueprint.ArchTests;

/// <summary>
/// Базовый класс для тестов
/// </summary>
public abstract class BaseTest
{
    protected internal static readonly Assembly DomainAssembly = typeof(IEntity).Assembly;
    protected internal static readonly Assembly SharedAssembly = typeof(EntityType).Assembly;
    protected internal static readonly Assembly EndpointsAssembly = typeof(BlueprintController).Assembly;
    protected internal static readonly Assembly ApplicationAssembly = typeof(IApplicationService).Assembly;
    protected internal static readonly Assembly AppAssembly = typeof(ServiceCollectionExtensions).Assembly;
    protected internal static readonly Assembly InfrastructureAssembly = typeof(InfrastructureService).Assembly;

    protected internal const string App = $"{nameof(FloralHub)}.{nameof(Blueprint)}.{nameof(Blueprint.App)}";
    protected internal const string Domain = $"{nameof(FloralHub)}.{nameof(Blueprint)}.{nameof(Blueprint.Domain)}";
    protected internal const string Endpoints = $"{nameof(FloralHub)}.{nameof(Blueprint)}.{nameof(Blueprint.Endpoints)}";
    protected internal const string Application = $"{nameof(FloralHub)}.{nameof(Blueprint)}.{nameof(Blueprint.Application)}";
    protected internal const string SharedKernel = $"{nameof(FloralHub)}.{nameof(Blueprint)}.{nameof(Blueprint.SharedKernel)}";
    protected internal const string Infrastructure = $"{nameof(FloralHub)}.{nameof(Blueprint)}.{nameof(Blueprint.Infrastructure)}";
}
