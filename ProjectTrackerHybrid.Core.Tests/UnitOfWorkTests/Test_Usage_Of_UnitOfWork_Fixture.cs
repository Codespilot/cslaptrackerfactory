using System;
using System.Reflection;
using NHibernate.Tool.hbm2ddl;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif 

namespace ProjectTracker.Library.Tests
{
    /// <summary>
    /// Summary description for Test_Usage_Of_UnitOfWork_Fixture
    /// </summary>
    [TestClass]
    public class Test_Usage_Of_UnitOfWork_Fixture : FixtureBase
    {

        

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private string dbKey = "PTracker";

        //[TestInitialize]
        public void SetupTest()
        {
            
            UnitOfWork.Configuration(dbKey).AddAssembly(Assembly.GetExecutingAssembly());
            new SchemaExport(UnitOfWork.Configuration(dbKey)).Execute(false, true, false, false);
        }

        //[TestMethod]
        public void Can_add_a_new_instance_of_an_entity_to_the_database()
        {
            using (UnitOfWork.Start(dbKey))
            {
                var person = new Person { Name = "John Doe", Birthdate = new DateTime(1915, 12, 15) };
                UnitOfWork.CurrentSession(dbKey).Save(person);
                UnitOfWork.Current.TransactionalFlush();
            }
        }
    }
}
