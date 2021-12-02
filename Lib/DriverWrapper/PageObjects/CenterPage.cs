using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Lib
{
    public class CenterPage : PageObject
    {

        [FindsBy(How = How.XPath, Using = "//a[text()='TRADE']")]
        private IWebElement _tradeButton;
        public TradingPage ClickTrade()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(_tradeButton)).
                Click();

            _webDriver.SwitchTo().Window(_webDriver.WindowHandles[1]);

            return new TradingPage(_webDriver);
        }


        public CenterPage(IWebDriver webDriver) : base(webDriver)
        {

        }

    }
}