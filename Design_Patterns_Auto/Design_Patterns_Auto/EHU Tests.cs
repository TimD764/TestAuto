using NUnit.Framework;
using EhuAutomationNUnit.Drivers;
using EhuAutomationNUnit.Pages;
using EhuAutomationNUnit.Models;

[assembly: Parallelizable(ParallelScope.All)] // Parallelization completely preserved

namespace EhuAutomationNUnit.Tests
{
    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class EhuTests
    {
        private MainPage _mainPage;
        private ContactPage _contactPage;

        [SetUp]
        public void Setup() // Test setup using Factory and Singleton patterns
        {
            var rawDriver = DriverFactory.CreateDriver("chrome");
            DriverManager.Instance.InitDriver(rawDriver);
            DriverManager.Instance.Driver.Manage().Window.Maximize();

            _mainPage = new MainPage();
            _contactPage = new ContactPage();
        }

        [TearDown]
        public void Teardown() // Test tear down
        {
            DriverManager.Instance.QuitDriver();
        }

        [Test]
        [Category("Navigation")]
        public void VerifyAboutPage_Test1()
        {
            _mainPage.NavigateTo("https://en.ehu.lt/");
            _mainPage.ClickAbout();

            Assert.That(DriverManager.Instance.Driver.Url, Does.Contain("about/"));
            Assert.That(DriverManager.Instance.Driver.Title, Does.Contain("About"));
        }

        [TestCase("study programs")]
        [TestCase("admissions")]
        [Category("Search")]
        public void VerifySearch_Test2(string searchTerm)
        {
            // Data Setup via Builder Pattern
            var searchContext = new SearchContextBuilder()
                .WithSearchTerm(searchTerm)
                .Build();

            _mainPage.NavigateTo($"https://en.ehuniversity.lt/?s={searchContext.ExpectedSubString}");

            Assert.That(DriverManager.Instance.Driver.Url, Does.Contain(searchContext.ExpectedSubString), "URL did not contain query");
            Assert.That(_mainPage.GetStudyProgramLinksCount(), Is.GreaterThan(0), "No links to study programs.");
        }

        [Test]
        [Category("UI")]
        public void VerifyLanguageChange_Test3()
        {
            _mainPage.NavigateTo("https://en.ehuniversity.lt/");
            _mainPage.AcceptCookiesIfPresent();
            _mainPage.SwitchToLithuanian();

            Assert.That(DriverManager.Instance.Driver.Url, Does.Contain("lt."), "URL did not change to Lithuanian subdomain");
        }

        [Test]
        [Category("UI")]
        public void VerifyContacts_Test4()
        {
            _mainPage.NavigateTo("https://en.ehuniversity.lt/contact/");
            string bodyText = _contactPage.GetPageBodyText();

            Assert.Multiple(() =>
            {
                Assert.That(bodyText, Does.Contain("franciskscarynacr@gmail.com"), "Email not found");
                Assert.That(bodyText, Does.Contain("+370 68 771365"), "There is no Lithuanian number");
                Assert.That(bodyText, Does.Contain("+375 29 5781488"), "There is no Belarusian number");
                Assert.That(bodyText, Does.Contain("Facebook"), "No Facebook link");
                Assert.That(bodyText, Does.Contain("Telegram"), "No Telegram link");
                Assert.That(bodyText, Does.Contain("VK"), "No VK link");
            });
        }
    }
}