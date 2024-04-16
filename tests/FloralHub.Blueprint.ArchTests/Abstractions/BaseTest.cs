namespace FloralHub.Blueprint.ArchTests.Abstractions;

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

    protected internal const string App = "FloralHub.Blueprint.App";
    protected internal const string Domain = "FloralHub.Blueprint.Domain";
    protected internal const string Endpoints = "FloralHub.Blueprint.Endpoints";
    protected internal const string Application = "FloralHub.Blueprint.Application";
    protected internal const string SharedKernel = "FloralHub.Blueprint.SharedKernel";
    protected internal const string Infrastructure = "FloralHub.Blueprint.Infrastructure";
}
