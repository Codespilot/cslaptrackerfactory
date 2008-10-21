﻿using Csla;
using Csla.Data;
using System;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;
using CslaFactory = Csla.Server.ObjectFactoryAttribute;

namespace ProjectTracker.Library.Admin
{
    // Note new attributes and signature change of class
    [CslaFactory("Factory Type=IBusinessBaseServerFactory;Item Type=ProjectTracker.Library.Admin.Role, ProjectTracker.Library")]
    [DatabaseKey(Database.PTrackerDb)]
    [Serializable()]
    public class Role : PTBusinessBase<Role>
    {

        #region  Business Methods

        private static PropertyInfo<int> IdProperty = RegisterProperty<int>(typeof(Role), new PropertyInfo<int>("Id"));
        private bool _idSet;
        public int Id
        {
            get
            {
                if (!_idSet)
                {
                    SetProperty<int>(IdProperty, GetMax() + 1);
                }
                return GetProperty<int>(IdProperty);
            }
            set
            {
                _idSet = true;
                SetProperty<int>(IdProperty, value);
            }
        }

        private int GetMax()
        {

            // generate a default id value
            _idSet = true;
            Roles parent = (Roles)this.Parent;
            int max = 0;
            foreach (Role item in parent)
            {
                if (item.Id > max)
                {
                    max = item.Id;
                }
            }
            return max;

        }

        private static PropertyInfo<string> NameProperty = RegisterProperty<string>(typeof(Role), new PropertyInfo<string>("Name"));
        public string Name
        {
            get
            {
                return GetProperty(NameProperty);
            }
            set
            {
                SetProperty<string>(NameProperty, value);
            }
        }

        private byte[] _timestamp = new byte[8];

        #endregion

        #region  Validation Rules

        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule<Role>(NoDuplicates, IdProperty);
            ValidationRules.AddRule(
              Csla.Validation.CommonRules.StringRequired, NameProperty);
        }

        private static bool NoDuplicates<T>(T target, Csla.Validation.RuleArgs e) where T : Role
        {
            Roles parent = (Roles)target.Parent;
            if (parent != null)
            {
                foreach (Role item in parent)
                {
                    if (item.Id == target.GetProperty<int>(IdProperty) && !(ReferenceEquals(item, target)))
                    {
                        e.Description = "Role Id must be unique";
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #region  Authorization Rules - Commented Out Temporarily

        protected override void AddAuthorizationRules()
        {
            //AuthorizationRules.AllowWrite(IdProperty, "Administrator");
            //AuthorizationRules.AllowWrite(NameProperty, "Administrator");
        }

        #endregion

        #region  Factory Methods - Partially Commented

        internal static Role NewRole()
        {
            return DataPortal.CreateChild<Role>();
        }

        //internal static Role GetRole(string rolename)
        //{
        //    return DataPortal.FetchChild<Role>(rolename);
        //}
        

      
        private Role()
        {
            /* require use of factory methods */
            MarkAsChild();
            
        }

        #endregion

        #region  Data Access - Totally Commented Out

        //private void Child_Fetch(ProjectTracker.DalLinq.getRolesResult data)
        //{
        //    LoadProperty<int>(IdProperty, data.Id);
        //    _idSet = true;
        //    LoadProperty<string>(NameProperty, data.Name);
        //    _timestamp = data.LastChanged.ToArray();
        //}

        //private void Child_Insert()
        //{
        //    using (var mgr = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    {
        //        System.Data.Linq.Binary lastChanged = _timestamp;
        //        mgr.DataContext.addRole(ReadProperty<int>(IdProperty), ReadProperty<string>(NameProperty), ref lastChanged);
        //        _timestamp = lastChanged.ToArray();
        //    }
        //}

        //private void Child_Update()
        //{
        //    using (var mgr = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    {
        //        System.Data.Linq.Binary lastChanged = null;
        //        mgr.DataContext.updateRole(ReadProperty<int>(IdProperty), ReadProperty<string>(NameProperty), _timestamp, ref lastChanged);
        //        _timestamp = lastChanged.ToArray();
        //    }
        //}

        //private void Child_DeleteSelf()
        //{
        //    using (var mgr = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    {
        //        mgr.DataContext.deleteRole(Id);
        //    }
        //}

        #endregion

    }

}