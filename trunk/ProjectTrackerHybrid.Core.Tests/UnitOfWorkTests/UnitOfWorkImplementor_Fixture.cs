using System.Data;
using NHibernate;
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
    /// Summary description for UnitOfWorkImplementor_Fixture
    /// </summary>
    [TestClass]
    [Ignore]
    public class UnitOfWorkImplementor_Fixture : FixtureBase
    {
        private string dbKey = "TEST";


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

        private readonly MockRepository _mocks = new MockRepository();
        private IUnitOfWorkFactory _factory;
        private ISession _session;
        private IUnitOfWorkImplementor _uow;

        [TestInitialize]
        public void SetupTest()
        {
            _factory = _mocks.DynamicMock<IUnitOfWorkFactory>();
            _session = _mocks.DynamicMock<ISession>();
        }

        [TestCleanup]
        public void cleanup()
        {
            _mocks.VerifyAll();
        }

        [TestMethod]
        public void Can_Dispose_UnitOfWorkImplementor()
        {
            using (_mocks.Record())
            {
                Expect.Call(() => _factory.DisposeUnitOfWork(null, dbKey)).IgnoreArguments();
                Expect.Call(_session.Dispose);
            }
            using(_mocks.Playback())
            {
                _uow = new UnitOfWorkImplementor(_factory, _session, dbKey);
                _uow.Dispose();
            }
        }

        [TestMethod]
        public void Can_BeginTransaction()
        {
            using (_mocks.Record())
            {
                Expect.Call(_session.BeginTransaction()).Return(null);
            }
            using (_mocks.Playback())
            {
                _uow = new UnitOfWorkImplementor(_factory, _session, dbKey);
                var transaction = _uow.BeginTransaction();
                Assert.IsNotNull(transaction);
            }
        }

        [TestMethod]
        public void Is_DatabaseKey_Correct()
        {
            using (_mocks.Record())
            {
            }
            using (_mocks.Playback())
            {
                _uow = new UnitOfWorkImplementor(_factory, _session, dbKey);
                Assert.AreEqual(_uow.DatabaseKey, dbKey);
            }

           
        }

        [TestMethod]
        public void Can_BeginTransaction_specifying_isolation_level()
        {
            var isolationLevel = IsolationLevel.Serializable;
            using (_mocks.Record())
            {
                Expect.Call(_session.BeginTransaction(isolationLevel)).Return(null);
            }

            using (_mocks.Playback())
            {
                _uow = new UnitOfWorkImplementor(_factory, _session, dbKey);
                var transaction = _uow.BeginTransaction(isolationLevel);
                Assert.IsNotNull(transaction); 
            }
        }

        [TestMethod]
        public void Can_execute_TransactionFlush()
        {
            var tx = _mocks.CreateMock<ITransaction>();
            var session = _mocks.DynamicMock<ISession>();
            SetupResult.For(session.BeginTransaction(IsolationLevel.ReadCommitted)).Return(tx);

            _uow = _mocks.PartialMock<UnitOfWorkImplementor>(_factory, _session, dbKey);

            using (_mocks.Record())
            {
                Expect.Call(tx.Commit);
                Expect.Call(tx.Dispose);
            }
            using (_mocks.Playback())
            {
                _uow = new UnitOfWorkImplementor(_factory, session, dbKey);
                _uow.TransactionalFlush();
            }
        }

        [TestMethod]
        public void Can_execute_TransactionFlush_specifying_isolation_level()
        {
            var tx = _mocks.StrictMock<ITransaction>();
            var session = _mocks.DynamicMock<ISession>();
            SetupResult.For(session.BeginTransaction(IsolationLevel.Serializable)).Return(tx);

            _uow = _mocks.PartialMock<UnitOfWorkImplementor>(_factory, _session, dbKey);

            using (_mocks.Record())
            {
                Expect.Call(tx.Commit);
                Expect.Call(tx.Dispose);
            }
            using (_mocks.Playback())
            {
                _uow = new UnitOfWorkImplementor(_factory, session, dbKey);
                _uow.TransactionalFlush(IsolationLevel.Serializable);
            }
        }

        
    }
}
