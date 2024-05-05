using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Visonex.Idmg.ExternalWebApi.Common
{
    public class HeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = CustomHeaders.OrganizationNameHeader,
                In = ParameterLocation.Header,
                Required = true
            });
        }
    }
}
