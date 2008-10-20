using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;

namespace ProjectTracker.Library.Framework
{
    [Serializable]
    public abstract class PTNameValueListBase<K,V,T> : NameValueListBase<K,V>
        where T : NameValueBase<K,V>
    {

        public new bool IsReadOnly
        {
            get { return base.IsReadOnly; }
            set { base.IsReadOnly = value; }
        }

        public virtual void SetNHibernateCriteria(object businessCriteria, NHibernate.ICriteria nhibernateCriteria) { }
    }

    /// <summary>
    /// Abstract base class to represent an individual name value pair object.
    /// </summary>
    /// <typeparam name="K">Is the <c>Key</c>.</typeparam>
    /// <typeparam name="V">Is the <c>Value</c>.</typeparam>
    [Serializable]
    public abstract class NameValueBase<K, V>
    {
        /// <summary>
        /// Converts a instance that inherits from the <c>NameValueBase</c> to
        /// the CSLA <see cref="NameValueListBase{K,V}.NameValuePair"/> type.
        /// </summary>
        /// <returns>A <see cref="NameValueListBase{K,V}.NameValuePair"/> instance object.</returns>
        public abstract NameValueListBase<K, V>.NameValuePair ToNameValuePair();
    }
}
