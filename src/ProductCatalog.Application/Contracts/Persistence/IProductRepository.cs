using ProductCatalog.Domain.Products.Entities;

namespace ProductCatalog.Application.Contracts.Persistence;

public interface IProductRepository
{

    Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct = default);
    Task AddAsync (Product product, CancellationToken ct = default);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<Product?> GetByIdWithPricesAsync(Guid id, CancellationToken ct);

    Task<(List<Product> Items, int Total)> ListAsync(
        int page,
        int pageSize,
        string? search,
        bool? isActive,
        CancellationToken ct);

}