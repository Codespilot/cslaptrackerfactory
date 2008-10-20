using System;
using ProjectTracker.Library.Data;


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
    public class DatabaseKeyFixture
    {
        [TestMethod]
        public void Can_Get_DatabaseKey_FromClass()
        {
            string ExpectedKey = "TESTDB";
            string value = DatabaseKeyAttribute.GetDatabaseKeyForClass(typeof(DBKeyTest));

            Assert.AreEqual(ExpectedKey, value);
        }
    }

    [DatabaseKey("TESTDB")]
    public class DBKeyTest
    {
        
    }
}
