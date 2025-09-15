using Microsoft.EntityFrameworkCore;
using TrainingDotnetAPI.Data;
using TrainingDotnetAPI.DTOs;
using TrainingDotnetAPI.Models;
using TrainingDotnetAPI.Repository.Interface;

namespace TrainingDotnetAPI.Repository.Implement
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext context;

        public ProductRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> AddManyAsync(IEnumerable<Product> products)
        {
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
            return products;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleteProduct = await context.Products.FindAsync(id);
            if ( deleteProduct == null)
            {
                return false;
            }
            context.Products.Remove(deleteProduct);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Product>> GetAllAsync(int pageNumber, int pageSize)
        {
            var totalCount = await context.Products.CountAsync();

            var items = await context.Products
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Product>(items, totalCount);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var existingProduct = await context.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return false;
            }
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            var rowEffect = await context.SaveChangesAsync();
            return rowEffect > 0;
        }
    }
}
