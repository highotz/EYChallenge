using Community.OData.Linq;
using EYChallenge.Utilities.SystemObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface ISearchableRepository<T, TKey> where T : IEntity<TKey>
    {
        T FindById(TKey value);
        IEnumerable<T> FindByIds(IEnumerable<TKey> ids);
        long Count(Expression<Func<T, bool>> filter = null);
        int CountOData(IODataQueryOptions options);
        IEnumerable<T> GetAll(IEnumerable<Sort<T>> orderBy = null, IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null);
        IQueryable<T> GetAllQueryable(IEnumerable<Sort<T>> orderBy = null, IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null);
        IQueryable<T> GetAllFromOData(IODataQueryOptions options);
        IEnumerable<T> GetAllPaged(int currentPage, int pageSize, IEnumerable<Sort<T>> orderBy = null, IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null);
        IEnumerable<T> FindPaged(Expression<Func<T, bool>> where, int currentPage, int pageSize, IEnumerable<Sort<T>> orderBy = null, IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null);
        IEnumerable<T> Find(Expression<Func<T, bool>> where, IEnumerable<Sort<T>> orderBy = null, bool includeExcludedRecords = false, IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null);
        T FindOne(Expression<Func<T, bool>> where, IEnumerable<Sort<T>> orderBy = null, bool includeExcludedRecords = false, IEnumerable<RelationExpression<T, object>> relations = null, Action<IQueryable<T>> beforeQuery = null);
    }
}
