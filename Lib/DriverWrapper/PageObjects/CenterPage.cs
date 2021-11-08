using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Lib
{
    public class CenterPage : PageObject
    {

        public TradingPage ClickTrade()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(By.XPath("//a[text()='TRADE']"))).
                Click();

            _webDriver.SwitchTo().Window(_webDriver.WindowHandles[1]);

            return new TradingPage(_webDriver);
        }


        public CenterPage(IWebDriver webDriver) : base(webDriver)
        {

        }

    }
}