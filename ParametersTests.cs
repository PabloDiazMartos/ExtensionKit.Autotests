using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace ExtensionKit.Autotests
{
    [TestClass]
    public class ParametersTests
    {
        public static IWebDriver _driver;
        public static Actions action;
        public static Helpers _helpers = new Helpers();

        [TestInitialize]
        public void TestInitialize()
        {
            _driver = new ChromeDriver();
            action = new Actions(_driver);

            _helpers.Login("admin@u4ppsandboxdirectory.onmicrosoft.com", "Sandbox1", _driver);

            _helpers.WaitForElementToBeInteractable(5, "a[href='/parameters'] .u4-main-nav-item-text", _driver);
            IWebElement parametersSection = _driver.FindElement(By.CssSelector("a[href='/parameters'] .u4-main-nav-item-text"));
            parametersSection.Click();

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _driver.Quit();
        }

        public bool checkIfParameterExists(string parameterName)
        {
            searchParameter(parameterName);
            var searchList = _driver.FindElements(By.CssSelector("mat-table.mat-table > mat-row"));
            if (searchList.Count == 0)
                return false;
            else 
                return true;
        }

        public void searchParameter(string parameterName)
        {
            _helpers.WaitForElementToBeInteractable(5, ".fa-search", _driver);
            IWebElement searchIcon = _driver.FindElement(By.CssSelector(".fa-search"));
            searchIcon.Click();
            _helpers.WaitForElementToBeInteractable(5, ".mat-input-element", _driver);
            IWebElement searchBar = _driver.FindElement(By.CssSelector(".mat-input-element"));
            searchBar.SendKeys(parameterName);
            searchBar.SendKeys(Keys.Enter);

            _helpers.WaitForElementToAppear(20, "mat-table.mat-table > mat-row:first-of-type", _driver);
        }

        public void DeleteParameterByName(string parameterName)
        {
            searchParameter(parameterName);
            IWebElement trashIcon = _driver.FindElement(By.CssSelector("mat-table.mat-table > mat-row:last-of-type .btn-reject"));
            trashIcon.Click();

            _helpers.WaitForElementToAppear(5, ".mat-dialog-actions", _driver);
            IWebElement deleteButton = _driver.FindElement(By.CssSelector(".mat-warn"));
            deleteButton.Click();
            _helpers.WaitForElementToAppear(5, ".mat-row:last-child", _driver);
            _driver.Navigate().Refresh();
        }

        public void CreateNewParameter(string parameterName)
        {
            _helpers.WaitForElementToBeInteractable(10, "button.btn-action > .mat-button-wrapper", _driver);
            IWebElement newParameterButton = _driver.FindElement(By.CssSelector("button.btn-action > .mat-button-wrapper"));
            newParameterButton.Click();

            IWebElement nameField = _driver.FindElement(By.CssSelector("[name='Name']"));
            nameField.SendKeys(parameterName);
            IWebElement valueField = _driver.FindElement(By.CssSelector("[name='Value']"));
            valueField.SendKeys("1");
            IWebElement saveButton = _driver.FindElement(By.CssSelector("button.mat-primary"));
            saveButton.Click();
            _helpers.WaitForElementToAppear(5, ".mat-dialog-actions", _driver);
            //_driver.Navigate().Refresh();
        }


        [TestMethod]
        public void TestIfBasicItemsExist()
        {
            IWebElement parametersList = _driver.FindElement(By.CssSelector("mat-header-cell.cdk-column-Name"));
            IWebElement searchIcon = _driver.FindElement(By.CssSelector(".fa-search"));
            IWebElement newParameterButton = _driver.FindElement(By.CssSelector("button.btn-action"));

            Assert.IsTrue(parametersList.Displayed, "Parameters' list not displayed");
            Assert.IsTrue(searchIcon.Displayed, "Search icon not displayed");
            Assert.IsTrue(newParameterButton.Displayed, "Create parameter button not displayed"); 
        }



        [TestMethod]
        public void TestSearchParameterByName()
        {
            IWebElement searchIcon = _driver.FindElement(By.CssSelector(".fa-search"));
            searchIcon.Click();
            IWebElement searchBar = _driver.FindElement(By.CssSelector(".mat-input-element"));
            searchBar.SendKeys("test");
            searchBar.SendKeys(Keys.Enter);

            _helpers.WaitForElementToAppear(20, "mat-table.mat-table > mat-row:first-of-type", _driver);

            var searchResults = _driver.FindElements(By.CssSelector("mat-table.mat-table > mat-row"));
            bool listIsCorrect = true;


            foreach (IWebElement ElementInResults in searchResults)
            {
                if (!(ElementInResults.Text.ToLower().Contains("test")))
                    listIsCorrect = false;
            }

            Assert.IsTrue(listIsCorrect, "Search results are not correct");
        }

    }
}
