using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using NLog;
using NLog.Targets;
using RawTestResultsTransmission.BAL;

namespace RawTestResultsTransmissionTests
{

    // Do I have a log file?

    /// <summary>
    /// Summary description for LoggingTests
    /// </summary>
    [TestClass]
    public class LoggingTests
    {
        public LoggingTests()
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

        Logger logger = LogManager.GetCurrentClassLogger();

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
        public void TestForLogFileAnyTarget()
        {
            Assert.IsTrue(logger.Factory.Configuration.AllTargets.Count > 0, "No targets can be found in the NLog.config file.");
        }
        [TestMethod]
        public void TestForLogFileFileTarget()
        {
            var target = (FileTarget) LogManager.Configuration.FindTargetByName("f");
            logger.Info("Hello World");
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
            string fileName = target.FileName.Render(logEventInfo);

            Assert.IsTrue(File.Exists(fileName), "File does not exist");            
        }        
    }
}
