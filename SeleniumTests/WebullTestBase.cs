using Lib;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Text.Json;

namespace SeleniumTests;

public abstract class WebullTestBase : IDisposable
{

    protected readonly IWebDriver Driver;

    protected readonly Credentials Credentials;

    protected readonly string Url = "https://www.webull.com/";
    private bool disposedValue;

    public WebullTestBase()
    {
        string currentDir = Environment.CurrentDirectory;

        var driverPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\drivers\"));

        var credentialsPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\credentials.json"));

        Credentials = JsonSerializer.Deserialize<Credentials>(File.ReadAllText(credentialsPath));

        Driver = new ChromeDriver(driverPath);

        Driver.Manage().Window.Maximize();

        Driver.Url = Url;

    }

    protected abstract void Prerequirments();

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Driver.Dispose();
            }

            disposedValue = true;
        }
    }

 
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
