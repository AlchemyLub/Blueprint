namespace FloralHub.Blueprint.Application.Data.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Entity> Entities { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
