using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lib
{
    public class TradingPage : PageObject
    {
        [FindsBy(How = How.XPath, Using = "//div[text() = 'Buy']")]
        IWebElement _buyButton;

        [FindsBy(How = How.XPath, Using = "//div[@role='presentation']")]
        IWebElement _graph;

        [FindsBy(How = How.XPath, Using = "//input[@valuetype='cash']")]
        IWebElement _amountInput;

        [FindsBy(How = How.XPath, Using = "//button[text()='Paper Trade']")]
        IWebElement _submitTradeButton;

        [FindsBy(How = How.CssSelector,Using = "i.webull-Papertrading__")]
        IWebElement _paperTradeButton;

        [FindsBy(How = How.CssSelector, Using ="li[draggable='true'] div:first-child span:first-child")]
        IWebElement _lastWatchListEntry;

        private readonly NumberFormatInfo _currencyFormatter = new() { CurrencyGroupSeparator = ",", CurrencySymbol = "", CurrencyDecimalSeparator = ".", NumberDecimalDigits = 2 };
        public TradingPage(IWebDriver webDriver) : base(webDriver)
        {

        }
 

        public TradingPage GetLimitPrice(out decimal limitPrice)
        {

            var containsDot = new Regex(@".*\..*");

            var limitPriceElementSelector = By.XPath("//div/span[text() = 'Limit Price']//ancestor::div[2]//div[2]/div[1]/input");

            var limitPriceElement = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(15)).
                Until(ExpectedConditions.ElementIsVisible(limitPriceElementSelector));

            var l = limitPriceElement.GetAttribute("value");

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(_ => containsDot.IsMatch(limitPriceElement.GetAttribute("value")));

            var limitPriceText = limitPriceElement.GetAttribute("value");

            NumberFormatInfo formatInfo = new() { CurrencyGroupSeparator = ",", CurrencySymbol = "" };

            limitPrice = decimal.Parse(limitPriceText, formatInfo);
            return this;

        }

        public TradingPage GetNetAccountValue(out decimal netAccountValue)
        {

            var containsDot = new Regex(@".*\..*");

            var netAccountValueSelector = By.XPath("//div[starts-with(text(), 'Net Account Value')]//following-sibling::div");

            var netAccountValueElement = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.ElementIsVisible(netAccountValueSelector));

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(_ => containsDot.IsMatch(netAccountValueElement.Text));

            var netAccountValueText = netAccountValueElement.Text;

            netAccountValue = decimal.Parse(netAccountValueText, _currencyFormatter);

            return this;

        }


        public TradingPage ChooseBuy()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                    Until(ExpectedConditions.
                    ElementToBeClickable(_buyButton)).Click();

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


        public TradingPage GetSelectedCompanyName(out string selectedCompanyName)
        {
            var selectedCompanySelector = By.XPath("//input[@placeholder='Symbol' and string-length(translate(normalize-space(@value), ' ', '')) > 0]");

            var selectedCompanyInput = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(20)).
                Until(ExpectedConditions.ElementIsVisible(selectedCompanySelector));

            selectedCompanyName = selectedCompanyInput.GetAttribute("value");


            return this;

        }

        public TradingPage OpenGraphContextMenu()
        {
         
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(20)).
                Until(ExpectedConditions.ElementToBeClickable(_graph));

            var actions = new Actions(_webDriver);

            actions.ContextClick(_graph).Perform();

            return this;
        }

        public TradingPage AddToMyWatchList()
        {
            var addToWatchListContextMenuOptionSelector = By.XPath("//span[text()='Add to Watchlist']");

            var addToWatchListContextMenuOption = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.ElementIsVisible(addToWatchListContextMenuOptionSelector));

            var actions = new Actions(_webDriver);

            actions.MoveToElement(addToWatchListContextMenuOption).Perform();

            var myWatchListOptionSelector = By.XPath("//li[text()='My Watchlist']");

            var myWatchListOption = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
             Until(ExpectedConditions.
             ElementToBeClickable(myWatchListOptionSelector));

            actions.MoveToElement(myWatchListOption).Perform();
            myWatchListOption.Click();


            return this;
        }
        public TradingPage EnterAmount(decimal amount)
        {

            string formatted = amount.ToString(_currencyFormatter);

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(_amountInput)).
                SendKeys(formatted);

            return this;

        }
        public TradingPage SubmitTrade()
        {
            new Actions(_webDriver).MoveToElement(_submitTradeButton).Perform();

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(_submitTradeButton)).
                Click();

            return this;
        }

        public TradingPage GetActiveTradesPrices(out IEnumerable<decimal> prices) 
        {
            
            decimal[] GetCurrent()
            {

                var selector = By.CssSelector("tr td:nth-child(6)");

                return _webDriver.FindElements(selector).Select(element => {
                    if (decimal.TryParse(element.Text, NumberStyles.Any, _currencyFormatter, out var price))
                    {
                        return new decimal?(price);
                    }
                    else return null;
                })
                    .Where(a => a.HasValue)
                    .Select(a => a!.Value).ToArray();
            }


            var initial = GetCurrent();

            var timeout = TimeSpan.FromSeconds(80);

            var step = TimeSpan.FromSeconds(1);

            while (timeout > TimeSpan.Zero)
            {
                Thread.Sleep(step);
                timeout -= step;

                var current = GetCurrent();

                if (current.Except(initial).Any()) {
                    prices = current;
                    return this;
                }

            }

            prices = initial;

            return this;
        }

        public TradingPage RemoveLastEntryFromMyWatchList()
        {
         

            var actions = new Actions(_webDriver);

            var lastEntry = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5)).
                Until(ExpectedConditions.ElementToBeClickable(_lastWatchListEntry));

            actions.ContextClick(lastEntry).Perform();

            var deleteMenuOptionLocator = By.XPath("//div[text()='Delete']");

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5)).
               Until(ExpectedConditions.ElementToBeClickable(deleteMenuOptionLocator)).Click();

            return this;


        }
        public TradingPage GetCompaniesFromMyWatchlist(out IEnumerable<string> names)
        {

            var locator = By.CssSelector("li[draggable='true'] div:first-child span:first-child");

            var initial = _webDriver.FindElements(locator).Select(element => element.Text).ToList();

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(40));

            wait.Until(driver => driver.FindElements(locator).Select(element => element.Text).Except(initial).Count() > 0);

            names = _webDriver.FindElements(locator).Select(element => element.Text);

            return this;

        }
        public TradingPage WaitUntilTradeWithGivenPriceAppears(decimal price)
        {
            var formatted = price.ToString(_currencyFormatter);

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(120)).
                    Until(ExpectedConditions.
                    ElementExists(By.
                    XPath($"//td/div[text()='{formatted}']")));

            return this;
        }


        public TradingPage ClickPaperTrade()
        {

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(_paperTradeButton)).
                Click();

            return this;

        }


    }
}