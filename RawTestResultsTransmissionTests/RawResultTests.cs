using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RawTestResultsTransmission.BAL;

namespace RawTestResultsTransmissionTests
{
    // Are the results in the proper format(TRT)?
    // Can I generate a new oppId value?
    // Can I determine if the results were transmitted successfully?

    /// <summary>
    /// Summary description for RawResultTests
    /// </summary>
    [TestClass]
    public class RawResultTests
    {        
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
        public void TestForRawResultEqualToZeroOppIdValue()
        {
            TestResult testResult = new TestResult();
            testResult.GenerateNewOppId("b");

            Assert.IsTrue(testResult.OppId == 0, testResult.Message);
        }

        [TestMethod]
        public void TestForRawResultGenerateNewOppIdValue()
        {
            TestResult testResult = new TestResult();
            testResult.GenerateNewOppId("1");

            Assert.IsTrue(testResult.OppId > 0, testResult.Message);
        }
    }
}
