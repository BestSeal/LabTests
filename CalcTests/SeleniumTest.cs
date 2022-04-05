using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Assert = Xunit.Assert;

namespace CalcTests;

public class SeleniumTest
{
    public static string chromeDriverPath = Environment.CurrentDirectory;

    private IWebDriver _driver;

    [OneTimeSetUp]
    public void Setup() => _driver = new ChromeDriver(chromeDriverPath);

    [Test]
    public void verifyLogo()
    {
        _driver.Navigate().GoToUrl("https://www.google.com/");

        var searchBar = _driver.FindElement(By.Name("q"));
        searchBar.SendKeys("total war warhammer");
        searchBar.SendKeys(Keys.Return);

        Thread.Sleep(200);

        var steamLink = _driver.FindElement(By.CssSelector(@"a[href='https://store.steampowered.com/app/364360/Total_War_WARHAMMER/']"));
        steamLink.Click();
        
        Thread.Sleep(1000);
        
        var tags = _driver.FindElements(By.CssSelector(@"a[class=app_tag]")).FirstOrDefault(x => x.Text == "Стратегия");
        
        Assert.NotNull(tags);
        
        _driver.Close();
    }
}