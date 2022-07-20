## TestingBot - NUnit

TestingBot provides an online grid of browsers and mobile devices to run Automated tests on via Selenium WebDriver.
This example demonstrates how to use NUnit to run a test in parallel across several browsers.

### Environment Setup

1. Global Dependencies
    * MS Visual Studio 2022 or later.
    * Install the NUnit and NUnit3TestAdapter packages through NuGet

2. TestingBot Credentials
    * Add your TestingBot Key and Secret as environmental variables. You can find these in the [TestingBot Dashboard](https://testingbot.com/members/).
    ```
    $ export TESTINGBOT_KEY=<your TestingBot Key>
    $ export TESTINGBOT_SECRET=<your TestingBot Secret>
    ```

3. Setup
    * Clone the repo
	* Open the solution `NUnit-TestingBot-Sample.sln` in Visual Studio 2022 or higher
	* Build the solution

### Running your tests from Test Explorer via NUnit Test Adapter
Click Run Unit Tests, you will see the test result in the [TestingBot Dashboard](https://testingbot.com/members/)

### Resources
##### [TestingBot Documentation](https://testingbot.com/support/)

##### [SeleniumHQ Documentation](http://www.seleniumhq.org/docs/)

##### [NUnit Documentation](https://nunit.org/)

