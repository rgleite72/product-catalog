using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.DTOs.Common;
using ProductCatalog.Application.DTOs.Products;
using ProductCatalog.Application.Services.Products;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductWithInitialPriceRequestDto request,
        CancellationToken ct)
    {
        var result = await _productService.CreateProductWithInitialPriceAsync(request, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponseDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _productService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponseDto>> Update(
        Guid id,
        [FromBody] UpdateProductRequestDto dto,
        CancellationToken ct)
    {
        var result = await _productService.UpdateAsync(id, dto, ct);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/inactivate")]
    public async Task<IActionResult> Inactivate(Guid id, CancellationToken ct)
    {
        await _productService.InactiveAsync(id, ct);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<ProductResponseDto>>> List(
        [FromQuery] ListProductRequestDto request,
        CancellationToken ct)
    {
        var result = await _productService.ListAsync(request, ct);
        return Ok(result);
    }
}