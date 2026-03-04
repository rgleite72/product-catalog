using ProductCatalog.Application.DTOs.Common;
using ProductCatalog.Application.DTOs.Products;

namespace ProductCatalog.Application.Services.Products;

public interface IProductService
{
    Task<ProductResponseDto> CreateProductWithInitialPriceAsync(
        CreateProductWithInitialPriceRequestDto dto,
        CancellationToken ct);



    Task<ProductResponseDto> UpdateAsync(Guid Id, UpdateProductRequestDto dto, CancellationToken ct);

    Task InactiveAsync(Guid Id, CancellationToken ct);

    Task<ProductResponseDto> GetByIdAsync(Guid Id, CancellationToken ct);

    Task<PagedResponseDto<ProductResponseDto>> ListAsync (ListProductRequestDto request, CancellationToken ct);


}





