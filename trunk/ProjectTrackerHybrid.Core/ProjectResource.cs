using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
    [Serializable()]
    public class ProjectResource : PTBusinessBase<ProjectResource>, IHoldRoles
    {

        #region  Business Methods

        
        internal Project Project
        {
            get; set;
            //get
            //{
            //    ProjectResources parent =
            //        this.Parent as ProjectResources;

            //    return parent.Project;
            //}
            //set { }
        }

        [NotUndoable]
        private ResourceInfo _resource;

        internal ResourceInfo Resource
        {
            get { return _resource; }
            set { _resource = value; }
            //{
            //    if(ReferenceEquals(_resource, null))
            //        _resource = Library.Resource.GetResource(ResourceId);
            //    return _resource;
            //}
        }


        private byte[] timeStamp = null;

        internal byte[] TimeStamp
        {
            get { return timeStamp;  }
            set { timeStamp = value;}
        }

        private static PropertyInfo<Guid> AssignmentIdProperty = RegisterProperty<Guid>(typeof(ProjectResource), new PropertyInfo<Guid>("AssignmentId", "Assignment Id", Guid.NewGuid()));
        internal Guid AssignmentId
        {
            get { return GetProperty<Guid>(AssignmentIdProperty); }
            set { SetProperty<Guid>(AssignmentIdProperty, value); }
        }

        private static PropertyInfo<Guid> ProjectIdProperty = RegisterProperty<Guid>(typeof(ProjectResource), new PropertyInfo<Guid>("ProjectId", "Project Id"));
        internal Guid ProjectId
        {
            get { return GetProperty<Guid>(ProjectIdProperty); }
            set { SetProperty<Guid>(ProjectIdProperty, value); }
        }

        private static PropertyInfo<int> ResourceIdProperty = RegisterProperty(new PropertyInfo<int>("ResourceId", "Resource id"));
        public int ResourceId
        {
            get { return Resource.Id; }
            internal set { SetProperty(ResourceIdProperty, value);}
        }

        public string FirstName
        {
            get { return Resource.FirstName; }
        }

        public string LastName
        {
            get { return Resource.LastName; }
        }

        public string FullName
        {
            get { return LastName + ", " + FirstName; }
        }

        private static PropertyInfo<SmartDate> AssignedProperty = RegisterProperty(new PropertyInfo<SmartDate>("Assigned", "Date assigned"));
        public string Assigned
        {
            get { return GetPropertyConvert<SmartDate, string>(AssignedProperty); }
            internal set { SetProperty(AssignedProperty, value); }
        }

        private static PropertyInfo<int> RoleProperty = RegisterProperty(new PropertyInfo<int>("Role", "Role assigned", 2));
        public int Role
        {
            get
            { return GetProperty<int>(RoleProperty); }
            set
            { SetProperty<int>(RoleProperty, value); }
        }

        public Resource GetResource()
        {
            CanExecuteMethod("GetResource", true);
            return Library.Resource.GetResource(Resource.Id);
        }

        public override string ToString()
        {
            return ResourceId.ToString();
        }

        public override bool Equals(object obj)
        {
            if(obj is ProjectResource)
            {
                ProjectResource temp = obj as ProjectResource;
                //if(temp.ProjectId == ReadProperty(ProjectIdProperty))
                    if(temp.ResourceId == ResourceId)
                        return true;
                
            }
            return false;
        }

        protected override object GetIdValue()
        {
            return ReadProperty(ProjectIdProperty).ToString() + ReadProperty(ResourceIdProperty).ToString();
        }

        public override int GetHashCode()
        {
            return (ReadProperty(ProjectIdProperty).ToString() + ReadProperty(ResourceIdProperty).ToString()).GetHashCode();
        }

        #endregion

        #region  Validation Rules

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(Assignment.ValidRole, RoleProperty);
        }

        #endregion

        #region  Authorization Rules

        protected override void AddAuthorizationRules()
        {
            //AuthorizationRules.AllowWrite(RoleProperty, "ProjectManager");
        }

        #endregion

        #region  Factory Methods

        public static ProjectResource NewProjectResource(int resourceId)
        {
            return new ProjectResource(resourceId, RoleList.DefaultRole());
        }

        internal static ProjectResource GetResource(int resourceId)
        {
            return DataPortal.FetchChild<ProjectResource>();
        }

        private ProjectResource()
        { /* require use of factory methods */
            MarkAsChild();
        }

        private ProjectResource(int resourceId, int role)
        {
            MarkAsChild();
            var resource = Library.Resource.GetResource(resourceId);
            LoadProperty<int>(ResourceIdProperty, resource.Id);
            LoadProperty<SmartDate>(AssignedProperty, Assignment.GetDefaultAssignedDate());
            LoadProperty<int>(RoleProperty, role);
            _resource = new ResourceInfo(resource.Id, resource.FirstName, resource.LastName);
            MarkNew();
        }

        #endregion

        #region Debugging

        /// <summary>
        /// For Debugging, dumps edit levels of current object and children
        /// </summary>
        /// <param name="sb"></param>
        public void DumpEditLevels(StringBuilder sb)
        {
            sb.AppendFormat("    {0} {1}: {2} {3}\r", this.GetType().Name, this.GetIdValue().ToString(), this.EditLevel, this.BindingEdit);
            //ServiceOrderTaskParts.DumpEditLevels(sb);

        }

        #endregion

    }
}
