using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

[assembly: Parallelizable(ParallelScope.All)]

namespace EhuAutomationNUnit;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class EhuTests
{
    private IWebDriver _driver;
    private WebDriverWait _wait;

    [SetUp]
    public void Setup()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    private IWebElement WaitForVisible(By locator)
    {
        return _wait.Until(d =>
        {
            var element = d.FindElements(locator).FirstOrDefault();
            return (element != null && element.Displayed) ? element : null;
        });
    }

    [TearDown]
    public void Teardown()
    {
        if (_driver != null)
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Quit(); 
            _driver.Dispose();
        }
    }

    [Test]
    [Category("Navigation")]
    public void VerifyAboutPage_Test1()
    {
        _driver.Navigate().GoToUrl("https://en.ehu.lt/");
        var aboutLink = _wait.Until(d => d.FindElement(By.LinkText("About")));
        aboutLink.Click();
        _wait.Until(d => d.Url.Contains("about/"));

        Assert.That(_driver.Url, Does.Contain("about/"));
        Assert.That(_driver.Title, Does.Contain("About"));
    }

    [TestCase("study programs")]
    [TestCase("admissions")]
    [Category("Search")]
    public void VerifySearch_Test2(string searchTerm)
    {
        _driver.Navigate().GoToUrl($"https://en.ehuniversity.lt/?s={searchTerm.Replace(" ", "+")}");
        _wait.Until(d => d.Url.Contains($"?s={searchTerm.Replace(" ", "+")}"));

        Assert.That(_driver.Url, Does.Contain(searchTerm.Replace(" ", "+")), "URL did not contain query");

        var resultLinks = _driver.FindElements(By.CssSelector("a[href*='/studies/']"));
        Assert.That(resultLinks.Count, Is.GreaterThan(0), "No links to study programs.");
    }

    [Test]
    [Category("UI")]
    public void VerifyLanguageChange_Test3()
    {
        _driver.Navigate().GoToUrl("https://en.ehuniversity.lt/");

        try
        {
            var cookies = _wait.Until(d => d.FindElement(By.XPath("//button[contains(., 'I agree') or contains(., 'Accept')]")));
            cookies.Click();
        }
        catch { }

        var ltButton = _wait.Until(d => d.FindElement(By.XPath("//a[text()='LT' or text()='lt' or contains(@href, 'lt.')]")));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", ltButton);
        _wait.Until(d => d.Url.Contains("lt."));

        Assert.That(_driver.Url, Does.Contain("lt."), "URL did not change to lithuanian subdomain");
    }

    [Test]
    [Category("UI")]
    public void VerifyContacts_Test4()
    {
        _driver.Navigate().GoToUrl("https://en.ehuniversity.lt/contact/");
        var bodyText = WaitForVisible(By.TagName("body")).Text;

        Assert.Multiple(() =>
        {
            Assert.That(bodyText, Does.Contain("franciskscarynacr@gmail.com"), "Email not found");
            Assert.That(bodyText, Does.Contain("+370 68 771365"), "There is no lithuanian number");
            Assert.That(bodyText, Does.Contain("+375 29 5781488"), "There is no Balarusian number");
            Assert.That(bodyText, Does.Contain("Facebook"), "No facebook link");
            Assert.That(bodyText, Does.Contain("Telegram"), "No Telegram link");
            Assert.That(bodyText, Does.Contain("VK"), "No vkontakte link");
        });
    }
}