using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.DAL.Repostiry.Abstraction;
using SmartLibrary.DTO.Models.Results;
using SmartLibrary.DAL.Entities.Abstraction;


namespace SmartLibrary.BLL.Services.Implementation
{
    /// <summary>
    /// Provides implementation of  interface.
    /// </summary>
    /// <typeparam name="TEntity">type of the entity</typeparam>
    /// <typeparam name="TDto">type of the dto</typeparam>
    public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto> where TEntity : class
    {
        /// <summary>
        /// Contains instance of repository
        /// </summary>
        protected IGenericRepository<TEntity, int> repository;

        /// <summary>
        /// Contains instance of mapper
        /// </summary>
        protected IMapper mapper;

        /// <summary>
        /// Constructs base service
        /// </summary>
        /// <param name="repository">Instance of <see cref="IGenericRepository{TEntity, int}"/></param>
        /// <param name="mapper">Instance of <see cref="IMapper"/></param>
        public BaseService(IGenericRepository<TEntity, int> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;

        }

        /// <summary>
        /// Returns dto by id
        /// </summary>
        /// <param name="id">Identifier of dto</param>
        /// <returns>Instance of <see cref="SingleResult<TDto>"/> model</returns>
        public virtual async Task<SingleResult<TDto>> GetAsync(int id)
        {
            var result = new SingleResult<TDto>();
            var entity = await repository.GetAsync(id);

            if (entity != null)
            {
                result.Data = mapper.Map<TEntity, TDto>(entity);
                result.IsSuccessful = true;
            }

            return result;
        }

        /// <summary>
        /// Returns dto by predicate
        /// </summary>
        /// <returns>Instance of <see cref="SingleResult<TDto>"/> model</returns>
        public virtual async Task<SingleResult<TDto>> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var result = new SingleResult<TDto>();
            var entity = await repository.GetAsync(predicate);

            if (entity != null)
            {
                result.Data = mapper.Map<TEntity, TDto>(entity);
                result.IsSuccessful = true;
            }

            return result;
        }

        /// <summary>
        /// Returns list of dto asynchronously
        /// </summary>
        /// <returns>Instance of <see cref="CollectionResult{TDto}"/> model</returns>
        public virtual async Task<CollectionResult<TDto>> SearchAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var result = new CollectionResult<TDto>();
            var entities = await repository.Get(predicate).ToListAsync();

            if (entities != null)
            {
                result.Items = mapper.Map<List<TEntity>, List<TDto>>(entities);
                result.IsSuccessful = true;
            }

            return result;
        }

        /// <summary>
        /// Creates dto
        /// </summary>
        /// <param name="model">Instance of <see cref="TDto"/> model</param>
        /// <returns>Instance of <see cref="SingleResult<TDto>"/> model</returns>
        public virtual async Task<SingleResult<TDto>> CreateAsync(TDto model)
        {
            var result = new SingleResult<TDto>();
            var entity = mapper.Map<TDto, TEntity>(model);
            SetDefaultProps(entity, true);
            var created = await repository.CreateAsync(entity);

            if (created != null)
            {
                result.Data = mapper.Map<TEntity, TDto>(created);
                result.IsSuccessful = true;
            }

            return result;
        }

        /// <summary>
        /// Creates list of dto
        /// </summary>
        /// <param name="items">The collection of <see cref="TDto"/> items</param>
        /// <returns>Instance of <see cref="CollectionResult{TDto}"/> model</returns>
        public virtual async Task<CollectionResult<TDto>> CreateRangeAsync(List<TDto> items)
        {
            var result = new CollectionResult<TDto>();
            var entities = new List<TEntity>();
            foreach (var item in items)
            {
                var entity = mapper.Map<TDto, TEntity>(item);
                SetDefaultProps(entity, true);
                entities.Add(entity);
            }

            var creationResult = await repository.CreateRangeAsync(entities);

            if (creationResult.Count() > 0)
            {
                result.Items = mapper.Map<List<TDto>>(creationResult);
                result.IsSuccessful = true;
            }

            return result;
        }

        /// <summary>
        /// Deletes by id
        /// </summary>
        /// <param name="id">Identifier of dto</param>
        /// <returns>Instance of <see cref="OperationResult"/> model</returns>
        public virtual async Task<OperationResult> DeleteAsync(int id)
        {
            var result = new OperationResult();
            await repository.DeleteAsync(id);

            result.IsSuccessful = true;

            return result;
        }

        /// <summary>
        /// Deletes range of entites by predicate
        /// </summary>
        /// <param name="predicate">Filter predicate</param>
        /// <returns>Instance of <see cref="OperationResult"/> model</returns>
        public virtual async Task<OperationResult> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = new OperationResult();
            await repository.DeleteByQueryAsync(predicate, isSoftDelete: false);

            result.IsSuccessful = true;

            return result;
        }

        /// <summary>
        /// Updates dto
        /// </summary>
        /// <param name="model">Instance of <see cref="Dto"/> model</param>
        /// <returns>Instance of <see cref="SingleResult<Dto>"/> model</returns>
        public virtual async Task<SingleResult<TDto>> UpdateAsync(TDto model)
        {
            var result = new SingleResult<TDto>();
            var entity = mapper.Map<TDto, TEntity>(model);
            SetDefaultProps(entity, false);
            var updated = await repository.UpdateAsync(entity);

            if (updated != null)
            {
                result.Data = mapper.Map<TEntity, TDto>(updated);
                result.IsSuccessful = true;
            }

            return result;
        }

        /// <summary>
        /// Updates list of dto
        /// </summary>
        /// <param name="items">The collection of <see cref="TDto"/> items</param>
        /// <returns>Instance of <see cref="CollectionResult{TDto}"/> model</returns>
        public virtual async Task<CollectionResult<TDto>> UpdateRangeAsync(List<TDto> items)
        {
            var result = new CollectionResult<TDto>();
            var entities = new List<TEntity>();

            foreach (var item in items)
            {
                var entity = mapper.Map<TDto, TEntity>(item);
                SetDefaultProps(entity, false);
                entities.Add(entity);
            }

            repository.DetachEntries();
            var updateResult = await repository.UpdateRangeAsync(entities);

            if (updateResult.Count() > 0)
            {
                result.Items = mapper.Map<List<TDto>>(updateResult);
                result.IsSuccessful = true;
            }

            return result;
        }

        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns>Instance of <see cref="OperationResult"/> model</returns>
        protected virtual async Task<OperationResult> DeleteAsync(TEntity entity)
        {
            var result = new OperationResult();
            await repository.DeleteAsync(entity);

            result.IsSuccessful = true;

            return result;
        }

        /// <summary>
        /// Set entity default values
        /// </summary>
        /// <param name="entity">Entity to set</param>
        protected void SetDefaultProps(TEntity entity, bool isNewCreated)
        {
            var timestamp = DateTime.UtcNow;

            if (entity is IHasCreationDate && isNewCreated)
            {
                ((IHasCreationDate)entity).CreationDate = timestamp;
            }

            if (entity is IHasModifyDate)
            {
                ((IHasModifyDate)entity).ModifyDate = timestamp;
            }

            if (entity is IEntity<int> && isNewCreated)
            {
                ((IEntity<int>)entity).IsActive = true;
            }
        }
    }
}
