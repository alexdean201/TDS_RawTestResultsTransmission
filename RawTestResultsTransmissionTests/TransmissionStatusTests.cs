using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RawTestResultsTransmission.BAL;
using System.Configuration;

namespace RawTestResultsTransmissionTests
{
    // Do I have a status file?
    // Can I determine the last time I ran?
    // Can I determine if any new results have been posted since the last time I ran?
    // Can I record the date/time of the run?


    /// <summary>
    /// Summary description for TransmissionStatusTests
    /// </summary>
    [TestClass]
    public class TransmissionStatusTests
    {
        public TransmissionStatusTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        public void TestForTransmissionStatusNotNullInformation()
        {
            transmissionStatus.LastRunStatusFile = ConfigurationManager.AppSettings["LastRunStatusFile"];

            Assert.IsNotNull(transmissionStatus.LastRunStatusFile, "LastRunStatusFile does not exist in App.config.");
        }
        [TestMethod]
        public void TestForTransmissionStatusNotEmptyInformation()
        {
            transmissionStatus.LastRunStatusFile = ConfigurationManager.AppSettings["LastRunStatusFile"];

            Assert.IsTrue(!transmissionStatus.LastRunStatusFile.Equals(""), "LastRunStatusFile is empty");
        }
        [TestMethod]
        public void TestForTransmissionStatusFileExists()
        {
            transmissionStatus.LastRunStatusFile = ConfigurationManager.AppSettings["LastRunStatusFile"];

            Assert.IsTrue(transmissionStatus.LastRunStatusFileExists(), transmissionStatus.Message);
        }
        [TestMethod]
        public void TestForTransmissionStatusCreateFileIfNotExists()
        {
            transmissionStatus.LastRunStatusFile = ConfigurationManager.AppSettings["LastRunStatusFile"];

            Assert.IsTrue(transmissionStatus.CreateLastRunStatusFile() == 1, transmissionStatus.Message);
        }
        [TestMethod]
        public void TestForTransmissionStatusDefaultLastRunDateTime()
        {
            transmissionStatus.LastRunDefaultDateTime = DateTime.Parse(ConfigurationManager.AppSettings["LastRunDefaultDateTime"]);

            Assert.IsNotNull(transmissionStatus.LastRunDefaultDateTime, "LastRunDefaultDateTime does not exist in App.config");
        }
        [TestMethod]
        public void TestForTransmissionStatusValidLastRunDateTime()
        {
            transmissionStatus.LastRunStatusFile = ConfigurationManager.AppSettings["LastRunStatusFile"];

            Assert.IsTrue(transmissionStatus.CheckIfLastRunDateTimeIsValid(), transmissionStatus.Message);
        }
        [TestMethod]
        public void TestForTransmissionStatusUseDefaultLastRunDateTime()
        {
            transmissionStatus.LastRunStatusFile = ConfigurationManager.AppSettings["LastRunStatusFile"];
            transmissionStatus.LastRunDefaultDateTime = DateTime.Parse(ConfigurationManager.AppSettings["LastRunDefaultDateTime"]);

            Assert.IsTrue(transmissionStatus.CheckIfNeedToUseDefaultLastRunDateTime(), transmissionStatus.Message);
        }        
        [TestMethod]
        public void TestForTransmissionStatusRecordLastRunDateTime()
        {
            transmissionStatus.LastRunStatusFile = ConfigurationManager.AppSettings["LastRunStatusFile"];

            Assert.IsTrue(transmissionStatus.RecordLastRunDateTime() == 1, transmissionStatus.Message);
        }
    }
}
