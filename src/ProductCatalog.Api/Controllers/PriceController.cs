using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.DTOs.Prices;
using ProductCatalog.Application.Services.Prices;


[ApiController]
[Route("api/products/{productId:guid}/price")]
public class PricesController : ControllerBase
{
    private readonly IPriceService _priceService;

    public PricesController(IPriceService priceService)
    {
        _priceService = priceService;
    }

    [HttpGet]
    public async Task<ActionResult<PriceResponseDto>> GetCurrentPrice(
        Guid productId,
        CancellationToken ct)
    {
        var price = await _priceService.GetCurrentPriceByProductIdAsync(productId, ct);
        return Ok(price);
    }

    [HttpPut]
    public async Task<ActionResult<PriceResponseDto>> UpdatePrice(
        Guid productId,
        UpdatePriceRequestDto dto,
        CancellationToken ct)
    {
        var price = await _priceService.UpdatePriceAsync(productId, dto, ct);
        return Ok(price);
    }
}