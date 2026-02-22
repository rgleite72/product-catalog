using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Products.Entities;

namespace ProductCatalog.Infrastructure.Persistence.Configurations;



public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder) 
    {
        builder.ToTable("products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Sku)
            .HasColumnName("sku")
            .HasMaxLength(64)
            .IsRequired();

        builder.HasIndex(x => x.Sku)
            .IsUnique()
            .HasDatabaseName("ux_products_sku");


        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();


        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(x => x.CreateAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdateAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // 1:N Product -> Prices
        builder.HasMany(x => x.Prices)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


    }
}