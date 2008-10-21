using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Server;
using ProjectTracker.Library;
using ProjectTracker.Library;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework.Factories;
using Rhino.Mocks;
using StructureMap.Attributes;
using StructureMap.Configuration;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectFactory=StructureMap.ObjectFactory;

#else
using NUnit.Framework;
using ObjectFactory=StructureMap.ObjectFactory;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif 

//using NUnit.Framework;

namespace ProjectTracker.Library.Tests
{
    /// <summary>
    /// Summary description for Project_Fixture
    /// </summary>
    [TestClass]
    public class Project_Fixture : FixtureBase
    {
        private MockRepository _mocks = new MockRepository();
        private IRepository<Project> _repository;
        private IBusinessBaseServerFactory<Project> _factory;
        //private GenericFactoryLoader _loader;
        private IObjectFactoryLoader _loader;
        

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

        [TestInitialize]
        public void SetupTest()
        {
            //StructureMap.AutoMocking.RhinoAutoMocker<BusinessBaseServerFactory<Project>>();

            _repository = _mocks.DynamicMock<IRepository<Project>>();
            _factory = _mocks.DynamicMock<IBusinessBaseServerFactory<Project>>();
            _loader = _mocks.DynamicMock<IObjectFactoryLoader>();

            //_loader = _mocks.DynamicMock<GenericFactoryLoader>();
            PTTestRegistry testRegistry = new PTTestRegistry();

            ObjectFactory.Initialize(x => x.AddRegistry(testRegistry));

            SetupResult
                .For(
                _loader.GetFactoryType(
                    "Factory Type=IBusinessBaseServerFactory;Item Type=ProjectTracker.Library.Project, ProjectTracker.Library"))
                .Return(_factory.GetType());
            SetupResult
                .For(_loader.GetFactory("IBusinessBaseServerFactory"))
                .Return(_factory);

        }

        [TestMethod]
        public void Can_Create_New_Project()
        {
            Project project = null;
            using (_mocks.Record())
            {
                Expect.Call(_factory.Create()).Return((Project) Activator.CreateInstance(typeof (Project), true));
            }
            using (_mocks.Playback())
            {
                ObjectFactory.Inject(typeof(IBusinessBaseServerFactory<Project>), _factory);
                ObjectFactory.Inject(typeof(IRepository<Project>),_repository);
                project = Project.NewProject();
                
            }
            Assert.IsNotNull(project);
            Assert.IsTrue(project.IsNew);
        }

        [TestMethod]
        public void Can_Save_Project()
        {
            Project project = null;


            Project _existingProject = Project.NewProject();
            // TODO: FIX THIS LINE OF CODE
            //_existingProject.Id = Guid.NewGuid();
            _existingProject.Name = "Existing Project";
            using(_mocks.Record())
            {
                Expect.Call(_factory.Create()).Return((Project)Activator.CreateInstance(typeof(Project), true));
                Expect.Call(_factory.Update(null)).IgnoreArguments().Return(_existingProject);
            }
            using(_mocks.Playback())
            {
                ObjectFactory.Inject(typeof(IBusinessBaseServerFactory<Project>), _factory);
                ObjectFactory.Inject(typeof(IRepository<Project>),_repository);
                project = Project.NewProject();
                project.Name = "Test Project";
                project = project.Save();

                Assert.AreEqual(_existingProject.Id, project.Id);
                Assert.AreEqual(_existingProject.Name, project.Name);
            }
        }
    }
}
