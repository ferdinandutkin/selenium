using Lib.WebullApi;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Lib
{
    public class LoginPage : PageObject
    {

        private string _login;
        private string _password;

        [FindsBy(How = How.CssSelector, Using = "input[placeholder='Password']")]
        private IWebElement _passwordInput;


        [FindsBy(How = How.CssSelector, Using = "input[placeholder='Email Address']")]
        private IWebElement _loginInput;


        [FindsBy(How = How.XPath, Using = "//button/span[text()='Log In']")]
        private IWebElement _loginButton;

        [FindsBy(How = How.XPath, Using = "//div[text()='Email Login']")]
        private IWebElement _emailLoginButton;

        public LoginPage(IWebDriver webDriver) : base(webDriver)
        {

        }

        public LoginPage EnterLogin(string login)
        {
            _login = login;

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(_loginInput)).
                SendKeys(login);

            return this;
        }

      
        

        public LoginPage EnterPassword(string password)
        {

            _password = password;

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
               Until(ExpectedConditions.
               ElementToBeClickable(_passwordInput)).
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
                ElementToBeClickable(_loginButton)).
                Click();

            return this;


        }


        public LoginPage SwitchToLoginViaEmail()
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10)).
                Until(ExpectedConditions.
                ElementToBeClickable(_emailLoginButton)).
                Click();

            return this;
        }


    }
}