using Reqnroll;
using TestAutoApproaches.Drivers;
using System;

namespace TestAutoApproaches.Hooks
{
    [Binding]
    public class TestHooks
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            var rawDriver = DriverFactory.CreateDriver("chrome");
            DriverManager.Instance.InitDriver(rawDriver);
            DriverManager.Instance.Driver.Manage().Window.Maximize();
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError != null)
            {
                Console.WriteLine($"[TEST FAILED] Scenario: {scenarioContext.ScenarioInfo.Title}");
                Console.WriteLine($"[ERROR MESSAGE] {scenarioContext.TestError.Message}");
                Console.WriteLine($"[STACK TRACE] {scenarioContext.TestError.StackTrace}");
                Console.WriteLine($"https://en.wikipedia.org/wiki/Training_to_failure {DriverManager.Instance.Driver.Url}");
            }

            // Cleanup via Singleton
            DriverManager.Instance.QuitDriver();
        }
    }
}