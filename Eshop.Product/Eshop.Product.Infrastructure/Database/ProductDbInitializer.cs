using Eshop.Product.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Product.Infrastructure.Database
{
    public class ProductDbInitializer
    {
        public static async Task Initialize(ProductDbContext context)
        {
            // Ensure the database is created before seeding.
            if (context.Database.EnsureCreated() == false)
                throw new Exception("Can not seed database with data");

            if (!context.Products.Any())
                await SeedProducts(context);
        }

        private static async Task SeedProducts(ProductDbContext context)
        {
            await context.AddRangeAsync(
                new ProductEntity { Id = 1, Description = "As fox as ox", ImgUri = "https://alza.cz", Name = "Fox", Price = 199.0m },
                new ProductEntity { Id = 2, Description = "Funny bunny", ImgUri = "https://alza.cz", Name = "Rabbit", Price = 49.0m },
                new ProductEntity { Id = 3, Description = "Pitty kitty", ImgUri = "https://alza.cz", Name = "Cat", Price = 99.0m });
            await context.SaveChangesAsync();
        }
    }
}
