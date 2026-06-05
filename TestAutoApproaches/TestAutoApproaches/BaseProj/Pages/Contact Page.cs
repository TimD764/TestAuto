using OpenQA.Selenium;

namespace TestAutoApproaches.Pages
{
    public class ContactPage : BasePage
    {
        public string GetPageBodyText()
        {
            return WaitForVisible(By.TagName("body")).Text;
        }
    }
}