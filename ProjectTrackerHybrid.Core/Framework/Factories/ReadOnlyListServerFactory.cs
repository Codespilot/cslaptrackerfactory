using System;
using System.Collections.Generic;
using Csla;
using NHibernate.Criterion;
using ProjectTracker.Library.Admin;
using ProjectTracker.Library.Data;

namespace ProjectTracker.Library.Framework.Factories
{
    public interface IReadOnlyListServerFactory<L, I>
    {
        L Create();
        L Fetch();
        L Fetch(CriteriaBase criteria);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="L">Csla ROL type</typeparam>
    /// <typeparam name="I">Csla ROL item type</typeparam>
    public class ReadOnlyListServerFactory<L, I> :
        AbstractServerBusinessFactory<L>, IReadOnlyListServerFactory<L, I>
        where L : PTReadOnlyListBase<L,I>
        where I : PTReadOnlyBase<I>
    {
        private readonly IRepository<I> _repository;

        public ReadOnlyListServerFactory(IRepository<I> repository)
        {
            _repository = repository;
        }

        private string DatabaseKey
        {
            get { return DatabaseKeyAttribute.GetDatabaseKeyForClass(typeof(I)); }
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

            using (UnitOfWork.Start(DatabaseKey))
            {
                IList<I> _list = _repository.FindAll(DetachedCriteria.For(typeof (I)));
                list.IsReadOnly = false;
                foreach (I item in _list)
                {

                    list.Add(item);
                }
                list.IsReadOnly = true;
            }
            MarkOld(list);
            list.RaiseListChangedEvents = true;
            
            return list;
        }

        public override L Fetch(CriteriaBase criteria)
        {
            if (criteria != null)
            {
                var list = (L)Activator.CreateInstance(typeof(L), true);
                IList<I> _list;

                using (UnitOfWork.Start(DatabaseKey))
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

    }
}