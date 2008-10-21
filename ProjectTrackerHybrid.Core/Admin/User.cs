using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using Csla.Server;
using ProjectTracker.Library;
using ProjectTracker.Library.Framework;
using ProjectTracker.Library.Data;

namespace ProjectTracker.Library.Admin
{
    //New Class
    // Note new attributes and signature change of class
    [ObjectFactory("Factory Type=IBusinessBaseServerFactory;Item Type=ProjectTracker.Library.Admin.User, ProjectTracker.Library")]
    [DatabaseKey(Database.PTSecurityDb)]
    [Serializable]
    public class User : PTBusinessBase<User>
    {
        #region  Business Methods

        //public override string DatabaseKey { get { return Database.PTSecurityDb; } }

        private static PropertyInfo<string> UsernameProperty = RegisterProperty(new PropertyInfo<string>("Username"));
        public string Username
        {
            get { return GetProperty<string>(UsernameProperty); }
            set { SetProperty<string>(UsernameProperty, value); }
        }

        private static PropertyInfo<string> PasswordProperty = RegisterProperty(new PropertyInfo<string>("Password"));
        public string Password
        {
            get { return GetProperty<string>(PasswordProperty); }
            set { SetProperty<string>(PasswordProperty, value); }
        }

        private static PropertyInfo<Roles> RolesProperty = RegisterProperty<Roles>(typeof(User), new PropertyInfo<Roles>("Roles", "Roles"));
        internal Roles Roles
        {

            get
            {
                if (!(FieldManager.FieldExists(RolesProperty)))
                {
                    LoadProperty<Roles>(RolesProperty, Admin.Roles.GetRoles());
                }
                return GetProperty<Roles>(RolesProperty);
            }
            set { SetProperty<Roles>(RolesProperty, value); }
        }

        internal IList<Role> RolesSet
        {
            get
            {
                return Roles as IList<Role>;
            }
            set
            {
                foreach (Role role in value)
                {
                    if (!Roles.Contains(role))
                        Roles.Add(role);
                }

            }
        }
        #endregion
    }
}
