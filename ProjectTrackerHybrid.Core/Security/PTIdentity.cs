using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Csla.Server;
using ProjectTracker.Library.Admin;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;
using DataPortal=Csla.DataPortal;

namespace ProjectTracker.Library
{
    namespace Security
    {
        [ObjectFactory("Factory Type=IReadOnlyBaseServerFactory;Item Type=ProjectTracker.Library.Security.PTIdentity, ProjectTracker.Library")]
        [DatabaseKey(Database.PTSecurityDb)]
        [Serializable()]
        public class PTIdentity : PTReadOnlyBase<PTIdentity>, IIdentity
        {

            #region  Business Methods

            protected override object GetIdValue()
            {
                return _name;
            }

            #region  IsInRole

            private List<string> _roles = new List<string>();

            internal bool IsInRole(string role)
            {
                if(!ReferenceEquals(roles, null))
                {
                    foreach (PTIdentityRole identityRole in roles)
                    {
                        if(identityRole.Role.Equals(role))
                            return true;
                    }
                }
                return false;
            }

            #endregion

            #region  IIdentity

            private bool _isAuthenticated;
            private string _name = "";

            public string AuthenticationType
            {
                get
                {
                    return "Csla";
                }
            }

            public bool IsAuthenticated
            {
                get
                {
                    return _isAuthenticated;
                }
            }

            public string Name
            {
                get
                {
                    return _name;
                }
            }

            public string Username { get; set; }


            private string _password;

            internal string Password
            {
                get { return _password; }
                set { _password = value;}
            }

            private IList<PTIdentityRole> roles = new List<PTIdentityRole>();

            public IList<PTIdentityRole> RolesSet
            {
                get { return roles;}
                set { roles = value;}
            }


            #endregion

            #endregion

            #region  Factory Methods

            internal static PTIdentity UnauthenticatedIdentity()
            {
                return new PTIdentity();
            }

            internal static PTIdentity GetIdentity(string username, string password)
            {

                PTIdentity dbValue = DataPortal.Fetch<PTIdentity>(new CredentialsCriteria(username, password));

                if(dbValue.Password == password)
                    if(dbValue.Username == username)
                    {
                        dbValue._isAuthenticated = true;
                        dbValue._name = username;
                    }

                return dbValue;
            }

            internal static PTIdentity GetIdentity(string username)
            {
                return DataPortal.Fetch<PTIdentity>(new LoadOnlyCriteria(username));
            }

            private PTIdentity()
            {
                // require use of factory methods
            }

            #endregion

            public override object GetObjectCriteriaValue(object businessCriteria)
            {
                // Cast the criteria back to the strongly-typed version
                CredentialsCriteria criteria = businessCriteria as CredentialsCriteria;

                // If it's a valid criteria object then check for filters
                if (!ReferenceEquals(criteria, null))
                {
                    // Set a reference to the NHibernate ICriteria (for local use only)
                    //_iCriteria = nhibernateCriteria;
                    return criteria.Username;
                }
                return null;
            }

            #region  Data Access

            [Serializable()]
            private class CredentialsCriteria
            {

                private string _username;
                private string _password;

                public string Username
                {
                    get
                    {
                        return _username;
                    }
                }

                public string Password
                {
                    get
                    {
                        return _password;
                    }
                }

                public CredentialsCriteria(string username, string password)
                {
                    _username = username;
                    _password = password;
                }
            }

            [Serializable()]
            private class LoadOnlyCriteria
            {

                private string mUsername;

                public string Username
                {
                    get
                    {
                        return mUsername;
                    }
                }

                public LoadOnlyCriteria(string username)
                {
                    mUsername = username;
                }
            }

            //private void DataPortal_Fetch(CredentialsCriteria criteria)
            //{
            //    using (var ctx = ContextManager<SecurityDataContext>.GetManager(ProjectTracker.DalLinq.Database.Security))
            //    {
            //        var data = from u in ctx.DataContext.Users
            //                   where u.Username == criteria.Username && u.Password == criteria.Password
            //                   select u;
            //        if (data.Count() > 0)
            //            Fetch(data.Single());
            //        else
            //            Fetch(null);
            //    }
            //}

            //private void DataPortal_Fetch(LoadOnlyCriteria criteria)
            //{
            //    using (var ctx = ContextManager<SecurityDataContext>.GetManager(ProjectTracker.DalLinq.Database.Security))
            //    {
            //        var data = from u in ctx.DataContext.Users
            //                   where u.Username == criteria.Username
            //                   select u;
            //        if (data.Count() > 0)
            //            Fetch(data.Single());
            //        else
            //            Fetch(null);
            //    }
            //}

            private void Fetch(User user)
            {
                if (user != null)
                {
                    _name = user.Username;
                    _isAuthenticated = true;
                    var roles = from r in user.Roles select r;
                    foreach (var role in roles)
                        _roles.Add(role.Name);
                }
                else
                {
                    _name = "";
                    _isAuthenticated = false;
                    _roles.Clear();
                }
            }

            #endregion

        }
    }
}
