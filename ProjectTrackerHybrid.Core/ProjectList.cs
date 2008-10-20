using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using Csla.Server;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;
using DataPortal=Csla.DataPortal;

namespace ProjectTracker.Library
{
    [ObjectFactory("Factory Type=IReadOnlyListServerFactory;List Type=ProjectTracker.Library.ProjectList, ProjectTracker.Library;Item Type=ProjectTracker.Library.ProjectInfo, ProjectTracker.Library;")]
    [Serializable]
    public class ProjectList : PTReadOnlyListBase<ProjectList, ProjectInfo>
    {
        public static ProjectList NewProjectList()
        {
            return DataPortal.Create<ProjectList>();

        }

        public static ProjectList GetProjectList()
        {
            return DataPortal.Fetch<ProjectList>();

        }

        public static ProjectList GetProjectList(string projectName)
        {
            return DataPortal.Fetch<ProjectList>(new SingleCriteria<ProjectList, string>(projectName));
        }


    }

    [DatabaseKey(Database.PTrackerDb)]
    [Serializable]
    public class ProjectInfo : PTReadOnlyBase<ProjectInfo>
    {
        private Guid _id;
        private string _name;

        public Guid Id
        {
            get
            {
                return _id;
            }
            internal set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            internal set
            {
                _name = value;
            }
        }

        protected override object GetIdValue()
        {
            return _id;
        }

        public override string ToString()
        {
            return _name;
        }

        private ProjectInfo()
        {
            // require use of factory methods
        }

        internal ProjectInfo(Guid id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}
