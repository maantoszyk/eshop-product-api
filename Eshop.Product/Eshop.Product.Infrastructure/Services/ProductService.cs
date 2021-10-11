using Eshop.Product.Core.Entities;
using Eshop.Product.Infrastructure.Repository.Interfaces;
using Eshop.Product.Infrastructure.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eshop.Product.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProducts()
        {
            return await _productRepo.GetAll();
        }

        public async Task<ProductEntity> GetProductById(int id)
        {
            return await _productRepo.GetById(id);
        }

        public async Task<ProductEntity> UpdateProduct(ProductEntity product)
        {
            return await _productRepo.Update(product);
        }
    }
}
