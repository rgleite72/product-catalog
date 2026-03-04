using ProductCatalog.Application.Contracts.Persistence;

namespace ProductCatalog.Infrastructure.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    
    private readonly ProductCatalogDbContext _db ;

    public EfUnitOfWork (ProductCatalogDbContext db)
    {
        _db = db;
    }

    public async Task<IUnitOfWorkTransaction> BeginTransasctionAsync(CancellationToken ct)
    {
        var tx = await _db.Database.BeginTransactionAsync(ct);
        return new EfUnitOfWorkTransaction(tx);
    }

    public Task SaveChangesASync(CancellationToken ct) => _db.SaveChangesAsync(ct);


    private sealed class EfUnitOfWorkTransaction: IUnitOfWorkTransaction
    {
       private readonly Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _tx;

       public EfUnitOfWorkTransaction (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction tx){

            _tx = tx;

       }
        public Task CommitAsync(CancellationToken ct) => _tx.CommitAsync(ct);
        public Task RollbackAsync(CancellationToken ct) => _tx.RollbackAsync(ct);
        public ValueTask DisposeAsync() => _tx.DisposeAsync();


    }



}