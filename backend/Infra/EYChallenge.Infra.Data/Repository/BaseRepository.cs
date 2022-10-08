using EYChallenge.Domain.Product.Interfaces.Repository;
using EYChallenge.Infra.Data.Settings;
using EYChallenge.Utilities.SystemObjects.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace EYChallenge.Infra.Data.Repository
{
    public abstract class BaseRepository<T> : IRepositoryBase<T> where T : MongoBaseEntity
    {
        protected readonly IMongoClient _mongoClient;
        protected readonly string _dataBaseName;
        protected readonly string _collectionName;

        public BaseRepository(IMongoClient mongoClient, IMongoDatabaseSettings settings, string collectionName)
        {
            _dataBaseName = settings.DatabaseName;

            (_mongoClient, _collectionName) = (mongoClient, collectionName);

            if (!_mongoClient.GetDatabase(_dataBaseName).ListCollectionNames().ToList().Contains(collectionName))
                _mongoClient.GetDatabase(_dataBaseName).CreateCollection(collectionName);
        }

        protected virtual IMongoCollection<T> Collection =>
            _mongoClient.GetDatabase(_dataBaseName).GetCollection<T>(_collectionName);

        public async Task InsertAsync(T obj) =>
            await Collection.InsertOneAsync(obj);

        public async Task UpdateAsync(T obj)
        {
            Expression<Func<T, string>> func = f => f.Id;
            var value = (string)obj.GetType().GetProperty(func.Body.ToString().Split(".")[1]).GetValue(obj, null);
            var filter = Builders<T>.Filter.Eq(func, value);

            if (obj != null)
                await Collection.ReplaceOneAsync(filter, obj);
        }

        public async Task DeleteAsync(string id) =>
            await Collection.DeleteOneAsync(f => f.Id == id);

        public async Task<T> GetByIdAsync(string id) =>
            await Collection.Find(f => f.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var filter = Builders<T>.Filter.Eq(x => x.Deleted, false);

            return await (await Collection.FindAsync(filter)).ToListAsync();
        }
    }
}
