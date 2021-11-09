using Lib.WebullApi;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Lib
{
    public class LoginPage : PageObject
    {

        private string _login;
        private string _password;
        public LoginPage(IWebDriver webDriver) : base(webDriver)
        {

        }


        public LoginPage SwitchToLoginViaEmail()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(By.XPath("//div[text()='Email Login']"))).
                Click();

            return this;
        }

        public LoginPage EnterLogin(string login)
        {
            _login = login;

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(By.CssSelector("input[placeholder='Email Address']"))).
                SendKeys(login);

            return this;
        }



        public LoginPage EnterPassword(string password)
        {

            _password = password;

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
               Until(ExpectedConditions.
               ElementToBeClickable(By.CssSelector("input[placeholder='Password']"))).
               SendKeys(password);

            return this;
        }


        public CenterPage BypassCapcha()
        {
            var api = new WebullApiWrapper();

            _webDriver.Navigate().GoToUrl(api.GetCaptchaBypassUrlViaEmail(_login, _password));

            return new CenterPage(_webDriver);
        }

        public LoginPage Submit()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(By.XPath("//button/span[text()='Log In']"))).
                Click();

            return this;


        }


    }
}