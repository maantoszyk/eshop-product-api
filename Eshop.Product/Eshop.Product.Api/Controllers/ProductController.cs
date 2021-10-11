using AutoMapper;
using Eshop.Product.Core.Dto;
using Eshop.Product.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Product.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IMapper mapper, IProductService productService)
        {
            _logger = logger;
            _mapper = mapper;
            _productService = productService;
        }

        /// <summary>
        /// Get product by given ID
        /// </summary>
        /// <param name="id">ID of the product</param>
        /// <response code="200">Returns the product</response>
        /// <response code="404">If the product is not found</response>     
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("[controller]/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                return product is not null ?
                    Ok(_mapper.Map<ProductDto>(product)) :
                    NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }    
        }

        /// <summary>
        /// Get all available products
        /// </summary>
        /// <response code="200">Return the products</response>
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        [HttpGet("[controller]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }           
        }

        /// <summary>
        /// Partial update of the product by given ID
        /// </summary>
        /// <param name="id">ID of the product</param>
        /// <param name="patchDocument">Patch document for update</param>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the patch document is null or empty</response>   
        /// <response code="404">If the product is not found</response>   
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("[controller]/{id}")]
        public async Task<IActionResult> PartialUpdateProduct(
            [FromRoute] int id, 
            [FromBody] JsonPatchDocument<ProductPatchDto> patchDocument)
        {
            try
            {
                if (patchDocument is null || patchDocument.Operations is null || patchDocument.Operations.Any() is false)
                    return BadRequest();

                var product = await _productService.GetProductById(id);
                if (product is null)
                    return NotFound();

                var productToPatch = _mapper.Map<ProductPatchDto>(product);
                patchDocument.ApplyTo(productToPatch);
                _mapper.Map(productToPatch, product);

                _ = await _productService.UpdateProduct(product);

                return Ok(_mapper.Map<ProductDto>(product));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Dummy endpoint
        /// </summary>
        [HttpGet("Dummy")]
        [MapToApiVersion("2.0")]
        [ApiExplorerSettings(GroupName = "v2")]
        public IActionResult Dummy()
        {
            return Ok($"Connection OK.{Environment.NewLine}You are using API v2.0");
        }
    }
}
