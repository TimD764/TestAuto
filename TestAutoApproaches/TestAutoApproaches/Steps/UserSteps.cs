using NUnit.Framework;
using Reqnroll;
using TestAutoApproaches.Drivers;
using TestAutoApproaches.Pages;
using TestAutoApproaches.Models;

namespace TestAutoApproaches.Steps
{
    [Binding]
    public class EhuUserJourneySteps
    {
        private readonly MainPage _mainPage = new MainPage();
        private readonly ContactPage _contactPage = new ContactPage();
        private SearchContext _searchContext;

        [Given(@"I navigate to the EHU home page")]
        public void GivenINavigateToTheEHUHomePage()
        {
            _mainPage.NavigateTo("https://en.ehuniversity.lt/");
        }

        [When(@"I click on the ""(.*)"" tab in the main navigation")]
        public void WhenIClickOnTheTabInTheMainNavigation(string tabName)
        {
            _mainPage.ClickAbout(); // You can parameterize this further in the Page Object if needed
        }

        [Then(@"I should be redirected to the About page")]
        public void ThenIShouldBeRedirectedToTheAboutPage()
        {
            Assert.That(DriverManager.Instance.Driver.Url, Does.Contain("about/"));
        }

        [Then(@"the page title should contain ""(.*)""")]
        public void ThenThePageTitleShouldContain(string expectedTitle)
        {
            Assert.That(DriverManager.Instance.Driver.Title, Does.Contain(expectedTitle));
        }

        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string searchTerm)
        {
            _searchContext = new SearchContextBuilder()
                .WithSearchTerm(searchTerm)
                .Build();

            _mainPage.NavigateTo($"https://en.ehuniversity.lt/?s={_searchContext.ExpectedSubString}");
        }

        [Then(@"the search results page should include the query in the URL")]
        public void ThenTheSearchResultsPageShouldIncludeTheQueryInTheURL()
        {
            Assert.That(DriverManager.Instance.Driver.Url, Does.Contain(_searchContext.ExpectedSubString));
        }

        [Then(@"the search results should contain links to study programs")]
        public void ThenTheSearchResultsShouldContainLinksToStudyPrograms()
        {
            Assert.That(_mainPage.GetStudyProgramLinksCount(), Is.GreaterThan(0), "No links to study programs found.");
        }

        [When(@"I accept cookies if present")]
        public void WhenIAcceptCookiesIfPresent()
        {
            _mainPage.AcceptCookiesIfPresent();
        }

        [When(@"I change the language to Lithuanian")]
        public void WhenIChangeTheLanguageToLithuanian()
        {
            _mainPage.SwitchToLithuanian();
        }

        [Then(@"I should be redirected to the Lithuanian version of the site")]
        public void ThenIShouldBeRedirectedToTheLithuanianVersionOfTheSite()
        {
            Assert.That(DriverManager.Instance.Driver.Url, Does.Contain("lt."));
        }

        [Given(@"I navigate to the EHU contact page")]
        public void GivenINavigateToTheEHUContactPage()
        {
            _mainPage.NavigateTo("https://en.ehuniversity.lt/contact/");
        }

        [Then(@"the contact information should be visible to the user")]
        public void ThenTheContactInformationShouldBeVisibleToTheUser()
        {
            string bodyText = _contactPage.GetPageBodyText();

            Assert.Multiple(() =>
            {
                Assert.That(bodyText, Does.Contain("franciskscarynacr@gmail.com"), "Email not found");
                Assert.That(bodyText, Does.Contain("+370 68 771365"), "LT phone missing");
                Assert.That(bodyText, Does.Contain("+375 29 5781488"), "BY phone missing");
                Assert.That(bodyText, Does.Contain("Facebook"), "Facebook link missing");
                Assert.That(bodyText, Does.Contain("Telegram"), "Telegram link missing");
                Assert.That(bodyText, Does.Contain("VK"), "VK link missing");
            });
        }
    }
}