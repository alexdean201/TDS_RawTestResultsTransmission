using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RawTestResultsTransmission.BAL;
using System.Configuration;

namespace RawTestResultsTransmissionTests
{
    // Do I have the necessary information to connect to the TDS database?
    // Can I connect to the TDS database?
    // Can I run a simple record count query?

    /// <summary>
    /// Summary description for TdsQueriesTests
    /// </summary>
    [TestClass]
    public class TdsQueriesTests
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

        TdsQueries tdsQueries = new TdsQueries();
        TransmissionStatus transmissionStatus = new TransmissionStatus();

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
        public void TestForTdsNotNullInformation()
        {
            tdsQueries.ConnectionString = ConfigurationManager.ConnectionStrings["exam_auditEntities"].ToString();

            Assert.IsNotNull(tdsQueries.ConnectionString, "The Connection String for 'exam_auditEntities' does not exist in App.config.");
        }
        [TestMethod]
        public void TestForTdsNotEmptyInformation()
        {
            tdsQueries.ConnectionString = ConfigurationManager.ConnectionStrings["exam_auditEntities"].ToString();

            Assert.IsTrue(!tdsQueries.ConnectionString.Equals(""), "Connection String for 'exam_auditEntities' is empty.");
        }
        [TestMethod]
        public void TestForTdsConnection()
        {
            Assert.IsTrue(tdsQueries.CheckIfConnectionIsGood(), tdsQueries.Message);
        }
        [TestMethod]
        public void TestForTdsSimpleQuery()
        {
            Assert.IsTrue(tdsQueries.RunSimpleQuery(), tdsQueries.Message);
        }
        [TestMethod]
        public void TestForTdsAnyNewTestResults()
        {
            transmissionStatus.LastRunStatusFile = ConfigurationManager.AppSettings["LastRunStatusFile"];
            transmissionStatus.CheckIfLastRunDateTimeIsValid();

            Assert.IsTrue(tdsQueries.NumberOfNewTestResults(transmissionStatus.LastRunDateTime) >= 0, transmissionStatus.Message);
        }
    }
}
