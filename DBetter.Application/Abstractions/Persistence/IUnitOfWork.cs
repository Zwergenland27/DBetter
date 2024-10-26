namespace DBetter.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    void BeginTransaction();
    
    Task CommitAsync(CancellationToken cancellationToken = default);
    
    Task AbortAsync(CancellationToken cancellationToken = default);
}