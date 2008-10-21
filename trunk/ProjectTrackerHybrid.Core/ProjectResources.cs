using System.Text;
using Csla;
using System;
using Csla.Core;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
    [Serializable()]
    public class ProjectResources : PTBusinessListBase<ProjectResources, ProjectResource>
    {

        #region  Business Methods - No Change

        public ProjectResource GetItem(int resourceId)
        {
            foreach (ProjectResource res in this)
                if (res.ResourceId == resourceId)
                    return res;
            return null;
        }

        public void Assign(int resourceId)
        {
            if (!(Contains(resourceId)))
            {
                ProjectResource resource = ProjectResource.NewProjectResource(resourceId);
                this.Add(resource);
            }
            else
            {
                throw new InvalidOperationException("Resource already assigned to project");
            }
        }

        public void Remove(int resourceId)
        {
            foreach (ProjectResource res in this)
            {
                if (res.ResourceId == resourceId)
                {
                    Remove(res);
                    break;
                }
            }
        }

        public bool Contains(int resourceId)
        {
            foreach (ProjectResource res in this)
                if (res.ResourceId == resourceId)
                    return true;
            return false;
        }

        public bool ContainsDeleted(int resourceId)
        {
            foreach (ProjectResource res in DeletedList)
                if (res.ResourceId == resourceId)
                    return true;
            return false;
        }

        #endregion

        #region  Factory Methods - Partially Commented

        internal static ProjectResources NewProjectResources()
        {
            return DataPortal.CreateChild<ProjectResources>();
        }

        // Not Needed
        //internal static ProjectResources GetProjectResources()
        //{
        //    return DataPortal.FetchChild<ProjectResources>();
        //}

        private ProjectResources()
        { /* require use of factory methods */ 
            MarkAsChild();        
        }

        #endregion

        #region  Data Access - Totally Commented

        //private void Child_Fetch(ProjectTracker.DalLinq.Assignment[] data)
        //{
        //    this.RaiseListChangedEvents = false;
        //    foreach (var value in data)
        //        this.Add(ProjectResource.GetResource(value));
        //    this.RaiseListChangedEvents = true;
        //}

        #endregion

    }
}