using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using AutoMapper;
using Template.API.Infrastructure.Swagger;
using Template.Database.Repositories;
using Template.Core;
using Template.Database;

namespace Template
{
    public class Startup
    {
        private readonly List<string> _versions = new List<string> { "1" };
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddControllersAsServices();
            services.AddAutoMapper();
            services.AddSwaggerGen(SwaggerGen);

            var connectionString = @"Server=.\sqlexpress;Database=Template;Integrated Security=True;";
            services.AddDbContext<TemplateContext>(o =>
                o.UseSqlServer(connectionString, x => x.MigrationsAssembly("Template.Database.Migrations")));

            var builder = new ContainerBuilder();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            builder.RegisterAssemblyTypes(typeof(GenericRepository<>).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Populate(services);

            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c => c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                if (!env.IsDevelopment())
                {
                    swaggerDoc.BasePath = "/";
                }
            }));

            app.UseSwaggerUI(UseSwaggerUI)
                .UseMvc()
                .MapWhen(x => x.Request.Path == "/", RedirectToSwagger);
        }

        /// <summary>
        ///     UseSwaggerUI
        /// </summary>
        /// <param name="options"></param>
        private void UseSwaggerUI(SwaggerUIOptions options)
        {
            _versions.ForEach(version => options.SwaggerEndpoint($"{version}/swagger.json", $"API v{version}"));
        }

        /// <summary>
        ///     SwaggerGen
        /// </summary>
        /// <param name="options"></param>
        private void SwaggerGen(SwaggerGenOptions options)
        {
            _versions.ForEach(version => options.SwaggerDoc(version,
                new Info { Title = "Travel Booking Configuration API", Version = $"v{version}" }));

            //Determine base path for the application.
            var basePath = AppContext.BaseDirectory;
            var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            var fileName = System.IO.Path.GetFileName(assemblyName + ".xml");
            var xmlPath = System.IO.Path.Combine(basePath, fileName);

            //Set the comments path for the Swagger JSON and UI.
            options.IncludeXmlComments(xmlPath);
            options.DescribeAllEnumsAsStrings();
            options.DocInclusionPredicate(HasVersion);

            //Setear configuracion para no harcodear el false
            options.OperationFilter<AddAuthorizationHeader>(false);
            options.OperationFilter<AddApiVersionHeader>(_versions.Last());
            options.OperationFilter<AddUnprocessibleEntityResponseType>();
            options.OperationFilter<AddInternalServerErrorResponseType>();
            options.OperationFilter<RemoveExtraneousContentTypes>();
            options.OrderActionsBy(x => $"{x.RelativePath} {x.HttpMethod}");
        }

        /// <summary>
        ///     RedirectToSwagger
        /// </summary>
        /// <param name="applicationBuilder"></param>
        private void RedirectToSwagger(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(y =>
            {
                y.Response.Redirect("swagger/");

                return Task.CompletedTask;
            });
        }

        /// <summary>
        ///     HasVersion
        /// </summary>
        /// <param name="docName"></param>
        /// <param name="apiDesc"></param>
        /// <returns></returns>
        private bool HasVersion(string docName, ApiDescription apiDesc)
        {
            if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

            var versions = methodInfo.DeclaringType
                .GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(attr => attr.Versions)
            .ToList();

            return versions.Any(v => v.ToString() == docName) ||
                   versions.Any();
        }
    }
}
