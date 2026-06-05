using TestAutoApproaches.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace TestAutoApproaches.Pages
{
    public class MainPage : BasePage
    {
        private readonly By _aboutLink = By.LinkText("About");
        private readonly By _cookieAcceptButton = By.XPath("//button[contains(., 'I agree') or contains(., 'Accept')]");
        private readonly By _ltLanguageButton = By.XPath("//a[text()='LT' or text()='lt' or contains(@href, 'lt.')]");
        private readonly By _studyProgramLinks = By.CssSelector("a[href*='/studies/']");

        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void ClickAbout()
        {
            Wait.Until(d => d.FindElement(_aboutLink)).Click();
        }

        public void AcceptCookiesIfPresent()
        {
            try
            {
                Wait.Until(d => d.FindElement(_cookieAcceptButton)).Click();
            }
            catch (WebDriverTimeoutException) { /* Cookies banner didn't show up */ }
        }

        public void SwitchToLithuanian()
        {
            var ltButton = Wait.Until(d => d.FindElement(_ltLanguageButton));
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", ltButton);
        }

        public int GetStudyProgramLinksCount()
        {
            return Driver.FindElements(_studyProgramLinks).Count;
        }
    }
}