using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace NUnit_TestingBot_Sample;

public class Tests
{
    [TestFixture("chrome", "latest", "Windows 10", "", "")]
    public class TbNUnit_Test
    {
        private IWebDriver driver;
        private String browser;
        private String version;
        private String os;
        private String deviceName;
        private String deviceOrientation;

        public TbNUnit_Test(String browser, String version, String os, String deviceName, String deviceOrientation)
        {
            this.browser = browser;
            this.version = version;
            this.os = os;
            this.deviceName = deviceName;
            this.deviceOrientation = deviceOrientation;
        }

        [SetUp]
        public void Init()
        {
            var chromeOptions = new ChromeOptions()
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10"
            };
            var tbOptions = new Dictionary<string, string>
            {
                ["key"] = Environment.GetEnvironmentVariable("TESTINGBOT_KEY"),
                ["secret"] = Environment.GetEnvironmentVariable("TESTINGBOT_SECRET"),
                ["name"] = TestContext.CurrentContext.Test.Name,
                ["selenium-version"] = "3.14.0"
            };

            chromeOptions.AddAdditionalOption("tb:options", tbOptions);

            driver = new RemoteWebDriver(new Uri("https://hub.testingbot.com/wd/hub"), chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
        }

        [Test]
        public void googleTest()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            StringAssert.Contains("Google", driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("TestingBot");
            query.Submit();
        }

        [TearDown]
        public void CleanUp()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            try
            {
                // Logs the result to TestingBot
                ((IJavaScriptExecutor)driver).ExecuteScript("tb:test-result=" + (passed ? "passed" : "failed"));
            }
            finally
            {
                // Terminates the remote webdriver session
                driver.Quit();
            }
        }
    }
}
