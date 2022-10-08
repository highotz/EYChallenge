using System.Linq.Expressions;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IWritableService<T, TKey> : IService where T : IEntity<TKey>
    {
        void Add(T entity);
        Task AddAsync(T entity);
        T AddAndReturn(T entity);
        Task<T> AddAndReturnAsync(T entity);
        T AddOrUpdate(T entity, Expression<Func<T, bool>> filter);
        IEnumerable<T> AddBatch(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddBatchAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddBatchNoAuditAsync(IEnumerable<T> entities);
        IEnumerable<T> AddBatchNoAudit(IEnumerable<T> entities);
        IEnumerable<T> UpdateBatch(IEnumerable<T> entities);
        Task<IEnumerable<T>> UpdateBatchAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> UpdateBatchNoAuditAsync(IEnumerable<T> entities);
        Task UpdateFieldsBatchNoAuditAsync(IEnumerable<T> entities, params Expression<Func<T, object>>[] fields);
        Task<T> AddOrUpdateAsync(T entity, Expression<Func<T, bool>> filter);
        void Update(T entity);
        Task UpdateAsync(T entity);
        T UpdateAndReturn(T entity);
        Task<T> UpdateAndReturnAsync(T entity);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        Task UpdateFields(T entity, params Expression<Func<T, object>>[] fields);
    }
}
