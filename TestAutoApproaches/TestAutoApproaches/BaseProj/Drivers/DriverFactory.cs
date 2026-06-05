using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace TestAutoApproaches.Drivers
{
    public static class DriverFactory
    {
        public static IWebDriver CreateDriver(string browserType)
        {
            return browserType.ToLower() switch
            {
                "chrome" => new ChromeDriver(),
                //  add "firefox" or "edge" here later if needed
                _ => throw new ArgumentException($"Browser type '{browserType}' is not supported.")
            };
        }
    }
}