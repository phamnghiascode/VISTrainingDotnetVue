using TrainingDotnetAPI.DTOs;
using TrainingDotnetAPI.Models;

namespace TrainingDotnetAPI.Repository.Interface
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetAllAsync(int pageNumber, int pageSize);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Product>> AddManyAsync(IEnumerable<Product> products);
    }
}
