using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
    /// Summary description for LocalData_Fixture
    /// </summary>
    [TestClass]
    [Ignore]
    public class LocalData_Fixture : FixtureBase
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

        [TestInitialize]
        public void TestSetup()
        {
            Local.Data.Clear();
        }

        [TestMethod]
        public void Can_store_values_in_local_data()
        {
            
            Local.Data["one"] = "This is a string";
            Local.Data["two"] = 99.9m;
            var person = new Person { Name = "John Doe", Birthdate = new DateTime(1915, 12, 15) };
            Local.Data[1] = person;

            Assert.AreEqual(3, Local.Data.Count);
            Assert.AreEqual("This is a string", Local.Data["one"]);
            Assert.AreEqual(99.9m, Local.Data["two"]);
            Assert.AreSame(person, Local.Data[1]);
        }

        [TestMethod]
        public void can_clear_local_data()
        {
            
            Local.Data["one"] = "This is a string";
            Local.Data["two"] = 99.9m;
            Assert.AreEqual(2, Local.Data.Count);
            Local.Data.Clear();
            Assert.AreEqual(0, Local.Data.Count);
        }

        private ManualResetEvent _event;

        [TestMethod]
        public void Local_data_is_thread_local()
        {
            Console.WriteLine("Starting in main thread {0}", Thread.CurrentThread.ManagedThreadId);
            Local.Data["one"] = "This is a string";
            Assert.AreEqual(1, Local.Data.Count);

            _event = new ManualResetEvent(false);
            var backgroundThread = new Thread(RunInOtherThread);
            backgroundThread.Start();

            // give the background thread some time to do its job
            Thread.Sleep(100);
            // we still have only one entry (in this thread)
            Assert.AreEqual(1, Local.Data.Count);

            Console.WriteLine("Signaling background thread from main thread {0}", Thread.CurrentThread.ManagedThreadId);
            _event.Set();
            backgroundThread.Join();
        }

        private void RunInOtherThread()
        {
            Console.WriteLine("Starting (background-) thread {0}", Thread.CurrentThread.ManagedThreadId);
            // initially the local data must be empty for this NEW thread!
            Assert.AreEqual(0, Local.Data.Count);
            Local.Data["one"] = "This is another string";
            Assert.AreEqual(1, Local.Data.Count);

            Console.WriteLine("Waiting on (background-) thread {0}", Thread.CurrentThread.ManagedThreadId);
            _event.WaitOne();
            Console.WriteLine("Ending (background-) thread {0}", Thread.CurrentThread.ManagedThreadId);
        }

    }

    internal class Person
    {
        
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }
    }
}
