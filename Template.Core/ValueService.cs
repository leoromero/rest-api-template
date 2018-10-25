using AutoMapper;
using Template.Database;
using Template.Database.Entities;
using Template.Database.Repositories;
using Template.Model;

namespace Template.Core
{
    public class ValueService : CrudService<Value, ValueModel, ValueModel, ValueModel>, IValueService
    {
        public ValueService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, unitOfWork.GetRepository<IValueRepository>(), mapper)
        {
        }
    }

    public interface IValueService : ICrudService<Value, ValueModel, ValueModel,ValueModel>
    {
    }
}