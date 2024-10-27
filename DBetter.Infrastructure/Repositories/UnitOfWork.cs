using DBetter.Application.Abstractions.Persistence;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore.Storage;

namespace DBetter.Infrastructure.Repositories;

public class UnitOfWork(DBetterContext context) : IUnitOfWork, IAsyncDisposable
{
    private IDbContextTransaction? _transaction;
    
    public async Task BeginTransaction(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            return;
        }
        
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("Cannot commit a transaction that has not been started.");
        }

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task AbortAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("Cannot abort a transaction that has not been started.");
        }
        
        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
        }
        await context.DisposeAsync();
    }
}