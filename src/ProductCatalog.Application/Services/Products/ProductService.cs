using ProductCatalog.Application.Contracts;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.DTOs.Common;
using ProductCatalog.Application.DTOs.Products;
using ProductCatalog.Application.Exceptions;
using ProductCatalog.Domain.Products.Entities;

namespace ProductCatalog.Application.Services.Products;

public sealed class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;
    private readonly IProductRepository _productRepository;


    public ProductService(
        IUnitOfWork uow,
        IProductRepository productRepo)
    {
        _uow = uow;
        _productRepository = productRepo;
        ;
    }

    public async Task<ProductResponseDto> CreateProductWithInitialPriceAsync(
        CreateProductWithInitialPriceRequestDto dto,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(dto.Sku))
            throw ValidationException.Required(nameof(dto.Sku));

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw ValidationException.Required(nameof(dto.Name));

        if (dto.InitialPrice <= 0)
            throw ValidationException.ForField(nameof(dto.InitialPrice), "Must be greater than zero.");

        var sku = dto.Sku.Trim();
        var name = dto.Name.Trim();
        var currency = string.IsNullOrWhiteSpace(dto.Currency)
            ? "BRL"
            : dto.Currency.Trim().ToUpperInvariant();

        var existsSKU = await _productRepository.ExistsBySkuAsync(sku, ct);

        if (existsSKU)
            throw new ConflictException($"SKU '{sku}' already exists.");


        await using var tx = await _uow.BeginTransasctionAsync(ct);

        try
        {
            var now = DateTime.UtcNow;

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Sku = sku,
                Name = name,
                Description = dto.Description?.Trim(),
                CreateAt = now,
                UpdateAt = now,
                IsActive = true
            };

            var price = new ProductPrice
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Amount = dto.InitialPrice,
                Currency = currency,
                ValideFrom = now,
                CreateAt = now
            };

            product.Prices.Add(price);

            await _productRepository.AddAsync(product, ct);

            await _uow.SaveChangesASync(ct);
            await tx.CommitAsync(ct);

            return new ProductResponseDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                CreatedAt = product.CreateAt.UtcDateTime,
                InitialPrice = price.Amount,
                Currency = price.Currency
            };
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }

    }


public async Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductRequestDto dto, CancellationToken ct)
{
    if (id == Guid.Empty)
        throw ValidationException.Required(nameof(id));

    if (dto is null)
        throw ValidationException.Required(nameof(dto));

    var product = await _productRepository.GetByIdAsync(id, ct);
    if (product is null)
        throw new NotFoundException("Product not found.");

    if (!string.IsNullOrWhiteSpace(dto.Name))
        product.Name = dto.Name.Trim();

    product.Description = dto.Description?.Trim();
    product.UpdateAt = DateTime.UtcNow; // mantém consistência com seu model atual (UpdateAt)

    await _uow.SaveChangesASync(ct);

    return new ProductResponseDto
    {
        Id = product.Id,
        Sku = product.Sku,
        Name = product.Name,
        CreatedAt = product.CreateAt.UtcDateTime, // seguindo seu padrão CreateAt
  
    };
}

    public async Task InactiveAsync(Guid id, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null)
            throw new NotFoundException("Product not found.");

        product.IsActive = false;
        product.UpdateAt = DateTime.UtcNow;
    
        await _uow.SaveChangesASync(ct);

    }




    public async Task<ProductResponseDto> GetByIdAsync(Guid id, CancellationToken ct)
    {
            var product = await _productRepository.GetByIdAsync(id, ct);
            if (product is null)
                throw new NotFoundException("Product not found.");

            return new ProductResponseDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                CreatedAt = product.CreateAt.UtcDateTime
            };


    }



    public async Task<PagedResponseDto<ProductResponseDto>> ListAsync(ListProductRequestDto request, CancellationToken ct)
    {
    var page = request.Page <= 0 ? 1 : request.Page;
    var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

    var (items, total) = await _productRepository.ListAsync(
        page,
        pageSize,
        request.Search,
        request.IsActive,
        ct);

    return new PagedResponseDto<ProductResponseDto>
    {
        Items = items.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Sku = p.Sku,
            Name = p.Name,
            CreatedAt = p.CreateAt.UtcDateTime
        }).ToList(),
        Page = page,
        PageSize = pageSize,
        TotalItems = total,
        TotalPages = (int)Math.Ceiling(total / (double)pageSize)
    };
        
    }


}