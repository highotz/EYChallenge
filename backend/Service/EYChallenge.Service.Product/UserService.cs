using EYChallenge.Domain.Product.Entities;
using EYChallenge.Domain.Product.Interfaces;
using EYChallenge.Domain.Product.Interfaces.Repository;
using EYChallenge.Service.Product.Common;
using EYChallenge.Utilities.SystemObjects.Interfaces;
using EYChallenge.Utilities.SystemObjects.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace EYChallenge.Service.Product
{
    internal class UserService : EntityService<User, string, MongoDbUnitOfWork>, IUserService
    {

        #region Variables

        private readonly new IUserRepository _repository;

        #endregion Variables

        public UserService(IUserRepository repository, IUnitOfWorkFactory<MongoDbUnitOfWork> unitOfWorkFactory, ILoggerFactory loggerFactory) : base(repository, unitOfWorkFactory, loggerFactory)
        {
            _repository = repository;

        }



        public async Task<User> Login(string email, string password)
        {
            return _repository.UserLogin(email, password);
        }
    }
}
