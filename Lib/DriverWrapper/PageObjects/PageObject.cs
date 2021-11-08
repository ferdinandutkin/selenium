using OpenQA.Selenium;

namespace Lib
{
    public class PageObject
    {
        protected IWebDriver _webDriver;

        public PageObject(IWebDriver driver)
        {
            _webDriver = driver;
        }
    }
}