using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions options) : base(options) { }
    public DbSet<Product> Products { get; set; }

    public DbSet<ProductBrand> ProductBrands { get; set; }

    public DbSet<ProductType> ProductTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreContext).Assembly);
        // or 
        // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
        // difference between the two is that the first one is more explicit and the second one is more implicit
    }

}