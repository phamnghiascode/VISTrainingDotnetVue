using TrainingDotnetAPI.DTOs;

namespace TrainingDotnetAPI.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(UpsertProductDto product);
        Task<bool> UpdateProductAsync(int id, UpsertProductDto product);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductDto>> CreateManyProductsAsync(IEnumerable<UpsertProductDto> productDtos);
    }
}
