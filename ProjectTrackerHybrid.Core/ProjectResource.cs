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
        #region New / Modified Business Methods

        /// <summary>
        /// This is modified from the Csla Default to be null. This is so
        /// NHibernate knows what to do with the TimeStamp
        /// </summary>
        private byte[] timeStamp = null;

        internal byte[] TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        // Automatic Property for NHibernate - notice it is internal
        //internal Project Project { get; set; }

        /// <summary>
        /// Property/Field combination for ResourceInfo
        /// NHibernate uses this for part of our Many-To-Many relationship
        /// since it is not a straightforward relationship, i.e. we store additional data
        /// in the assignment table.
        /// </summary>
        [NotUndoable]
        private ResourceInfo _resource;

        internal ResourceInfo Resource
        {
            get { return _resource; }
            set { _resource = value; }
        }

        // NHibernate needs a property to load and save ProjectId to/from
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
            internal set { SetProperty(ResourceIdProperty, value); } // Added internal set for NHibernate
        }

        // No need for PropertyInfo since we have a copy of ResourceInfo Local
        public string FirstName
        {
            get { return Resource.FirstName; }
        }

        // No need for PropertyInfo since we have a copy of ResourceInfo Local
        public string LastName
        {
            get { return Resource.LastName; }
        }

        private static PropertyInfo<SmartDate> AssignedProperty = RegisterProperty(new PropertyInfo<SmartDate>("Assigned", "Date assigned"));
        public string Assigned
        {
            get { return GetPropertyConvert<SmartDate, string>(AssignedProperty); }
            internal set { SetProperty(AssignedProperty, value); } // Added internal set for NHibernate
        }

        #region System.Object overrides -- These are needed for NHibernate mapping and relationships

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="ProjectResource"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is ProjectResource)
            {
                ProjectResource temp = obj as ProjectResource;
                //if(temp.ProjectId == ReadProperty(ProjectIdProperty))
                if (temp.ResourceId == ResourceId)
                    return true;

            }
            return false;
        }

        public override int GetHashCode()
        {
            return (ReadProperty(ProjectIdProperty).ToString() + ReadProperty(ResourceIdProperty).ToString()).GetHashCode();
        }
        #endregion

        #endregion

        #region  Business Methods Partially Commented

        /// NOT USING - See modified version above
        //private static PropertyInfo<string> FirstNameProperty = RegisterProperty(new PropertyInfo<string>("FirstName", "First name"));
        //public string FirstName
        //{
        //    get
        //    {
        //        return GetProperty<string>(FirstNameProperty);
        //    }
        //}

        //private static PropertyInfo<string> LastNameProperty = RegisterProperty(new PropertyInfo<string>("LastName", "Last name"));
        //public string LastName
        //{
        //    get
        //    {
        //        return GetProperty<string>(LastNameProperty);
        //    }
        //}

        public string FullName
        {
            get { return LastName + ", " + FirstName; }
        }

        private static PropertyInfo<int> RoleProperty = RegisterProperty(new PropertyInfo<int>("Role", "Role assigned", 2));
        public int Role
        {
            get { return GetProperty<int>(RoleProperty); }
            set { SetProperty<int>(RoleProperty, value); }
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

        #endregion 

        #region  Validation Rules - No Change

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(Assignment.ValidRole, RoleProperty);
        }

        #endregion

        #region  Authorization Rules - Commented Temporarily

        protected override void AddAuthorizationRules()
        {
            //AuthorizationRules.AllowWrite(RoleProperty, "ProjectManager");
        }

        #endregion

        #region  Factory Methods - Slightly Modified

        public static ProjectResource NewProjectResource(int resourceId)
        {
            // uses a private constructor now because the object factory does not do child objects
            return new ProjectResource(resourceId, RoleList.DefaultRole());
        }

        // Not Used
        //internal static ProjectResource GetResource(int resourceId)
        //{
        //    return DataPortal.FetchChild<ProjectResource>();
        //}

        private ProjectResource()
        { /* require use of factory methods */
            MarkAsChild(); // Have to mark as child since we don't have the dataportal to do it for us
        }

        /// <summary>
        /// Returns a new ProjectResource by loading a <see cref="Resource"/>
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="role"></param>
        private ProjectResource(int resourceId, int role)
        {
            // Have to mark as child since we don't have the dataportal to do it for us
            MarkAsChild();

            var resource = Library.Resource.GetResource(resourceId);
            LoadProperty<int>(ResourceIdProperty, resource.Id);
            LoadProperty<SmartDate>(AssignedProperty, Assignment.GetDefaultAssignedDate());
            LoadProperty<int>(RoleProperty, role);
            // Load our local ResourceInfo with values
            _resource = new ResourceInfo(resource.Id, resource.FirstName, resource.LastName);
            
            // Have to Mark New b/c the DataPortal is not involved.
            MarkNew();
        }

        #endregion

        #region  Data Access - Totally Commented

        //protected override void Child_Create()
        //{
        //    LoadProperty<SmartDate>(AssignedProperty, new SmartDate(System.DateTime.Today));
        //}

        //private void Child_Create(int resourceId, int role)
        //{
        //    var res = Resource.GetResource(resourceId);
        //    LoadProperty<int>(ResourceIdProperty, res.Id);
        //    LoadProperty<string>(LastNameProperty, res.LastName);
        //    LoadProperty<string>(FirstNameProperty, res.FirstName);
        //    LoadProperty<SmartDate>(AssignedProperty, Assignment.GetDefaultAssignedDate());
        //    LoadProperty<int>(RoleProperty, role);
        //}

        //private void Child_Fetch(ProjectTracker.DalLinq.Assignment data)
        //{
        //    LoadProperty<int>(ResourceIdProperty, data.ResourceId);
        //    LoadProperty<string>(LastNameProperty, data.Resource.LastName);
        //    LoadProperty<string>(FirstNameProperty, data.Resource.FirstName);
        //    LoadProperty<SmartDate>(AssignedProperty, data.Assigned);
        //    LoadProperty<int>(RoleProperty, data.Role);
        //    _timestamp = data.LastChanged.ToArray();
        //}

        //private void Child_Insert(Project project)
        //{
        //    _timestamp = Assignment.AddAssignment(
        //      project.Id,
        //      ReadProperty<int>(ResourceIdProperty),
        //      ReadProperty<SmartDate>(AssignedProperty),
        //      ReadProperty<int>(RoleProperty));
        //}

        //private void Child_Update(Project project)
        //{
        //    _timestamp = Assignment.UpdateAssignment(
        //      project.Id,
        //      ReadProperty<int>(ResourceIdProperty),
        //      ReadProperty<SmartDate>(AssignedProperty),
        //      ReadProperty<int>(RoleProperty),
        //      _timestamp);
        //}

        //private void Child_DeleteSelf(Project project)
        //{
        //    Assignment.RemoveAssignment(
        //      project.Id,
        //      ReadProperty<int>(ResourceIdProperty));
        //}

        #endregion
    }
}