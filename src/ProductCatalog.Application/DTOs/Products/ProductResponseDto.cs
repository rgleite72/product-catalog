namespace ProductCatalog.Application.DTOs.Products;

public sealed class ProductResponseDto
{
    
    public Guid Id { get; set; }
    public string Sku { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public decimal InitialPrice { get; set; }
    public string Currency { get; set; } = default!;



}