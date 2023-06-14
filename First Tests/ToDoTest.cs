namespace First_Tests;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using System;






public class ToDoTest : IDisposable
{
    private  IWebDriver _driver;
    private readonly WebDriverWait _webDriverWait;
    public string url = "https://todomvc.com/";
    public int WAIT_FOR = 30;
    private Actions _action;

    //constructor - intilialize the class
    public ToDoTest()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(WAIT_FOR));
        _action = new Actions(_driver);
    }

    //Closes the window
    public void Dispose()
    {
        _driver.Quit();
    }

    //Test if you can add and remov things from the todo list
    [Fact]
    public void TestIfDestinationIsCorrect()
    {

        _driver.Navigate().GoToUrl(url);
        NavigateToLink("React");
        AddNewItem("Test 1");
        AddNewItem("Test 2");
        AddNewItem("Test 3");
        CrossOut("Test 2");
        Assert.True(AssertItemsLeft(2));

    }

    // Returns True if the counter is equal to the number of items added
    private bool AssertItemsLeft(int number)
    {
        IWebElement counter = WaitForElement(By.XPath("//footer/span"));
        string counterText = counter.Text;
        System.Diagnostics.Debug.WriteLine(counterText);


        if (counterText == $"{number} items left")
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    // Cross out one of the items
    public void CrossOut(string item)
    {
        IWebElement itemInList = WaitForElement(By.XPath("/html/body/section/div/section/ul/li/div/input"));
        itemInList.Click();
    }

    // Navigates to given link on the page
    public void NavigateToLink (string name)
    {
        IWebElement link = WaitForElement(By.LinkText(name));
        link.Click();
    }

    // Adds new item
    public void AddNewItem (string item)
    {
        IWebElement textbox = WaitForElement(By.XPath("/html/body/section/div/header/input"));
        textbox.SendKeys(item);
        _action.Click(textbox).SendKeys(Keys.Enter).Perform();
    }

    // Add wait attribute to the driver (it will try to locate the element until the countdown is over)
    private IWebElement WaitForElement(By locator)
    {
        return _webDriverWait.Until(ExpectedConditions.ElementExists(locator));
    }
}
