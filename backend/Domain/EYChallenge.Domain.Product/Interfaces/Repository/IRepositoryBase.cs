using EYChallenge.Utilities.SystemObjects.Entities;

namespace EYChallenge.Domain.Product.Interfaces.Repository
{
    public interface IRepositoryBase<T> where T : MongoBaseEntity
    {
        Task InsertAsync(T obj);

        Task UpdateAsync(T obj);

        Task DeleteAsync(string id);
    }
}
