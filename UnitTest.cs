using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests
{
    [TestClass]
    public class SiteTests
    {
        private IWebDriver? driver;

        [TestInitialize]
        public void Setup()
        {
            try
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://www.1morepage.com.ua/");
            }
            catch (Exception ex)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Setup failed: " + ex.Message);
            }
        }

        [TestCleanup]
        public void Teardown()
        {
            driver?.Quit();
        }

        private void WaitUntilVisible(By selector, int timeoutSeconds = 10)
        {
            var wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(selector));
        }

        [TestMethod]
        public void Test_ShcheOdnuStorinku_NavigatesToHome()
        {
            var link = driver!.FindElement(By.LinkText("Ще одну сторінку"));
            link.Click();
            Thread.Sleep(1000);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(driver.Url.Contains("1morepage.com.ua"));
        }

        [TestMethod]
        public void Test_Holovna_NavigatesToHome()
        {
            var link = driver!.FindElement(By.LinkText("Головна"));
            link.Click();
            Thread.Sleep(1000);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(driver.Url.Contains("1morepage.com.ua"));
        }

        [TestMethod]
        public void Test_Magazyn_NavigatesToCatalog()
        {
            var link = driver!.FindElement(By.LinkText("Магазин"));
            link.Click();
            Thread.Sleep(1000);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(driver.Url.Contains("/bookstore") || driver.Title.Contains("Каталог"));
        }

        [TestClass]
        public class DeProfundisTests
        {
            private IWebDriver? driver;
            private WebDriverWait? wait;

            [TestInitialize]
            public void Setup()
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://www.1morepage.com.ua/product-page/de-profundis");
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            }

            [TestCleanup]
            public void Teardown()
            {
                driver?.Quit();
            }

            private void WaitUntilVisible(By selector, int timeoutSeconds = 10)
            {
                new WebDriverWait(driver!, TimeSpan.FromSeconds(timeoutSeconds))
                    .Until(ExpectedConditions.ElementIsVisible(selector));
            }

            [TestMethod]
            public void Test_DeProfundis_PageLoads()
            {
                WaitUntilVisible(By.CssSelector("h1[data-hook='product-title']"));
                var title = driver!.FindElement(By.CssSelector("h1[data-hook='product-title']")).Text;
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(title.Contains("De Profundis", StringComparison.OrdinalIgnoreCase), "Incorrect product loaded.");
            }

            [TestMethod]
            public void Test_DeProfundis_AddToCart()
            {
                WaitUntilVisible(By.CssSelector("button[data-hook='add-to-cart']"));
                var addToCartButton = driver!.FindElement(By.CssSelector("button[data-hook='add-to-cart']"));
                addToCartButton.Click();

                WaitUntilVisible(By.CssSelector(".header-cart .count"));
                var cartCount = driver.FindElement(By.CssSelector(".header-cart .count")).Text;

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(!string.IsNullOrWhiteSpace(cartCount) && cartCount != "0", "Item was not added to the cart.");
            }
        }

    }
}
