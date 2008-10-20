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
        // Internal property required for NHibernate, so we can get a reference to the parent object
        // from its children
        internal Project Project
        {
            get { return Parent as Project; }
        }

        #region  Business Methods

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
                //resource.Project = parent;
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

        #region  Factory Methods

        internal static ProjectResources NewProjectResources()
        {
            return DataPortal.CreateChild<ProjectResources>();
        }

        internal static ProjectResources GetProjectResources()
        {
            return DataPortal.FetchChild<ProjectResources>();
        }

        private ProjectResources()
        { /* require use of factory methods */ 
            MarkAsChild();        
        }

        #endregion

        #region  Data Access

        //private void Child_Fetch(ProjectTracker.DalLinq.Assignment[] data)
        //{
        //    this.RaiseListChangedEvents = false;
        //    foreach (var value in data)
        //        this.Add(ProjectResource.GetResource(value));
        //    this.RaiseListChangedEvents = true;
        //}

        #endregion

        #region Debugging

        public void DumpEditLevels(StringBuilder sb)
        {
            sb.AppendFormat("  {0} {1}: {2}\r", this.GetType().Name, "n/a", this.EditLevel);
            foreach (ProjectResource item in DeletedList)
                item.DumpEditLevels(sb);
            foreach (ProjectResource item in this)
                item.DumpEditLevels(sb);
        }


        #endregion

    }
}