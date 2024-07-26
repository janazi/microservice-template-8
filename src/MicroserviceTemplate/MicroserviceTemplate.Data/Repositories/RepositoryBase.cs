using MicroserviceTemplate.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceTemplate.Infra.Data.Repositories;

public abstract class RepositoryBase<TDbContext>(TDbContext dbContext)
    where TDbContext : DbContext
{
    protected async Task<OperationResult<TEntity>> TryCreateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            return new OperationResult<TEntity>(new ArgumentException("Entity cannot be null"));

        try
        {
            await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            var operationResult = HandleException<TEntity>(ex);

            if (operationResult != null)
                return operationResult;

            throw;
        }

        return new OperationResult<TEntity>(entity);
    }

    protected async Task<OperationResult<TEntity>> TryUpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            return new OperationResult<TEntity>(new ArgumentException("Entity cannot be null"));

        try
        {
            dbContext.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            var operationResult = HandleException<TEntity>(ex);

            if (operationResult != null)
                return operationResult;

            throw;
        }

        return new OperationResult<TEntity>(entity);
    }

    protected OperationResult<TEntity>? HandleException<TEntity>(DbUpdateException ex)
    {
        // this exception was due to a concurrency token failure
        // (the record with a concurrency token column was updated by something else first
        // requiring corrective action by the calling code such as re-attempting the operation)
        if (ex is DbUpdateConcurrencyException)
            return new OperationResult<TEntity>(ex, true);

        var sqlException = ex.InnerException as SqlException;

        // 2601: Violation of unique index
        // 2627: Violation of unique constraint
        // 544:  Violation of identity_insert
        // https://docs.microsoft.com/en-us/sql/relational-databases/errors-events/database-engine-events-and-errors?view=sql-server-ver15
        return sqlException?.Number is not (2601 or 2627 or 544) ? null : new OperationResult<TEntity>(ex);
    }
}
