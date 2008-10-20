using System;
using NHibernate;
using NHibernate.Cfg;

namespace ProjectTracker.Library
{
    public static class UnitOfWork
    {
        private static readonly IUnitOfWorkFactory _unitOfWorkFactory = new UnitOfWorkFactory();

        public static Configuration Configuration(string dbKey)
        {
            return _unitOfWorkFactory.Configuration(dbKey);
        }

        public const string CurrentUnitOfWorkKey = "CurrentUnitOfWork.Key";

        private static IUnitOfWork CurrentUnitOfWork
        {
            get { return Local.Data[CurrentUnitOfWorkKey] as IUnitOfWork; }
            set { Local.Data[CurrentUnitOfWorkKey] = value; }
        }

        public static IUnitOfWork Current
        {
            get
            {
                var unitOfWork = CurrentUnitOfWork;
                if (unitOfWork == null)
                    throw new InvalidOperationException("You are not in a unit of work");
                return unitOfWork;
            }
        }

        public static bool IsStarted
        {
            get { return CurrentUnitOfWork != null; }
        }

        public static ISession CurrentSession(string dbKey)
        {
            return _unitOfWorkFactory.GetCurrentSession(dbKey);

        }
        internal static void SetCurrentSession(string dbKey, ISession value)
        {
            _unitOfWorkFactory.SetCurrentSession(dbKey, value);
        }

        public static IUnitOfWork Start(string dbKey)
        {
            if (CurrentUnitOfWork != null && CurrentUnitOfWork.DatabaseKey == dbKey)
                throw new InvalidOperationException("You cannot start more than one unit of work");//return CurrentUnitOfWork;
            
            var unitOfWork = _unitOfWorkFactory.Create(dbKey);
            CurrentUnitOfWork = unitOfWork;
            return unitOfWork;
        }

        public static void DisposeUnitOfWork(IUnitOfWorkImplementor unitOfWork)
        {
            CurrentUnitOfWork = null;
        }
    }
}
