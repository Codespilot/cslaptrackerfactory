using System;
using System.Collections.Generic;
using System.Text;
using Csla;
using NHibernate.Criterion;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;
using StructureMap;
using CslaFactory = Csla.Server.ObjectFactoryAttribute;
using Csla.Security;

namespace ProjectTracker.Library
{
    // NOTICE THE NEW ATTRIBUTES AND THE CLASS SIGNATURE HAS CHANGED
    [CslaFactory("Factory Type=IBusinessBaseServerFactory;Item Type=ProjectTracker.Library.Project, ProjectTracker.Library")]
    [DatabaseKey(Database.PTrackerDb)]
    [Serializable]
    public class Project :  PTBusinessBase<Project>
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

        private static PropertyInfo<Guid> IdProperty = RegisterProperty<Guid>(typeof(Project), new PropertyInfo<Guid>("Id"));
        // Not Needed [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get { return GetProperty<Guid>(IdProperty); }
            internal set { SetProperty<Guid>(IdProperty, value); } // We need to have a Set for NHibernate, this can be internal though
        }

        /// <summary>
        /// New Property/Field combination for NHibernate. NHibernate only wants to load and set an IList object
        /// so that is what we feed it.
        /// </summary>
        [NotUndoable]
        private IList<ProjectResource> _resourcesSet = ProjectResources.NewProjectResources();
        public IList<ProjectResource> ResourcesSet
        {
            get { return Resources as IList<ProjectResource>; }
            set { Resources.Add(value); }
        }

        #endregion

        #region  Business Methods

        private static PropertyInfo<string> NameProperty = RegisterProperty(new PropertyInfo<string>("Name"));
        public string Name
        {
            get { return GetProperty<string>(NameProperty); }
            set { SetProperty<string>(NameProperty, value); }
        }

        private static PropertyInfo<SmartDate> StartedProperty = RegisterProperty(new PropertyInfo<SmartDate>("Started"));
        public string Started
        {
            get { return GetPropertyConvert<SmartDate,string>(StartedProperty); }
            set { SetPropertyConvert<SmartDate, string>(StartedProperty, value); }
        }

        private static PropertyInfo<SmartDate> EndedProperty = RegisterProperty(new PropertyInfo<SmartDate>("Ended", new SmartDate(SmartDate.EmptyValue.MaxDate)));
        public string Ended
        {
            get { return GetPropertyConvert<SmartDate, string>(EndedProperty); }
            set { SetPropertyConvert<SmartDate, string>(EndedProperty, value); }
        }

        private static PropertyInfo<string> DescriptionProperty = RegisterProperty(new PropertyInfo<string>("Description"));
        public string Description
        {
            get { return GetProperty<string>(DescriptionProperty); }
            set { SetProperty<string>(DescriptionProperty, value); }
        }

        private static PropertyInfo<ProjectResources> ResourcesProperty = RegisterProperty(new PropertyInfo<ProjectResources>("Resources"));
        public ProjectResources Resources
        {
            get
            {
                if (!(FieldManager.FieldExists(ResourcesProperty)))
                {
                    LoadProperty<ProjectResources>(ResourcesProperty, ProjectResources.NewProjectResources());
                }
                return GetProperty<ProjectResources>(ResourcesProperty);
            }
        }
        
        public override string ToString()
        {
            return Id.ToString();
        }

        #endregion

        #region  Validation Rules

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringRequired,
                                    new Csla.Validation.RuleArgs(NameProperty));
            ValidationRules.AddRule(Csla.Validation.CommonRules.StringMaxLength,
                                    new Csla.Validation.CommonRules.MaxLengthRuleArgs(NameProperty, 50));

            ValidationRules.AddRule<Project>(StartDateGTEndDate<Project>, StartedProperty);
            ValidationRules.AddRule<Project>(StartDateGTEndDate<Project>, EndedProperty);

            ValidationRules.AddDependentProperty(StartedProperty, EndedProperty, true);
        }

        private static bool StartDateGTEndDate<T>(T target, Csla.Validation.RuleArgs e) where T : Project
        {
            if (target.GetProperty<SmartDate>(StartedProperty) > target.GetProperty<SmartDate>(EndedProperty))
            {
                e.Description = "Start date can't be after end date";
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region  Authorization Rules - Commented Out Temporarily

        protected override void AddAuthorizationRules()
        {
            // add AuthorizationRules here
            //AuthorizationRules.AllowWrite(NameProperty, "ProjectManager");
            //AuthorizationRules.AllowWrite(StartedProperty, "ProjectManager");
            //AuthorizationRules.AllowWrite(EndedProperty, "ProjectManager");
            //AuthorizationRules.AllowWrite(DescriptionProperty, "ProjectManager");
        }

        protected static void AddObjectAuthorizationRules()
        {
            // add object-level authorization rules here
            //AuthorizationRules.AllowCreate(typeof(Project), "ProjectManager");
            //AuthorizationRules.AllowEdit(typeof(Project), "ProjectManager");
            //AuthorizationRules.AllowDelete(typeof(Project), "ProjectManager");
            //AuthorizationRules.AllowDelete(typeof(Project), "Administrator");
        }

        #endregion

        #region  Factory Methods - No Change

        public static Project NewProject()
        {
            return DataPortal.Create<Project>();
        }

        public static Project GetProject(Guid id)
        {
            return DataPortal.Fetch<Project>(new SingleCriteria<Project, Guid>(id));
        }

        public static void DeleteProject(Guid id)
        {
            DataPortal.Delete(new SingleCriteria<Project, Guid>(id));
        }

        private Project()
        { /* require use of factory methods */ }

        #endregion

        #region Data Access - Totally Commented Out

        //[RunLocal]
        //protected override void DataPortal_Create()
        //{
        //    //_repository = ObjectFactory.GetInstance < IRepository<Project>>();

        //}

        //private void DataPortal_Fetch(SingleCriteria<Project, Guid> criteria)
        //{
        //    using (UnitOfWork.Start())
        //    {
        //        //    UnitOfWork.CurrentSession.Load(this,criteria.Value);

        //        //    MarkOld();




        //        if (criteria != null) _repository.Load(this,criteria.Value);
        //    }
        //}

        //protected override void DataPortal_Insert()
        //{
        //    using (UnitOfWork.Start())
        //    {

        //        DataPortal.UpdateChild(ReadProperty(CustomerProperty), this);
        //        if (_repository != null) _repository.Save(this);
        //        //UnitOfWork.CurrentSession.Save(this);
        //        DataPortal.UpdateChild(ReadProperty(TasksProperty), this);
        //        UnitOfWork.Current.TransactionalFlush();
        //        UnitOfWork.CurrentSession.Clear();


        //    }
        //}



        //protected override void DataPortal_Update()
        //{
        //    using (UnitOfWork.Start())
        //    {
        //        DataPortal.UpdateChild(ReadProperty(CustomerProperty), this);
        //        if (_repository != null) _repository.SaveOrUpdate(this);

        //        DataPortal.UpdateChild(ReadProperty(TasksProperty), this);

        //        UnitOfWork.Current.TransactionalFlush();
        //        UnitOfWork.CurrentSession.Clear();
        //    }
        //}

        //protected override void DataPortal_DeleteSelf()
        //{
        //    DataPortal_Delete(new SingleCriteria<Project, Guid>(Id));
        //}

        //private void DataPortal_Delete(SingleCriteria<Project, Guid> criteria)
        //{
        //    // TODO: delete object's data
        //}

        #endregion

        #region Base Overrides / NHibernate Helpers

        private NHibernate.ICriteria _iCriteria = null;

        public override void SetNHibernateCriteria(object businessCriteria, NHibernate.ICriteria nhibernateCriteria)
        {
            // Cast the criteria back to the strongly-typed version
            SingleCriteria<Project, Guid> criteria = businessCriteria as SingleCriteria<Project, Guid>;

            // If it's a valid criteria object then check for filters
            if (!ReferenceEquals(criteria, null))
            {
                // Set a reference to the NHibernate ICriteria (for local use only)
                _iCriteria = nhibernateCriteria;

                AddCriterionName(criteria.Value);
              
            }
        }

        /// <summary>
        /// Returns the criteria value of a SingleCriteria type for this class
        /// </summary>
        /// <param name="businessCriteria"></param>
        /// <returns></returns>
        public override object GetObjectCriteriaValue(object businessCriteria)
        {
            // Cast the criteria back to the strongly-typed version
            SingleCriteria<Project, Guid> criteria = businessCriteria as SingleCriteria<Project, Guid>;

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
        private void AddCriterionName(Guid id)
        {
            NHibernate.Criterion.SimpleExpression expression = Restrictions.Eq("Id", id);
            _iCriteria.Add(expression);
        }

        #endregion

    }
}
