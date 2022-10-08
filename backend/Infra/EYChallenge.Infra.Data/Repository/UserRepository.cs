using EYChallenge.Domain.Product.Entities;
using EYChallenge.Domain.Product.Interfaces.Repository;
using EYChallenge.Infra.Data.Settings;
using EYChallenge.Utilities.SystemObjects.Interfaces;
using MongoDB.Driver;

namespace EYChallenge.Infra.Data.Repository
{
    public class UserRepository : NonRelationalGenericRepository<User, string>, IUserRepository
    {
        public override string CollectionName { get => MongoCollections.USERS; }
        public UserRepository(IMongoClient client, IMongoDatabaseSettings settings, IAuditingServiceFactory auditService) : base(client, settings, auditService)
        {
        }
        
        public  User UserLogin(string email, string password)
        {
            return  base.FindOne(u => u.Email == email && u.Password == password);
        }
    }
}
