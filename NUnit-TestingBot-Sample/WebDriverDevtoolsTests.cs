using System.Collections.Generic;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Chrome;
using System.Linq;

namespace NUnit_TestingBot_Sample
{
    [TestFixture("chrome", "137", "Windows 10")]
    [Category("DevTools")]
    public class WebDriverDevtoolsTests
    {
        private IWebDriver? _driver;
        private readonly string _browser;
        private readonly string _version;
        private readonly string _os;

        public WebDriverDevtoolsTests(string browser, string version, string os)
        {
            _browser = browser;
            _version = version;
            _os = os;
        }

        [SetUp]
        public void SetUp()
        {
            var browserOptions = CreateBrowserOptions();
            var testingBotOptions = CreateTestingBotOptions();

            browserOptions.AddAdditionalOption("tb:options", testingBotOptions);

            _driver = new RemoteWebDriver(
                new Uri("https://hub.testingbot.com/wd/hub"),
                browserOptions.ToCapabilities(),
                TimeSpan.FromSeconds(600));

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public async Task RecordNetworkResponse()
        {
            INetwork networkInterceptor = _driver!.Manage().Network;
            networkInterceptor.NetworkResponseReceived += (_, e) =>
            {
                Console.WriteLine($"Response body: {e.ResponseBody}");
                Console.WriteLine($"Response headers: {string.Join(", ", e.ResponseHeaders.Select(h => $"{h.Key}: {h.Value}"))}");
                Console.WriteLine($"Response status code: {e.ResponseStatusCode}");
                Console.WriteLine($"Response URL: {e.ResponseUrl}");
            };
            await networkInterceptor.StartMonitoring();

            _driver!.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/alerts.html");
            await networkInterceptor.StopMonitoring();
        }

        private ChromeOptions CreateBrowserOptions()
        {
            var options = new ChromeOptions
            {
                BrowserVersion = _version,
                PlatformName = _os,
                UseWebSocketUrl = true
            };
            return options;
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }

        private Dictionary<string, object> CreateTestingBotOptions()
        {
            var key = Environment.GetEnvironmentVariable("TESTINGBOT_KEY");
            var secret = Environment.GetEnvironmentVariable("TESTINGBOT_SECRET");

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException(
                    "TESTINGBOT_KEY and TESTINGBOT_SECRET environment variables must be set");
            }

            return new Dictionary<string, object>
            {
                ["key"] = key,
                ["secret"] = secret,
                ["name"] = $"{TestContext.CurrentContext.Test.Name}",
                ["selenium-version"] = "4.33.0",
                ["build"] = $"DevTools-Tests-{DateTime.Now:yyyy-MM-dd}"
            };
        }
    }
}