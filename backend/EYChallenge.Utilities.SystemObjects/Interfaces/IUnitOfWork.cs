using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();

    }
}
