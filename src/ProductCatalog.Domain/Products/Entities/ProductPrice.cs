namespace ProductCatalog.Domain.Products.Entities;

public class ProductPrice
{
    
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProductId { get; set;}
    public Product Product {get; set;} = default!;


    public decimal Amount {get; set; }

    public string Currency {get; set;} = "BRL";
    
    public DateTimeOffset ValideFrom {get; set;} = DateTimeOffset.UtcNow;
    public DateTimeOffset? ValideTo {get; set;}

    public DateTimeOffset CreateAt {get; set;} = DateTimeOffset.UtcNow;
    

}