using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace ProjectTracker.Library.Data
{
    public interface IRepository<T>
    {
        //IList<T> FetchList();
        ICriteria CreateCriteria();
        T Get(object id);
        void Load(T obj, object id);
        void Delete(T entity);
       
        void DeleteAll();
        void DeleteAll(DetachedCriteria where);
        T Save(T entity);
        T SaveOrUpdate(T entity);
        T SaveOrUpdateCopy(T entity);
        void Update(T entity);
        long Count(DetachedCriteria criteria);
        long Count();
        bool Exists(DetachedCriteria criteria);
        bool Exists();
        IList<T> FindAll(DetachedCriteria criteria, params Order[] orders);
        IList<T> FindAll(Order order, params ICriterion[] criteria);
        IList<T> FindAll(Order[] orders, params ICriterion[] criteria);
        T FindFirst(params Order[] orders);
        T FindFirst(DetachedCriteria criteria, params Order[] orders);
        T FindOne(params ICriterion[] criteria);
        T FindOne(DetachedCriteria criteria);

        ProjT ReportOne<ProjT>(ProjectionList projectionList);
        ProjT ReportOne<ProjT>(DetachedCriteria criteria, ProjectionList projectionList);
        IList<ProjT> ReportAll<ProjT>(ProjectionList projectionList);
        IList<ProjT> ReportAll<ProjT>(bool distinctResults, ProjectionList projectionList);
        IList<ProjT> ReportAll<ProjT>(ProjectionList projectionList, params Order[] orders);
        IList<ProjT> ReportAll<ProjT>(bool distinctResults, ProjectionList projectionList, params Order[] orders);
        IList<ProjT> ReportAll<ProjT>(DetachedCriteria criteria, ProjectionList projectionList, params Order[] orders);
        IList<ProjT> ReportAll<ProjT>(bool distinctResults, DetachedCriteria criteria, ProjectionList projectionList, params Order[] orders);
    }
}
