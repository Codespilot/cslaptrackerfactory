using System;
using System.Reflection;

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
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
#endif 



namespace ProjectTracker.Library.Tests
{

    #region NUnitVersion 
    //[TestFixture]
    //public class UnitOfWork_With_Factory_Fixture
    //{
    //    private string dbKey = "PTracker";
    //    private readonly MockRepository _mocks = new MockRepository();
    //    private IUnitOfWorkFactory _factory;
    //    private IUnitOfWork _unitOfWork;
    //    private ISession _session;

    //    [TestFixtureSetUp]
    //    public void TestFixtureSetUp()
    //    {
    //        ResetUnitOfWork();
    //    }

    //    [TestFixtureTearDown]
    //    public void FixtureTeardown()
    //    {
    //        // brute force attack to set my own factory via reflection
    //        var fieldInfo = typeof(UnitOfWork).GetField("_unitOfWorkFactory",
    //                            BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
    //        fieldInfo.SetValue(null, Activator.CreateInstance(typeof(UnitOfWorkFactory), true));
    //    }

    //    [SetUp]
    //    public void SetupContext()
    //    {
    //        _factory = _mocks.DynamicMock<IUnitOfWorkFactory>();
    //        _unitOfWork = _mocks.DynamicMock<IUnitOfWork>();
    //        _session = _mocks.DynamicMock<ISession>();

    //        InstrumentUnitOfWork();

    //        _mocks.BackToRecordAll();
    //        SetupResult.For(_factory.Create(dbKey)).Return(_unitOfWork);
    //        SetupResult.For(_factory.GetCurrentSession(dbKey)).Return(_session);
    //        _mocks.ReplayAll();
    //    }

    //    [TearDown]
    //    public void TearDownContext()
    //    {
    //        _mocks.VerifyAll();

    //        ResetUnitOfWork();
    //    }

    //    private void InstrumentUnitOfWork()
    //    {
    //        // brute force attack to set my own factory via reflection
    //        var fieldInfo = typeof(UnitOfWork).GetField("_unitOfWorkFactory",
    //                            BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
    //        fieldInfo.SetValue(null, _factory);
    //    }

    //    private void ResetUnitOfWork()
    //    {
    //        // assert that the UnitOfWork is reset
    //        var propertyInfo = typeof(UnitOfWork).GetProperty("CurrentUnitOfWork",
    //                            BindingFlags.Static | BindingFlags.SetProperty | BindingFlags.NonPublic);
    //        propertyInfo.SetValue(null, null, null);
    //        //var fieldInfo = typeof(UnitOfWork).GetField("_innerUnitOfWork",
    //        //                    BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
    //        //fieldInfo.SetValue(null, null);
    //    }

    //    [Test]
    //    public void Can_Start_and_Dispose_UnitOfWork()
    //    {
    //        IUnitOfWork uow = UnitOfWork.Start(dbKey);
    //        uow.Dispose();
    //    }

    //    [Test]
    //    public void Can_access_current_unit_of_work()
    //    {
    //        IUnitOfWork uow = UnitOfWork.Start(dbKey);
    //        var current = UnitOfWork.Current;
    //        uow.Dispose();
    //    }

    //    [Test]
    //    public void Accessing_Current_UnitOfWork_if_not_started_throws()
    //    {
    //        try
    //        {
    //            var current = UnitOfWork.Current;
    //        }
    //        catch (InvalidOperationException ex)
    //        { }
    //    }

    //    [Test]
    //    public void Starting_UnitOfWork_if_already_started_throws()
    //    {
    //        UnitOfWork.Start(dbKey);
    //        try
    //        {
    //            UnitOfWork.Start(dbKey);
    //        }
    //        catch (InvalidOperationException ex)
    //        { }
    //    }

    //    [Test]
    //    public void Can_test_if_UnitOfWork_Is_Started()
    //    {
    //        Assert.IsFalse(UnitOfWork.IsStarted);

    //        IUnitOfWork uow = UnitOfWork.Start(dbKey);
    //        Assert.IsTrue(UnitOfWork.IsStarted);
    //    }

    //    [Test]
    //    public void Can_get_valid_current_session_if_UoW_is_started()
    //    {
    //        using (UnitOfWork.Start(dbKey))
    //        {
    //            ISession session = UnitOfWork.CurrentSession(dbKey);
    //            Assert.IsNotNull(session);
    //        }
    //    }

    //    [Test]
    //    public void Get_current_session_if_UoW_is_not_started_throws()
    //    {
    //        try
    //        {
    //            ISession session = UnitOfWork.CurrentSession(dbKey);
    //        }
    //        catch (InvalidOperationException ex)
    //        { }
    //    }
    //} 
    #endregion


    /// <summary>
    /// Summary description for UnitOfWork_With_Factory_Fixture
    /// </summary>
    [TestClass]
    [Ignore]
    public class UnitOfWork_With_Factory_Fixture : FixtureBase
    {
        private string dbKey = "PTracker";

        public UnitOfWork_With_Factory_Fixture()
        {
            //ResetUnitOfWork();
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

        private readonly MockRepository _mocks = new MockRepository();
        private IUnitOfWorkFactory _factory;
        private IUnitOfWork _unitOfWork;
        private ISession _session;



        [TestInitialize]
        public void SetUpContext()
        {
            ResetUnitOfWork();

            _factory = _mocks.DynamicMock<IUnitOfWorkFactory>();
            _unitOfWork = _mocks.DynamicMock<IUnitOfWork>();
            _session = _mocks.DynamicMock<ISession>();

            //var fieldInfo = typeof(UnitOfWork).GetField("_innerUnitOfWork",
            //    BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
            //fieldInfo.SetValue(null, null);
            InstrumentUnitOfWork();

            _mocks.BackToRecordAll();
            SetupResult.For(_factory.Create(dbKey)).Return(_unitOfWork);
            SetupResult.For(_factory.GetCurrentSession(dbKey)).Return(_session);
            SetupResult.For(_unitOfWork.DatabaseKey).Return(dbKey);
            _mocks.ReplayAll();
        }

        [TestCleanup]
        public void TearDownContext()
        {
            _mocks.VerifyAll();


            ResetUnitOfWork();
            ////assert that the UnitOfWork is reset
            var fieldInfo = typeof(UnitOfWork).GetField("_unitOfWorkFactory",
                BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
            fieldInfo.SetValue(null, null);

            _mocks.BackToRecordAll();

            // brute force attack to set my own factory via reflection
            var fieldInfo2 = typeof(UnitOfWork).GetField("_unitOfWorkFactory",
                                BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
            fieldInfo2.SetValue(null, Activator.CreateInstance(typeof(UnitOfWorkFactory), true));
        }

        private static void ResetUnitOfWork()
        {
            // assert that the UnitOfWork is reset
            var propertyInfo = typeof(UnitOfWork).GetProperty("CurrentUnitOfWork",
                                BindingFlags.Static | BindingFlags.SetProperty | BindingFlags.NonPublic);
            propertyInfo.SetValue(null, null, null);

        }

        private void InstrumentUnitOfWork()
        {
            // brute force attack to set my own factory via reflection
            var fieldInfo = typeof(UnitOfWork).GetField("_unitOfWorkFactory",
                                BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
            fieldInfo.SetValue(null, _factory);
        }

        [TestMethod]
        public void Can_Start_and_Dispose_UnitOfWork()
        {
            IUnitOfWork uow = UnitOfWork.Start(dbKey);
            uow.Dispose();
        }

        [TestMethod]
        public void Can_access_current_unit_of_work()
        {
            IUnitOfWork uow = UnitOfWork.Start(dbKey);
            var current = UnitOfWork.Current;
            Assert.AreSame(uow, current);
            uow.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Accessing_Current_UnitOfWork_if_not_started_throws()
        {
            var current = UnitOfWork.Current;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Starting_UnitOfWork_if_already_started_throws()
        {
            UnitOfWork.Start(dbKey);

            UnitOfWork.Start(dbKey);


        }

        [TestMethod]
        public void Can_test_if_UnitOfWork_Is_Started()
        {
            Assert.IsFalse(UnitOfWork.IsStarted);

            IUnitOfWork uow = UnitOfWork.Start(dbKey);
            Assert.IsTrue(UnitOfWork.IsStarted);
        }

        [TestMethod]
        public void Can_get_valid_current_session_if_UoW_is_started()
        {
            using (UnitOfWork.Start(dbKey))
            {
                ISession session = UnitOfWork.CurrentSession(dbKey);
                Assert.IsNotNull(session);
            }
        }

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void Get_current_session_if_UoW_is_not_started_throws()
        //{

        //    ISession session = UnitOfWork.CurrentSession(dbKey);
        //}

    }
}
