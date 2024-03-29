using System;
using System.Data;
using NHibernate;

namespace ProjectTracker.Library
{
    public class UnitOfWorkImplementor : IUnitOfWorkImplementor
    {
        private readonly IUnitOfWorkFactory _factory;
        private readonly ISession _session;
        private string _dbKey;

        public string DatabaseKey
        {
            get { return _dbKey; }
        }

        public UnitOfWorkImplementor(IUnitOfWorkFactory factory, ISession session, string dbKey)
        {
            _factory = factory;
            _session = session;
            _dbKey = dbKey;
        }

        public void Dispose()
        {
            _factory.DisposeUnitOfWork(this, _dbKey);
            _session.Dispose();
        }

        public void IncrementUsages()
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            _session.Flush();
        }

        public bool IsInActiveTransaction
        {
            get
            {
                return _session.Transaction.IsActive;
            }
        }

        public IUnitOfWorkFactory Factory
        {
            get { return _factory; }
        }

        public ISession Session
        {
            get { return _session; }
        }

        public IGenericTransaction BeginTransaction()
        {
            return new GenericTransaction(_session.BeginTransaction());
        }

        public IGenericTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new GenericTransaction(_session.BeginTransaction(isolationLevel));
        }

        public void TransactionalFlush()
        {
            TransactionalFlush(IsolationLevel.ReadCommitted);
        }

        public void TransactionalFlush(IsolationLevel isolationLevel)
        {
            // $$$$$$$$$$$$$$$$ gns: take this, when making thread safe! $$$$$$$$$$$$$$
            //IUoWTransaction tx = UnitOfWork.Current.BeginTransaction(isolationLevel);   

            IGenericTransaction tx = BeginTransaction(isolationLevel);
            try
            {
                //forces a flush of the current unit of work
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            finally
            {
                tx.Dispose();
            }
        }
    }
}