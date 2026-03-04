namespace ProductCatalog.Application.Contracts.Persistence;



public interface IUnitOfWorkTransaction: IAsyncDisposable
{
    Task CommitAsync(CancellationToken ct);
    Task RollbackAsync(CancellationToken ct);
}


public interface IUnitOfWork
{
    Task <IUnitOfWorkTransaction> BeginTransasctionAsync(CancellationToken ct);
    Task SaveChangesASync (CancellationToken ct);
    
}

