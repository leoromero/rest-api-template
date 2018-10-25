using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Template.API.Infrastructure.Swagger
{
    public class AddAuthorizationHeader : IOperationFilter
    {
        #region properties

        private readonly bool _authorizationRequired;

        #endregion

        /// <summary>
        ///     AddAuthorizationHeader
        /// </summary>
        /// <param name="authorizationRequired"></param>
        public AddAuthorizationHeader(bool authorizationRequired)
        {
            _authorizationRequired = authorizationRequired;
        }

        /// <summary>
        ///     Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            var methodInfo = default(MethodInfo);
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                 methodInfo = controllerActionDescriptor.MethodInfo;
            }

            var allowAnonymous = methodInfo?.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            if (allowAnonymous.HasValue && !allowAnonymous.Value)
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "JWT access token",
                    Required = _authorizationRequired,
                    Type = "string"
                });
            }
        }
    }
}