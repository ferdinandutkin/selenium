using Lib;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Text.Json;
using Xunit;

namespace Selenium
{
    public class UnitTest : IDisposable
    {
        readonly DriverWrapper wrapper;
        public UnitTest()
        {
            string currentDir = Environment.CurrentDirectory;

            var driverPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\drivers\"));

            var credentialsPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\credentials.json"));

            var credentials = JsonSerializer.Deserialize<Credentials>(File.ReadAllText(credentialsPath));

            wrapper = new DriverWrapper(new ChromeDriver(driverPath), credentials);

        }
      
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PlaceOrder_WhenNetAccountValueIsSufficient(bool bypassCaptcha)
        {
            wrapper.Prerequirements(bypassCaptcha);

            wrapper.ChooseBuy();

            wrapper.ChooseUSD();

            var limitPrice = wrapper.GetLimitPrice();

            var netAccountValue = wrapper.GetNetAccountValue();

            Assert.True(netAccountValue > limitPrice);

            wrapper.Trade(limitPrice);

            wrapper.WaitUntilTradeWithGivenPriceAppears(limitPrice);

           
        }


        public void Dispose()
        {
            wrapper.Quit();
            wrapper.Dispose();
        }

    }
}