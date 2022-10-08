using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IUnitOfWorkFactory<TUnitOfWork> where TUnitOfWork : IUnitOfWork
    {

        IUnitOfWork Create(IsolationLevel isolationLevel);
        IUnitOfWork Create();
    }
}
