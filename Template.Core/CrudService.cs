using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Template.Database;
using Template.Database.Entities;
using Template.Database.Repositories;
using Template.Model.Base;

namespace Template.Core
{
    public abstract class CrudService<T, TViewModel, TCreateModel, TUpdateModel> : BaseService, ICrudService<T, TViewModel, TCreateModel, TUpdateModel>
        where T : BaseEntity
        where TViewModel : IViewModel
        where TCreateModel : ICreateModel
        where TUpdateModel : IUpdateModel
    {
        protected readonly IRepository<T> Repository;

        protected CrudService(IUnitOfWork unitOfWork, IRepository<T> repository, IMapper mapper)
            : base(unitOfWork, mapper)
        {
            Repository = repository;
        }
        public virtual async Task<IEnumerable<TViewModel>> GetAsync()
        {
            var result = await Repository.GetAsync();
            return Mapper.Map<IEnumerable<TViewModel>>(result);
        }

        public virtual async Task<TViewModel> GetAsync(long id)
        {
            var result = await Repository.GetAsync(id);
            return Mapper.Map<TViewModel>(result);
        }

        public virtual async Task<long> CreateAsync(TCreateModel model)
        {
            var entity = Mapper.Map<T>(model);
            Repository.Create(entity);
            await UnitOfWork.Complete();
            return entity.Id;
        }

        public virtual async Task DeleteAsync(long id)
        {
            var entity = await Repository.GetAsync(id);
            entity.DeletedDate = DateTime.Now;
            await UnitOfWork.Complete();
        }

        public virtual async Task UpdateAsync(long id, TUpdateModel model)
        {
            var entity = await Repository.GetAsync(id);

            Mapper.Map(model, entity);

            await UnitOfWork.Complete();
        }
    }

    public interface ICrudService<T, TViewModel, in TCreateModel, in TUpdateModel>
    {
        Task<IEnumerable<TViewModel>> GetAsync();
        Task<TViewModel> GetAsync(long id);
        Task<long> CreateAsync(TCreateModel model);
        Task UpdateAsync(long id, TUpdateModel model);
        Task DeleteAsync(long id);
    }
}