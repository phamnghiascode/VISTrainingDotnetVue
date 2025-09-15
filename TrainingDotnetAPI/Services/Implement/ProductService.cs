using FluentValidation;
using TrainingDotnetAPI.DTOs;
using TrainingDotnetAPI.Mappers;
using TrainingDotnetAPI.Repository.Interface;
using TrainingDotnetAPI.Services.Interface;

namespace TrainingDotnetAPI.Services.Implement
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;
        private readonly IValidator<UpsertProductDto> createProductValidator;

        public ProductService(IProductRepository repository, 
            IValidator<UpsertProductDto> validator)
        {
            this.repository = repository;
            this.createProductValidator = validator;
        }

        public async Task<IEnumerable<ProductDto>> CreateManyProductsAsync(IEnumerable<UpsertProductDto> productDtos)
        {
            foreach(var item in productDtos)
            {
                await createProductValidator.ValidateAndThrowAsync(item);
            }

            var products = productDtos.Select(p => p.ToProductEntity());
            var result = await repository.AddManyAsync(products);
            return result.Select(p => p.ToProductDto());
        }

        public async Task<ProductDto> CreateProductAsync(UpsertProductDto product)
        {
            await createProductValidator.ValidateAndThrowAsync(product);
            var result = await repository.AddAsync(product.ToProductEntity());
            return result.ToProductDto();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var pagedProducts = await repository.GetAllAsync(pageNumber, pageSize);
            var products = pagedProducts.Items.Select(p => p.ToProductDto()).ToList();
            return products;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await repository.GetByIdAsync(id);
            return product?.ToProductDto();
        }

        public async Task<bool> UpdateProductAsync(int id, UpsertProductDto product)
        {
            await createProductValidator.ValidateAndThrowAsync(product);
            var existingProduct = await repository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return false; 
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;

            return await repository.UpdateAsync(existingProduct);
        }
    }
}
