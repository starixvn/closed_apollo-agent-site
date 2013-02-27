using Sunplus.Core.LoggingLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoggingLibUnitTest
{
    using System;

    
    /// <summary>
    ///This is a test class for EntLibLoggingServiceTest and is intended
    ///to contain all EntLibLoggingServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EntLibLoggingServiceTest
    {

        private const string TestCategory = "Customer Setting";
        private EntLibLogger entLibService;
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

        
        [TestInitialize]
        public void Initialize()
        {
            entLibService = new EntLibLogger(); // TODO: Initialize to an appropriate value
            
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion       


        /// <summary>
        ///A test for Debug
        ///</summary>
        [TestMethod()]
        public void Write_Log_Verbose_Severity_With_Default_Category()
        {
            
            string format = string.Empty; // TODO: Initialize to an appropriate value
            object[] args = new object[]{"arg1 value", "arg2 value"}; // TODO: Initialize to an appropriate value
            entLibService.Debug("This is unittest for logging lib with arg1={0} and arg2={1}", args);
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }


        [TestMethod]
        public void Write_Log_Verbose_Severity_With_Specific_Category()
        {            
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Debug("This is unittest for logging lib", TestCategory);
        }

        [TestMethod]
        public void Write_Log_Error_Serverity_With_Default_Category()
        {
            
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Error("This is unittest for logging lib");
        }

        [TestMethod]
        public void Write_Log_Error_Severity_With_Specific_Category()
        {
            
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Error("This is unittest for logging lib", TestCategory);
        }


        [TestMethod]
        public void Write_Log_Error_Severity_Exception_With_Default_Category()
        {
            System.Exception exception = new Exception("Unit test exception");
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Error("This is unittest for logging lib", exception);
        }

        [TestMethod]
        public void Write_Log_Error_Severity_Exception_With_Specified_Category()
        {
            System.Exception exception = new Exception("Unit test exception");
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Error("This is unittest for logging lib", TestCategory, exception);
        }



        [TestMethod]
        public void Write_Log_Warn_Serverity_With_Default_Category()
        {
            
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Warn("This is unittest for logging lib");
        }

        [TestMethod]
        public void Write_Log_Warn_Severity_With_Specific_Category()
        {
            
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Warn("This is unittest for logging lib", TestCategory);
        }

        [TestMethod]
        public void Write_Log_Info_Serverity_With_Default_Category()
        {
            
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Info("This is unittest for logging lib");
        }

        [TestMethod]
        public void Write_Log_Info_Severity_With_Specific_Category()
        {
            
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Info("This is unittest for logging lib", TestCategory);
        }

        [TestMethod]
        public void Write_Log_Critical_Serverity_With_Default_Category()
        {
            
            string format = string.Empty; // TODO: Initialize to an appropriate value            
            entLibService.Critical("This is unittest for logging lib");
        }

        [TestMethod]
        public void Write_Log_Critical_Severity_With_Specific_Category()
        {
            try
            {

                string format = string.Empty; // TODO: Initialize to an appropriate value            
                entLibService.Critical("This is unittest for logging lib", TestCategory);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI");
                if (rethrow)
                    throw; 
            }
        }
    }
}
