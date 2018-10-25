using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Template.API.Infrastructure.Swagger
{
    internal class RemoveExtraneousContentTypes : IOperationFilter
    {

        /// <summary>
        ///     Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Produces?.Any() == true)
            {
                operation.Produces.Remove("text/plain");
                operation.Produces.Remove("text/json");
            }

            if (operation.Consumes?.Any() == true)
            {
                operation.Consumes.Remove("application/json-patch+json");
                operation.Consumes.Remove("text/json");
                operation.Consumes.Remove("application/*+json");
            }
        }
    }
}