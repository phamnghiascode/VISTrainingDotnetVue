using Microsoft.EntityFrameworkCore;
using TrainingDotnetAPI.Models;

namespace TrainingDotnetAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
