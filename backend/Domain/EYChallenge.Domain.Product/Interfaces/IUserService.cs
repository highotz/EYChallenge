using EYChallenge.Domain.Product.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Domain.Product.Interfaces
{
    public interface IUserService
    {
        Task<User> Login(string email, string password);
    }
}
