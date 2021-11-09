using OpenQA.Selenium;
using System.Diagnostics;
using SeleniumExtras.PageObjects;

namespace Lib
{

    public class DriverWrapper : IDisposable
    {

        private readonly IWebDriver _webDriver;

        private readonly Credentials _credentials;


        private CenterPage _centerPage;

        private TradingPage _tradePage;

        private readonly string _pageUrl = "https://www.webull.com/";
    
        private void Login(bool bypassCaptcha)
        {
            var loginPage = new HomePage(_webDriver).
                ClickLogin().
                SwitchToLoginViaEmail().
                EnterLogin(_credentials.Login).
                EnterPassword(_credentials.Password);


            _centerPage = bypassCaptcha ? loginPage.BypassCapcha() : loginPage.Submit();

        }

        //c созданным аккаунтом открыть вкладку trade на сайте
        public void Prerequirements(bool bypassCaptcha = false)
        {
            Login(bypassCaptcha);
            GoToPaperTrading();
        }

        public decimal GetLimitPrice() => _tradePage.GetLimitPrice();
  

        public decimal GetNetAccountValue() => _tradePage.GetNetAccountValue();

        private void GoToPaperTrading() => _tradePage = _centerPage.ClickTrade().ClickPaperTrade();
   
        public void Trade(decimal value) => _tradePage.EnterAmount(value).SubmitTrade();

        public void ChooseBuy() => _tradePage.ChooseBuy();

        public void ChooseUSD() => _tradePage.ChooseUSD();

        public void WaitUntilTradeWithGivenPriceAppears(decimal price) => _tradePage.WaitUntilTradeWithGivenPriceAppears(price);

        public void Dispose() => _webDriver.Dispose();
        public void Quit() => _webDriver.Quit();

        public DriverWrapper(IWebDriver webDriver, Credentials credentials)
        {

            this._webDriver = webDriver;
            this._webDriver.Manage().Window.Maximize();
            this._credentials = credentials;
            webDriver.Url = _pageUrl;
        }


    }
}