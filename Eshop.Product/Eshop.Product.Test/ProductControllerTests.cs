using AutoMapper;
using Eshop.Product.Api.Controllers;
using Eshop.Product.Core.Dto;
using Eshop.Product.Core.Entities;
using Eshop.Product.Infrastructure.Mappers;
using Eshop.Product.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Eshop.Product.Test
{
    public class ProductControllerTests
    {
        private readonly ProductController _sut;
        private readonly Mock<IProductService> _productServiceMock = new();
        private readonly Mock<ILogger<ProductController>> _logger = new();

        public ProductControllerTests ()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            _sut = new ProductController(_logger.Object, mappingConfig.CreateMapper(), _productServiceMock.Object);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var productEntity = new ProductEntity { Id = productId, Description = "Test", Price = 50.0m, ImgUri = "Img", Name = "Name" };
            var productDto = new ProductDto { Description = "Test", Price = 50.0m, ImgUri = "Img", Name = "Name" };
            _productServiceMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(productEntity);

            // Act
            var result = await _sut.GetById(productId);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult?.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(productDto), JsonSerializer.Serialize(okResult?.Value));
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var rnd = new Random();
            _productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetById(rnd.Next());
            var notFoundResult = result as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnProductsOrEmptyList()
        {
            // Arrange
            _productServiceMock.Setup(x => x.GetAllProducts()).ReturnsAsync(Enumerable.Empty<ProductEntity>());

            // Act
            var result = await _sut.GetAll();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult?.StatusCode);
        }

        [Fact]
        public async Task PatchProduct_ShouldReturnOk_WhenDescriptionReplaced()
        {
            // Arrange
            var productId = 1;
            var newDescription = "New description";

            var productEntity = new ProductEntity { Id = productId, Description = "Test" }; // Before update
            var productEntityUpdated = new ProductEntity { Id = productId, Description = newDescription }; // After update
            var productDto = new ProductDto { Description = newDescription }; // Product Dto
            var jsonPatch = new JsonPatchDocument<ProductPatchDto>();
            jsonPatch.Operations.Add(new Operation<ProductPatchDto> { op = "replace", path = "/description", value = newDescription });

            _productServiceMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(productEntity);
            _productServiceMock.Setup(x => x.UpdateProduct(productEntity)).ReturnsAsync(productEntityUpdated);

            // Act
            var result = await _sut.PartialUpdateProduct(productId, jsonPatch);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult?.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(productDto), JsonSerializer.Serialize(okResult?.Value));
        }

        [Fact]
        public async Task PatchProduct_ShouldReturnBadRequest_WhenPatchDocumentEmpty()
        {
            // Arrange
            var productId = 1;
            var newDescription = "New description";

            var productEntity = new ProductEntity { Id = productId, Description = "Test" }; // Before update
            var productEntityUpdated = new ProductEntity { Id = productId, Description = newDescription }; // After update
            var productDto = new ProductDto { Description = newDescription }; // Product Dto
            var jsonPatch = new JsonPatchDocument<ProductPatchDto>();

            _productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(() => null);
            _productServiceMock.Setup(x => x.UpdateProduct(null)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.PartialUpdateProduct(productId, jsonPatch);
            var okResult = result as BadRequestResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(400, okResult?.StatusCode);
        }

        [Fact]
        public async Task PatchProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var rnd = new Random();
            var jsonPatch = new JsonPatchDocument<ProductPatchDto>();
            jsonPatch.Operations.Add(new Operation<ProductPatchDto> { op = "replace", path = "/description", value = "HelloWorld" });

            _productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(() => null);
            _productServiceMock.Setup(x => x.UpdateProduct(null)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.PartialUpdateProduct(rnd.Next(), jsonPatch);
            var notFoundResult = result as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
        }
    }


    /*
    public class ProductControllerTestsApp : WebApplicationFactory<Program>
    {
        private readonly Action<IServiceCollection> _serviceOverride;

        public ProductControllerTestsApp(Action<IServiceCollection> serviceOverride)
        {
            _serviceOverride = serviceOverride;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(_serviceOverride);

            return base.CreateHost(builder);
        }
    }
    */
}
