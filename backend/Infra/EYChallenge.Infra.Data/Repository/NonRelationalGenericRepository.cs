using Community.OData.Linq;
using EYChallenge.Infra.Data.Settings;
using EYChallenge.Utilities.SystemObjects.Entities;
using EYChallenge.Utilities.SystemObjects.Enum;
using EYChallenge.Utilities.SystemObjects.Interfaces;
using EYChallenge.Utilities.SystemObjects.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EYChallenge.Infra.Data.Repository
{
    public abstract class NonRelationalGenericRepository<T, TKey> : IGenericRepository<T, TKey>
        where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly IMongoDatabase _mongoDatabase;
        protected readonly IAuditingService _auditService;

        public virtual IMongoCollection<T> Collection { get { return _collection; } }
        public virtual string CollectionName { get; }
        public MongoCollectionSettings CollectionSettings { get; protected set; }
        protected virtual Action<T> OnBeforeSave { get => (entity) => { }; }
        protected virtual Func<IQueryable<T>, IQueryable<T>> OnBeforeQuery { get => null; }
        public object Context { get => _collection; }
        public IAuditingService AuditService => _auditService;

        protected NonRelationalGenericRepository(IMongoClient client, IMongoDatabaseSettings settings, IAuditingServiceFactory auditServiceFactory)
        {
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<T>(CollectionName, this.CollectionSettings);
            _mongoDatabase = database;
            _auditService = auditServiceFactory.Create(ServiceStrategy.NonRelational);
        }

        public virtual void Add(T entity)
        {
            _auditService.FillAuditingProperties<T, TKey>(entity, AuditActionType.Create, () => this.CollectionName);
            OnBeforeSave(entity);

            _collection.InsertOne(entity);
            _auditService.QueueAutitingTrail(AuditActionType.Create, () => entity.Id, null, entity);
        }

        public virtual async Task AddAsync(T entity)
        {
            _auditService.FillAuditingProperties<T, TKey>(entity, AuditActionType.Create, () => this.CollectionName);
            OnBeforeSave(entity);

            await _collection.InsertOneAsync(entity);
            await _auditService.QueueAutitingTrailAsync(AuditActionType.Create, () => entity.Id, null, entity);
        }

        public virtual void Delete(T entity)
        {
            entity.Deleted = true;
            Update(entity);
        }

        public virtual void DeletePermanently(T entity)
        {
            T oldEntity = _collection.Find(c => c.Id.Equals(entity.Id)).FirstOrDefault();

            _collection.DeleteOne(c => c.Id.Equals(entity.Id));
            _auditService.QueueAutitingTrail(AuditActionType.Delete, () => entity.Id, oldEntity, entity);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            entity = FindById(entity.Id);
            entity.Deleted = true;
            await UpdateAsync(entity);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> where, IEnumerable<Sort<T>> orderBy = null, bool includeExcludedRecords = false,
            IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null)
        {
            IEnumerable<T> results = RunQuery(
                filter: where,
                orderBy: orderBy,
                includeDeletedRecords: includeExcludedRecords,
                beforeQuery: beforeQuery);

            return results;
        }

        public virtual IEnumerable<T> FindPaged(Expression<Func<T, bool>> where, int currentPage, int pageSize, IEnumerable<Sort<T>> orderBy = null,
            IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null)
        {
            IEnumerable<T> results = RunQuery(
                filter: where,
                orderBy: orderBy,
                page: currentPage,
                pageSize: pageSize,
                beforeQuery: beforeQuery);

            return results;
        }

        public virtual IEnumerable<T> GetAll(IEnumerable<Sort<T>> orderBy = null, IEnumerable<RelationExpression<T, object>> relations = null,
            Action<IQueryable<T>> beforeQuery = null)
        {
            IEnumerable<T> results = RunQuery(orderBy: orderBy, beforeQuery: beforeQuery);
            return results;
        }

        public virtual IEnumerable<T> GetAllPaged(int currentPage, int pageSize, IEnumerable<Sort<T>> orderBy = null,
            IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null)
        {
            IEnumerable<T> results = RunQuery(
                page: currentPage,
                pageSize: pageSize,
                orderBy: orderBy,
                beforeQuery: beforeQuery);

            return results;
        }

        public virtual void Update(T entity)
        {
            T oldEntity = _collection.Find(c => c.Id.Equals(entity.Id)).FirstOrDefault();

            _auditService.FillAuditingProperties<T, TKey>(entity, AuditActionType.Update, () => this.CollectionName);
            OnBeforeSave(entity);

            _collection.ReplaceOne<T>(c => c.Id.Equals(entity.Id), entity);
            _auditService.QueueAutitingTrail(AuditActionType.Update, () => entity.Id, oldEntity, entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            T oldEntity = _collection.Find(c => c.Id.Equals(entity.Id)).FirstOrDefault();

            _auditService.FillAuditingProperties<T, TKey>(entity, AuditActionType.Update, () => this.CollectionName);
            OnBeforeSave(entity);

            _collection.ReplaceOne(c => c.Id.Equals(entity.Id), entity);
            await _auditService.QueueAutitingTrailAsync(AuditActionType.Update, () => entity.Id, oldEntity, entity);
        }

        protected virtual IEnumerable<T> RunQuery(Expression<Func<T, bool>> filter = null, IEnumerable<Sort<T>> orderBy = null, int? page = null,
            int pageSize = 10, bool includeDeletedRecords = false, Action<IQueryable<T>> beforeQuery = null)
        {
            return BuildQuery(filter, orderBy, page, pageSize, includeDeletedRecords, beforeQuery);
        }

        private IQueryable<T> BuildQuery(Expression<Func<T, bool>> filter = null, IEnumerable<Sort<T>> orderBy = null, int? page = null,
            int? pageSize = null, bool? includeDeletedRecords = null, Action<IQueryable<T>> beforeQuery = null)
        {
            IQueryable<T> query = _collection.AsQueryable<T>();

            if (!includeDeletedRecords.GetValueOrDefault())
            {
                query = query.Where(c => !c.Deleted);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (page.HasValue)
            {
                int take = (pageSize ?? 20);
                int skip = page.Value <= 1 ? 0 : (page.Value - 1) * take;
                query = query.Skip(skip).Take(take);
            }

            if (orderBy != null)
            {
                foreach (var order in orderBy)
                {
                    query = (IQueryable<T>)(order.Direction == Utilities.SystemObjects.Enum.SortDirection.Asc ? query.OrderBy(order.Action) : query.OrderByDescending(order.Action));
                }
            }

            beforeQuery?.Invoke(query);

            if (OnBeforeQuery != null)
                query = OnBeforeQuery(query);

            return query;
        }

        public virtual T FindById(TKey value)
        {
            return _collection.Find<T>(c => c.Id.Equals(value)).FirstOrDefault();
        }

        public virtual void Delete(TKey id)
        {
            T entity = FindById(id);
            OnBeforeSave(entity);

            entity.Deleted = true;
            Update(entity);
        }

        public long Count(Expression<Func<T, bool>> filter = null)
        {
            return OnBeforeQuery != null ? OnBeforeQuery(_collection.AsQueryable()).Where(filter).Count() : _collection.AsQueryable().Where(c => !c.Deleted).Where(filter).Count();
        }

        public virtual T FindOne(Expression<Func<T, bool>> where, IEnumerable<Sort<T>> orderBy = null, bool includeExcludedRecords = false,
            IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null)
        {
            return BuildQuery(where, orderBy, null, null, includeExcludedRecords, beforeQuery).FirstOrDefault();
        }

        public virtual IQueryable<T> GetAllQueryable(IEnumerable<Sort<T>> orderBy = null, IEnumerable<RelationExpression<T, object>> relations = null,
            Action<IQueryable<T>> beforeQuery = null)
        {
            return BuildQuery(filter: null);
        }

        public async Task AddBatchAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _auditService.FillAuditingProperties<T, TKey>(entity, AuditActionType.Create, () => this.CollectionName);
                OnBeforeSave(entity);
            }

            if (entities.Any())
                await _collection.InsertManyAsync(entities, new InsertManyOptions { IsOrdered = false });

            foreach (var entity in entities)
            {
                _auditService.QueueAutitingTrail(AuditActionType.Create, () => entity.Id, null, entity);
            }
        }

        public async Task AddBatchNoAuditAsync(IEnumerable<T> entities)
        {
            if (entities.Any())
                await _collection.InsertManyAsync(entities, new InsertManyOptions { IsOrdered = false });
        }

        public void AddBatch(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _auditService.FillAuditingProperties<T, TKey>(entity, AuditActionType.Create, () => this.CollectionName);
                OnBeforeSave(entity);
            }

            _collection.InsertMany(entities, new InsertManyOptions { IsOrdered = false });

            foreach (var entity in entities)
            {
                _auditService.QueueAutitingTrail(AuditActionType.Create, () => entity.Id, null, entity);
            }
        }

        public async Task UpdateBatchAsync(IEnumerable<T> entities)
        {
            var models = new WriteModel<T>[entities.Count()];

            for (int index = 0; index < entities.Count(); index++)
            {
                var entity = entities.ElementAt(index);

                _auditService.FillAuditingProperties<T, TKey>(entity, AuditActionType.Update, () => this.CollectionName);
                OnBeforeSave(entity);

                models[index] = new ReplaceOneModel<T>(Builders<T>.Filter.Where(c => c.Id.Equals(entity.Id)), entity);
            }

            if (entities.Any())
                await _collection.BulkWriteAsync(models, new BulkWriteOptions { IsOrdered = false });

            foreach (var entity in entities)
            {
                _auditService.QueueAutitingTrail(AuditActionType.Update, () => entity.Id, null, entity);
            }
        }

        public async Task UpdateBatchNoAuditAsync(IEnumerable<T> entities)
        {
            var models = new List<WriteModel<T>>();

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    models.Add(new ReplaceOneModel<T>(Builders<T>.Filter.Where(c => c.Id.Equals(entity.Id)), entity));
                }

                await _collection.BulkWriteAsync(models, new BulkWriteOptions { IsOrdered = false });
            }
        }

        public void UpdateBatchNoAudit(IEnumerable<T> entities)
        {
            var models = new List<WriteModel<T>>();

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    models.Add(new ReplaceOneModel<T>(Builders<T>.Filter.Where(c => c.Id.Equals(entity.Id)), entity));
                }

                _collection.BulkWrite(models, new BulkWriteOptions { IsOrdered = false });
            }
        }

        public void UpdateBatch(IEnumerable<T> entities)
        {
            var models = new WriteModel<T>[entities.Count()];

            for (int index = 0; index < entities.Count(); index++)
            {
                var entity = entities.ElementAt(index);

                _auditService.FillAuditingProperties<T, TKey>(entity, AuditActionType.Update, () => this.CollectionName);
                OnBeforeSave(entity);

                models[index] = new ReplaceOneModel<T>(Builders<T>.Filter.Where(c => c.Id.Equals(entity.Id)), entity);
            }

            _collection.BulkWrite(models, new BulkWriteOptions { IsOrdered = false });

            foreach (var entity in entities)
            {
                _auditService.QueueAutitingTrail(AuditActionType.Update, () => entity.Id, null, entity);
            }
        }

        public bool IsValidObjectId(IEnumerable<string> objectId)
        {
            if (objectId == null)
                return false;

            ObjectId id;

            foreach (var obj in objectId)
            {
                if (!ObjectId.TryParse(obj, out id))
                    return false;
            }

            return true;
        }

        public virtual int CountOData(IODataQueryOptions options)
        {
            var query = OnBeforeQuery != null ?
                OnBeforeQuery(_collection.AsQueryable<T>().Where(w => !w.Deleted)) : _collection.AsQueryable<T>().Where(w => !w.Deleted);

            return query.OData(c =>
            {
                c.DefaultQuerySettings.MaxTop = 50;
                c.QuerySettings.PageSize = null;
                c.ValidationSettings.MaxNodeCount = 1000;
                c.ValidationSettings.MaxAnyAllExpressionDepth = 3;
            }).ApplyQueryOptionsWithoutSelectExpand(options).Select(c => c).Count();
        }

        public virtual IQueryable<T> GetAllFromOData(IODataQueryOptions options)
        {
            var query = OnBeforeQuery != null ?
                OnBeforeQuery(_collection.AsQueryable<T>().Where(w => !w.Deleted)) : _collection.AsQueryable<T>().Where(w => !w.Deleted);

            return query.OData(c =>
            {
                c.ValidationSettings.MaxNodeCount = 1000;
                c.ValidationSettings.MaxAnyAllExpressionDepth = 3;
            }).ApplyQueryOptionsWithoutSelectExpand(options);
        }

        public virtual async Task UpdateFields(T entity, params Expression<Func<T, object>>[] fields)
        {
            var filter = Builders<T>.Filter.Eq(c => c.Id, entity.Id);
            UpdateDefinition<T> updates = null;

            foreach (var item in fields)
            {
                updates = updates == null ? Builders<T>.Update.Set(item, entity.GetValue(item)) : updates.Set(item, entity.GetValue(item));
            }

            await _collection.UpdateOneAsync(filter, updates);
        }

        public async Task UpdateFieldsBatchNoAuditAsync(IEnumerable<T> entities, params Expression<Func<T, object>>[] fields)
        {
            var models = new List<WriteModel<T>>();

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    UpdateDefinition<T> updates = null;

                    foreach (var item in fields)
                    {
                        updates = updates == null ? Builders<T>.Update.Set(item, entity.GetValue(item)) : updates.Set(item, entity.GetValue(item));
                    }

                    models.Add(new UpdateOneModel<T>(Builders<T>.Filter.Where(c => c.Id.Equals(entity.Id)), updates));
                }

                await _collection.BulkWriteAsync(models, new BulkWriteOptions { IsOrdered = false });
            }
        }

        public void UpdateFieldsBatchNoAudit(IEnumerable<T> entities, params Expression<Func<T, object>>[] fields)
        {
            var models = new List<WriteModel<T>>();

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    UpdateDefinition<T> updates = null;

                    foreach (var item in fields)
                    {
                        updates = updates == null ? Builders<T>.Update.Set(item, entity.GetValue(item)) : updates.Set(item, entity.GetValue(item));
                    }

                    models.Add(new UpdateOneModel<T>(Builders<T>.Filter.Where(c => c.Id.Equals(entity.Id)), updates));
                }

                _collection.BulkWrite(models, new BulkWriteOptions { IsOrdered = false });
            }
        }

        public IEnumerable<T> AddBatchNoAudit(IEnumerable<T> entities)
        {
            if (entities.Any())
                _collection.InsertMany(entities, new InsertManyOptions { IsOrdered = false });

            return entities;
        }

        public void DeletePermanentlyBatchNoAudit(IEnumerable<T> entities)
        {
            List<TKey> Ids = entities.Select(e => e.Id).Distinct().ToList();

            int total = Ids.Count;
            int pageSize = 1000;
            int totalPages = (int)Math.Ceiling((decimal)total / (decimal)pageSize);

            foreach (var page in Enumerable.Range(0, totalPages))
            {
                var pageItems = Ids.Skip(page * pageSize).Take(pageSize);
                _collection.DeleteMany(d => pageItems.Contains(d.Id));
            }
        }

        public IEnumerable<T> FindByIds(IEnumerable<TKey> ids)
        {
            return _collection.AsQueryable().Where(c => ids.Contains(c.Id));
        }

    }
}
