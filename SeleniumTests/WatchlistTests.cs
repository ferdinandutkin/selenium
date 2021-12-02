using Lib;
using Xunit;

namespace SeleniumTests
{
    [Collection("Sequential")]
    public class WatchlistTests : WebullTestBase
    {

        private TradingPage _tradingPage;


        protected override void Prerequirments()
        {
            _tradingPage = new HomePage(Driver).ClickLogin()
                                               .SwitchToLoginViaEmail()
                                               .EnterLogin(Credentials.Login)
                                               .EnterPassword(Credentials.Password)
                                               .Submit()
                                               .BypassCapcha()
                                               .ClickTrade()
                                               .ClickPaperTrade();

        }
        public WatchlistTests() => Prerequirments();

        [Fact]
        public void AddingSelectedCompanyToMyWatchlist_AddsItToMyWatchlist()
        {
            _tradingPage.GetSelectedCompanyName(out var name)
                        .OpenGraphContextMenu()
                        .AddToMyWatchList()
                        .GetCompaniesFromMyWatchlist(out var names);

            Assert.Contains(name, names);
       
        }

        protected override void Dispose(bool disposing)
        {
           
            if (disposing)
            {
                _tradingPage.RemoveLastEntryFromMyWatchList();
            }
            base.Dispose(disposing);
        }
    }
}
