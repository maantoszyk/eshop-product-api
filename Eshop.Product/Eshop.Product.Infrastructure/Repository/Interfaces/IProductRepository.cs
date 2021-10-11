using Eshop.Product.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eshop.Product.Infrastructure.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductEntity>> GetAll();
        Task<ProductEntity> GetById(int id);
        Task<ProductEntity> Update(ProductEntity product);
    }
}
