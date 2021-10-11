using AutoMapper;
using Eshop.Product.Core.Dto;
using Eshop.Product.Core.Entities;

namespace Eshop.Product.Infrastructure.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductEntity, ProductDto>();
            CreateMap<ProductPatchDto, ProductEntity>();
            CreateMap<ProductEntity, ProductPatchDto>();
        }
    }
}
