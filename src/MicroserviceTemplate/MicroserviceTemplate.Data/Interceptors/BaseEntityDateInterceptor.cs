using MicroserviceTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MicroserviceTemplate.Infra.Data.Interceptors;

public class BaseEntityDateInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null ||
            !eventData.Context.ChangeTracker.Entries<IAuditableEntity>().ToList().Any())
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entityEntry in eventData.Context.ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    {
                        entityEntry.Entity.DateCreated = DateTime.UtcNow;
                        break;
                    }
                case EntityState.Modified:
                    {
                        entityEntry.Entity.DateModified = DateTime.UtcNow;
                        break;
                    }
                default:
                    continue;
            }
        }

        var now = DateTime.UtcNow;

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}