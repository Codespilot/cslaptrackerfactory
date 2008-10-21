using Csla;
using Csla.C5;
using Csla.Data;
using Csla.Security;
using System;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;
using CslaFactory = Csla.Server.ObjectFactoryAttribute;

namespace ProjectTracker.Library.Admin
{

    /// <summary>
    /// Used to maintain the list of roles
    /// in the system.
    /// </summary>
    // Note new attributes and signature change of class
    [CslaFactory("Factory Type=IBusinessListBaseServerFactory;List Type=ProjectTracker.Library.Admin.Roles;Item Type=ProjectTracker.Library.Admin.Role, ProjectTracker.Library")]
    [DatabaseKey(Database.PTrackerDb)]
    [Serializable()]
    public class Roles : PTBusinessListBase<Roles, Role>
    {

        #region  Business Methods

        /// <summary>
        /// Remove a role based on the role's
        /// id value.
        /// </summary>
        /// <param name="id">Id value of the role to remove.</param>
        public void Remove(int id)
        {
            foreach (Role item in this)
            {
                if (item.Id == id)
                {
                    Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// Get a role bsaed on its id value.
        /// </summary>
        /// <param name="id">Id value of the role to return.</param>
        public Role GetRoleById(int id)
        {
            foreach (Role item in this)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
            
        }

        protected override object AddNewCore()
        {
            Role item = Role.NewRole();
            Add(item);
            return item;
        }

        #endregion

        #region  Authorization Rules - Commented Out Temporarily

        protected static void AddObjectAuthorizationRules()
        {
            // add object-level authorization rules here
            //AuthorizationRules.AllowCreate(typeof(Roles), "Administrator");
            //AuthorizationRules.AllowEdit(typeof(Roles), "Administrator");
            //AuthorizationRules.AllowDelete(typeof(Roles), "Administrator");
        }

        #endregion

        #region  Factory Methods

        public static Roles GetRoles()
        {
            return DataPortal.Fetch<Roles>();
        }

        private Roles()
        {
            this.Saved += new EventHandler<Csla.Core.SavedEventArgs>(Roles_Saved);
            this.AllowNew = true;
            this.AllowRemove = true;
        }

        #endregion

        #region  Data Access - Partially Commented

        private void Roles_Saved(object sender, Csla.Core.SavedEventArgs e)
        {
            // this runs on the client and invalidates
            // the RoleList cache
            RoleList.InvalidateCache();
        }

        protected override void DataPortal_OnDataPortalInvokeComplete(Csla.DataPortalEventArgs e)
        {
            if (ApplicationContext.ExecutionLocation == ApplicationContext.ExecutionLocations.Server &&
                e.Operation == DataPortalOperations.Update)
            {
                // this runs on the server and invalidates
                // the RoleList cache
                RoleList.InvalidateCache();
            }
        }

        //private void DataPortal_Fetch()
        //{
        //    //this.RaiseListChangedEvents = false;
        //    //using (var ctx = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    //{
        //    //    foreach (var value in ctx.DataContext.getRoles())
        //    //        this.Add(Role.GetRole(value));
        //    //}
        //    //this.RaiseListChangedEvents = true;
        //}

        //[Transactional(TransactionalTypes.TransactionScope)]
        //protected override void DataPortal_Update()
        //{
        //    //this.RaiseListChangedEvents = false;
        //    //using (var ctx = ContextManager<ProjectTracker.DalLinq.PTrackerDataContext>.GetManager(ProjectTracker.DalLinq.Database.PTracker))
        //    //{
        //    //    Child_Update();
        //    //}
        //    //this.RaiseListChangedEvents = true;
        //}

        #endregion

    }
}