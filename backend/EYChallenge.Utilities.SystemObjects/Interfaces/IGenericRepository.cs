using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IGenericRepository<T, TKey> :
    ISearchableRepository<T, TKey>,
    IWritableRepository<T, TKey> where T : class, IEntity<TKey>
    {
        object Context { get; }
        IAuditingService AuditService { get; }

        bool IsValidObjectId(IEnumerable<string> objectId);
    }
}
