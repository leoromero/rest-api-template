using System.Collections.Generic;
using System.Net;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Template.API.Infrastructure.Swagger
{
    public class AddInternalServerErrorResponseType : IOperationFilter
    {
        /// <summary>
        ///     Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Responses == null)
            {
                operation.Responses = new Dictionary<string, Response>();
            }

            operation.Responses.Add(((int)HttpStatusCode.InternalServerError).ToString(),
                new Response { Description = "Internal Server Error" });
        }
    }
}