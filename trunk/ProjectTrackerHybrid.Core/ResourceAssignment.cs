using Csla;
using System;

namespace ProjectTracker.Library
{
    [Serializable()]
    public class ResourceAssignment : BusinessBase<ResourceAssignment>, IHoldRoles
    {
        #region Business Methods

        private byte[] _timestamp;
        /// <summary>
        /// Added internal TimeStamp accessor for NHibernate optimistic Concurrency
        /// </summary>
        internal byte[] TimeStamp
        {
            get { return _timestamp;}
            set { _timestamp = value;}
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
        ///  Required for NHibernate mapping so we get a reference to our parent object
        /// Set can be empty because we do not set our parent from the database.
        /// </summary>
        
        //internal Resource Resource
        //{
        //    get;// { ResourceAssignments myParent = Parent as ResourceAssignments;
        //        //return myParent.Resource;
        //    //}
        //    set;// { }
        //}

        //private static PropertyInfo<Guid> AssignmentIdProperty = RegisterProperty<Guid>(typeof(ResourceAssignment), new PropertyInfo<Guid>("AssignmentId", "Assignment Id"));
        //internal Guid AssignmentId
        //{
        //    get { return GetProperty<Guid>(AssignmentIdProperty); }
        //    set { SetProperty<Guid>(AssignmentIdProperty, value); }
        //}

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
            internal set { SetProperty(ProjectIdProperty, ref _projectId, value);}
        }

        private static PropertyInfo<string> ProjectNameProperty = RegisterProperty(new PropertyInfo<string>("ProjectName"));
        private string _projectName = ProjectNameProperty.DefaultValue;
        public string ProjectName
        {
            get
            {
                return Project.Name;
            }
            
        }

        private static PropertyInfo<SmartDate> AssignedProperty = RegisterProperty(new PropertyInfo<SmartDate>("Assigned"));
        private SmartDate _assigned = new SmartDate(System.DateTime.Today);
        public string Assigned
        {
            get
            {
                return GetPropertyConvert<SmartDate, string>(AssignedProperty, _assigned);
            }
            internal set { SetProperty(AssignedProperty, value);}
        }

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

        public override bool Equals(object obj)
        {
            if(obj is ResourceAssignment)
            {
                ResourceAssignment value = obj as ResourceAssignment;

                if(value.ProjectId == Project.Id)
                    if(value.ResourceId == ResourceId)
                        return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _projectId.ToString().GetHashCode(); // + ReadProperty(ResourceIdProperty).ToString()).GetHashCode();
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

        internal static ResourceAssignment NewResourceAssignment(Guid projectId)
        {
            return new ResourceAssignment(projectId, RoleList.DefaultRole());
        }

        internal static ResourceAssignment GetResourceAssignment(string test)
        {
            return DataPortal.FetchChild<ResourceAssignment>(test);
        }

        private ResourceAssignment()
        {
            /* require use of factory methods */
            MarkAsChild();
        }

        private ResourceAssignment(Guid projectId, int RoleId)
        {
            MarkAsChild();
            var project = Library.Project.GetProject(projectId);
            _projectName = project.Name;
            _projectId = project.Id;
            _project = new ProjectInfo(_projectId, _projectName);
            _assigned = Assignment.GetDefaultAssignedDate();
            _role = RoleId;
            MarkNew();
        }

        #endregion

        #region  Data Access

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