using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace Lib
{
    public class PageObject
    {
        protected IWebDriver _webDriver;


        public PageObject(IWebDriver driver)
        {
            _webDriver = driver;
            PageFactory.InitElements(driver, this);
        }
    }
}