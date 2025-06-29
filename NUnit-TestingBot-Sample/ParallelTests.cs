using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using FluentAssertions;

namespace NUnit_TestingBot_Sample;

[TestFixture("chrome", "latest", "Windows 10")]
[TestFixture("firefox", "latest", "macOS Sequoia")]
[Parallelizable(ParallelScope.Fixtures)]
[Category("Parallel")]
public class ParallelSeleniumTests
{
    private IWebDriver? _driver;
    private readonly string _browser;
    private readonly string _version;
    private readonly string _os;

    public ParallelSeleniumTests(string browser, string version, string os)
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
        _driver.Manage().Window.Maximize();
    }


    [Test]
    [Parallelizable(ParallelScope.Self)]
    public void ShouldHandleSimpleFormInteraction()
    {
        _driver!.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

        var usernameField = _driver.FindElement(By.Id("username"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var loginButton = _driver.FindElement(By.ClassName("radius"));

        usernameField.SendKeys("tomsmith");
        passwordField.SendKeys("SuperSecretPassword!");
        loginButton.Click();

        var successMessage = _driver.FindElement(By.CssSelector(".flash.success"));
        successMessage.Text.Should().Contain("You logged into a secure area!");
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

    private DriverOptions CreateBrowserOptions()
    {
        return _browser.ToLower() switch
        {
            "firefox" => new FirefoxOptions
            {
                BrowserVersion = _version,
                PlatformName = _os
            },
            "safari" => new SafariOptions
            {
                BrowserVersion = _version,
                PlatformName = _os
            },
            _ => new ChromeOptions
            {
                BrowserVersion = _version,
                PlatformName = _os
            }
        };
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
            ["name"] = $"{TestContext.CurrentContext.Test.Name} ({_browser} on {_os})",
            ["selenium-version"] = "4.27.0"
        };
    }
}
