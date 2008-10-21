using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Server;
using ProjectTracker.Library.Framework;
using DataPortal=Csla.DataPortal;

namespace ProjectTracker.Library
{
    [ObjectFactory("Factory Type=IReadOnlyListServerFactory;List Type=ProjectTracker.Library.ResourceList, ProjectTracker.Library;Item Type=ProjectTracker.Library.ResourceInfo, ProjectTracker.Library;")]
    [Serializable()]
    public class ResourceList : PTReadOnlyListBase<ResourceList, ResourceInfo>
    {
        #region  Factory Methods - No Change

        public static ResourceList EmptyList()
        {
            return new ResourceList();
        }

        public static ResourceList GetResourceList()
        {
            return DataPortal.Fetch<ResourceList>();
        }

        private ResourceList()
        { /* require use of factory methods */ }

        #endregion

        #region  Data Access - Totally Commented

        //private void DataPortal_Fetch()
        //{
        //    RaiseListChangedEvents = false;
        //    using (var ctx = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    {
        //        var data = from r in ctx.DataContext.Resources
        //                   select new ResourceInfo(r.Id, r.LastName, r.FirstName);
        //        IsReadOnly = false;
        //        this.AddRange(data);
        //        IsReadOnly = true;
        //    }
        //    RaiseListChangedEvents = true;
        //}

        #endregion

    }
}
