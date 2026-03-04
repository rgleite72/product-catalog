using ProductCatalog.Application.DTOs.Prices;

namespace ProductCatalog.Application.Services.Prices;

public interface IPriceService
{
    Task<PriceResponseDto> GetCurrentPriceByProductIdAsync(Guid productId, CancellationToken ct);

    Task<PriceResponseDto> UpdatePriceAsync(Guid productId, UpdatePriceRequestDto dto, CancellationToken ct);
}
