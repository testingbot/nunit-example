using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace NUnit_TestingBot_Sample;

public class ParallelTests
{
    [TestFixture("chrome", "latest", "Windows 10")]
    [TestFixture("firefox", "latest", "SONOMA")]
    [Parallelizable(ParallelScope.Fixtures)]
    public class TbNUnit_ParallelTest
    {
        private IWebDriver driver;
        private string browser;
        private string version;
        private string os;

        public TbNUnit_ParallelTest(String browser, String version, String os)
        {
            this.browser = browser;
            this.version = version;
            this.os = os;
        }

        [SetUp]
        public void Init()
        {
            DriverOptions browserOptions;
            if (this.browser == "firefox")
            {
                browserOptions = new FirefoxOptions()
                {
                    BrowserVersion = this.version,
                    PlatformName = this.os
                };
            }
            else if (this.browser == "safari")
            {
                browserOptions = new SafariOptions()
                {
                    BrowserVersion = this.version,
                    PlatformName = this.os
                };
            }
            else
            {
                browserOptions = new ChromeOptions()
                {
                    BrowserVersion = this.version,
                    PlatformName = this.os
                };
            }

            var tbOptions = new Dictionary<string, string>
            {
                ["key"] = Environment.GetEnvironmentVariable("TESTINGBOT_KEY"),
                ["secret"] = Environment.GetEnvironmentVariable("TESTINGBOT_SECRET"),
                ["name"] = TestContext.CurrentContext.Test.Name,
                ["selenium-version"] = "3.14.0"
            };

            browserOptions.AddAdditionalOption("tb:options", tbOptions);

            driver = new RemoteWebDriver(new Uri("https://hub.testingbot.com/wd/hub"), browserOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
        }

        [Test]
        public void GoogleTest()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            Assert.That(driver.Title, Is.EqualTo("Google"));
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
                driver.Dispose();
            }
        }
    }
}
