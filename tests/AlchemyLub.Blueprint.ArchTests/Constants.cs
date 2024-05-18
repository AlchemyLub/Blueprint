namespace AlchemyLub.Blueprint.ArchTests;

/// <summary>
/// Набор констант/статики, используемый в тестах
/// </summary>
internal static class Constants
{
    /// <summary>
    /// Сборки
    /// </summary>
    internal static class Assemblies
    {
        internal static readonly Assembly DomainAssembly = typeof(Entity).Assembly;
        internal static readonly Assembly ClientsAssembly = typeof(IEntitiesClient).Assembly;
        internal static readonly Assembly EndpointsAssembly = typeof(EntitiesController).Assembly;
        internal static readonly Assembly ApplicationAssembly = typeof(IApplicationService).Assembly;
        internal static readonly Assembly InfrastructureAssembly = typeof(InfrastructureService).Assembly;
    }

    /// <summary>
    /// Полные названия проектов
    /// </summary>
    internal static class FullProjectNames
    {
        internal const string App = $"{nameof(AlchemyLub)}.{nameof(Blueprint)}.{nameof(Blueprint.App)}";
        internal const string Domain = $"{nameof(AlchemyLub)}.{nameof(Blueprint)}.{nameof(Blueprint.Domain)}";
        internal const string Clients = $"{nameof(AlchemyLub)}.{nameof(Blueprint)}.{nameof(Blueprint.Clients)}";
        internal const string Endpoints = $"{nameof(AlchemyLub)}.{nameof(Blueprint)}.{nameof(Blueprint.Endpoints)}";
        internal const string Application = $"{nameof(AlchemyLub)}.{nameof(Blueprint)}.{nameof(Blueprint.Application)}";
        internal const string Infrastructure = $"{nameof(AlchemyLub)}.{nameof(Blueprint)}.{nameof(Blueprint.Infrastructure)}";
    }
}
