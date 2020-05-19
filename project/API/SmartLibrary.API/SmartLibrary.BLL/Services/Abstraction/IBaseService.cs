using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.BLL.Services.Abstraction
{
    /// <summary>
    /// Represents the interface for base generic service
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity</typeparam>
    /// <typeparam name="TDto">Type of the dto</typeparam>
    public interface IBaseService<TEntity, TDto> where TEntity : class
    {
        /// <summary>
        /// Returns dto by id
        /// </summary>
        /// <param name="id">Identifier of dto</param>
        /// <returns>Instance of <see cref="SingleResult<TDto>"/> model</returns>
        Task<SingleResult<TDto>> GetAsync(int id);

        /// <summary>
        /// Returns dto by predicate
        /// </summary>
        /// <returns>Instance of <see cref="SingleResult<TDto>"/> model</returns>
        Task<SingleResult<TDto>> GetAsync(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Returns list of dto
        /// </summary>
        /// <returns>Instance of <see cref="CollectionResult{TDto}"/> model</returns>
        Task<CollectionResult<TDto>> SearchAsync(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Creates dto
        /// </summary>
        /// <param name="model">Instance of <see cref="Dto"/> model</param>
        /// <returns>Instance of <see cref="SingleResult<Dto>"/> model</returns>
        Task<SingleResult<TDto>> CreateAsync(TDto model);

        /// <summary>
        /// Creates list of dto
        /// </summary>
        /// <param name="items">The collection of <see cref="TDto"/> items</param>
        /// <returns>Instance of <see cref="CollectionResult{TDto}"/> model</returns>
        Task<CollectionResult<TDto>> CreateRangeAsync(List<TDto> items);

        /// <summary>
        /// Deletes by id
        /// </summary>
        /// <param name="id">Identifier of dto</param>
        /// <returns>Instance of <see cref="OperationResult"/> model</returns>
        Task<OperationResult> DeleteAsync(int id);

        /// <summary>
        /// Updates dto
        /// </summary>
        /// <param name="model">Instance of <see cref="Dto"/> model</param>
        /// <returns>Instance of <see cref="SingleResult<Dto>"/> model</returns>
        Task<SingleResult<TDto>> UpdateAsync(TDto model);

        /// <summary>
        /// Updates list of dto
        /// </summary>
        /// <param name="items">The collection of <see cref="TDto"/> items</param>
        /// <returns>Instance of <see cref="CollectionResult{TDto}"/> model</returns>
        Task<CollectionResult<TDto>> UpdateRangeAsync(List<TDto> items);
    }
}
