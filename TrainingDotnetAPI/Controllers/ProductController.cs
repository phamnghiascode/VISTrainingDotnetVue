using Microsoft.AspNetCore.Mvc;
using TrainingDotnetAPI.DTOs;
using TrainingDotnetAPI.Models;
using TrainingDotnetAPI.Services.Interface;

namespace TrainingDotnetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> logger;
        private readonly IProductService productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            this.logger=logger;
            this.productService=productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {
            try
            {
                var products = await productService.GetAllProductsAsync(pageNumber, pageSize);

                return Ok(products);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting products.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(UpsertProductDto product)
        {
            try
            {
                await productService.CreateProductAsync(product);
                return CreatedAtAction(nameof(GetProducts), product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(int id, UpsertProductDto createProductDto)
        {
            try
            {
                var result = await productService.UpdateProductAsync(id, createProductDto);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("batch")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> CreateManyProducts([FromBody] IEnumerable<UpsertProductDto> createDtos)
        {
            try
            {
                var newProducts = await productService.CreateManyProductsAsync(createDtos);
                return Ok(newProducts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
