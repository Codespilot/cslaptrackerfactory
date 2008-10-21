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
    // Note new attributes and signature change of class
    [CslaFactory("Factory Type=IBusinessBaseServerFactory;Item Type=ProjectTracker.Library.Resource, ProjectTracker.Library")]
    [DatabaseKey(Database.PTrackerDb)]
    [Serializable()]
    public class Resource : PTBusinessBase<Resource>
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

        private static PropertyInfo<int> IdProperty = RegisterProperty(new PropertyInfo<int>("Id"));
        public int Id
        {
            get { return GetProperty<int>(IdProperty); }
            internal set { SetProperty(IdProperty, value); } // Added for NHibernate
        }

        // Added for NHibernate mapping
        internal IList<ResourceAssignment> AssignmentsSet
        {
            get { return Assignments as IList<ResourceAssignment>; }
            set
            {
                foreach (ResourceAssignment assignment in value)
                {
                    if (!Assignments.Contains(assignment))
                        Assignments.Add(assignment);
                }
            }
        }
        #endregion

        #region  Business Methods

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

        public override string ToString()
        {
            return GetProperty<int>(IdProperty).ToString();
        }

        #endregion

        #region  Validation Rules - No Change

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

        #region  Authorization Rules - Commented Temporarily

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

        #region  Factory Methods - No Change

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

        #region  Data Access - Totally Commented

        //private void DataPortal_Fetch(SingleCriteria<Resource, int> criteria)
        //{
        //    using (var ctx = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    {
        //        var data = (from r in ctx.DataContext.Resources
        //                    where r.Id == criteria.Value
        //                    select r).Single();
        //        _id = data.Id;
        //        _lastName = data.LastName;
        //        _firstName = data.FirstName;
        //        _timestamp = data.LastChanged.ToArray();

        //        LoadProperty<ResourceAssignments>(AssignmentsProperty, ResourceAssignments.GetResourceAssignments(data.Assignments.ToArray()));
        //    }
        //}

        //[Transactional(TransactionalTypes.TransactionScope)]
        //protected override void DataPortal_Insert()
        //{
        //    using (var ctx = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    {
        //        int? newId = null;
        //        System.Data.Linq.Binary newLastChanged = null;
        //        ctx.DataContext.addResource(_lastName, _firstName, ref newId, ref newLastChanged);
        //        _id = System.Convert.ToInt32(newId);
        //        _timestamp = newLastChanged.ToArray();
        //        FieldManager.UpdateChildren(this);
        //    }
        //}

        //[Transactional(TransactionalTypes.TransactionScope)]
        //protected override void DataPortal_Update()
        //{
        //    using (var ctx = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    {
        //        if (IsSelfDirty)
        //        {
        //            System.Data.Linq.Binary newLastChanged = null;
        //            ctx.DataContext.updateResource(_id, _lastName, _firstName, _timestamp, ref newLastChanged);
        //            _timestamp = newLastChanged.ToArray();
        //        }
        //        FieldManager.UpdateChildren(this);
        //    }
        //}

        //[Transactional(TransactionalTypes.TransactionScope)]
        //protected override void DataPortal_DeleteSelf()
        //{
        //    DataPortal_Delete(new SingleCriteria<Resource, int>(_id));
        //}

        //[Transactional(TransactionalTypes.TransactionScope)]
        //private void DataPortal_Delete(SingleCriteria<Resource, int> criteria)
        //{
        //    using (var ctx = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    {
        //        ctx.DataContext.deleteResource(criteria.Value);
        //    }
        //}

        #endregion

        #region  Exists - Not Converted Yet

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

        #region Base Class Overrides / NHibernate Helpers

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

        #endregion

    }
}