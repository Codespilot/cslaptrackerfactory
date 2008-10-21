using System;
using NHibernate;
using NHibernate.Cfg;
using ProjectTracker.Library.Commons;
using StructureMap;

namespace ProjectTracker.Library
{
    public static class UnitOfWork
    {
        private static IUnitOfWork globalNonThreadSafeUnitOfwork;

        public const string CurrentUnitOfWorkKey = "CurrentUnitOfWork.Key";
        public const string CurrentLongConversationIdKey = "CurrentLongConversationId.Key";

        #region Long Conversation - Note: I might implement this later

        ///// <summary>
        ///// Signals the start of an application/user transaction that spans multiple page requests
        ///// </summary>
        ///// <remarks>
        ///// Used in conjunction with <see cref="UnitOfWorkApplication"/>, will ensure that the current UoW
        ///// (see <see cref="Current"/>) is kept intact across multiple page requests. 
        ///// <para>
        ///// Note: This method does not start a physical database transaction.
        ///// </para>
        ///// </remarks>
        //public static void StartLongConversation()
        //{
        //    if (InLongConversation)
        //        throw new InvalidOperationException("You are already in a long conversation");

        //    Local.Data[CurrentLongConversationIdKey] = Guid.NewGuid();
        //}

        ///// <summary>
        ///// Signals the end of the current application/user transaction <seealso cref="StartLongConversation"/>
        ///// </summary>
        ///// <remarks>
        ///// Actual disposal of the current UoW is deferred until the end the current page request 
        ///// </remarks>
        //public static void EndLongConversation()
        //{
        //    Local.Data[CurrentLongConversationIdKey] = null;
        //}


        //public static bool InLongConversation
        //{
        //    get { return CurrentLongConversationId != null; }
        //}

        //public static Guid? CurrentLongConversationId
        //{
        //    get { return (Guid?)Local.Data[CurrentLongConversationIdKey]; }
        //    internal set { Local.Data[CurrentLongConversationIdKey] = value; }
        //}

        #endregion

        public static bool IsStarted
        {
            get
            {
                if (globalNonThreadSafeUnitOfwork != null)
                    return true;
                return Local.Data[CurrentUnitOfWorkKey] != null;
                //return CurrentUnitOfWork != null;
            }
        }
        public static Configuration Configuration(string dbKey)
        {
            //return _unitOfWorkFactory.Configuration(dbKey);
            return StructureMap.ObjectFactory.GetInstance<IUnitOfWorkFactory>().Configuration(dbKey);
        }

        // Might want to use later
        //public static IDisposable SetCurrentSessionName(string name)
        //{
        //    return IoC.Resolve<IUnitOfWorkFactory>().SetCurrentSessionName(name);
        //}

        /// <summary>
        /// NOT thread safe! Mostly intended to support mocking of the unit of work. 
        /// You must pass a null argument when finished  to ensure atomic units of work UnitOfWorkRegisterGlobalUnitOfWork(null);
        /// You can also call Dispose() on the result of this method, or put it in a using statement (preferred)
        /// </summary>
        public static IDisposable RegisterGlobalUnitOfWork(IUnitOfWork global)
        {
            globalNonThreadSafeUnitOfwork = global;
            return new DisposableAction(delegate
            {
                globalNonThreadSafeUnitOfwork = null;
            });
        }


        /// <summary>
        /// Start a Unit of Work
        /// is called
        /// </summary>
        /// <returns>
        /// An IUnitOfwork object that can be used to work with the current UoW.
        /// </returns>
        public static IUnitOfWork Start(string dbKey)
        {
            if (globalNonThreadSafeUnitOfwork != null)
            {
                return globalNonThreadSafeUnitOfwork;
            }
                

            IUnitOfWorkImplementor existing = (IUnitOfWorkImplementor)Local.Data[CurrentUnitOfWorkKey];
            if(existing != null && Current.DatabaseKey == dbKey)
            {
                existing.IncrementUsages();
                return existing;
            }

            //if (CurrentUnitOfWork != null && CurrentUnitOfWork.DatabaseKey == dbKey)
            //    throw new InvalidOperationException("You cannot start more than one unit of work");//return CurrentUnitOfWork;

            //Current = _unitOfWorkFactory.Create(dbKey);
            Current = StructureMap.ObjectFactory.GetInstance<IUnitOfWorkFactory>().Create(dbKey);
            return Current;
        }

        public static IUnitOfWork Current
        {
            get
            {
                if (!IsStarted)
                    throw new InvalidOperationException("You are not in a unit of work");
                return globalNonThreadSafeUnitOfwork ?? (IUnitOfWork)Local.Data[CurrentUnitOfWorkKey];

                //var unitOfWork = CurrentUnitOfWork;
                //if (unitOfWork == null)
                //    throw new InvalidOperationException("You are not in a unit of work");
                //return unitOfWork;
            }
            internal set { Local.Data[CurrentUnitOfWorkKey] = value; }
        }


        //private static IUnitOfWork CurrentUnitOfWork
        //{
        //    get { return Local.Data[CurrentUnitOfWorkKey] as IUnitOfWork; }
        //    set { Local.Data[CurrentUnitOfWorkKey] = value; }
        //}

        

        

        public static ISession CurrentSession(string dbKey)
        {
            return ObjectFactory.GetInstance<IUnitOfWorkFactory>().GetCurrentSession(dbKey);
            //return _unitOfWorkFactory.GetCurrentSession(dbKey);

        }
        public static void SetCurrentSession(string dbKey, ISession value)
        {
            ObjectFactory.GetInstance<IUnitOfWorkFactory>().SetCurrentSession(dbKey, value);
            //_unitOfWorkFactory.SetCurrentSession(dbKey, value);
        }

        

        public static void DisposeUnitOfWork(IUnitOfWorkImplementor unitOfWork)
        {
            // Current = unitOfWork.Previous;
            Current = null;
        }

        
    }
}
