namespace ProductCatalog.Application.DTOs.Prices;

public sealed class PriceResponseDto
{
    
    public Guid ProductId{get; set;}

    public decimal Amount{get; set;}

    public string Currency{get; set;} = default!;

    public DateTimeOffset ValideFrom { get; set; }

    public DateTimeOffset? ValideTo { get; set; }

}