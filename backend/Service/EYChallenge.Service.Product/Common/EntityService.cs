using Community.OData.Linq;
using EYChallenge.Domain.Product.Constants;
using EYChallenge.Domain.Product.ViewModels;
using EYChallenge.Utilities.SystemObjects.Entities;
using EYChallenge.Utilities.SystemObjects.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serialize.Linq.Serializers;
using System.Linq.Expressions;

namespace EYChallenge.Service.Product.Common
{
    public abstract class EntityService<T, TKey, TUnitOfWork> : IEntityService<T, TKey>
        where T : class, IEntity<TKey>
        where TUnitOfWork : class, IUnitOfWork
    {
        #region Dependencies

        protected IGenericRepository<T, TKey> _repository;
        protected readonly IUnitOfWorkFactory<TUnitOfWork> _unitOfWorkFactory;

        protected ILogger<EntityService<T, TKey, TUnitOfWork>> _logger;

        #endregion Dependencies

        #region Constructors

        public EntityService(
            IGenericRepository<T, TKey> repository,
            IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
            ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _logger = loggerFactory.CreateLogger<EntityService<T, TKey, TUnitOfWork>>();
        }

        #endregion Constructors

        #region Persistence

        public virtual void Add(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _repository.Add(entity);

                uow.Commit();
            }
        }

        public virtual async Task AddAsync(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _repository.Add(entity);

                await uow.CommitAsync();
            }
        }

        public virtual void Update(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _repository.Update(entity);

                uow.Commit();
            }
        }

        public virtual async Task UpdateAsync(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _repository.Update(entity);

                await uow.CommitAsync();
            }
        }

        public virtual void Delete(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _repository.Delete(entity);

                uow.Commit();
            }
        }

        public virtual async Task DeleteAsync(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _repository.Delete(entity);

                await uow.CommitAsync();
            }
        }

        public virtual T AddAndReturn(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {

                if (entity == null)
                    throw new ArgumentNullException("entity");

                _repository.Add(entity);

                uow.Commit();
            }

            return FindByIdCached(entity);
        }

        public virtual async Task<T> AddAndReturnAsync(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                await _repository.AddAsync(entity);

                await uow.CommitAsync();
            }
            return FindByIdCached(entity);
        }

        private T FindByIdCached(T entity)
        {
            T data = _repository.FindById(entity.Id);

            return data;
        }

        public virtual T UpdateAndReturn(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _repository.Update(entity);

                uow.Commit();

                return FindByIdCached(entity);
            }
        }

        public virtual async Task<T> UpdateAndReturnAsync(T entity)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _repository.Update(entity);

                await uow.CommitAsync();

                return FindByIdCached(entity);
            }
        }

        public virtual IEnumerable<T> AddBatch(IEnumerable<T> entities)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                _repository.AddBatch(entities);

                uow.Commit();
            }

            return entities;
        }

        public virtual async Task<IEnumerable<T>> AddBatchAsync(IEnumerable<T> entities)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                int batchSize = 500;
                int maxBatches = (int)Math.Ceiling((decimal)entities.Count() / batchSize);

                foreach (var page in Enumerable.Range(0, maxBatches))
                {
                    await _repository.AddBatchAsync(entities.Skip(page * batchSize).Take(batchSize));
                }

                await uow.CommitAsync();
            }

            return entities;
        }

        public virtual async Task<IEnumerable<T>> AddBatchNoAuditAsync(IEnumerable<T> entities)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                int batchSize = 500;
                int maxBatches = (int)Math.Ceiling((decimal)entities.Count() / batchSize);

                foreach (var page in Enumerable.Range(0, maxBatches))
                {
                    await _repository.AddBatchNoAuditAsync(entities.Skip(page * batchSize).Take(batchSize));
                }

                await uow.CommitAsync();
            }


            return entities;
        }

        public virtual IEnumerable<T> AddBatchNoAudit(IEnumerable<T> entities)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                int batchSize = 500;
                int maxBatches = (int)Math.Ceiling((decimal)entities.Count() / batchSize);

                foreach (var page in Enumerable.Range(0, maxBatches))
                {
                    _repository.AddBatchNoAudit(entities.Skip(page * batchSize).Take(batchSize));
                }

                uow.Commit();
            }

            return entities;
        }

        public virtual IEnumerable<T> UpdateBatch(IEnumerable<T> entities)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                _repository.UpdateBatch(entities);

                uow.Commit();
            }

            return entities;
        }

        public virtual IEnumerable<T> UpdateBatchNoAudit(IEnumerable<T> entities)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                _repository.UpdateBatchNoAuditAsync(entities);

                uow.Commit();
            }

            return entities;
        }

        public virtual async Task<IEnumerable<T>> UpdateBatchAsync(IEnumerable<T> entities)
        {
            if (entities.Any())

                using (IUnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    if (entities == null)
                        throw new ArgumentNullException("entities");

                    await _repository.UpdateBatchAsync(entities);

                    await uow.CommitAsync();
                }

            return entities;
        }

        public virtual async Task<IEnumerable<T>> UpdateBatchNoAuditAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            int batchSize = 500;
            int maxBatches = (int)Math.Ceiling((decimal)entities.Count() / batchSize);

            foreach (var page in Enumerable.Range(0, maxBatches))
            {
                await _repository.UpdateBatchNoAuditAsync(entities.Skip(page * batchSize).Take(batchSize));
            }

            return entities;
        }

        public virtual T AddOrUpdate(T entity, Expression<Func<T, bool>> filter)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                var searchEntity = _repository.FindOne(filter, includeExcludedRecords: true);

                if (searchEntity != null)
                {
                    entity.Id = searchEntity.Id;
                    _repository.Update(entity);
                }
                else
                {
                    _repository.Add(entity);
                }

                uow.Commit();
            }

            return entity;
        }

        public virtual async Task<T> AddOrUpdateAsync(T entity, Expression<Func<T, bool>> filter)
        {
            using (IUnitOfWork uow = _unitOfWorkFactory.Create())
            {
                var searchEntity = _repository.FindOne(filter, includeExcludedRecords: true);

                if (searchEntity != null)
                {
                    entity.Id = searchEntity.Id;
                    await _repository.UpdateAsync(entity);
                }
                else
                {
                    await _repository.AddAsync(entity);
                }

                await uow.CommitAsync();
            }

            return entity;
        }

        #endregion Persistence

        #region Search

        public virtual IEnumerable<T> GetAll()
        {
            IEnumerable<T> data = _repository.GetAll();

            return data;
        }

        public virtual T FindById(TKey id)
        {
            return _repository.FindById(id);
        }

        public virtual IEnumerable<T> FindByIds(IEnumerable<TKey> ids)
        {
            return _repository.FindByIds(ids);
        }

        public virtual IEnumerable<T> GetAllPaged(int currentPage, int pageSize)
        {
            IEnumerable<T> data = _repository.GetAllPaged(currentPage, pageSize);

            return data;
        }

        protected string BuildCacheKey(string key)
        {
            return string.Format("{0}:{1}", typeof(T).FullName, key);
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, IEnumerable<RelationExpression<T, object>> relations)
        {
            IEnumerable<T> data = _repository.Find(filter, null, relations: relations);
         
            return data;
        }

        protected string SerializeExpression(Expression exp)
        {
            string serializedExpression = string.Empty;

            if (exp != null)
            {
                var serializer = new ExpressionSerializer(new Serialize.Linq.Serializers.JsonSerializer());
                serializedExpression = serializer.SerializeText(exp);
            }

            return serializedExpression;
        }

        protected string SerializeOData(IODataQueryOptions options)
        {
            return JsonConvert.SerializeObject(options);
        }

        public virtual IQueryable<T> GetAllQueryable()
        {
            return _repository.GetAllQueryable();
        }

        public virtual IEnumerable<T> GetAllFromOData(IODataQueryOptions options, out int totalRecords, out int pageSize)
        {
            IQueryable<T> query = _repository.GetAllFromOData(options);

            CustomODataQueryOptionsViewModel countOptions = new CustomODataQueryOptionsViewModel()
            {
                Filters = options.Filters,
                Top = null
            };

            totalRecords = _repository.CountOData(countOptions);

            pageSize = !string.IsNullOrEmpty(options.Top) ? Int32.Parse(options.Top) : PagingSettings.PAGE_SIZE;
            IEnumerable<T> data;

            data = query.ToList();

            return data;
        }

        public virtual async Task UpdateFields(T entity, params Expression<Func<T, object>>[] fields)
        {
            await _repository.UpdateFields(entity, fields);
        }

        public async Task UpdateFieldsBatchNoAuditAsync(IEnumerable<T> entities, params Expression<Func<T, object>>[] fields)
        {
            int batchSize = 1000;
            int batches = (int)Math.Ceiling((decimal)entities.Count() / (decimal)batchSize);

            foreach (int batch in Enumerable.Range(0, batches))
            {
                await _repository.UpdateFieldsBatchNoAuditAsync(entities.Skip(batch * batchSize).Take(batchSize), fields);
            }
        }

        public void UpdateFieldsBatchNoAudit(IEnumerable<T> entities, params Expression<Func<T, object>>[] fields)
        {
            int batchSize = 1000;
            int batches = (int)Math.Ceiling((decimal)entities.Count() / (decimal)batchSize);

            foreach (int batch in Enumerable.Range(0, batches))
            {
                _repository.UpdateFieldsBatchNoAudit(entities.Skip(batch * batchSize).Take(batchSize), fields);
            }
        }

        #endregion Search

    }
}
