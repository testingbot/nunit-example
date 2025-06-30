using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;

namespace NUnit_TestingBot_Sample
{
    [TestFixture("chrome", "latest", "Windows 10")]
    [Category("BiDi")]
    public class BidiTest
    {
        private IWebDriver _driver;
        private CancellationToken _token = CancellationToken.None;
        private readonly string _browser;
        private readonly string _version;
        private readonly string _os;

        public BidiTest(string browser, string version, string os)
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
                TimeSpan.FromSeconds(60)
            );
        }

        [Test]
        public async Task CaptureNetworkLog()
        {
            _driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/bidi/logEntryAdded.html");
            var bidi = await _driver.AsBiDiAsync();

            await bidi.Network.OnResponseStartedAsync(res =>
            {
                Console.WriteLine($"URL: {res.Request.Url}");
                Console.WriteLine($"Method: {res.Request.Method}");
                Console.WriteLine($"Status: {res.Response.Status}");
                Console.WriteLine($"Headers: {string.Join(", ", res.Response.Headers.Select(h => $"{h.Name}: {h.Value}"))}");
            });

            _driver.FindElement(By.Id("jsException")).Click();
            System.Threading.Thread.Sleep(4000);
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver == null) return;

            var testPassed = TestContext.CurrentContext.Result.Outcome.Status ==
                            NUnit.Framework.Interfaces.TestStatus.Passed;

            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript($"tb:test-result={testPassed.ToString().ToLower()}");
            }
            catch (Exception ex)
            {
                TestContext.Out.WriteLine($"Failed to report test result: {ex.Message}");
            }
            finally
            {
                _driver.Quit();
                _driver.Dispose();
            }
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
                ["name"] = TestContext.CurrentContext.Test.Name,
                ["selenium-version"] = "4.27.0"
            };
        }

        private DriverOptions CreateBrowserOptions()
        {
            return _browser.ToLower() switch
            {
                "firefox" => new FirefoxOptions
                {
                    BrowserVersion = _version,
                    PlatformName = _os,
                    UseWebSocketUrl = true
                },
                "safari" => new SafariOptions
                {
                    BrowserVersion = _version,
                    PlatformName = _os,
                    UseWebSocketUrl = true
                },
                _ => new ChromeOptions
                {
                    BrowserVersion = _version,
                    PlatformName = _os,
                    UseWebSocketUrl = true
                }
            };
        }
    }
}
