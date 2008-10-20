using System;
using System.Collections.Generic;
using Csla;
using Csla.Server;
using NHibernate.Criterion;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework.Factories;

namespace ProjectTracker.Library.Framework.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Csla PTNameValueList type</typeparam>
    /// <typeparam name="K">KeyValueList: Key type</typeparam>
    /// <typeparam name="V">KeyValueList: Value type</typeparam>
    /// <typeparam name="I">A class that inherits from <see cref="NameValueBase{K,V}"/>.</typeparam>
    public class NameValueListServerFactory<T, K, V, I> :
        ObjectFactory, INameValueListServerFactory<T, K, V, I>
        where T : PTNameValueListBase<K, V, I>
        where I : NameValueBase<K, V>
    {
        private readonly IRepository<I> _repository;

        public NameValueListServerFactory(IRepository<I> repository)
        {
            _repository = repository;
        }

        private string DatabaseKey
        {
            get { return DatabaseKeyAttribute.GetDatabaseKeyForClass(typeof (I)); }
        }

        #region INameValueListServerFactory<T,K,V,I> Members

        public T Create() 
        {
            var obj = (T) Activator.CreateInstance(typeof (T), true);
                
            return obj;
        }

        public T Fetch() 
        {
            var list = (T) Activator.CreateInstance(typeof (T), true);

            IList<I> _list;

            if (UnitOfWork.IsStarted)
                _list = _repository.FindAll(DetachedCriteria.For(typeof (I)));
            else
            {
                using (UnitOfWork.Start(DatabaseKey))
                {
                    _list = _repository.FindAll(DetachedCriteria.For(typeof (I)));
                }
            }

            list.IsReadOnly = false;
            foreach (I item in _list)
            {
                list.Add(item.ToNameValuePair());
            }
            list.IsReadOnly = true;

            return list;
        }

        public T Fetch(CriteriaBase criteria)
        {
            
            if(criteria != null)
            {
                var list = (T)Activator.CreateInstance(typeof(T), true);
                IList<I> _list;
                using (UnitOfWork.Start(DatabaseKey))
                {
                    NHibernate.ICriteria crit = _repository.CreateCriteria();

                    list.SetNHibernateCriteria(criteria, crit);
                    _list = crit.List<I>();
                }
                list.IsReadOnly = false;
                foreach (I item in _list)
                {
                    list.Add(item.ToNameValuePair());
                }
                list.IsReadOnly = true;
                return list;
            }
            return Fetch();
        }

        #endregion

        public T Create(SingleCriteria<T, int> criteria)
        {
            throw new NotImplementedException();
        }

    }
}