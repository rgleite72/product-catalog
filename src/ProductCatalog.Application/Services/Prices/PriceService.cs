using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.DTOs.Common;
using ProductCatalog.Application.DTOs.Prices;
using ProductCatalog.Application.Exceptions;
using ProductCatalog.Domain.Products.Entities;

namespace ProductCatalog.Application.Services.Prices;

public sealed class PriceService : IPriceService
{

    private readonly IUnitOfWork _uow;

    private readonly IProductRepository _productRepository;

    public PriceService(IUnitOfWork uow, IProductRepository productRepository)
    {
        
        _uow = uow;
        _productRepository = productRepository;

    }


public async Task<PriceResponseDto> GetCurrentPriceByProductIdAsync(Guid productId, CancellationToken ct)
{
    var product = await _productRepository.GetByIdWithPricesAsync(productId, ct);

    if (product is null)
        throw new NotFoundException("Product not found");

    var price = product.Prices
        .OrderByDescending(p => p.ValideFrom)
        .FirstOrDefault(p => p.ValideTo == null);

    if (price is null)
        throw new NotFoundException("Price not found");

    return new PriceResponseDto
    {
        ProductId = productId,
        Amount = price.Amount,
        Currency = price.Currency,
        ValideFrom = price.ValideFrom,
        ValideTo = price.ValideTo
    };
}


public async Task<PriceResponseDto> UpdatePriceAsync(Guid productId, UpdatePriceRequestDto dto, CancellationToken ct)
{
    if (productId == Guid.Empty)
        throw ValidationException.Required(nameof(productId));

    if (dto is null)
        throw ValidationException.Required(nameof(dto));

    if (dto.Amount <= 0)
        throw ValidationException.ForField(nameof(dto.Amount), "Must be greater than zero.");

    var product = await _productRepository.GetByIdWithPricesAsync(productId, ct);
    if (product is null)
        throw new NotFoundException("Product not found");

    await using var tx = await _uow.BeginTransasctionAsync(ct);

    try
    {
        var now = DateTime.UtcNow;

        var current = product.Prices
            .OrderByDescending(p => p.ValideFrom)
            .FirstOrDefault(p => p.ValideTo == null);

        if (current is not null)
            current.ValideTo = now;

        var currency = string.IsNullOrWhiteSpace(dto.Currency)
            ? (current?.Currency ?? "BRL")
            : dto.Currency.Trim().ToUpperInvariant();

        var newPrice = new ProductPrice
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            Amount = dto.Amount,
            Currency = currency,
            ValideFrom = now,
            ValideTo = null,
            CreateAt = now
        };

        product.Prices.Add(newPrice);

        await _uow.SaveChangesASync(ct);
        await tx.CommitAsync(ct);

        return new PriceResponseDto
        {
            ProductId = productId,
            Amount = newPrice.Amount,
            Currency = newPrice.Currency,
            ValideFrom = newPrice.ValideFrom,
            ValideTo = newPrice.ValideTo
        };
    }
    catch
    {
        await tx.RollbackAsync(ct);
        throw;
    }
}


}