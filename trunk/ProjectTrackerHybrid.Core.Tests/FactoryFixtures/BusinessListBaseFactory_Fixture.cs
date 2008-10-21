using Csla;
using NHibernate.Criterion;
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
    public class BusinessListBaseFactory_Fixture : FixtureBase
    {
        IRepository<Product> _repository;
        NHibernate.ICriteria _criteria;
        private BusinessListBaseServerFactory<ProductList, Product> _factory;
        

        [TestInitialize]
        public void Test_Setup()
        {
            SetupTest();
            _repository = MockRepository.GenerateStub<IRepository<Product>>(); //_mocks.DynamicMock<IRepository<Product>>();
            _factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            base.CleanupTest();

        }

        [TestMethod]
        public void BusinessListBaseFactory_Can_Create_Factory()
        {
            BusinessListBaseServerFactory<ProductList, Product> factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);

            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void BusinessListBaseFactory_Can_Create_Object()
        {
            ProductList products = _factory.Create();

            Assert.IsNotNull(products);
        }

        [TestMethod]
        public void BusinessListBaseFactory_Fetch_No_Criteria()
        {
            ProductList list = new ProductList();
            list.Add(new Product() { Name = "Test" });
            list.Add(new Product() { Name = "Test1" });
            list.Add(new Product() { Name = "Test2" });

            ProductList products = _factory.Create();
            
            _repository.Expect(x => x.FindAll(DetachedCriteria.For(typeof (Product)))).IgnoreArguments().Return(list);
            _repository.Replay();

            products = _factory.Fetch();
            
            _repository.AssertWasCalled(x => x.FindAll(DetachedCriteria.For(typeof(Product))), r => r.IgnoreArguments());
            Assert.AreEqual(3, products.Count);
        }

        [TestMethod]
        public void BusinessListBaseFactory_Fetch_WithCriteria_ReturnsObject()
        {
            _criteria = MockRepository.GenerateStub<NHibernate.ICriteria>();

            ProductList list = new ProductList();
            list.Add(new Product() { Name = "Test" });
            list.Add(new Product() { Name = "Test1" });
            list.Add(new Product() { Name = "Test2" });

            NHibernate.Criterion.SimpleExpression expression = Restrictions.Eq("Name", "Test");
            _criteria.Expect(c => c.Add(expression)).Return(_criteria);
            _repository.Expect(r => r.CreateCriteria()).Return(_criteria);
            _criteria.Expect(c => c.List<Product>()).Return(list);

            ProductList products = _factory.Fetch(new SingleCriteria<ProductList, string>("Test"));
            
            _repository.AssertWasCalled(r => r.CreateCriteria());
            _criteria.AssertWasCalled(c => c.List<Product>());
            Assert.AreEqual(3, products.Count);
        }

        [TestMethod]
        public void BusinessListBaseFactory_Fetch_Null_Criteria_ReturnsList()
        {
            ProductList list = new ProductList();
            list.Add(new Product() { Name = "Test" });
            list.Add(new Product() { Name = "Test1" });
            list.Add(new Product() { Name = "Test2" });

            ProductList products = _factory.Create();

            _repository.Expect(x => x.FindAll(DetachedCriteria.For(typeof(Product)))).IgnoreArguments().Return(list);
            _repository.Replay();

            products = _factory.Fetch(null);

            _repository.AssertWasCalled(x => x.FindAll(DetachedCriteria.For(typeof(Product))), r => r.IgnoreArguments());
            Assert.AreEqual(3, products.Count);
        }

        [TestMethod]
        public void BusinessListBaaseFactory_Save_New_items()
        {
            ProductList list = new ProductList();
            list.Add(new Product() { Name = "Test" });
            list.Add(new Product() { Name = "Test1" });
            list.Add(new Product() { Name = "Test2" });

            ProductList products = _factory.Create();

            products = _factory.Update(list);

            _repository.AssertWasCalled(x => x.Save(null), r => r.IgnoreArguments().Repeat.Times(3));
            unitOfWorkStub.AssertWasCalled(x => x.TransactionalFlush());
        }

        [TestMethod]
        public void BusinessListBaseFactory_Update_Existing_items()
        {
            ProductList list = ProductList.GetOldProducts();

            Product product = new Product() { Name = "Saved Project" };

            _repository.Expect(r => r.SaveOrUpdate(null)).IgnoreArguments().Return(product);
            _factory.Update(list);

            // Only one item in the list should be dirty
            _repository.AssertWasCalled(x => x.SaveOrUpdate(null), r => r.IgnoreArguments().Repeat.Times(1));
            unitOfWorkStub.AssertWasCalled(x => x.TransactionalFlush());
        }

        [TestMethod]
        public void BusinessListBase_Update_Delete_removed_Items()
        {
            ProductList list = ProductList.GetOldProducts();

            list.Remove("Updated Project");

            _factory.Update(list);

            _repository.AssertWasCalled(x => x.Delete(null), r => r.IgnoreArguments().Repeat.Times(1));
            unitOfWorkStub.AssertWasCalled(x => x.TransactionalFlush());
        }
    }
}
