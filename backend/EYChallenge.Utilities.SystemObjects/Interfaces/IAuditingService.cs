using EYChallenge.Utilities.SystemObjects.Entities;
using EYChallenge.Utilities.SystemObjects.Enum;
using System.Linq.Expressions;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IAuditingService
    {
        Task QueueAutitingTrailAsync(AuditActionType actionType, Expression<Func<object>> keyFieldExpression, object valueBefore, object valueAfter);
        void QueueAutitingTrail(AuditActionType actionType, Expression<Func<object>> keyFieldExpression, object valueBefore, object valueAfter);
        void FillAuditingProperties<TEntity, TKey>(TEntity entity, AuditActionType type, Func<string> tableNameResolver = null) where TEntity : class, IEntity<TKey>;
        void Save();
        List<AuditChangeDelta> Compare(object valueBefore, object valueAfter);
    }
}
