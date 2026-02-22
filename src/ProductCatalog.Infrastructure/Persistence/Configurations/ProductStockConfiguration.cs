using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Products.Entities;

namespace ProductCatalog.Domain.Products.Entities;


public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {
        
        builder.ToTable("product_stocks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(x => x.QuantityOnHand)
            .HasColumnName("quantity_on_hand")
            .IsRequired();

        builder.Property(x => x.ReservedQuantity)
            .HasColumnName("reserved_quantity")
            .IsRequired();

        builder.Property(x => x.MinQuantity)
            .HasColumnName("min_quantity");


        builder.HasIndex(x => x.ProductId)
            .IsUnique()
            .HasDatabaseName("ux_product_stocks_product_id");

        builder.ToTable(t =>
            {
                t.HasCheckConstraint("ck_product_stocks_qty_nonnegative", "quantity_on_hand >= 0");
                t.HasCheckConstraint("ck_product_stocks_reserved_nonnegative", "reserved_quantity >= 0");
            }
        );

    }
    
}