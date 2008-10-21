using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using NHibernate;
using ICriteria = NHibernate.ICriteria;

namespace ProjectTracker.Library.Framework
{
    [Serializable]
    public abstract class PTBusinessListBase<T, C> : BusinessListBase<T, C>
        where T : PTBusinessListBase<T, C>
        where C : PTBusinessBase<C>
    {

        public virtual void SetNHibernateCriteria(object businessCriteria, NHibernate.ICriteria nhibernateCriteria) { }

        public void Add(IList<C> theList)
        {
            RaiseListChangedEvents = false;

            foreach (C businessObject in theList)
            {
                businessObject.InitializeObject();

                if(!this.Contains(businessObject))
                    Add(businessObject);
            }
            
            RaiseListChangedEvents = true;

        }

        /// <summary>
        /// Return the Deleted list
        /// </summary>
        /// <returns></returns>
        internal IList<C> GetDeletedList()
        {
            return DeletedList;
        }
    }
}
