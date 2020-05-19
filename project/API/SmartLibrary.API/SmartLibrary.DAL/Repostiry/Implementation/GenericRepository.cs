using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using EFCore.BulkExtensions;

using SmartLibrary.DAL.Repostiry.Abstraction;
using SmartLibrary.DAL.Entities.Abstraction;
using SmartLibrary.DAL.Extensions;

namespace SmartLibrary.DAL.Repostiry.Implementation
{
    /// <summary>
    /// Provides implementation of ICrudRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">type of the entity</typeparam>
    /// <typeparam name="TKey">type of the entity's Key</typeparam>
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary> 
        /// Returns database context for working with new entities 
        /// </summary>
        protected readonly DbContext context;

        /// <summary>
        /// Field stores set of entities.
        /// </summary>
        private readonly DbSet<TEntity> entityDbSet;

        /// <summary>
        /// Creates instance of repository
        /// </summary>
        public GenericRepository()
        {

        }

        /// <summary>
        /// Creates instance of repository
        /// </summary>
        /// <param name="context">Database context</param>
        public GenericRepository(DbContext context)
        {
            this.context = context;
            entityDbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Returns entity by id
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        /// <returns>Entity</returns>
        public TEntity Get(TKey id)
        {
            return entityDbSet.Find(id);
        }

        /// <summary>
        /// Returns entity by id asynchronously
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        /// <returns>Entity</returns>
        public virtual async Task<TEntity> GetAsync(TKey id)
        {
            return await entityDbSet.FindAsync(id);
        }

        /// <summary>
        /// Returns entity by predicate
        /// </summary>
        /// <param name="predicate">Search predicate</param>
        /// <returns>Entity</returns>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await entityDbSet.FirstOrDefaultAsync(predicate ?? (x => true));
        }

        /// <summary>
        /// Returns result IQueryable that satisfies expression. 
        /// </summary>
        /// <param name="predicate">Predicate method</param>
        /// <param name="onlyActive">Only active</param>
        /// <returns>Collection of model of entity</returns>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, bool onlyActive = true)
        {
            return entityDbSet.WhereIf(onlyActive, x => x.IsActive).Where(predicate ?? (x => true));
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">Model of entity</param>
        /// <returns>Added entity</returns>
        public TEntity Create(TEntity entity)
        {
            var created = entityDbSet.Add(entity).Entity;
            context.SaveChanges();
            return created;
        }

        /// <summary>
        /// Creates a new entity asynchronously
        /// </summary>
        /// <param name="entity">Model of entity</param>
        /// <returns>Added entity</returns>
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var creationResult = await entityDbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return creationResult.Entity;
        }

        /// <summary>
        /// Creates a list of new entities asynchronously
        /// </summary>
        /// <param name="entities">The list of entities</param>
        /// <returns>Added entities</returns>
        public async Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities)
        {
            entityDbSet.AddRange(entities.AsEnumerable());
            await context.SaveChangesAsync();
            return entities;
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        /// <param name="entity">Model of entity</param>
        public void Update(TEntity entity)
        {
            entityDbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;

            if (entity is IHasCreationDate)
            {
                context.Entry(entity).Property(nameof(IHasCreationDate.CreationDate)).IsModified = false;
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Updates entity asynchronously
        /// </summary>
        /// <param name="entity">Model of entity</param>
        /// <returns>Updated entity</returns>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var updated = entityDbSet.Attach(entity).Entity;
            context.Entry(entity).State = EntityState.Modified;

            if (entity is IHasCreationDate)
            {
                context.Entry(entity).Property(nameof(IHasCreationDate.CreationDate)).IsModified = false;
            }

            await context.SaveChangesAsync();
            return updated;
        }

        /// <summary>
        /// Updates collection of entities asynchronously
        /// </summary>
        /// <param name="entities">Collection of entites</param>
        /// <returns>Collection of updated entities</returns>
        public async Task<List<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities != null && entities.Any())
            {
                var updated = new List<TEntity>();
                foreach (var e in entities)
                {
                    updated.Add(entityDbSet.Attach(e).Entity);
                    context.Entry(e).State = EntityState.Modified;

                    if (e is IHasCreationDate)
                    {
                        context.Entry(e).Property(nameof(IHasCreationDate.CreationDate)).IsModified = false;
                    }
                }
                await context.SaveChangesAsync();

                return updated;
            }

            return entities?.ToList();
        }

        /// <summary>
        /// Deletes entity.
        /// </summary>
        /// <param name="entity">Model of entity</param>
        public async Task DeleteAsync(TEntity entity)
        {
            entity.IsActive = false;
            await UpdateAsync(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletes entity asynchronously
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        public async Task DeleteAsync(TKey id)
        {
            var deleted = await GetAsync(id);
            deleted.IsActive = false;
            await UpdateAsync(deleted);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all entities that satisfy criteria
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="isSoftDelete">Indicates if delete should be performed softly</param>
        public async Task DeleteByQueryAsync(Expression<Func<TEntity, bool>> predicate = null, bool isSoftDelete = true)
        {
            var deleted = await Get(predicate, false).ToListAsync();

            if (isSoftDelete)
            {
                deleted.ForEach(x => x.IsActive = false);
                await UpdateRangeAsync(deleted);
            }
            else
            {
                entityDbSet.RemoveRange(deleted);
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Detaches all entities from change tracker
        /// </summary>
        public void DetachEntries()
        {
            context.ChangeTracker.Entries().ToList().ForEach(x => x.State = EntityState.Detached);
        }

        /// <summary>
        /// Performs bulk insert of collection of entities
        /// </summary>
        /// <param name="entities">Collection of entities to insert</param>
        public async Task BulkInsertAsync(IList<TEntity> entities)
        {
            await context.BulkInsertAsync(entities, new BulkConfig() { BulkCopyTimeout = 0 });
        }

        /// <summary>
        /// Performs bulk update of collection of entities
        /// </summary>
        /// <param name="entities">Collection of entities to update</param>
        public async Task BulkUpdateAsync(IList<TEntity> entities)
        {
            await context.BulkUpdateAsync(entities);
        }

        /// <summary>
        /// Performs bulk update of collection of entities
        /// </summary>
        /// <param name="entities">Collection of entities to update</param>
        public void BulkUpdate(IList<TEntity> entities)
        {
            context.BulkUpdate(entities, new BulkConfig() { BulkCopyTimeout = 0 });
        }

        /// <summary>
        /// Performs bulk insert or update of collection of entities
        /// </summary>
        /// <param name="entities">Collection of entities to insert or update</param>
        /// <param name="keepIdentity">Indicates if identity should be preserved from source</param>
        public async Task BulkInsertOrUpdateAsync(IList<TEntity> entities, bool keepIdentity)
        {
            var config = new BulkConfig();

            if (keepIdentity)
            {
                config.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity;
            }

            await context.BulkInsertOrUpdateAsync(entities, config);
        }

        /// <summary>
        /// Performs buld delete of all elements in the set
        /// </summary>
        public async Task BulkDeleteAllAsync()
        {
            await context.BulkDeleteAsync(await context.Set<TEntity>().ToListAsync());
        }

        /// <summary>
        /// Executes native sql query
        /// </summary>
        /// <param name="sql">Native sql query</param>
        [Obsolete]
        public async Task ExecuteRawSql(string sql)
        {
            await context.Database.ExecuteSqlCommandAsync(sql);
        }

        /// <summary>
        /// Begins db transaction
        /// </summary>
        /// <returns>Started transaction</returns>
        public IDbContextTransaction BeginTran()
        {
            return context.Database.BeginTransaction();
        }

        /// <summary>
        /// Commits transaction
        /// </summary>
        /// <param name="tran">Transaction to commit</param>
        public void CommitTran(IDbContextTransaction tran)
        {
            tran.Commit();
        }

        /// <summary>
        /// Rollbacks transaction
        /// </summary>
        /// <param name="tran">Transaction to rollback</param>
        public void RollbackTran(IDbContextTransaction tran)
        {
            tran.Rollback();
        }
    }
}
