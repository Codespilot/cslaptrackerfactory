using System;
using Csla;
using ProjectTracker.Library.Data;
using StructureMap;

namespace ProjectTracker.Library
{
    [Serializable]
    public class Customer : BusinessBase<Customer>, ICustomer
    {
        private IRepository<Customer> _repository;

        private static PropertyInfo<Guid> IdProperty = RegisterProperty<Guid>(typeof(Customer), new PropertyInfo<Guid>("Id", "Id"));
        public Guid Id
        {
            get { return GetProperty<Guid>(IdProperty); }
            set { SetProperty<Guid>(IdProperty, value); }
        }

        private static PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(typeof(Customer), new PropertyInfo<string>("FirstName", "First Name"));
        public string FirstName
        {
            get { return GetProperty<string>(FirstNameProperty); }
            set { SetProperty<string>(FirstNameProperty, value); }
        }

        private static PropertyInfo<string> LastNameProperty = RegisterProperty<string>(typeof(Customer), new PropertyInfo<string>("LastName", "Last Name"));
        public string LastName
        {
            get { return GetProperty<string>(LastNameProperty); }
            set { SetProperty<string>(LastNameProperty, value); }
        }

        private static PropertyInfo<string> AddressProperty = RegisterProperty<string>(typeof(Customer), new PropertyInfo<string>("Address", "Address"));
        public string Address
        {
            get { return GetProperty<string>(AddressProperty); }
            set { SetProperty<string>(AddressProperty, value); }
        }

        #region Factory Methods

        public static Customer NewCustomer()
        {
            return DataPortal.Create<Customer>();
        }

        public static Customer GetCustomer(Guid id)
        {
            return DataPortal.Fetch<Customer>(new SingleCriteria<Customer, Guid>(id));
        }

        public static void DeleteCustomer(Guid id)
        {
            DataPortal.Delete(new SingleCriteria<Customer, Guid>(id));
        }

        private Customer()
        {  /* Require use of factory methods */

            _repository = ObjectFactory.GetInstance<IRepository<Customer>>();
        }

        public static Customer NewCustomerChild()
        {
            return DataPortal.CreateChild<Customer>();
        }

        public static Customer GetCustomerChild(Guid id)
        {
            return DataPortal.FetchChild<Customer>(new SingleCriteria<Customer, Guid>(id));
        }

        #endregion

        #region Data Access

        //private void DataPortal_Fetch(SingleCriteria<Customer, Guid> criteria)
        //{
        //    // TODO: load values into object
        //}

        //protected override void DataPortal_Insert()
        //{
        //    using (UnitOfWork.Start())
        //    {
        //        UnitOfWork.CurrentSession.Save(this);
        //        UnitOfWork.Current.TransactionalFlush();
        //        UnitOfWork.CurrentSession.Clear();
        //    }
        //}

        //protected override void DataPortal_Update()
        //{
        //    using (UnitOfWork.Start())
        //    {
        //        UnitOfWork.CurrentSession.Update(this);
        //        UnitOfWork.Current.TransactionalFlush();
        //        UnitOfWork.CurrentSession.Clear();
        //    }
        //}

        private void Child_Fetch()
        {

        }

        private void Child_Insert()
        {

            if (_repository != null) _repository.Save(this);
        }

        private void Child_Update()
        {
            if (_repository != null) _repository.SaveOrUpdate(this);
        }



        #endregion
    }
}
