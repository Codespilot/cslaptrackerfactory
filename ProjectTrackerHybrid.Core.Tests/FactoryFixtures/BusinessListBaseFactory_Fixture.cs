using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Csla;
using Csla.Server;
using NHibernate.Criterion;
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
    public class BusinessListBaseFactory_Fixture : FixtureBase
    {
        MockRepository _mocks = new MockRepository();
        IRepository<Product> _repository;

        [TestInitialize]
        public void Fixture_Setup()
        {
            /// This is not needed now, I have set all the UnitOfWorkTests to be ignored so we won't have issues with this.
            //var fieldInfo2 = typeof(UnitOfWork).GetField("_unitOfWorkFactory",
            //                    BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
            //fieldInfo2.SetValue(null, Activator.CreateInstance(typeof(UnitOfWorkFactory), true));

            _repository = _mocks.DynamicMock<IRepository<Product>>();
        }

        [TestMethod]
        public void BusinessListBaseFactory_Can_Create_Factory()
        {
            BusinessListBaseServerFactory<ProductList, Product> _factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);

            Assert.IsNotNull(_factory);
        }

        [TestMethod]
        public void BusinessListBaseFactory_Can_Create_Object()
        {
            BusinessListBaseServerFactory<ProductList, Product> _factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);

            ProductList products = _factory.Create();

            Assert.IsNotNull(products);
        }

        [TestMethod]
        public void BusinessBaseFactory_Fetch_No_Criteria()
        {
            BusinessListBaseServerFactory<ProductList, Product> _factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);

            ProductList list = new ProductList();
            list.Add(new Product() { Name = "Test" });
            list.Add(new Product() { Name = "Test1" });
            list.Add(new Product() { Name = "Test2" });

            ProductList products = _factory.Create();
            using (_mocks.Record())
            {
                Expect.Call(_repository.FindAll(DetachedCriteria.For(typeof(Product)))).IgnoreArguments().Return(list);
            }
            using (_mocks.Playback())
            {
                products = _factory.Fetch();
            }
            Assert.AreEqual(3, products.Count);
        }

        [TestMethod]
        public void BusinessBaseFactory_Fetch_WithCriteria_ReturnsObject()
        {
            BusinessListBaseServerFactory<ProductList, Product> _factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);
            _criteria = _mocks.DynamicMock<NHibernate.ICriteria>();

            ProductList list = new ProductList();
            list.Add(new Product() { Name = "Test" });
            list.Add(new Product() { Name = "Test1" });
            list.Add(new Product() { Name = "Test2" });
            
            NHibernate.Criterion.SimpleExpression expression = Restrictions.Eq("Name", "Test");
            SetupResult.For(_criteria.Add(expression)).Return(_criteria);

            ProductList _returnValues = new ProductList();
            ProductList products = _factory.Create();
            using (_mocks.Record())
            {
                Expect.Call(_repository.CreateCriteria()).Return(_criteria);//.Do(getCriteria());
                Expect.Call(_criteria.List <Product>()).Return(list);
            }
            using (_mocks.Playback())
            {
                products = _factory.Fetch(new SingleCriteria<ProductList, string>("Test"));
            }

            Assert.AreEqual(3, products.Count);
        }

        [TestMethod]
        public void BusinessListBaseFactory_Fetch_Null_Criteria_ReturnsList()
        {
            BusinessListBaseServerFactory<ProductList, Product> _factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);

            ProductList list = new ProductList();
            list.Add(new Product() { Name = "Test" });
            list.Add(new Product() { Name = "Test1" });
            list.Add(new Product() { Name = "Test2" });

            ProductList products = _factory.Create();
            using (_mocks.Record())
            {
                Expect.Call(_repository.FindAll(DetachedCriteria.For(typeof(Product)))).IgnoreArguments().Return(list);
            }
            using (_mocks.Playback())
            {
                products = _factory.Fetch(null);
            }
            Assert.AreEqual(3, products.Count);
        }

        [TestMethod]
        public void BusinessListBaaseFactory_Save_New_items()
        {
            BusinessListBaseServerFactory<ProductList, Product> _factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);

            ProductList list = new ProductList();
            list.Add(new Product() { Name = "Test" });
            list.Add(new Product() { Name = "Test1" });
            list.Add(new Product() { Name = "Test2" });

            Product product = new Product() { Name = "Saved Project" };

            using (_mocks.Record())
            {
                Expect.Call(_repository.Save(null)).IgnoreArguments().Return(product);
            }
            using (_mocks.Playback())
            {
                _factory.Update(list);
            }

        }

        [TestMethod]
        public void BusinessListBaseFactory_Update_Existing_items()
        {
            BusinessListBaseServerFactory<ProductList, Product> _factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);

            ProductList list = ProductList.GetOldProducts();

            Product product = new Product() { Name = "Saved Project" };

            using (_mocks.Record())
            {
                Expect.Call(_repository.SaveOrUpdate(null)).IgnoreArguments().Return(product);
            }
            using (_mocks.Playback())
            {
                _factory.Update(list);
            }
        }

        [TestMethod]
        public void BusinessListBase_Update_Delete_removed_Items()
        {
            BusinessListBaseServerFactory<ProductList, Product> _factory = new BusinessListBaseServerFactory<ProductList, Product>(_repository);

            ProductList list = ProductList.GetOldProducts();

            Product productToRemove = list[2];

            list.Remove("Updated Project");

            Product product = new Product() { Name = "Saved Project" };

            using (_mocks.Record())
            {
                Expect.Call(() =>_repository.Delete(null)).IgnoreArguments();
            }
            using (_mocks.Playback())
            {
                _factory.Update(list);
            }
        }

        
        //delegate NHibernate.ICriteria CriteriaAction();

        //private CriteriaAction getCriteria()
        //{
        //    return delegate()
        //    {
        //        return _criteria;
        //    };
            
        //}

        //private delegate IList<Product> GetList();
        //private IList<Product> GetCriteriaList()
        //{
        //    return new ProductList();
        //}

        private NHibernate.ICriteria _criteria = null;
    }

    public class ProductList : PTBusinessListBase<ProductList, Product>
    {
        private NHibernate.ICriteria _iCriteria = null;

        public override void SetNHibernateCriteria(object businessCriteria, NHibernate.ICriteria nhibernateCriteria)
        {
            // Cast the criteria back to the strongly-typed version
            SingleCriteria<ProductList, string> criteria = businessCriteria as SingleCriteria<ProductList, string>;

            // If it's a valid criteria object then check for filters
            if (!ReferenceEquals(criteria, null))
            {
                // Set a reference to the NHibernate ICriteria (for local use only)
                _iCriteria = nhibernateCriteria;

                // Name
                if (!String.IsNullOrEmpty(criteria.Value))
                {
                    AddCriterionName(criteria.Value);
                }
            }
        }

        /// <summary>
        /// Adds a criterion to the NHibernate <see cref="ICriteria"/> that filters by name.
        /// </summary>
        /// <param name="name">The name to use as a filter.</param>
        private void AddCriterionName(string name)
        {
            NHibernate.Criterion.SimpleExpression expression = Restrictions.Eq("Name", name);
            _iCriteria.Add(expression);
        }


        public static ProductList GetOldProducts()
        {
            ProductList _list = new ProductList();

            for (int i = 0; i < 4; i++)
            {
                Product prod1 = new Product();
                prod1.Name = "Test" + i.ToString();
                prod1.MarkAsChild();
                prod1.MarkOld();
                


                if (i == 2)
                    prod1.Name = "Updated Project";

                _list.Add(prod1);

            }

            return _list;
        }

        public void Remove(string name)
        {
            foreach (Product prod in this)
            {
                if (prod.Name.Equals(name))
                {
                    Remove(prod);
                    break;
                }
            }
        }
    }
}
