using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Csla;
using Csla.Server;
using ProjectTracker.Library;
using ProjectTracker.Library;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;
using ProjectTracker.Library.Framework.Factories;
using Rhino.Mocks;
using StructureMap.Attributes;
using StructureMap.Configuration;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectFactory = StructureMap.ObjectFactory;

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
        MockRepository _mocks = new MockRepository();
        IRepository<Product> _repository;

        [TestInitialize]
        public void Fixture_Setup()
        {
            // brute force attack to set my own factory via reflection
            var fieldInfo2 = typeof(UnitOfWork).GetField("_unitOfWorkFactory",
                                BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
            fieldInfo2.SetValue(null, Activator.CreateInstance(typeof(UnitOfWorkFactory), true));

            _repository = _mocks.DynamicMock<IRepository<Product>>();
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
            using(_mocks.Record())
            {
                Expect.Call(() => _repository.Load(product, 32)).IgnoreArguments();
            }
            using(_mocks.Playback())
            {
                product = _factory.Fetch(new SingleCriteria<Product, int>(32));
            }
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

            Product expected = new Product();
            expected.Name = "Product XYZ";

            Product product = _factory.Create();

            using(_mocks.Record())
            {
                Expect.Call(_repository.Save(null)).Return(expected).IgnoreArguments();
            }
            using(_mocks.Playback())
            {
                product.Name = "Test 123";
                _factory.Update(product);
            }
        }

        [TestMethod]
        public void BusinessBaseFactory_Can_Save_Existing_Object()
        {
            BusinessBaseServerFactory<Product> _factory = new BusinessBaseServerFactory<Product>(_repository);

            using (_mocks.Record())
            {
                Expect.Call(() => _repository.Update(null)).IgnoreArguments();
            }
            using (_mocks.Playback())
            {
                Product product = Product.GetOldProduct();
                _factory.Update(product);
            }
        }
        
    }

    [DatabaseKey("PTracker")]
    public class Product : PTBusinessBase<Product>
    {
        private static PropertyInfo<string> NameProperty = RegisterProperty<string>(typeof(Product), new PropertyInfo<string>("Name", "Name"));
        public string Name
        {
            get { return GetProperty<string>(NameProperty); }
            set { SetProperty<string>(NameProperty, value); }
        }

        public override object GetObjectCriteriaValue(object businessCriteria)
        {
            // Cast the criteria back to the strongly-typed version
            SingleCriteria<Product, int> criteria = businessCriteria as SingleCriteria<Product, int>;

            // If it's a valid criteria object then check for filters
            if (!ReferenceEquals(criteria, null))
            {
                // Set a reference to the NHibernate ICriteria (for local use only)
                //_iCriteria = nhibernateCriteria;
                return criteria.Value;
            }
            return null;
        }

        public static Product GetOldProduct()
        {
            Product product = new Product();
            product.MarkOld();
            return product;
        }
    }
}
