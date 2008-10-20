using Csla;
using Csla.Data;
using System;
using System.Linq;
using Csla.Server;
using NHibernate.Criterion;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;
using DataPortal=Csla.DataPortal;

namespace ProjectTracker.Library
{
    [ObjectFactory("Factory Type=INameValueListServerFactory;List Type=ProjectTracker.Library.RoleList;Item Type=ProjectTracker.Library.RoleNV, ProjectTracker.Library;Key Type=System.Int32;Value Type=System.String")]
    [DatabaseKey(Database.PTrackerDb)]
    [Serializable()]
    public class RoleList : PTNameValueListBase<int, string, RoleNV>
    {
        #region  Business Methods

        public static int DefaultRole()
        {
            RoleList list = GetList();
            if (list.Count > 0)
                return list.Items[0].Key;
            else
                throw new NullReferenceException("No roles available; default role can not be returned");
        }

        #endregion

        #region  Factory Methods

        private static RoleList _list;

        public static RoleList GetList()
        {
            if (_list == null)
                _list = DataPortal.Fetch<RoleList>();
            return _list;
        }

        public static RoleList GetList(string name)
        {
            if (_list == null)
                _list = DataPortal.Fetch<RoleList>(new SingleCriteria<RoleList, string>(name));
            return _list;

        }

        /// <summary>
        /// Clears the in-memory RoleList cache
        /// so the list of roles is reloaded on
        /// next request.
        /// </summary>
        public static void InvalidateCache()
        {
            _list = null;
        }

        private RoleList()
        { /* require use of factory methods */ }

        #endregion

        #region  Data Access

        //private void DataPortal_Fetch()
        //{
        //    this.RaiseListChangedEvents = false;
        //    using (var ctx = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    {
        //        var data = from role in ctx.DataContext.Roles
        //                   select new NameValuePair(role.Id, role.Name);
        //        IsReadOnly = false;
        //        this.AddRange(data);
        //        IsReadOnly = true;
        //    }
        //    this.RaiseListChangedEvents = true;
        //}

        #endregion

        private NHibernate.ICriteria _iCriteria = null;

        public override void SetNHibernateCriteria(object businessCriteria, NHibernate.ICriteria nhibernateCriteria)
        {
            // Cast the criteria back to the strongly-typed version
            SingleCriteria<RoleList, string> criteria = businessCriteria as SingleCriteria<RoleList, string>;

            // If it's a valid criteria object then check for filters
            if (!ReferenceEquals(criteria, null))
            {
                // Set a reference to the NHibernate ICriteria (for local use only)
                _iCriteria = nhibernateCriteria;

                // Name
                if (!String.IsNullOrEmpty(criteria.Value))
                {
                    AddCriterionName(criteria.Value);
                }
            }
        }

        /// <summary>
        /// Adds a criterion to the NHibernate <see cref="ICriteria"/> that filters by name.
        /// </summary>
        /// <param name="name">The name to use as a filter.</param>
        private void AddCriterionName(string name)
        {
            NHibernate.Criterion.SimpleExpression expression = Restrictions.Eq("Name", name);
            _iCriteria.Add(expression);
        }

        #region embedded Role class

        

        #endregion

    }

    /// <summary>
        /// Represents a <c>Role</c> name-value pair.
        /// </summary>
        [DatabaseKey(Database.PTrackerDb)]
        public class RoleNV : NameValueBase<int, string>
        {
            #region NameValuePairBase<K,V> overrides

            public override NameValueListBase<int,string>.NameValuePair ToNameValuePair()
            {
                return new NameValueListBase<int,string>.NameValuePair(_id, _name);
            }

            #endregion

            #region fields (in database schema order)

            private int _id;

            public int Id
            {
                get { return _id;}
                set { _id = value;}
            }

            private string _name;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            #endregion

            #region constructor

            /// <summary>Direct construction not allowed.</summary>
            private RoleNV() { }

            #endregion
            
        }
}
