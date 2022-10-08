using EYChallenge.Domain.Product.Entities;
using EYChallenge.Utilities.SystemObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Domain.Product.Interfaces.Repository
{
    public interface IUserRepository : IGenericRepository<User, string>
    {
        User UserLogin(string username, string password);
    }
}
