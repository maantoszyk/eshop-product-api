using Eshop.Product.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eshop.Product.Infrastructure.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductEntity>> GetAllProducts();
        Task<ProductEntity> GetProductById(int id);
        Task<ProductEntity> UpdateProduct(ProductEntity product);
    }
}
