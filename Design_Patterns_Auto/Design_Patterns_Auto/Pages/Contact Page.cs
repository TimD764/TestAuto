using Design_Patterns_Auto.Pages;
using OpenQA.Selenium;

namespace EhuAutomationNUnit.Pages
{
    public class ContactPage : BasePage
    {
        public string GetPageBodyText()
        {
            return WaitForVisible(By.TagName("body")).Text;
        }
    }
}