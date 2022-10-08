using Community.OData.Linq;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IEntityService<T, TKey> : ISearchableService<T, TKey>, IWritableService<T, TKey> where T : IEntity<TKey>
    {
        IQueryable<T> GetAllQueryable();
        IEnumerable<T> GetAllFromOData(IODataQueryOptions options, out int totalRecords, out int pageSize);
    }
}
