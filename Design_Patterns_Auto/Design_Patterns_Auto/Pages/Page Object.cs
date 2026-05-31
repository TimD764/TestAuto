using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using EhuAutomationNUnit.Drivers;
using System;
using System.Linq;

namespace Design_Patterns_Auto.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver => DriverManager.Instance.Driver;
        protected WebDriverWait Wait;

        protected BasePage()
        {
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        }

        protected IWebElement WaitForVisible(By locator)
        {
            return Wait.Until(d =>
            {
                var element = d.FindElements(locator).FirstOrDefault();
                return (element != null && element.Displayed) ? element : null;
            });
        }
    }
}