namespace ProductCatalog.Application.DTOs.Products;

public sealed class UpdateProductRequestDto
{
    
    public string Name {get; set;} = default!;
    public string? Description {get; set;}
    
}