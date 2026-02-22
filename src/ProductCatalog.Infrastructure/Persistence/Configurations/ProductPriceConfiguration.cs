using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Products.Entities;


namespace ProductCatalog.Infrastructure.Persistence.Configurations;

public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
{
    public void Configure(EntityTypeBuilder<ProductPrice> builder)
    {
            builder.ToTable("product_prices");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .HasPrecision(18,2)
                .IsRequired();
            
            builder.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(x => x.ValideFrom)
                .HasColumnName("valid_from")
                .IsRequired();

            builder.Property(x => x.ValideTo)
                .HasColumnName("valid_to");

            builder.Property(x => x.CreateAt)
                .HasColumnName("created_at")
                .IsRequired();

            
            builder.HasIndex(x => new{x.ProductId, x.ValideFrom})
                    .HasDatabaseName("ix_product_prices_product_id_valid_from");

            
            //Preço não pode ser negativo
            builder.ToTable(t => t.HasCheckConstraint("ck_product_prices_amount_nonnegative", "amount >=0"));


    }
}
