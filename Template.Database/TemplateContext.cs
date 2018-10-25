using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Template.Database.Entities;

namespace Template.Database
{
    public class TemplateContext : DbContext
    {
        public TemplateContext(DbContextOptions<TemplateContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Value> Values { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new EntityNameEntityTypeConfiguration());
        }
    }

    public class TemplateContextFactory : IDesignTimeDbContextFactory<TemplateContext>
    {
        public TemplateContext CreateDbContext(string[] args)
        {
            //var configurationPackage = Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            //var connectionStringParameter = configurationPackage.Settings.Sections["UserDatabase"].Parameters["UserDatabaseConnectionString"];
            //var baseUri = Environment.GetEnvironmentVariable("ConnectionString");

            var builder = new DbContextOptionsBuilder<TemplateContext>();
            builder.UseSqlServer(@"Server=.\sqlexpress;Database=Template;Integrated Security=True;");
            return new TemplateContext(builder.Options);
        }
    }
}
