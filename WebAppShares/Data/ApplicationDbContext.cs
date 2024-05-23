using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebAppShares.Data.Identity;
using WebAppShares.Models;

namespace WebAppShares.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductsModel> Products { get; set; }
        public DbSet<ImageModel> Images { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<BuyProduct> BuyProducts { get; set; }
        public DbSet<BuyProductPurchasedGoods> BuyProductPurchasedGoods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Basket>()
                .HasOne(b => b.Product)
                .WithMany() 
                .HasForeignKey(b => b.ProductId);

            builder.Entity<Basket>()
                .HasOne(b => b.User)
                .WithMany() 
                .HasForeignKey(b => b.UserId);

            builder.Entity<BuyProductPurchasedGoods>()
                .HasOne(bp => bp.BuyProductValue)
                .WithMany(bp => bp.PurchasedGoods)
                .HasForeignKey(bp => bp.BuyProductId);

            builder.Entity<BuyProductPurchasedGoods>()
                .HasOne(bp => bp.ProductsModelValue)
                .WithMany()
                .HasForeignKey(bp => bp.ProductsModelId);
        }
    }
}
