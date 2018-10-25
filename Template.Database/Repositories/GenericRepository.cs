using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Database.Entities;

namespace Template.Database.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Create(T entity);

        void CreateRange(IEnumerable<T> entity);

        Task<T> GetAsync(long id);
        Task<List<T>> GetAsync();
    }

    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly TemplateContext _context;
        protected readonly DbSet<T> DbSet;

        protected GenericRepository(TemplateContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public virtual void Create(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            DbSet.Add(entity);
        }

        public virtual void CreateRange(IEnumerable<T> entities)
        {
            var list = entities as IList<T> ?? entities.ToList();
            foreach (var entity in list)
                entity.CreatedDate = DateTime.Now;

            DbSet.AddRange(list);
        }

        public virtual async Task<T> GetAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<T>> GetAsync()
        {
            return await DbSet.ToListAsync();
        }
    }
}