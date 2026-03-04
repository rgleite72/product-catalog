using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain.Products.Entities;
using ProductCatalog.Infrastructure.Persistence;

namespace ProductCatalog.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly ProductCatalogDbContext _db;

    public ProductRepository(ProductCatalogDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct) =>
        _db.Products.AnyAsync(p => p.Sku == sku, ct);

    public Task AddAsync(Product product, CancellationToken ct) =>
        _db.Products.AddAsync(product, ct).AsTask();



    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);


    public Task<Product?> GetByIdWithPricesAsync(Guid id, CancellationToken ct) 
        {
        return _db.Products
            .Include(p => p.Prices)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }


    public async Task<(List<Product> Items, int Total)> ListAsync(
        int page,
        int pageSize,
        string? search,
        bool? isActive,
        CancellationToken ct)
    {
        var query = _db.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Sku.Contains(search) || p.Name.Contains(search));

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

}