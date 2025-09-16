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
            services.AddLogging();

            #region Service
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITokenService, TokenService>();  
            #endregion

            #region Repository
            services.AddScoped<IProductRepository, ProductRepository>();
            #endregion

            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            return services;
        }
    }
}
