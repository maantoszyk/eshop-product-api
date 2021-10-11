using Microsoft.OpenApi.Models;
using System;

namespace Eshop.Product.Api.Swagger
{
    public class OpenApiInfoFactory
    {
        public static OpenApiInfo GetOpenApiInfo(string version)
        {
            var info = GetOpenApiInfo();
            info.Version = version;
            return info;
        }

        private static OpenApiInfo GetOpenApiInfo()
        {
            return new OpenApiInfo
            {
                Title = "Product API",
                Description = $"Product API for eshop\r\n\r\n © Copyright {DateTime.Now.Year} Marián Antoszyk.",
                Contact = new OpenApiContact()
                {
                    Name = "Alza.cz a.s.",
                    Url = new Uri("https://www.alza.cz")
                }
            };
        }
    }
}
