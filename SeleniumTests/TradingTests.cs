using Lib;
using System;
using Xunit;

namespace SeleniumTests;

[Collection("Sequential")]
public class TradingTests : WebullTestBase, IDisposable
{


    private TradingPage tradingPage;

    protected override void Prerequirments()
    {
        
        tradingPage = new HomePage(Driver).ClickLogin()
                                          .SwitchToLoginViaEmail()
                                          .EnterLogin(Credentials.Login)
                                          .EnterPassword(Credentials.Password)
                                          .Submit()
                                          .BypassCapcha()
                                          .ClickTrade()
                                          .ClickPaperTrade()
                                          .GetLimitPrice(out var limitPrice)
                                          .GetNetAccountValue(out var netAccountValue);

        Assert.True(netAccountValue > limitPrice, "net account value is smaller that limit price which violates prerequirements and makes placing an order impossible");

    }
    public TradingTests() => Prerequirments();

    [Fact]
    public void PlaceOrder_WhenNetAccountValueIsSufficient()
    {
        tradingPage.ChooseBuy()
                   .ChooseUSD()
                   .GetLimitPrice(out var buyingPrice)
                   .EnterAmount(buyingPrice)
                   .SubmitTrade()
                   .GetActiveTradesPrices(out var prices);

        Assert.Contains(buyingPrice, prices);  

    }

 



}
