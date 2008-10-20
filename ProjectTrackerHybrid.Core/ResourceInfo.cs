using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
    [DatabaseKey(Database.PTrackerDb)]
    [Serializable()]
    public class ResourceInfo : PTReadOnlyBase<ResourceInfo>
    {
        private int _id;
        private string _name;

        public int Id
        {
            get
            {
                return _id;
            }
            internal set { _id = value; }
        }

        internal string FirstName{ get; set;}
        internal string LastName { get; set;}

        public string Name
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
            internal set { _name = value; }
        }

        protected override object GetIdValue()
        {
            return _id;
        }

        public override string ToString()
        {
            return _name;
        }

        internal ResourceInfo(int id, string firstName, string lastName)
        {
            _id = id;
            FirstName = firstName;
            LastName = lastName;
        }
        internal ResourceInfo()
        {
            
        }
    }
}
