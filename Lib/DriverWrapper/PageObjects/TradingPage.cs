using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lib
{
    public class TradingPage : PageObject
    {


        private readonly NumberFormatInfo _currencyFormatter = new() { CurrencyGroupSeparator = ",", CurrencySymbol = "", CurrencyDecimalSeparator = ".", NumberDecimalDigits = 2 };
        public TradingPage(IWebDriver webDriver) : base(webDriver)
        {

        }


        public decimal GetLimitPrice()
        {


            var containsDot = new Regex(@".*\..*");

            var limitPriceElementSelector = By.XPath("//div/span[text() = 'Limit Price']//ancestor::div[2]//div[2]/div[1]/input");

            var limitPriceElement = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.ElementIsVisible(limitPriceElementSelector));

            var l = limitPriceElement.GetAttribute("value");

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(_ => containsDot.IsMatch(limitPriceElement.GetAttribute("value")));

            var limitPriceText = limitPriceElement.GetAttribute("value");

            NumberFormatInfo formatInfo = new() { CurrencyGroupSeparator = ",", CurrencySymbol = "" };

            return decimal.Parse(limitPriceText, formatInfo);

        }

        public decimal GetNetAccountValue()
        {

            var containsDot = new Regex(@".*\..*");

            var netAccountValueSelector = By.XPath("//div[starts-with(text(), 'Net Account Value')]//following-sibling::div");

            var netAccountValueElement = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.ElementIsVisible(netAccountValueSelector));

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(_ => containsDot.IsMatch(netAccountValueElement.Text));

            var netAccoutValueText = netAccountValueElement.Text;

            return decimal.Parse(netAccoutValueText, _currencyFormatter);

        }


        public TradingPage ChooseBuy()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                    Until(ExpectedConditions.
                    ElementToBeClickable(By.XPath("//div[text() = 'Buy']"))).Click();

            return this;
        }


        public TradingPage ChooseUSD()
        {
            IReadOnlyCollection<IWebElement> elements = null;

            do
            {
                new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                    Until(ExpectedConditions.
                    ElementToBeClickable(By.
                    XPath("//*[contains(@class,'webull-Amount__')] | //*[contains(@class,'webull-Quantity__')] | //*[contains(@class,'webull-Percentage__')]"))).
                    Click();

                elements = _webDriver.FindElements(By.CssSelector("i.webull-Amount__"));
            }
            while (elements!.Count != 1);


            return this;

        }


        public TradingPage EnterAmount(decimal amount)
        {

            string formatted = amount.ToString(_currencyFormatter);

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(By.XPath("//input[@valuetype='cash']"))).
                SendKeys(formatted);

            return this;

        }
        public TradingPage SubmitTrade()
        {
            var selector = By.XPath("//button[text()='Paper Trade']");

            var element = _webDriver.FindElement(selector);

            new Actions(_webDriver).MoveToElement(element).Perform();

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(selector)).
                Click();

            return this;


        }


        public TradingPage WaitUntilTradeWithGivenPriceAppears(decimal price)
        {
            var formatted = price.ToString(_currencyFormatter);

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(30)).
                    Until(ExpectedConditions.
                    ElementExists(By.
                    XPath($"//td/div[text()='{formatted}']")));

            return this;
        }


        public TradingPage ClickPaperTrade()
        {

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(By.CssSelector("i.webull-Papertrading__"))).
                Click();

            return this;

        }


    }
}