
using System;
using System.Collections.Generic;
using System.Text;
using Template.Database.Entities;

namespace Template.Database.Repositories
{
    public class ValueRepository : GenericRepository<Value>, IValueRepository
    {
        public ValueRepository(TemplateContext context) : base(context)
        {
        }
    }

    public interface IValueRepository : IRepository<Value>
    {
    }
}
