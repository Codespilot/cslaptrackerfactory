using System;
using NHibernate;
using NHibernate.Engine;
using Rhino.Mocks;

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
    /// Summary description for UnitOfWorkFactory_Fixture
    /// </summary>
    [TestClass]
    [Ignore]
    public class UnitOfWorkFactory_Fixture : FixtureBase
    {
        private string dbKey = "PTracker";
        public UnitOfWorkFactory_Fixture()
        {
            //
            // TODO: Add constructor logic here
            //
        }



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

        private IUnitOfWorkFactory _factory;

        //private readonly MockRepository _mocks = new MockRepository();
        private ISession _session;
        private IUnitOfWork _uow;

        [TestInitialize]
        public void SetupContext()
        {
            _factory = (IUnitOfWorkFactory)Activator.CreateInstance(typeof(UnitOfWorkFactory), true);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Accessing_CurrentSession_When_no_session_open_throws()
        {
            var session = _factory.GetCurrentSession(dbKey);
        }


        [TestMethod]
        public void Can_Create_unit_of_work()
        {
            IUnitOfWork implementor = _factory.Create(dbKey);
            Assert.IsNotNull(implementor);
            Assert.IsNotNull(_factory.GetCurrentSession(dbKey));
            Assert.AreEqual(FlushMode.Commit, _factory.GetCurrentSession(dbKey).FlushMode);

        }

        [TestMethod]
        public void Can_configure_NHibernate()
        {
            var configuration = _factory.Configuration(dbKey);
            Assert.IsNotNull(configuration);
            Assert.AreEqual("NHibernate.Connection.DriverConnectionProvider", configuration.Properties["connection.provider"]);
            string expected = "NHibernate.Dialect.MsSql2005Dialect";
            Assert.AreEqual(expected, configuration.Properties["dialect"].ToString().Substring(0, expected.Length)); 
            expected = "NHibernate.Driver.SqlClientDriver";
            Assert.AreEqual(expected, configuration.Properties["connection.driver_class"].Substring(0, expected.Length)); 
            //expected = ;
            Assert.AreEqual("Data Source=SCSQLSRVTEST03;initial catalog=ProjectTracker;Integrated Security=SSPI;", configuration.Properties["connection.connection_string"]); 
        }

        [TestMethod]
        public void Can_create_and_access_session_factory()
        {
            var sessionFactory = _factory.SessionFactory(dbKey);
            Assert.IsNotNull(sessionFactory);
            Assert.AreEqual("NHibernate.Dialect.MsSql2005Dialect", ((ISessionFactoryImplementor)sessionFactory).Dialect.ToString());
        }

        
        //[TestMethod]
        //public void Can_Dispose_Unit_Of_Work()
        //{
        //    _session = _mocks.DynamicMock<ISession>();
        //    _uow = _mocks.PartialMock<UnitOfWorkImplementor>(_factory, _session);

        //    using (_mocks.Record())
        //    {
        //        Expect.Call(_session.Dispose);
                
        //    }
        //    using (_mocks.Playback())
        //    {
        //        _uow.Dispose();
        //    }
        //}
       
    }
}
