using System;
using Csla;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework.Factories;
using Rhino.Mocks;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;

#else
using NUnit.Framework;
using ObjectFactory=StructureMap.ObjectFactory;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif 

namespace ProjectTracker.Library.Tests.FactoryFixtures
{
    [TestClass]
    public class BusinessBaseFactoryFixture : FixtureBase
    {
        IRepository<Product> _repository;

        [TestInitialize]
        public void Setup()
        {
            SetupTest();

            _repository = MockRepository.GenerateStub<IRepository<Product>>();
        }

        [TestCleanup]
        public void TearDown()
        {
            disposeGlobalUnitOfWorkRegistration.Dispose();
        }


        [TestMethod]
        public void Can_Create_Factory()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            Assert.IsNotNull(_factory);
        }

        [TestMethod]
        public void BusinessBaseFactory_Can_Create_Object()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            Product product = _factory.Create();

            Assert.IsNotNull(product);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), "You must pass critiria to load a Root Object")]
        public void BusinessBaseFactory_Fetch_No_Criteria_Throws()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);
            Product product = _factory.Fetch();
        }

        [TestMethod]
        public void BusinessBaseFactory_Fetch_WithCriteria_ReturnsObject()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            Product product = _factory.Create();
            product = _factory.Fetch(new SingleCriteria<Product, int>(32));

            _repository.AssertWasCalled(x => x.Load(product, 32));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), "You must pass critiria to load a Root Object")]
        public void BusinessBaseFactory_Fetch_WithNull_Critiria_Throws()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            Product product = _factory.Create();
            product = _factory.Fetch(null);
        }

        [TestMethod]
        public void BusinessBaseFactory_Can_Save_New_Object()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            Product product = _factory.Create();
            product.Name = "Test 123";
            _factory.Update(product);

            _repository.AssertWasCalled(x => x.Save(product));
            unitOfWorkStub.AssertWasCalled(x => x.TransactionalFlush());
        }

        [TestMethod]
        public void BusinessBaseFactory_Can_Save_Existing_Object()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            Product product = Product.GetOldProduct();
            product.Name = "Updated Data";
            _factory.Update(product);

            _repository.AssertWasCalled(x => x.Update(product));
            unitOfWorkStub.AssertWasCalled(x => x.TransactionalFlush());
        }

        [TestMethod]
        public void BusinessBaseFactory_Calls__Repository_Delete_with_Criteria()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            _factory.Delete(new SingleCriteria<Product, int>(32));

            _repository.AssertWasCalled(x => x.Load(null, null), y => y.IgnoreArguments());
            _repository.AssertWasCalled(x => x.Delete(null),y=> y.IgnoreArguments());
            unitOfWorkStub.AssertWasCalled(x => x.TransactionalFlush());
        }

        [TestMethod]
        public void BusinessBaseFactory_Calls_Repository_Delete_with_Object()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            Product product = Product.GetOldProduct();
            _factory.Delete(product);

            _repository.AssertWasCalled(x => x.Delete(product));
            unitOfWorkStub.AssertWasCalled(x => x.TransactionalFlush());
        }

        [TestMethod]
        public void BusinessBaseFactory_Update_DeletedObject_Calls_Delete()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            Product product = Product.GetOldProduct();
            product.MarkDeleted();
            _factory.Update(product);

            _repository.AssertWasCalled(x => x.Delete(product));
            unitOfWorkStub.AssertWasCalled(x => x.TransactionalFlush());
        }

    }
}
