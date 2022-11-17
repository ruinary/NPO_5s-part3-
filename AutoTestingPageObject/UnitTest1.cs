using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace AutoTesting
{
    public class Browser
    {
        public static IWebDriver driver;

        public Browser(string url)
        {
            driver = new ChromeDriver();
            OpenOtherPage(url);
        }
        public void OpenOtherPage(string url)
        {
            driver.Navigate().GoToUrl(url);
        }
        public void CloseBrowser()
        {
            driver.Quit();
        }
        public static void ButtonClick(By item)
        {
            driver.FindElement(item).Click();
        }
        public static void ButtonClick(ReadOnlyCollection<IWebElement> items)
        {
            items[0].Click();
        }
        public static ReadOnlyCollection<IWebElement> GetAllElementsBy(By item)
        {
            return driver.FindElements(item);
        }
        public class MainPage
        {
            private static readonly By searchBar = By.Name("q");
            private static readonly By searchButton = By.ClassName("material-icons");
            private static readonly By cartPageButton = By.ClassName("basket-top-new");
            public class TopBar
            {
                private static readonly By topBar1 = By.ClassName("sub-head");
                private static readonly By topBar2 = By.XPath("//div[@class='outer-block nav-outer-block-2']");
                private static readonly By topBar3 = By.XPath("//div[@class='background - site outer-block block-navigation-2']");
                private static readonly By translateToEN = By.LinkText("EN");

                public class Tests
                {
                    Browser browser = new Browser("https://shop-alesyaoao.by/");

                    public static bool isEN(string text)
                    {
                        return (!Regex.IsMatch(text, @"^[А-Яа-я0-9]+[[\s]*[А-Яа-я0-9]*]*$"));
                    }
                    public static bool CheckAllItemsInBlockIsEN(By item)
                    {
                        bool flag = true;

                        var topBarItems = GetAllElementsBy(item);

                        for (int i = 0; i < topBarItems.Count; i++)
                        {
                            if (isEN(topBarItems[i].Text))
                            {
                                flag = false;
                            }

                            if (flag == false)
                            {
                                break;
                            }
                        }

                        return flag;
                    }

                    [Test]
                    public void Test5_1()
                    {
                        ButtonClick(translateToEN);

                        Assert.That(CheckAllItemsInBlockIsEN(topBar1), Is.EqualTo(true));
                    }
                    [Test]
                    public void Test5_2()
                    {
                        ButtonClick(translateToEN);

                        Assert.That(CheckAllItemsInBlockIsEN(topBar2), Is.EqualTo(true));
                    }
                    [Test]
                    public void Test5_3()
                    {
                        ButtonClick(translateToEN);

                        Assert.That(CheckAllItemsInBlockIsEN(topBar3), Is.EqualTo(true));
                    }

                    [TearDown]
                    public void TearDown()
                    {
                        browser.CloseBrowser();
                    }

                }

            }
            public class SearchBar
            {
                private static readonly By catalogItems = By.XPath("//div[@class='catalog-item']");
                public SearchBar(string text)
                {
                    writeSearchTask(text);
                    sendSerchTask();
                }

                public void writeSearchTask(string text)
                {
                    driver.FindElement(searchBar).Clear();
                    driver.FindElement(searchBar).SendKeys(text);
                }
                public void sendSerchTask()
                {
                    driver.FindElement(searchButton).Click();
                }

                public static int quantifyItems(By item)
                {
                    return driver.FindElements(item).Count();
                }

                public class Tests
                {
                    Browser browser = new Browser("https://shop-alesyaoao.by/");

                    [Test]
                    public void Test1()
                    {
                        string searchText = "Платье";

                        new SearchBar(searchText);

                        Console.WriteLine("Page title is: " + driver.Title);

                        Assert.That(driver.Title, Is.EqualTo("Результаты поиска по запросу \"" + searchText + "\""));
                    }

                    [Test]
                    public void Test7()
                    {
                        string searchText = "";

                        new SearchBar(searchText);

                        Console.WriteLine("Found items: " + quantifyItems(catalogItems));

                        Assert.That(quantifyItems(catalogItems), Is.EqualTo(0));
                    }

                    [TearDown]
                    public void TearDown()
                    {
                        browser.CloseBrowser();
                    }
                }
            }
            public static class CompareItems
            {
                private static readonly By addToCompareCatalogItem1 = By.XPath("//a[@class='add-to compare-1168']");
                private static readonly By addToCompareCatalogItem2 = By.XPath("//a[@class='add-to compare-1169']");
                private static readonly By nameOfCompareItem = By.XPath("//a[@class='color-site']");

                public class Tests
                {
                    Browser browser = new Browser("https://shop-alesyaoao.by/");

                    [Test]
                    public void Test2()
                    {
                        browser.OpenOtherPage("https://shop-alesyaoao.by/catalog/genskiy-trikotag");

                        ButtonClick(addToCompareCatalogItem1);
                        ButtonClick(addToCompareCatalogItem2);

                        browser.OpenOtherPage("https://shop-alesyaoao.by/compare");

                        var comparedItems = driver.FindElements(nameOfCompareItem);

                        Console.WriteLine("Added items to compare: " + comparedItems.Count);
                        Console.WriteLine("Total added items to compare: " + 2);

                        Assert.That((comparedItems.Count), Is.EqualTo(2));
                    }

                    [TearDown]
                    public void TearDown()
                    {
                        browser.CloseBrowser();
                    }
                }
            }
            public class Tests
            {
                Browser browser = new Browser("https://shop-alesyaoao.by/");

                [Test]
                public void Test3()
                {

                    ButtonClick(cartPageButton);

                    Console.WriteLine("Browser URL: " + driver.Url);

                    Assert.That((driver.Url), Is.EqualTo("https://shop-alesyaoao.by/cart"));
                }

                [TearDown]
                public void TearDown()
                {
                    browser.CloseBrowser();
                }
            }
        }
        public class CatalogPage
        {
            private static readonly By lastViewProducts = By.ClassName("last-viewed-products");
            private static readonly By moreInfoCatalogItem = By.XPath("//a[@class='more-info-product button2']");
            public class Tests
            {
                Browser browser = new Browser("https://shop-alesyaoao.by/");

                [Test]
                public void Test9()
                {
                    bool flag = false;
                    driver.Navigate().GoToUrl("https://shop-alesyaoao.by/catalog/genskiy-trikotag");
                    ButtonClick(GetAllElementsBy(moreInfoCatalogItem));
                    driver.Navigate().GoToUrl("https://shop-alesyaoao.by/catalog/genskiy-trikotag");

                    if (driver.FindElement(lastViewProducts) != null) flag = true;
                    Assert.That(flag, Is.EqualTo(true));
                }

                [TearDown]
                public void TearDown()
                {
                    browser.CloseBrowser();
                }
            }
        }
    }
}