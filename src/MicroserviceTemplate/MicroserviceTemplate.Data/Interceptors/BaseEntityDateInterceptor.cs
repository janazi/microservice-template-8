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
        if (!IsAuditEntity(eventData))
            return base.SavingChangesAsync(eventData, result, cancellationToken);


        SetAuditProperties(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static bool IsAuditEntity(DbContextEventData eventData)
    {
        return eventData.Context is not null &&
            eventData.Context.ChangeTracker.Entries<IAuditableEntity>().ToList().Count > 0;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (!IsAuditEntity(eventData))
            return base.SavingChanges(eventData, result);

        SetAuditProperties(eventData);
        return base.SavingChanges(eventData, result);
    }

    private static void SetAuditProperties(DbContextEventData eventData)
    {
        var now = DateTime.UtcNow;
        foreach (var entityEntry in eventData.Context.ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    {
                        entityEntry.Entity.DateCreated = now;
                        break;
                    }
                case EntityState.Modified:
                    {
                        entityEntry.Entity.DateModified = now;
                        break;
                    }
                default:
                    continue;
            }
        }
    }
}