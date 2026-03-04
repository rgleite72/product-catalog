namespace ProductCatalog.Application.DTOs.Products;

public sealed class CreateProductWithInitialPriceRequestDto
{
    
    public string Sku {get; set;} = default!;
    public string Name {get; set;} = default!;
    public string? Description {get; set;}

    public decimal InitialPrice {get; set;}
    public string Currency {get; set;} = "BRL";



}