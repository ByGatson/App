using AppAPI.Application.Interfaces;
using AppAPI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AppAPI.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IGenerateSbifService, GenerateSbifService>();
            return services;
        }
    }
}
