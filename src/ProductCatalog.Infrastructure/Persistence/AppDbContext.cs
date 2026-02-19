using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Infrastructure.Persistence;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}


    // public DbSet<Product> Products => Set<Product>();

}