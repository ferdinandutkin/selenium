using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Lib
{
    public class HomePage : PageObject
    {

        public LoginPage ClickLogin()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                 Until(ExpectedConditions.
                 ElementToBeClickable(By.XPath("//div[text()='LOG IN']"))).
                 Click();

            return new LoginPage(_webDriver);

        }


        public HomePage(IWebDriver webDriver) : base(webDriver)
        {

        }
    }
}