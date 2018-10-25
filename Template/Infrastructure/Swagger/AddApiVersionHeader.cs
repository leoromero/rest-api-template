using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Template.API.Infrastructure.Swagger
{
    public class AddApiVersionHeader : IOperationFilter
    {
        #region properties

        private readonly string _defaultApiVersion;

        #endregion

        /// <summary>
        ///     AddApiVersionHeader
        /// </summary>
        /// <param name="defaultApiVersion"></param>
        public AddApiVersionHeader(string defaultApiVersion)
        {
            _defaultApiVersion = defaultApiVersion;
        }

        /// <summary>
        ///     Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var methodInfo = default(MethodInfo);
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                methodInfo = controllerActionDescriptor.MethodInfo;
            }
            if (methodInfo.GetCustomAttributes<ApiVersionNeutralAttribute>().Any())
            {
                return;
            }

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "api-version",
                In = "header",
                Description = "API Version",
                Required = true,
                Type = "string",
                Default = _defaultApiVersion
            });
        }
    }
}