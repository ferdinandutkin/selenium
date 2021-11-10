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
        private readonly DriverWrapper _wrapper;
        public UnitTest()
        {
            string currentDir = Environment.CurrentDirectory;

            var driverPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\drivers\"));

            var credentialsPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\credentials.json"));

            var credentials = JsonSerializer.Deserialize<Credentials>(File.ReadAllText(credentialsPath));

            _wrapper = new DriverWrapper(new ChromeDriver(driverPath), credentials);
            _wrapper.Prerequirements();

        }
      
        [Fact]
        public void PlaceOrder_WhenNetAccountValueIsSufficient()
        {
          
            _wrapper.ChooseBuy();

            _wrapper.ChooseUSD();

            var limitPrice = _wrapper.GetLimitPrice();

            var netAccountValue = _wrapper.GetNetAccountValue();

            Assert.True(netAccountValue > limitPrice);

            _wrapper.Trade(limitPrice);

            _wrapper.WaitUntilTradeWithGivenPriceAppears(limitPrice);
     
        }


        public void Dispose() => _wrapper.Dispose();
        

    }
}