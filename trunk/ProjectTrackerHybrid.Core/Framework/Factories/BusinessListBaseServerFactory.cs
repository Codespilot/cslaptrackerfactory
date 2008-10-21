using System;
using System.Collections.Generic;
using Csla;
using NHibernate.Criterion;
using ProjectTracker.Library.Data;

namespace ProjectTracker.Library.Framework.Factories
{
    public interface IBusinessListBaseServerFactory<L, I>
    {
        L Create();
        L Fetch();
        L Fetch(CriteriaBase criteria);
        L Update(L obj);
        
    }

    public class BusinessListBaseServerFactory<L, I> :
        AbstractServerBusinessFactory<L>, IBusinessListBaseServerFactory<L, I>
        where L : PTBusinessListBase<L,I>
        where I : PTBusinessBase<I>
    {
        private readonly IRepository<I> _repository;

        private string DatabaseKey
        {
            get { return DatabaseKeyAttribute.GetDatabaseKeyForClass(typeof(I)); }
        }

        public BusinessListBaseServerFactory(IRepository<I> repository)
        {
            _repository = repository;
        }

        public override L Create(SingleCriteria<L, int> criteria)
        {

            throw new NotImplementedException();
        }

        public override L Create()
        {
            var obj = (L)Activator.CreateInstance(typeof(L), true);
            MarkNew(obj);
            return obj;
        }
        
        public override L Fetch()
        {
            L list = (L)Activator.CreateInstance(typeof(L), true);
            list.RaiseListChangedEvents = false;
            IList<I> _list;

            using (UnitOfWork.Start(DatabaseKey))
            {
               _list = _repository.FindAll(DetachedCriteria.For(typeof (I)));
            }
            foreach (I item in _list)
            {
                list.Add(item);
                MarkOld(item);
            }
            MarkOld(list);
            list.RaiseListChangedEvents = true;

            return list;
        }

        public override L Fetch(CriteriaBase criteria)
        {
            if (criteria != null)
            {
                var list = (L) Activator.CreateInstance(typeof (L), true);
                IList<I> _list;

                using(UnitOfWork.Start(DatabaseKey))
                {
                    NHibernate.ICriteria crit = _repository.CreateCriteria();

                    list.SetNHibernateCriteria(criteria, crit);
                    _list = crit.List<I>();
                }

                list.RaiseListChangedEvents = false;
                
                foreach (var item in _list)
                {
                    list.Add(item);
                }

                list.RaiseListChangedEvents = true;
                MarkOld(list);
                return list;
                
            }
            return Fetch();
        }

        public L Update(L obj)
        {
            //go through the deleted list
            foreach (I item in obj.GetDeletedList())
            {
                if (UnitOfWork.IsStarted)
                    _repository.Delete(item);
                else
                {
                    using (UnitOfWork.Start(DatabaseKey))
                    {
                        _repository.Delete(item);
                        UnitOfWork.Current.TransactionalFlush();
                        UnitOfWork.CurrentSession(DatabaseKey).Clear();
                    }
                }
            }

            foreach (I item in obj)
            {
                
                if (item.IsDirty)
                {
                    if (UnitOfWork.IsStarted)
                    {
                        if (item.IsNew)
                            _repository.Save(item);
                        else
                            _repository.SaveOrUpdate(item);
                    }
                    else
                    {
                        using (UnitOfWork.Start(DatabaseKey))
                        {
                            if (item.IsNew)
                                _repository.Save(item);
                            else
                                _repository.SaveOrUpdate(item);
                            UnitOfWork.Current.TransactionalFlush();
                            UnitOfWork.CurrentSession(DatabaseKey).Clear();
                        }
                    }
                }
            }
          
            MarkOld(obj);
            return obj;
        }
    }
}