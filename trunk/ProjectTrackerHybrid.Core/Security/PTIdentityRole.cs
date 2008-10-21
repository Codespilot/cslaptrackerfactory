using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Server;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library.Security
{
    //New Class for use with NHibernate
    // Note new Attributes and Class signature
    [ObjectFactory("Factory Type=IReadOnlyBaseServerFactory;Item Type=ProjectTracker.Library.Security.PTIdentityRole, ProjectTracker.Library")]
    [DatabaseKey(Database.PTSecurityDb)]
    [Serializable]
    public class PTIdentityRole : PTReadOnlyBase<PTIdentityRole>
    {
        private string _username = string.Empty;
        private string _role = string.Empty;

        public string Username
        {
            get { return _username;}
            set { _username = value;}
        }

        public string Role
        {
            get { return _role; }   
            set { _role = value;}
        }

        public override bool Equals(object obj)
        {
            if(obj is PTIdentityRole)
            {
                PTIdentityRole role = obj as PTIdentityRole;

                if(role.Username == Username)
                    if(role.Role == Role)
                        return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Username + Role).GetHashCode();
        }
    }
}
