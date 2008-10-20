using System;
using System.Collections.Generic;
using Csla;
using Csla.Data;
using Csla.Validation;
using Csla.Security;
using NHibernate.Criterion;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;
using CslaFactory = Csla.Server.ObjectFactoryAttribute;

namespace ProjectTracker.Library
{
    [CslaFactory("Factory Type=IBusinessBaseServerFactory;Item Type=ProjectTracker.Library.Resource, ProjectTracker.Library")]
    [DatabaseKey(Database.PTrackerDb)]
    [Serializable()]
    public class Resource : PTBusinessBase<Resource>
    {
        #region  Business Methods

        private byte[] _timestamp = new byte[8];

        private static PropertyInfo<int> IdProperty = RegisterProperty(new PropertyInfo<int>("Id"));
        public int Id
        {
            get
            {
                return GetProperty<int>(IdProperty);
            }
            set { SetProperty(IdProperty, value);}
        }

        private static PropertyInfo<string> LastNameProperty = RegisterProperty(new PropertyInfo<string>("LastName", "Last name"));
        private string _lastName = LastNameProperty.DefaultValue;
        public string LastName
        {
            get
            {
                return GetProperty<string>(LastNameProperty, _lastName);
            }
            set
            {
                SetProperty<string>(LastNameProperty, ref _lastName, value);
            }
        }

        private static PropertyInfo<string> FirstNameProperty = RegisterProperty(new PropertyInfo<string>("FirstName", "First name"));
        private string _firstName = FirstNameProperty.DefaultValue;
        public string FirstName
        {
            get
            {
                return GetProperty<string>(FirstNameProperty, _firstName);
            }
            set
            {
                SetProperty<string>(FirstNameProperty, ref _firstName, value);
            }
        }

        private static PropertyInfo<string> FullNameProperty = RegisterProperty(new PropertyInfo<string>("FullName", "Full name"));
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        private static PropertyInfo<ResourceAssignments> AssignmentsProperty = RegisterProperty(new PropertyInfo<ResourceAssignments>("Assignments"));
        public ResourceAssignments Assignments
        {
            get
            {
                if (!(FieldManager.FieldExists(AssignmentsProperty)))
                {
                    LoadProperty<ResourceAssignments>(AssignmentsProperty, ResourceAssignments.NewResourceAssignments());
                }
                return GetProperty<ResourceAssignments>(AssignmentsProperty);
            }
        }

        internal IList<ResourceAssignment> AssignmentsSet
        {
            get { return Assignments as IList<ResourceAssignment>; }
            set
            {
                foreach (ResourceAssignment assignment in value)
                {
                    if(!Assignments.Contains(assignment))
                        Assignments.Add(assignment);
                }
            }
        }
       

        public override string ToString()
        {
            return GetProperty<int>(IdProperty).ToString();
        }

        #endregion

        #region  Validation Rules

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringRequired, FirstNameProperty);
            ValidationRules.AddRule(
              Csla.Validation.CommonRules.StringMaxLength,
              new Csla.Validation.CommonRules.MaxLengthRuleArgs(FirstNameProperty, 50));

            ValidationRules.AddRule(Csla.Validation.CommonRules.StringRequired, LastNameProperty);
            ValidationRules.AddRule(
              Csla.Validation.CommonRules.StringMaxLength,
              new Csla.Validation.CommonRules.MaxLengthRuleArgs(LastNameProperty, 50));
        }

        #endregion

        #region  Authorization Rules

        protected override void AddAuthorizationRules()
        {
            // add AuthorizationRules here
            //AuthorizationRules.AllowWrite(LastNameProperty, "ProjectManager");
            //AuthorizationRules.AllowWrite(FirstNameProperty, "ProjectManager");
        }

        protected static void AddObjectAuthorizationRules()
        {
            // add object-level authorization rules here
            //AuthorizationRules.AllowCreate(typeof(Resource), "ProjectManager");
            //AuthorizationRules.AllowEdit(typeof(Resource), "ProjectManager");
            //AuthorizationRules.AllowDelete(typeof(Resource), "ProjectManager");
            //AuthorizationRules.AllowDelete(typeof(Resource), "Administrator");
        }

        #endregion

        #region  Factory Methods

        public static Resource NewResource()
        {
            return DataPortal.Create<Resource>();
        }

        public static void DeleteResource(int id)
        {
            DataPortal.Delete<Resource>(new SingleCriteria<Resource, int>(id));
        }

        public static Resource GetResource(int id)
        {
            return DataPortal.Fetch<Resource>(new SingleCriteria<Resource, int>(id));
        }

        private Resource()
        { /* require use of factory methods */ }

        #endregion

        

        #region  Exists

        public static bool Exists(int id)
        {
            return ExistsCommand.Exists(id);
        }

        [Serializable()]
        private class ExistsCommand : CommandBase
        {
            private int _id;
            private bool _exists;

            public bool ResourceExists
            {
                get
                {
                    return _exists;
                }
            }

            public static bool Exists(int id)
            {
                ExistsCommand result = null;
                result = DataPortal.Execute<ExistsCommand>(new ExistsCommand(id));
                return result.ResourceExists;
            }

            private ExistsCommand(int id)
            {
                _id = id;
            }

            protected override void DataPortal_Execute()
            {
                //using (var ctx = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
                //{
                //    _exists = (from p in ctx.DataContext.Resources
                //               where p.Id == _id
                //               select p).Count() > 0;
                //}
            }
        }

        #endregion

        private NHibernate.ICriteria _iCriteria = null;

        public override void SetNHibernateCriteria(object businessCriteria, NHibernate.ICriteria nhibernateCriteria)
        {
            // Cast the criteria back to the strongly-typed version
            SingleCriteria<Resource, int> criteria = businessCriteria as SingleCriteria<Resource, int>;

            // If it's a valid criteria object then check for filters
            if (!ReferenceEquals(criteria, null))
            {
                // Set a reference to the NHibernate ICriteria (for local use only)
                _iCriteria = nhibernateCriteria;

                AddCriterionName(criteria.Value);

            }
        }

        public override object GetObjectCriteriaValue(object businessCriteria)
        {
            // Cast the criteria back to the strongly-typed version
            SingleCriteria<Resource, int> criteria = businessCriteria as SingleCriteria<Resource, int>;

            // If it's a valid criteria object then check for filters
            if (!ReferenceEquals(criteria, null))
            {
                // Set a reference to the NHibernate ICriteria (for local use only)
                //_iCriteria = nhibernateCriteria;
                return criteria.Value;
            }
            return null;
        }

        /// <summary>
        /// Adds a criterion to the NHibernate <see cref="ICriteria"/> that filters by name.
        /// </summary>
        /// <param name="id">The name to use as a filter.</param>
        private void AddCriterionName(int id)
        {
            NHibernate.Criterion.SimpleExpression expression = Restrictions.Eq("Id", id);
            _iCriteria.Add(expression);
        }


    }
}