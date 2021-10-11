using Eshop.Product.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Product.Infrastructure.Database
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public virtual DbSet<ProductEntity> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductEntity>().HasData(
                new ProductEntity { Id = 1, Description = "As fox as ox", ImgUri = "https://alza.cz", Name = "Fox", Price = 199.0m },
                new ProductEntity { Id = 2, Description = "Funny bunny", ImgUri = "https://alza.cz", Name = "Rabbit", Price = 49.0m },
                new ProductEntity { Id = 3, Description = "Pitty kitty", ImgUri = "https://alza.cz", Name = "Cat", Price = 99.0m });
        }
    }
}
