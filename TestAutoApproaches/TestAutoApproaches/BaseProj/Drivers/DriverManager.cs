using OpenQA.Selenium;
using System;
using System.Threading;

namespace TestAutoApproaches.Drivers
{
    public sealed class DriverManager
    {
        private static readonly Lazy<DriverManager> _instance = new Lazy<DriverManager>(() => new DriverManager());
        private readonly ThreadLocal<IWebDriver> _driver = new ThreadLocal<IWebDriver>();

        private DriverManager() { }

        public static DriverManager Instance => _instance.Value;

        public IWebDriver Driver
        {
            get => _driver.Value ?? throw new InvalidOperationException("Driver has not been initialized. Call InitDriver first.");
            set => _driver.Value = value;
        }

        public void InitDriver(IWebDriver driver)
        {
            _driver.Value = driver;
        }

        public void QuitDriver()
        {
            if (_driver.Value != null)
            {
                _driver.Value.Manage().Cookies.DeleteAllCookies();
                _driver.Value.Quit();
                _driver.Value.Dispose();
                _driver.Value = null;
            }
        }
    }
}