[![Tests](https://github.com/testingbot/nunit-example/actions/workflows/test.yml/badge.svg?branch=main)](https://github.com/testingbot/nunit-example/actions/workflows/test.yml)

## TestingBot - NUnit

TestingBot provides an online grid of browsers and mobile devices to run Automated tests on via Selenium WebDriver.
This example demonstrates how to use NUnit to run a test in parallel across several browsers.

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
    * Single test: `dotnet test --filter "Single"`
    * Paralle tests: `dotnet test --filter "Parallel"` (run 2 tests simultaneously)
    * DevTools (CDP) test: `dotnet test --filter "DevTools"` (runs a CDP test)
    * WebDriver BiDi test: `dotnet test --filter "BiDi"`

### Running your tests from Test Explorer via NUnit Test Adapter
Click Run Unit Tests, you will see the test result in the [TestingBot Dashboard](https://testingbot.com/members/)


### Resources
##### [TestingBot Documentation](hhttps://testingbot.com/support/web-automate/selenium/csharp/nunit)

##### [SeleniumHQ Documentation](http://www.seleniumhq.org/docs/)

##### [NUnit Documentation](https://nunit.org/)

