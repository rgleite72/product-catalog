namespace ProductCatalog.Application.DTOs.Prices;


public sealed class UpdatePriceRequestDto
{
    
    public decimal Amount{get; set;}
    public string Currency{get; set;} = default!;


}


