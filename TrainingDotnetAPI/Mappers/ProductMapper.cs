using TrainingDotnetAPI.DTOs;
using TrainingDotnetAPI.Models;

namespace TrainingDotnetAPI.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }

        public static Product ToProductEntity(this UpsertProductDto createProductDto)
        {
            return new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price
            };
        }
    }
}
