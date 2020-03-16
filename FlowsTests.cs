using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;


namespace ExtensionKit.Autotests
{
    [TestClass]
    public class FlowsTests
    {

        public static IWebDriver _driver;
        public static Actions action;
        public void WaitForElementToBeInteractable(int seconds, string cssSelector)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            var actionContainer = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector(cssSelector)));

        }
        public void WaitForElementToAppear(int seconds, string cssSelector)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            var popUp = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(cssSelector)));

        }

        public void StartNewFlowCreation()
        {
            IWebElement newFlowButton = _driver.FindElement(By.XPath("//span[contains(.,'New flow')]"));
            newFlowButton.Click();
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
        }

        [TestMethod]
        public void TestIfFlowSectionIsDisplayed()
        {
            IWebElement flowsButton = _driver.FindElement(By.CssSelector("a[href='/flows'] .u4-main-nav-item-text"));
            flowsButton.Click();

            IWebElement flowsView = _driver.FindElement(By.CssSelector("div[infinite-scroll]"));
            IWebElement newFlowButton = _driver.FindElement(By.CssSelector(".mat-button-wrapper"));
            IWebElement searchIcon = _driver.FindElement(By.CssSelector(".fa-search"));


            Assert.IsTrue(flowsView.Displayed, "Flows view is not correctly displayed");
            Assert.IsTrue(newFlowButton.Displayed, "New flow button is not correctly displayed");
            Assert.IsTrue(searchIcon.Displayed, "Search icon is not correctly displayed");

            _driver.Quit();
        }

        [TestMethod]
        public void TestSearchFlowByTittle()
        {
            IWebElement searchIcon = _driver.FindElement(By.CssSelector(".fa-search"));
            searchIcon.Click();
            IWebElement searchBar = _driver.FindElement(By.CssSelector(".mat-input-element"));
            searchBar.SendKeys("Test");
            searchBar.SendKeys(Keys.Enter);

            WaitForElementToAppear(20, ".flow-card--body-title:first-of-type");

            var flowsListTittles = _driver.FindElements(By.CssSelector(".flow-card--body-title"));
            bool listIsCorrect = true;


            foreach (IWebElement ElementInResults in flowsListTittles)
            {
                if (!(ElementInResults.Text.ToLower().Contains("test")))
                    listIsCorrect = false;
            }

            Assert.IsTrue(listIsCorrect, "Search results not loaded");

            _driver.Quit();
        }

        [TestMethod]
        public void TestCreateNewFlowScreen()
        {
            StartNewFlowCreation();
            WaitForElementToAppear(20, ".label");

            IWebElement flowName = _driver.FindElement(By.CssSelector(".label"));
            IWebElement triggerIcon = _driver.FindElement(By.CssSelector("div[title='Trigger'] .initials"));
            IWebElement firstActionIcon = _driver.FindElement(By.CssSelector("div[title='Action'] > .text-container"));
            IWebElement addActionIcon = _driver.FindElement(By.CssSelector(".mid-step-content"));
            IWebElement overviewSection = _driver.FindElement(By.CssSelector(".flow-form--body-dynamic-section"));
            IWebElement saveButton = _driver.FindElement(By.XPath("//span[.='Save flow']"));
            IWebElement cancelButton = _driver.FindElement(By.CssSelector("button[routerlink='/flows'] > .mat-button-wrapper"));
            bool saveButtonClickable = SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(saveButton).Equals(true);

            Assert.IsTrue(flowName.Displayed, "Flows' name is not displayed");
            Assert.IsTrue(triggerIcon.Displayed, "Trigger's icon is not displayed");
            Assert.IsTrue(firstActionIcon.Displayed, "First action's icon is not displayed");
            Assert.IsTrue(addActionIcon.Displayed, "Add step's icon is not displayed");
            Assert.IsTrue(overviewSection.Displayed, "Overview section is not displayed");
            Assert.IsTrue(saveButton.Displayed, "Save button is not displayed");
            Assert.IsTrue(cancelButton.Displayed, "Cancel button is not displayed");
            Assert.IsFalse(saveButtonClickable, "Save button is not disabled");

            _driver.Quit();
        }

        [TestMethod]
        public void TestHideLeftPannel()
        {
            IWebElement hideButton = _driver.FindElement(By.CssSelector("button.u4-main-sidenav--header--nav--button-collapse .mat-icon"));

            hideButton.Click();
            IWebElement leftPannel = _driver.FindElement(By.CssSelector(".mat-drawer"));
            String leftPannelClass = leftPannel.GetAttribute("class");

            Assert.IsTrue(leftPannelClass.Contains("collapsed"), "Left pannel is still visible");

            _driver.Quit();
        }

        [TestMethod]
        public void TestUnhideLeftPannel()
        {
            IWebElement hideButton = _driver.FindElement(By.CssSelector("button.u4-main-sidenav--header--nav--button-collapse .mat-icon"));

            hideButton.Click();
            hideButton.Click();

            IWebElement leftPannel = _driver.FindElement(By.CssSelector(".mat-drawer-inner-container"));
            String leftPannelClass = leftPannel.GetAttribute("class");

            Assert.IsFalse(leftPannelClass.Contains("collapsed"), "Left pannel is still hidden");

            _driver.Quit();
        }

        [TestMethod]
        public void TestDiscardFlowCreationAndOrEdition()
        {
            StartNewFlowCreation();
            WaitForElementToAppear(10, "button[routerlink='/flows']");

            IWebElement cancelButton = _driver.FindElement(By.CssSelector("button[routerlink='/flows']"));
            cancelButton.Click();

            WaitForElementToAppear(10, ".mat-dialog-actions");
            IWebElement discardButton = _driver.FindElement(By.CssSelector("button.mat-warn > .mat-button-wrapper"));
            discardButton.Click();

            WaitForElementToAppear(20, "div[infinite-scroll]");
            Assert.IsTrue(_driver.Url == "https://u4ek-dev-portal.azurewebsites.net/flows", "Discard flow didn't redirect to flows screen");

            _driver.Quit();
        }


        [TestMethod]
        public void NewFlowCreation()
        {
            StartNewFlowCreation();

            WaitForElementToBeInteractable(10, "div[title='Trigger'] > .text-container");
            IWebElement triggerButton = _driver.FindElement(By.CssSelector("div[title='Trigger'] > .text-container"));
            triggerButton.Click();

            WaitForElementToBeInteractable(10, "div[title='Webhook'] > .text-container");
            IWebElement webhookButton = _driver.FindElement(By.CssSelector("div[title='Webhook'] > .text-container"));
            webhookButton.Click();

            WaitForElementToBeInteractable(10, "[placeholder='Name']");
            IWebElement webhookName = _driver.FindElement(By.CssSelector("[placeholder='Name']"));
            webhookName.SendKeys("test");
            IWebElement authentication = _driver.FindElement(By.CssSelector(".mat-select-placeholder"));
            authentication.Click();
            IWebElement noneOption = _driver.FindElement(By.CssSelector("div.mat-select-panel > mat-option:nth-of-type(1) > .mat-option-text"));
            noneOption.Click();

            IWebElement actionButton = _driver.FindElement(By.CssSelector(".initials"));



        }
    }
}
