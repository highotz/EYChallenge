using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IWritableRepository<T, in TKey> where T : IEntity<TKey>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(TKey id);
        void DeletePermanently(T entity);
        void DeletePermanentlyBatchNoAudit(IEnumerable<T> entities);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task AddBatchAsync(IEnumerable<T> entities);
        Task AddBatchNoAuditAsync(IEnumerable<T> entities);
        IEnumerable<T> AddBatchNoAudit(IEnumerable<T> entities);
        Task UpdateBatchAsync(IEnumerable<T> entities);
        void AddBatch(IEnumerable<T> entities);
        void UpdateBatch(IEnumerable<T> entities);
        Task UpdateBatchNoAuditAsync(IEnumerable<T> entities);
        void UpdateBatchNoAudit(IEnumerable<T> entities);
        Task UpdateFields(T entity, params Expression<Func<T, object>>[] fields);
        Task UpdateFieldsBatchNoAuditAsync(IEnumerable<T> entities, params Expression<Func<T, object>>[] fields);
        void UpdateFieldsBatchNoAudit(IEnumerable<T> entities, params Expression<Func<T, object>>[] fields);
    }
}
