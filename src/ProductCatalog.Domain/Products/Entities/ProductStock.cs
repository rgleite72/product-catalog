namespace ProductCatalog.Domain.Products.Entities;

public class ProductStock
{
    

    public Guid Id {get; set;} = Guid.NewGuid();

    public Guid ProductId {get; set;}
    public Product Product {get; set;} = default!;

    public int QuantityOnHand {get; set;}
    public int ReservedQuantity {get; set;}
    public int? MinQuantity {get; set;}

    public DateTimeOffset UpdateAt {get; set;} = DateTimeOffset.UtcNow;

}