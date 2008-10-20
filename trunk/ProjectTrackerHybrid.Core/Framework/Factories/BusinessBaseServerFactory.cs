using System;
using System.ComponentModel;
using Csla;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework.Factories;

namespace ProjectTracker.Library.Framework.Factories
{
    /// <summary>
    /// Factory for PTBusinessBase objects. The factory does all data access for the object.
    /// </summary>
    /// <typeparam name="T">PT BusinessBase type</typeparam>
    public class BusinessBaseServerFactory<T> :
        AbstractServerBusinessFactory<T>, IBusinessBaseServerFactory<T>
        where T : PTBusinessBase<T>
    {
        private readonly IRepository<T> _repository;

        private string DatabaseKey { get { return DatabaseKeyAttribute.GetDatabaseKeyForClass(typeof(T)); } }

        public BusinessBaseServerFactory(IRepository<T> repository)
        {
            _repository = repository;
        }

        public override T Create()
        {
            var obj = Activator.CreateInstance(typeof(T), true);
            MarkNew(obj);
            return (T)obj;
        }

        public override T Create(SingleCriteria<T, int> criteria)
        {
            var obj = Activator.CreateInstance(typeof(T),true);
            MarkNew(obj);
            return (T)obj;
        }

        public override T Fetch()
        {
            throw new NotImplementedException("You must pass critiria to load a Root Object");
        }

        public override T Fetch(CriteriaBase criteria)
        {
            

            if (criteria != null)//(criteria.GetType().GetGenericTypeDefinition() == typeof (SingleCriteria<,>))
            {
                var obj = (T) Activator.CreateInstance(typeof (T), true);

                using (UnitOfWork.Start(DatabaseKey))
                {
                    object crit = obj.GetObjectCriteriaValue(criteria);

                    _repository.Load(obj, crit);
                    
                }
                
                MarkOld(obj);
                return obj;
            }
            return Fetch();
            
        }


        public T Update(T obj)
        {
            if (obj.IsDeleted)
            {
                if (UnitOfWork.IsStarted)
                    _repository.Delete(obj);
                else
                {
                    using (UnitOfWork.Start(DatabaseKey))
                    {
                        _repository.Delete(obj);
                        UnitOfWork.Current.TransactionalFlush();
                        UnitOfWork.CurrentSession(DatabaseKey).Clear();
                    }
                }
            }
            else
            {
                if (UnitOfWork.IsStarted)
                {
                    if(obj.IsNew)
                        _repository.Save(obj);
                    else
                        _repository.SaveOrUpdate(obj);
                }
                else
                {
                    using (UnitOfWork.Start(DatabaseKey))
                    {
                        if (obj.IsNew)
                            _repository.Save(obj);
                        else
                            _repository.Update(obj);

                        UnitOfWork.Current.TransactionalFlush();
                        UnitOfWork.CurrentSession(DatabaseKey).Clear();
                    }
                }
            }
         
            MarkOld(obj);
            return obj;
        }
        public void Delete(CriteriaBase criteria)
        {
            var obj = (T) Fetch(criteria);
            if (UnitOfWork.IsStarted)
            {
                _repository.Delete(obj);
            }
            else
            {
                using (UnitOfWork.Start(DatabaseKey))
                {
                    _repository.Delete(obj);
                    UnitOfWork.Current.TransactionalFlush();
                    UnitOfWork.CurrentSession(DatabaseKey).Clear();
                }
            }

         
        }

      
    }
}