using Microsoft.EntityFrameworkCore;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.DB;

public class PetShopContext : DbContext
{
    public DbSet<Category> Category { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<User?> User { get; set; }
    public DbSet<Manufacturer> Manufacturer { get; set; }
    public DbSet<Photo> Photo { get; set; }
    public DbSet<ProductInTransaction> ProductInTransaction { get; set; }
    public DbSet<ProductPhoto> ProductPhoto { get; set; }
    public DbSet<ProductTag> ProductTag { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<UserPrincipal> UserPrincipal { get; set; }
    
    public PetShopContext(DbContextOptions options) : base (options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}