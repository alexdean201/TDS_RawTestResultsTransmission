using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RawTestResultsTransmission.BAL;
using System.Configuration;

namespace RawTestResultsTransmissionTests
{
    // Do I have the necessary information to connect to the ETS service end point?
    // Can I connect to the ETS service end point with a simple GET method?

    /// <summary>
    /// Summary description for EtsServiceEndpointTests
    /// </summary>
    [TestClass]
    public class EtsServiceEndpointTests
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
        
        EtsService etsService = new EtsService();

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
        public void TestForEtsServiceNotNullInformation()
        {
            etsService.Url = ConfigurationManager.AppSettings["EtsServiceUrl"];
            etsService.Username = ConfigurationManager.AppSettings["EtsServiceUsername"];
            etsService.Password = ConfigurationManager.AppSettings["EtsServicePassword"];

            Assert.IsNotNull(etsService.Url, "EtsServiceUrl does not exist in App.config.");
            Assert.IsNotNull(etsService.Username, "EtsServiceUsername does not exist in App.config.");
            Assert.IsNotNull(etsService.Password, "EtsServicePassword does not exist in App.config.");
        }
        [TestMethod]
        public void TestForEtsServiceNotEmptyInformation()
        {
            etsService.Url = ConfigurationManager.AppSettings["EtsServiceUrl"];
            etsService.Username = ConfigurationManager.AppSettings["EtsServiceUsername"];
            etsService.Password = ConfigurationManager.AppSettings["EtsServicePassword"];

            Assert.IsTrue(!etsService.Url.Equals(""), "EtsServiceUrl is empty.");
            Assert.IsTrue(!etsService.Username.Equals(""), "EtsServiceUsername is empty.");
            Assert.IsTrue(!etsService.Password.Equals(""), "EtsPassword is empty.");
        }
        [TestMethod]
        public void TestForEtsServiceValidUrl()
        {
            etsService.Url = ConfigurationManager.AppSettings["EtsServiceUrl"];

            Assert.IsTrue(etsService.CheckIfUrlIsValid(), "EtsServiceUrl is not a valid Url: '" + etsService.Url + "'");
        }
        [TestMethod]
        public void TestForEtsServiceConnection()
        {
            etsService.Url = ConfigurationManager.AppSettings["EtsServiceUrl"];
            etsService.Username = ConfigurationManager.AppSettings["EtsServiceUsername"];
            etsService.Password = ConfigurationManager.AppSettings["EtsServicePassword"];

            Assert.IsTrue(etsService.CheckIfConnectionIsGood(), etsService.Message);
        }
    }
}
