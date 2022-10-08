using EYChallenge.Utilities.SystemObjects.Entities;
using System.Linq.Expressions;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface ISearchableService<T, TKey> : IService where T : IEntity<TKey>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, IEnumerable<RelationExpression<T, object>> relations);
        IEnumerable<T> GetAllPaged(int currentPage, int pageSize);
        T FindById(TKey id);
        IEnumerable<T> FindByIds(IEnumerable<TKey> ids);

    }
}
