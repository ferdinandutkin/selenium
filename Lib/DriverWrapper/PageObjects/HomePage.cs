using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Lib
{
    public class HomePage : PageObject
    {

        [FindsBy(How = How.XPath, Using = "//div[text()='LOG IN']")]
        private IWebElement loginButton;

        public LoginPage ClickLogin()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                 Until(ExpectedConditions.
                 ElementToBeClickable(loginButton)).
                 Click();

            return new LoginPage(_webDriver);

        }


        public HomePage(IWebDriver webDriver) : base(webDriver)
        {

        }
    }
}