using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Eshop.Product.Api.Swagger.Filters
{
    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters.Count > 0)
            {
                var versionparameter = operation.Parameters.Single(a => a.Name == "version");
                operation.Parameters.Remove(versionparameter);
            }
        }
    }
}
