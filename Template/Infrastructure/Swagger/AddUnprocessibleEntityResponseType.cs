using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Template.API.Infrastructure.Swagger
{
    public class AddUnprocessibleEntityResponseType : IOperationFilter
    {
        #region Properties

        private const string UnprocessableEntity = "Unprocessable Entity";

        #endregion        

        /// <summary>
        ///     Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (!operation.Parameters.Any(x => x is BodyParameter))
            {
                return;
            }

            if (operation.Responses == null)
            {
                operation.Responses = new Dictionary<string, Response>();
            }

            operation.Responses.Add("422",
                new Response { Description = UnprocessableEntity });
        }
    }
}