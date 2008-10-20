using System;
using System.Data;

namespace ProjectTracker.Library
{
    public static partial class With
    {
        public static void Transaction(IsolationLevel level, Action transactional, string dbKey)
        {
            using (UnitOfWork.Start(dbKey))
            {
                // If we are already in a transaction, don't start a new one
                if (UnitOfWork.Current.IsInActiveTransaction)
                {
                    transactional();
                }
                else
                {
                    IGenericTransaction tx = UnitOfWork.Current.BeginTransaction(level);
                    try
                    {
                        transactional();
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

        public static void Transaction(Action transactional, string dbKey)
        {
            Transaction(IsolationLevel.ReadCommitted, transactional, dbKey);
        }
    }
}