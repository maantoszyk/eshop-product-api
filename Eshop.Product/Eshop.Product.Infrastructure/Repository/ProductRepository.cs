using Eshop.Product.Core.Entities;
using Eshop.Product.Infrastructure.Database;
using Eshop.Product.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eshop.Product.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductEntity>> GetAll()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductEntity> GetById(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProductEntity> Update(ProductEntity product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
