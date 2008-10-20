using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using NHibernate;
using ICriteria=NHibernate.ICriteria;

namespace ProjectTracker.Library.Framework
{
    [Serializable]
    public abstract class PTReadOnlyListBase<T,C> : ReadOnlyListBase<T,C>
        where T : PTReadOnlyListBase<T, C>
        where C : PTReadOnlyBase<C>
    {

        public new bool IsReadOnly
        {
            get { return base.IsReadOnly; }
            set { base.IsReadOnly = value; }
        }

        public virtual void SetNHibernateCriteria(object businessCriteria, NHibernate.ICriteria nhibernateCriteria) { }
    }
}
