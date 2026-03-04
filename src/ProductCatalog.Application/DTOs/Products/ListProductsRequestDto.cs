namespace ProductCatalog.Application.DTOs.Products;

public sealed class ListProductRequestDto{
    
    public int Page {get; set;}
    public int PageSize {get; set;}

    public string? Search {get; set;}
    public bool? IsActive {get; set;}

}