using Csla;
using System;

namespace ProjectTracker.Library
{
    [Serializable()]
    public class ResourceAssignment : BusinessBase<ResourceAssignment>, IHoldRoles
    {

        #region New/Modified Business Methods

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

        /// <summary>
        /// We have to maintain a reference to the other half of the Many-to-Many relationship here
        /// We are using ProjectInfo to reduce the amount of data coming from the database. If a <see cref="Library.Project"/> were
        /// used it would also load all of its child objects as well.
        /// </summary>
        [NotUndoable]
        private ProjectInfo _project;
        internal ProjectInfo Project
        {
            get { return _project; }
            set { _project = value; }
        }


        /// <summary>
        /// Added ResourceId Property for NHibernate
        /// </summary>
        private static PropertyInfo<int> ResourceIdProperty = RegisterProperty<int>(typeof(ResourceAssignment), new PropertyInfo<int>("ResourceId", "Resource Id"));
        internal int ResourceId
        {
            get { return GetProperty<int>(ResourceIdProperty); }
            set { SetProperty<int>(ResourceIdProperty, value); }
        }

        private static PropertyInfo<Guid> ProjectIdProperty = RegisterProperty(new PropertyInfo<Guid>("ProjectId", "Project id", Guid.Empty));
        private Guid _projectId = ProjectIdProperty.DefaultValue;
        public Guid ProjectId
        {
            get
            {
                return Project.Id;
            }
            internal set { SetProperty(ProjectIdProperty, ref _projectId, value); } // Added internal Set for NHibernate
        }

        private static PropertyInfo<SmartDate> AssignedProperty = RegisterProperty(new PropertyInfo<SmartDate>("Assigned"));
        //private SmartDate _assigned = new SmartDate(System.DateTime.Today);
        public string Assigned
        {
            get
            {
                return GetPropertyConvert<SmartDate, string>(AssignedProperty);
            }
            internal set { SetProperty(AssignedProperty, value); } // Added internal Set for NHibernate
        }

        public string ProjectName
        {
            get
            {
                return Project.Name;
            }

        }

        #region System.Object overrides -- These are needed for NHibernate mapping and relationships

        public override bool Equals(object obj)
        {
            if (obj is ResourceAssignment)
            {
                ResourceAssignment value = obj as ResourceAssignment;

                if (value.ProjectId == Project.Id)
                    if (value.ResourceId == ResourceId)
                        return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _projectId.ToString().GetHashCode(); // + ReadProperty(ResourceIdProperty).ToString()).GetHashCode();
        }

        #endregion

        #endregion

        #region Business Methods

        // Not needed since we have a reference to the ProjectInfo
        //private static PropertyInfo<string> ProjectNameProperty = RegisterProperty(new PropertyInfo<string>("ProjectName"));
        //private string _projectName = ProjectNameProperty.DefaultValue;
        //public string ProjectName
        //{
        //    get
        //    {
        //        return GetProperty<string>(ProjectNameProperty, _projectName);
        //    }
        //}

        private static PropertyInfo<int> RoleProperty = RegisterProperty(new PropertyInfo<int>("Role"));
        private int _role = RoleProperty.DefaultValue;
        public int Role
        {
            get
            {
                return GetProperty<int>(RoleProperty, _role);
            }
            set
            {
                SetProperty<int>(RoleProperty, ref _role, value);
            }
        }

        public Project GetProject()
        {
            CanExecuteMethod("GetProject", true);
            return Library.Project.GetProject(Project.Id);
        }

        public override string ToString()
        {
            return _projectId.ToString();
        }

        #endregion

        #region  Validation Rules - No Change

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(Assignment.ValidRole, RoleProperty);
        }

        #endregion

        #region  Authorization Rules - Temporarily Commented

        protected override void AddAuthorizationRules()
        {
            //AuthorizationRules.AllowWrite(RoleProperty, "ProjectManager");
        }

        #endregion

        #region  Factory Methods

        internal static ResourceAssignment NewResourceAssignment(Guid projectId)
        {
            // uses a private constructor now because the object factory does not do child objects
            return new ResourceAssignment(projectId, RoleList.DefaultRole());
        }

        // Not Needed
        //internal static ResourceAssignment GetResourceAssignment(string test)
        //{
        //    return DataPortal.FetchChild<ResourceAssignment>(test);
        //}

        private ResourceAssignment()
        {
            /* require use of factory methods */
            // Have to mark as child since we don't have the dataportal to do it for us
            MarkAsChild();
        }

        private ResourceAssignment(Guid projectId, int RoleId)
        {
            // Have to mark as child since we don't have the dataportal to do it for us
            MarkAsChild();
            var project = Library.Project.GetProject(projectId);
            _projectId = project.Id;
            _project = new ProjectInfo(_projectId, project.Name);
            LoadProperty(AssignedProperty, Assignment.GetDefaultAssignedDate());
            _role = RoleId;

            // Have to Mark New b/c the DataPortal is not involved.
            MarkNew();
        }

        #endregion

        #region  Data Access - Totally Commented

        //private void Child_Create(Guid projectId, int role)
        //{
        //    var proj = Project.GetProject(projectId);
        //    _projectId = proj.Id;
        //    _projectName = proj.Name;
        //    _assigned.Date = Assignment.GetDefaultAssignedDate();
        //    _role = role;
        //}

        //private void Child_Fetch(ProjectTracker.DalLinq.Assignment data)
        //{
        //    _projectId = data.ProjectId;
        //    _projectName = data.Project.Name;
        //    _assigned = data.Assigned;
        //    _role = data.Role;
        //    _timestamp = data.LastChanged.ToArray();
        //}

        //private void Child_Insert(Resource resource)
        //{
        //    _timestamp = Assignment.AddAssignment(_projectId, resource.Id, _assigned, _role);
        //}

        //private void Child_Update(Resource resource)
        //{
        //    _timestamp = Assignment.UpdateAssignment(_projectId, resource.Id, _assigned, _role, _timestamp);
        //}

        //private void Child_DeleteSelf(Resource resource)
        //{
        //    Assignment.RemoveAssignment(_projectId, resource.Id);
        //}

        #endregion

    }
}