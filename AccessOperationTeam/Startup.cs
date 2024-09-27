using AccessOperationTeam.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using ECommerceSkinet.Core.Helpers;
using ECommerceSkinet.WebAPI.Middleware;
using ECommerceSkinet.WebAPI.Extensions;

namespace ECommerceSkinet.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers().AddXmlSerializerFormatters();
            services.AddApiVersioning(config =>
            {
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("default"));
            });
            services.AddApplicationServices();
            //Enable Swagger
            services.AddSwaggerDocumentation(); 
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // exception handler middleware
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); 

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
