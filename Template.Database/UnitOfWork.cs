using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TemplateContext _context;

        public UnitOfWork(IServiceProvider serviceProvider, TemplateContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public T GetRepository<T>()
        {
            return (T)_serviceProvider.GetService<T>();
        }

    }

    public interface IUnitOfWork
    {
        Task<int> Complete();
        T GetRepository<T>();
    }
}
