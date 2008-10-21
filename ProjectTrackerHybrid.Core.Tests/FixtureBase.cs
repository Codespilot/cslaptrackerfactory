#if !NUNIT
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Rhino.Mocks;
using StructureMap;

#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif 


namespace ProjectTracker.Library.Tests
{
    public class FixtureBase
    {

        protected IUnitOfWork unitOfWorkStub;
        protected IDisposable disposeGlobalUnitOfWorkRegistration;
        protected ISession sessionStub;

        public FixtureBase()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new PTTestRegistry()));
        }

        protected void SetupTest()
        {
            unitOfWorkStub = MockRepository.GenerateStub<IUnitOfWork>();
            disposeGlobalUnitOfWorkRegistration = UnitOfWork.RegisterGlobalUnitOfWork(unitOfWorkStub);
            sessionStub = MockRepository.GenerateStub<ISession>();

            UnitOfWork.SetCurrentSession("TEST", sessionStub);

        }

#if !NUNIT
        #region TestContext
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion
#endif

        protected void CleanupTest()
        {
            disposeGlobalUnitOfWorkRegistration.Dispose();
        }
    }
}
