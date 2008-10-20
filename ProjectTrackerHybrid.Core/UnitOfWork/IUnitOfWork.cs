using System;
using System.Data;

namespace ProjectTracker.Library
{
    public interface IUnitOfWork : IDisposable
    {
        void Flush();
        bool IsInActiveTransaction { get; }

        string DatabaseKey { get;}
        IGenericTransaction BeginTransaction();
        IGenericTransaction BeginTransaction(IsolationLevel isolationLevel);
        void TransactionalFlush();
        void TransactionalFlush(IsolationLevel isolationLevel);
    }
}