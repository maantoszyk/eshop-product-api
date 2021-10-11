using Eshop.Product.Api.Swagger;
using Eshop.Product.Api.Swagger.Filters;
using Eshop.Product.Infrastructure.Database;
using Eshop.Product.Infrastructure.Mappers;
using Eshop.Product.Infrastructure.Repository;
using Eshop.Product.Infrastructure.Repository.Interfaces;
using Eshop.Product.Infrastructure.Services;
using Eshop.Product.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace Eshop.Product.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            // API Versioning
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
            });

            // Swagger documentation
            if (Configuration.GetValue<bool>("UseSwagger"))
            {
                services.AddSwaggerGen(swagger =>
                {
                    swagger.SwaggerDoc("v1", OpenApiInfoFactory.GetOpenApiInfo("v1"));
                    swagger.SwaggerDoc("v2", OpenApiInfoFactory.GetOpenApiInfo("v2"));
                    swagger.OperationFilter<RemoveVersionFromParameter>();
                    swagger.DocumentFilter<ReplaceVersionWithSelected>();
                    swagger.ResolveConflictingActions(a => a.First());

                    // Set the comments path for the Swagger JSON and UI.
                    foreach (var file in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
                        swagger.IncludeXmlComments(file);
                });
            }

            // Logger
            services.AddSingleton(Log.Logger);

            // DB Contexts
            services.AddDbContext<ProductDbContext>(opt => opt.UseInMemoryDatabase("IMDB"));

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();

            // Custom services
            services.AddScoped<IProductService, ProductService>();

            // Automapper
            services.AddAutoMapper(new Type[] {
                typeof(MappingProfile)
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (Configuration.GetValue<bool>("UseSwagger"))
            {
                app.UseStaticFiles(); // For swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
                    c.InjectStylesheet("/swagger/swagger-ui-custom.css");
                    c.DefaultModelsExpandDepth(-1); // (Un)comment this if you want to hide/show swagger Schemas section
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
