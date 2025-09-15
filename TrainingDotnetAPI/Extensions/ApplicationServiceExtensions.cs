using TrainingDotnetAPI.Repository.Implement;
using TrainingDotnetAPI.Repository.Interface;
using TrainingDotnetAPI.Services.Implement;
using TrainingDotnetAPI.Services.Interface;
using FluentValidation;

namespace TrainingDotnetAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            #region Service
            services.AddScoped<IProductService, ProductService>();
            #endregion

            #region Repository
            services.AddScoped<IProductRepository, ProductRepository>();
            #endregion

            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            return services;
        }
    }
}
