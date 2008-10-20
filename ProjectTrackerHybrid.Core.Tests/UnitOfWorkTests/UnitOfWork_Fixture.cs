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
#endif 
namespace ProjectTracker.Library.Tests
{

    //[TestFixture]
    //public class UnitOfWork_Fixture
    //{
    //    private string dbKey = "PTracker";
    //    private readonly MockRepository _mocks = new MockRepository();

    //    [Test]
    //    public void Can_Start_UnitOfWork()
    //    {
    //        var factory = _mocks.DynamicMock<IUnitOfWorkFactory>();
    //        var unitOfWork = _mocks.DynamicMock<IUnitOfWork>();

    //         brute force attack to set my own factory via reflection
    //        var fieldInfo = typeof(UnitOfWork).GetField("_unitOfWorkFactory",
    //            BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
    //        fieldInfo.SetValue(null, factory);

    //        using (_mocks.Record())
    //        {
    //            Expect.Call(factory.Create(dbKey)).Return(unitOfWork);
    //        }
    //        using (_mocks.Playback())
    //        {
    //            var uow = UnitOfWork.Start(dbKey);
    //        }
    //    }
    //}

    /// <summary>
    /// Summary description for UnitOfWork_Fixture
    /// </summary>
    [TestClass]
    public class UnitOfWork_Fixture : FixtureBase
    {
        Rhino.Mocks.MockRepository _mocks = new Rhino.Mocks.MockRepository();
        private string dbKey = "PTracker";


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


        [TestMethod]
        public void Can_Start_UnitOfWork()
        {
            var factory = _mocks.DynamicMock<IUnitOfWorkFactory>();
            var unitOfWork = _mocks.DynamicMock<IUnitOfWork>();

            var fieldInfo = typeof(UnitOfWork).GetField("_unitOfWorkFactory",
                BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);

            fieldInfo.SetValue(null, factory);

            using (_mocks.Record())
            {
                Expect.Call(factory.Create(dbKey)).Return(unitOfWork);
            }
            using (_mocks.Playback())
            {
                var uow = UnitOfWork.Start(dbKey);
            }

        }

        [TestMethod]
        public void Can_Flush_session()
        {
            var _session = _mocks.DynamicMock<ISession>();
            var factory = _mocks.DynamicMock<IUnitOfWorkFactory>();
            var unitOfWork = _mocks.PartialMock<UnitOfWorkImplementor>(factory, _session, dbKey);

            using (_mocks.Record())
            {
                Expect.Call(_session.Flush);
            }
            using (_mocks.Playback())
            {
                unitOfWork.Flush();
            }


        }
    }
}
