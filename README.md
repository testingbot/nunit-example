[![Tests](https://github.com/testingbot/nunit-example/actions/workflows/test.yml/badge.svg?branch=main)](https://github.com/testingbot/nunit-example/actions/workflows/test.yml)

## TestingBot - NUnit

TestingBot provides an online grid of browsers and mobile devices to run Automated tests on via Selenium WebDriver.
This example demonstrates how to use NUnit to run a test in parallel across several browsers.

### Requirements

* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (the project targets `net9.0`; the .NET 8 SDK fails with `NETSDK1045`)

### Environment Setup

1. TestingBot Credentials
    * Add your TestingBot Key and Secret as environmental variables. You can find these in the [TestingBot Dashboard](https://testingbot.com/members/).
    ```
    $ export TESTINGBOT_KEY=<your TestingBot Key>
    $ export TESTINGBOT_SECRET=<your TestingBot Secret>
    ```

2. Setup
    * Clone the repo

3. Run tests
    * Single test: `dotnet test --filter "TestCategory=Single"`
    * Parallel tests: `dotnet test --filter "TestCategory=Parallel"` (runs 2 tests simultaneously)
    * DevTools (CDP) test: `dotnet test --filter "TestCategory=DevTools"` (runs a CDP test)
    * WebDriver BiDi test: `dotnet test --filter "TestCategory=BiDi"`

### Running your tests from Test Explorer via NUnit Test Adapter
Click Run Unit Tests, you will see the test result in the [TestingBot Dashboard](https://testingbot.com/members/)


### Resources
##### [TestingBot Documentation](https://testingbot.com/support/web-automate/selenium/csharp/nunit)

##### [Selenium Documentation](https://www.selenium.dev/documentation/)

##### [NUnit Documentation](https://nunit.org/)

