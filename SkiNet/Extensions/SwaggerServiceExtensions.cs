using Microsoft.OpenApi.Models;

namespace ECommerceSkinet.WebAPI.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SkiNet API", Version = "v1" });
            });

            return services;
        }
    }
}
