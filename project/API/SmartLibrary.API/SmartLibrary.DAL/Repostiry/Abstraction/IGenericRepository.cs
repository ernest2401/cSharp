using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Storage;


namespace SmartLibrary.DAL.Repostiry.Abstraction
{
    /// <summary>
    /// Represents the interface for generic repository
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity</typeparam>
    /// <typeparam name="TKey">Type of the entity's Key</typeparam>
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        /// <summary>
        /// Returns entity by id
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        /// <returns>Entity</returns>
        TEntity Get(TKey id);

        /// <summary>
        /// Returns entity by id asynchronously
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetAsync(TKey id);

        /// <summary>
        /// Returns entity by predicate
        /// </summary>
        /// <param name="predicate">Search predicate</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Returns entities by predicate
        /// </summary>
        /// <param name="predicate">Search predicate</param>
        /// <returns>Entities</returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, bool onlyActive = true);

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">Model of entity</param>
        /// <returns>Added entity</returns>
        TEntity Create(TEntity entity);

        /// <summary>
        /// Creates a new entity asynchronously
        /// </summary>
        /// <param name="entity">Model of entity</param>
        /// <returns>Added entity</returns>
        Task<TEntity> CreateAsync(TEntity entity);

        /// <summary>
        /// Creates a new entities asynchronously
        /// </summary>
        /// <param name="entities">The list of entities</param>
        /// <returns>Added entities</returns>
        Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities);

        /// <summary>
        /// Updates entity.
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Updates entity asynchronously
        /// </summary>
        /// <param name="entity">Model of entity</param>
        /// <returns>Updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates list of entities asynchronously
        /// </summary>
        /// <param name="entities">List of entities</param>
        /// <returns>List of updated entities</returns>
        Task<List<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes entity.
        /// </summary>
        /// <param name="entity"></param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes entity asynchronously
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// Deletes all entities that satisfy criteria
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="isSoftDelete">Indicates if delete should be performed softly</param>
        Task DeleteByQueryAsync(Expression<Func<TEntity, bool>> predicate = null, bool isSoftDelete = true);

        /// <summary>
        /// Detaches all entities from change tracker
        /// </summary>
        void DetachEntries();

        /// <summary>
        /// Performs bulk insert of collection of entities
        /// </summary>
        /// <param name="entities">Collection of entities to insert</param>
        Task BulkInsertAsync(IList<TEntity> entities);

        /// <summary>
        /// Performs bulk update of collection of entities
        /// </summary>
        /// <param name="entities">Collection of entities to update</param>
        Task BulkUpdateAsync(IList<TEntity> entities);

        /// <summary>
        /// Performs bulk insert or update of collection of entities
        /// </summary>
        /// <param name="entities">Collection of entities to insert or update</param>
        /// <param name="keepIdentity">Indicates if identity should be preserved from source</param>
        Task BulkInsertOrUpdateAsync(IList<TEntity> entities, bool keepIdentity);

        /// <summary>
        /// Performs bulk update of collection of entities
        /// </summary>
        /// <param name="entities">Collection of entities to update</param>
        void BulkUpdate(IList<TEntity> entities);

        /// <summary>
        /// Performs buld delete of all elements in the set
        /// </summary>
        Task BulkDeleteAllAsync();

        /// <summary>
        /// Executes native sql query
        /// </summary>
        /// <param name="sql">Native sql query</param>
        Task ExecuteRawSql(string sql);

        /// <summary>
        /// Begins db transaction
        /// </summary>
        /// <returns>Started transaction</returns>
        IDbContextTransaction BeginTran();

        /// <summary>
        /// Commits transaction
        /// </summary>
        /// <param name="tran">Transaction to commit</param>
        void CommitTran(IDbContextTransaction tran);

        /// <summary>
        /// Rollbacks transaction
        /// </summary>
        /// <param name="tran">Transaction to rollback</param>
        void RollbackTran(IDbContextTransaction tran);
    }
}
