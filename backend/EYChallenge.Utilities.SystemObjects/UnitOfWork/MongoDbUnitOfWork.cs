using EYChallenge.Utilities.SystemObjects.Enum;
using EYChallenge.Utilities.SystemObjects.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Utilities.SystemObjects.UnitOfWork
{
    public class MongoDbUnitOfWork : IUnitOfWork
    {

        private IClientSessionHandle _transaction;
        private readonly IAuditingService _auditingService;
        private bool _disposed = false;

        public MongoDbUnitOfWork(IAuditingServiceFactory auditingServiceFactory)
        {
            _auditingService = auditingServiceFactory.Create(ServiceStrategy.NonRelational);

        }
        public void Commit()
        {
            _auditingService.Save();
        }

        public async Task CommitAsync()
        {
            _auditingService.Save();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
                _disposed = true;
            }
        }
        public virtual void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MongoDbUnitOfWork()
        {
            Dispose(false);
        }
    }
}
