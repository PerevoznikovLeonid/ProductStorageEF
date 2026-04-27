using Microsoft.EntityFrameworkCore;

namespace ProductStorageEF.Model;

public class ProductsContext(string connectionString) : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }
}