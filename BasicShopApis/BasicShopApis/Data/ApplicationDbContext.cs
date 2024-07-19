using BasicShopApis.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicShopApis.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
           .Property(p => p.Id)
           .ValueGeneratedOnAdd();

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Quantity)
                .IsRequired();

            modelBuilder.Entity<Cart>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.CartId)
                .IsRequired();

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems);

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.ProductId)
                .IsRequired();

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
