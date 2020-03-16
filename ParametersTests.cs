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

        public void WaitForElementToAppear(int seconds, string cssSelector)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            var actionContainer = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(cssSelector)));

        }
        public void WaitForElementToBeInteractable(int seconds, string cssSelector)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            var actionContainer = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector(cssSelector)));

        }

        public void DeleteLastCreatedParameter()
        {
            IWebElement trashIcon = _driver.FindElement(By.CssSelector("mat-table.mat-table > mat-row:last-of-type .btn-reject"));
            trashIcon.Click();

            WaitForElementToAppear(5, ".mat-dialog-actions");
            IWebElement deleteButton = _driver.FindElement(By.CssSelector(".mat-warn"));
            deleteButton.Click();
            WaitForElementToAppear(5, ".mat-row:last-child");
            _driver.Navigate().Refresh();
        }

        public void CreateNewParameter(string parameterName)
        {
            IWebElement newParameterButton = _driver.FindElement(By.XPath("//span[.='New parameter']"));
            newParameterButton.Click();

            IWebElement nameField = _driver.FindElement(By.CssSelector("[name='Name']"));
            nameField.SendKeys(parameterName);
            IWebElement valueField = _driver.FindElement(By.CssSelector("[name='Value']"));
            valueField.SendKeys("1");
            IWebElement saveButton = _driver.FindElement(By.CssSelector("button.mat-primary"));
            saveButton.Click();
            WaitForElementToAppear(10, ".mat-row:last-child");
            _driver.Navigate().Refresh();
        }

        [TestInitialize]

        public void TestInitialize()
        {
            _driver = new ChromeDriver();
            action = new Actions(_driver);

            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Navigate().GoToUrl("https://u4ek-dev-portal.azurewebsites.net/tenant/admin");
            _driver.Manage().Window.Maximize();
            IWebElement user = _driver.FindElement(By.Id("i0116"));
            user.SendKeys("pablo.diaz.martos@unit4.com");
            IWebElement submitButton = _driver.FindElement(By.CssSelector(".btn"));
            WaitForElementToBeInteractable(5, ".btn");
            submitButton.Click();
            /*WaitForElementToAppear(20, "#i0118");
            IWebElement password = _driver.FindElement(By.Id("i0118"));
            password.SendKeys("Wolwhaljo_15");
            IWebElement submitButton2 = _driver.FindElement(By.CssSelector(".btn"));
            WaitForElementToBeInteractable(5, ".btn");
            submitButton2.Click();*/
            WaitForElementToAppear(20, "#mat-dialog-0");
            IWebElement understoodButton = _driver.FindElement(By.XPath("//span[.='Understood']"));
            understoodButton.Click();
            IWebElement parametersButton = _driver.FindElement(By.CssSelector("a[href='/parameters'] .u4-main-nav-item-text"));
            parametersButton.Click();
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

            _driver.Quit();
        }

        [TestMethod]
        public void TestCreateNewParameter()
        {
            WaitForElementToAppear(10, ".mat-row:last-child");
            CreateNewParameter("NewParameter");

            WaitForElementToAppear(10, "mat-table.mat-table");

            IWebElement createdParameter = _driver.FindElement(By.CssSelector(".mat-row:last-child"));
            Assert.IsTrue(createdParameter.Displayed, "New parameter not saved correctly");

            DeleteLastCreatedParameter();
            _driver.Quit();
        }

        [TestMethod]
        public void TestUpdateParameter()
        {
            CreateNewParameter("ParameterToBeUpdated");
            WaitForElementToAppear(10, ".mat-row:last-child");
            IWebElement parameterToBeUpdated = _driver.FindElement(By.CssSelector(".mat-row:last-child"));
            parameterToBeUpdated.Click();

            WaitForElementToAppear(10, ".mat-dialog-actions");

            IWebElement valueField = _driver.FindElement(By.CssSelector("[name='Value']"));
            valueField.Clear();
            valueField.SendKeys("1.0");

            IWebElement saveButton = _driver.FindElement(By.CssSelector("button.mat-primary"));
            saveButton.Click();

            WaitForElementToAppear(5, "mat-table.mat-table");
            _driver.Navigate().Refresh();
            WaitForElementToAppear(5, "mat-table.mat-table");
            IWebElement updatedValue = _driver.FindElement(By.CssSelector("mat-table.mat-table > mat-row:last-child > .cdk-column-Value"));
            Assert.IsTrue(updatedValue.Text == "1.0", "Updated value not saved");

            DeleteLastCreatedParameter();
            _driver.Quit();

        }

        [TestMethod]
        public void TestParametersWithSameNameCannotBeCreated()
        {
            CreateNewParameter("DuplicatedNameParameter");

            WaitForElementToAppear(10, ".mat-row:last-child");
            IWebElement newParameterButton = _driver.FindElement(By.XPath("//button[@class='btn-action mat-button mat-button-base']"));
            newParameterButton.Click();

            WaitForElementToAppear(5, "[name='Name']");
            IWebElement nameField = _driver.FindElement(By.CssSelector("[name='Name']"));
            nameField.SendKeys("DuplicatedNameParameter");

            WaitForElementToAppear(5, ".mat-error");
            IWebElement errorMessage = _driver.FindElement(By.CssSelector(".mat-error"));
            IWebElement saveButton = _driver.FindElement(By.CssSelector("button.mat-primary"));
            string errorMessageContent = errorMessage.Text;
            bool saveButtonClickable = SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(saveButton).Equals(true);
            IWebElement cancelButton = _driver.FindElement(By.XPath("//button[@class='mat-button mat-button-base']"));
            cancelButton.Click();
            _driver.Navigate().Refresh();
            WaitForElementToAppear(5, "mat-table.mat-table");


            Assert.IsFalse(saveButtonClickable, "Save button is not disabled");
            Assert.IsTrue(errorMessageContent == "Name already in use", "Wrong error message");

            DeleteLastCreatedParameter();
            _driver.Quit();
        }

        [TestMethod]
        public void TestSearchParameterByName()
        {
            IWebElement searchIcon = _driver.FindElement(By.CssSelector(".fa-search"));
            searchIcon.Click();
            IWebElement searchBar = _driver.FindElement(By.CssSelector(".mat-input-element"));
            searchBar.SendKeys("test");
            searchBar.SendKeys(Keys.Enter);

            WaitForElementToAppear(20, "mat-table.mat-table > mat-row:first-of-type");

            var searchResults = _driver.FindElements(By.CssSelector("mat-table.mat-table > mat-row"));
            bool listIsCorrect = true;


            foreach (IWebElement ElementInResults in searchResults)
            {
                if (!(ElementInResults.Text.ToLower().Contains("test")))
                    listIsCorrect = false;
            }

            Assert.IsTrue(listIsCorrect, "Search results are not correct");

            _driver.Quit();
        }

        [TestMethod]
        public void TestDeleteParameterConfirmationPopUpAndAction()
        {
            CreateNewParameter("ParameterToBeDeleted");
            WaitForElementToAppear(10, ".mat-row:last-child");

            var previousItems = _driver.FindElements(By.CssSelector("mat-table.mat-table > mat-row"));

            IWebElement trashIcon = _driver.FindElement(By.CssSelector("mat-table.mat-table > mat-row:last-of-type .btn-reject"));
            trashIcon.Click();

            WaitForElementToAppear(5, ".mat-dialog-actions");
            IWebElement deleteButton = _driver.FindElement(By.CssSelector(".mat-warn"));
            deleteButton.Click();
            WaitForElementToAppear(5, ".mat-row:last-child");
            _driver.Navigate().Refresh();
            WaitForElementToAppear(5, ".mat-row:last-child");
            var currentItems = _driver.FindElements(By.CssSelector("mat-table.mat-table > mat-row"));

            Assert.IsTrue(previousItems.Count > currentItems.Count, "Element not deleted");

            _driver.Quit();
        }

        [TestMethod]
        public void TestDeleteParameterConfirmationCancel()
        {

            WaitForElementToAppear(10, ".mat-row:last-child");

            var previousItems = _driver.FindElements(By.CssSelector("mat-table.mat-table > mat-row"));

            IWebElement trashIcon = _driver.FindElement(By.CssSelector("mat-table.mat-table > mat-row:last-of-type .btn-reject"));
            trashIcon.Click();

            WaitForElementToAppear(5, ".mat-dialog-actions");
            IWebElement cancelButton = _driver.FindElement(By.XPath("//button[@class='mat-button mat-button-base']"));
            cancelButton.Click();
            WaitForElementToAppear(5, ".mat-row:last-child");
            _driver.Navigate().Refresh();
            WaitForElementToAppear(5, ".mat-row:last-child");
            var currentItems = _driver.FindElements(By.CssSelector("mat-table.mat-table > mat-row"));

            Assert.IsTrue(previousItems.Count == currentItems.Count, "Element deletion not canceled");

            _driver.Quit();
        }

        [TestMethod]
        public void TestSensitiveDataTrigger()
        {
            IWebElement newParameterButton = _driver.FindElement(By.CssSelector("button.btn-action"));
            newParameterButton.Click();
            IWebElement nameField = _driver.FindElement(By.CssSelector("[name='Name']"));
            nameField.SendKeys("TestSensitive");
            IWebElement valueField = _driver.FindElement(By.CssSelector("[name='Value']"));
            valueField.SendKeys("test");

            IWebElement sensitiveDataSwitch = _driver.FindElement(By.CssSelector(".mat-slide-toggle-bar"));
            sensitiveDataSwitch.Click();

            IWebElement saveButton = _driver.FindElement(By.CssSelector("button.mat-primary"));
            saveButton.Click();
            WaitForElementToAppear(5, ".mat-row:last-child");
            _driver.Navigate().Refresh();
            WaitForElementToAppear(5, ".mat-row:last-child");

            IWebElement sensitiveValue = _driver.FindElement(By.CssSelector("mat-table.mat-table > mat-row:last-of-type > .cdk-column-Value"));

            Assert.IsTrue(sensitiveValue.Text == "**********", "Sensitive data not saved hidden");

            DeleteLastCreatedParameter();
            _driver.Quit();
        }

        [TestMethod]
        public void testParameterWithInvalidCharactersCannotBeCreated()
        {
            WaitForElementToAppear(10, ".mat-row:last-child");
            IWebElement newParameterButton = _driver.FindElement(By.XPath("//button[@class='btn-action mat-button mat-button-base']"));
            newParameterButton.Click();

            WaitForElementToAppear(5, "[name='Name']");
            IWebElement nameField = _driver.FindElement(By.CssSelector("[name='Name']"));
            nameField.SendKeys("Wrong.parameter");

            WaitForElementToAppear(5, ".mat-error");
            IWebElement errorMessage = _driver.FindElement(By.CssSelector(".mat-error"));
            IWebElement saveButton = _driver.FindElement(By.CssSelector("button.mat-primary"));
            bool saveButtonClickable = SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(saveButton).Equals(true);
            IWebElement cancelButton = _driver.FindElement(By.XPath("//button[@class='mat-button mat-button-base']"));
            cancelButton.Click();

            Assert.IsFalse(saveButtonClickable, "Save button is not disabled");
            Assert.IsTrue(errorMessage.Text == "Name can only contain letters, numbers, and underscores", "Wrong error message");

            _driver.Quit();
        }

    }
}
