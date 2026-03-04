namespace ProductCatalog.Domain.Products.Entities;

public class Product
{
    
    public Guid Id {get; set; } = Guid.NewGuid();

    public string Sku {get; set;} = default!;
    public string Name {get; set; } = default!;

    public string? Description {get; set;}

    public bool IsActive {get; set;} =  true;

    public DateTimeOffset CreateAt {get; set;} = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdateAt {get; set; } = DateTimeOffset.UtcNow;

    public ICollection<ProductPrice> Prices {get; set;} = new List<ProductPrice>();



}