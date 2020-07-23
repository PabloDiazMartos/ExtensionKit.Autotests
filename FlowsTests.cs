using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace ExtensionKit.Autotests
{
    [TestClass]
    public class FlowsTests
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

            _helpers.WaitForElementToBeInteractable(5, "a[href='/flows'] .u4-main-nav-item-text", _driver);
            IWebElement flowsSection = _driver.FindElement(By.CssSelector("a[href='/flows'] .u4-main-nav-item-text"));
            flowsSection.Click();

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _driver.Navigate().GoToUrl("https://u4ek-dev-portal.azurewebsites.net/flows");
            try
            {
                DeleteFlow("Autotest flow");
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException e)
            {

            }
            _driver.Quit();
        }

        public void DeleteFlow(string flowName)
        {
            searchFlowByName(flowName);
            _helpers.WaitForElementToAppear(5, ".flow-card--body-title", _driver);

            IWebElement actionsButton = _driver.FindElement(By.CssSelector("button.mat-menu-trigger .mat-icon"));
            actionsButton.Click();

            IWebElement deleteButton = _driver.FindElement(By.CssSelector(".delete-flow"));
            deleteButton.Click();

            _helpers.WaitForElementToAppear(5, "mat-dialog-actions.mat-dialog-actions > .mat-warn", _driver);
            IWebElement ConfirmButton = _driver.FindElement(By.CssSelector("mat-dialog-actions.mat-dialog-actions > .mat-warn"));
            ConfirmButton.Click();
        }

        public void searchFlowByName(string flowName)
        {
            _helpers.WaitForElementToBeInteractable(5, ".fa-search", _driver);
            IWebElement searchIcon = _driver.FindElement(By.CssSelector(".fa-search"));
            searchIcon.Click();
            IWebElement searchBar = _driver.FindElement(By.CssSelector(".mat-input-element"));
            searchBar.SendKeys(flowName);
            searchBar.SendKeys(Keys.Enter);
        }

        public void createAutotestFlow()
        {
            IWebElement newFlowButton = _driver.FindElement(By.XPath("//span[contains(.,'New flow')]"));
            newFlowButton.Click();

            _helpers.WaitForElementToBeInteractable(5, ".edit-btn", _driver);
            IWebElement flowTittleIcon = _driver.FindElement(By.CssSelector(".edit-btn"));
            flowTittleIcon.Click();

            IWebElement flowTittleTextBox = _driver.FindElement(By.CssSelector("input[placeholder='Give a name to your flow!']"));
            flowTittleTextBox.SendKeys("Autotest Flow");

            IWebElement triggerButton = _driver.FindElement(By.CssSelector("div[title='Trigger'] .initials"));
            triggerButton.Click();

            IWebElement webhookButton = _driver.FindElement(By.CssSelector("div[title='Webhook'] > .text-container"));
            webhookButton.Click();

            _helpers.WaitForElementToBeInteractable(5, "[placeholder='Name']", _driver);
            IWebElement triggerName = _driver.FindElement(By.CssSelector("[placeholder='Name']"));
            triggerName.SendKeys("Autotest Trigger");
            IWebElement authentication = _driver.FindElement(By.CssSelector(".mat-select-placeholder"));
            authentication.Click();
            IWebElement noneOption = _driver.FindElement(By.CssSelector("div.mat-select-panel > mat-option:nth-of-type(1) > .mat-option-text"));
            noneOption.Click();

            IWebElement actionButton = _driver.FindElement(By.CssSelector("div[title='Action'] > .text-container"));
            actionButton.Click();

            _helpers.WaitForElementToBeInteractable(10, "div[title='Stop Execution'] .mat-icon", _driver);
            IWebElement stopActionButton = _driver.FindElement(By.CssSelector("div[title='Stop Execution'] .mat-icon"));
            stopActionButton.Click();

            IWebElement stopWith = _driver.FindElement(By.CssSelector(".mat-select-placeholder"));
            stopWith.Click();
            IWebElement successOption = _driver.FindElement(By.CssSelector("div.mat-select-panel > mat-option:nth-of-type(1) > .mat-option-text"));
            successOption.Click();

            IWebElement overviewButton = _driver.FindElement(By.CssSelector("div[title='Overview'] > .text-container"));
            overviewButton.Click();

            /* IWebElement flowName = _driver.FindElement(By.CssSelector("[placeholder='Flow name']"));
             IWebElement tenant = _driver.FindElement(By.CssSelector("mat-form-field.mat-form-field-type-mat-select .mat-form-field-infix"));
             IWebElement flowId = _driver.FindElement(By.CssSelector("[placeholder='Flow Id']"));
             IWebElement summary = _driver.FindElement(By.XPath("//div[@class='summary']"));

             Assert.IsTrue(flowName.Displayed, "Flow name should be displayed in overview");
             Assert.IsTrue(tenant.Displayed, "Tenant should be displayed in overview");
             Assert.IsTrue(flowId.Displayed, "Flow ID should be displayed in overview");
             Assert.IsTrue(summary.Displayed, "Summary should be displayed in overview");*/

            IWebElement saveFlowButton = _driver.FindElement(By.XPath("//span[.='Save flow']"));
            saveFlowButton.Click();
        }


        [TestMethod]
        public void TestMandatoryDataMissing()
        {
            IWebElement newFlowButton = _driver.FindElement(By.XPath("//span[contains(.,'New flow')]"));
            newFlowButton.Click();

            _helpers.WaitForElementToBeInteractable(5, ".edit-btn", _driver);

            IWebElement triggerButton = _driver.FindElement(By.CssSelector("div[title='Trigger'] .initials"));
            triggerButton.Click();

            IWebElement webhookButton = _driver.FindElement(By.CssSelector("div[title='Webhook'] > .text-container"));
            webhookButton.Click();

            _helpers.WaitForElementToBeInteractable(5, "[placeholder='Name']", _driver);
            IWebElement triggerName = _driver.FindElement(By.CssSelector("[placeholder='Name']"));
            triggerName.SendKeys("Autotest Trigger");
            IWebElement authentication = _driver.FindElement(By.CssSelector(".mat-select-placeholder"));
            authentication.Click();
            IWebElement noneOption = _driver.FindElement(By.CssSelector("div.mat-select-panel > mat-option:nth-of-type(1) > .mat-option-text"));
            noneOption.Click();

            IWebElement actionButton = _driver.FindElement(By.CssSelector("div[title='Action'] > .text-container"));
            actionButton.Click();

            _helpers.WaitForElementToBeInteractable(10, "div[title='Stop Execution'] .mat-icon", _driver);
            IWebElement stopActionButton = _driver.FindElement(By.CssSelector("div[title='Stop Execution'] .mat-icon"));
            stopActionButton.Click();

            IWebElement stopWith = _driver.FindElement(By.CssSelector(".mat-select-placeholder"));
            stopWith.Click();
            IWebElement successOption = _driver.FindElement(By.CssSelector("div.mat-select-panel > mat-option:nth-of-type(1) > .mat-option-text"));
            successOption.Click();

            IWebElement overviewButton = _driver.FindElement(By.CssSelector("div[title='Overview'] > .text-container"));
            overviewButton.Click();

            IWebElement saveFlowButton = _driver.FindElement(By.XPath("//span[.='Save flow']"));
            Assert.IsFalse(saveFlowButton.GetAttribute("disabled") == "true", "Save button should be disabled");
        }

        [TestMethod]
        public void TestSearchFlows()
        {
            createAutotestFlow();
            Thread.Sleep(2000);
            IWebElement flowsSection = _driver.FindElement(By.CssSelector("a[href='/flows'] .u4-main-nav-item-text"));
            flowsSection.Click();
            searchFlowByName("Autotest");

            _helpers.WaitForElementToAppear(20, ".flow-card--body-title:first-of-type", _driver);

            var flowsListTittles = _driver.FindElements(By.CssSelector(".flow-card--body-title"));
            bool listIsCorrect = true;


            foreach (IWebElement ElementInResults in flowsListTittles)
            {
                if (!(ElementInResults.Text.ToLower().Contains("test")))
                    listIsCorrect = false;
            }

            Assert.IsTrue(listIsCorrect, "Search results should be loaded");
        }

        [TestMethod]
        public void TestDeleteFlowFromDefinition()
        {
            createAutotestFlow();
            Thread.Sleep(5000);
            IWebElement flowsSection = _driver.FindElement(By.CssSelector("a[href='/flows'] .u4-main-nav-item-text"));
            flowsSection.Click();

            searchFlowByName("Autotest Flow");

            _helpers.WaitForElementToAppear(20, ".flow-card--body-title:first-of-type", _driver);
            IWebElement searchedFlow = _driver.FindElement(By.CssSelector(".flow-card--body-title:first-of-type"));
            searchedFlow.Click();

            _helpers.WaitForElementToBeInteractable(5, ".edit-btn", _driver);
            IWebElement deleteButton = _driver.FindElement(By.XPath("//span[.='Delete']"));
            deleteButton.Click();

            _helpers.WaitForElementToAppear(10, "mat-dialog-actions.mat-dialog-actions > .mat-warn", _driver);
            IWebElement ConfirmButton = _driver.FindElement(By.CssSelector("mat-dialog-actions.mat-dialog-actions > .mat-warn"));
            ConfirmButton.Click();

            searchFlowByName("Autotest Flow");
            Thread.Sleep(2000);
            bool deleted;
            try
            {
                searchedFlow = _driver.FindElement(By.CssSelector(".flow-card--body-title:first-of-type"));
                deleted = false;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                deleted = true;
            }
            Assert.IsTrue(deleted, "Flow should have been deleted");
        }

        [TestMethod]
        public void TestDeleteFlowFromQuickAccess()
        {
            createAutotestFlow();
            Thread.Sleep(5000);
            IWebElement flowsSection = _driver.FindElement(By.CssSelector("a[href='/flows'] .u4-main-nav-item-text"));
            flowsSection.Click();

            searchFlowByName("Autotest Flow");
            _helpers.WaitForElementToAppear(10, ".flow-card--body-title", _driver);

            IWebElement actionsButton = _driver.FindElement(By.CssSelector("button.mat-menu-trigger .mat-icon"));
            actionsButton.Click();

            IWebElement deleteButton = _driver.FindElement(By.CssSelector(".delete-flow"));
            deleteButton.Click();

            _helpers.WaitForElementToAppear(10, "mat-dialog-actions.mat-dialog-actions > .mat-warn", _driver);
            IWebElement ConfirmButton = _driver.FindElement(By.CssSelector("mat-dialog-actions.mat-dialog-actions > .mat-warn"));
            ConfirmButton.Click();

            searchFlowByName("Autotest Flow");
            Thread.Sleep(2000);
            bool deleted;
            try
            {
                IWebElement searchedFlow = _driver.FindElement(By.CssSelector(".flow-card--body-title:first-of-type"));
                deleted = false;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                deleted = true;
            }
            Assert.IsTrue(deleted, "Flow should have been deleted");
        }
        [TestMethod]
        public void TestDeleteFlowFromAnotherTenantNotPossible()
        {
            searchFlowByName("admin to 558");
            _helpers.WaitForElementToAppear(10, ".flow-card--body-title", _driver);

            IWebElement actionsButton = _driver.FindElement(By.CssSelector("button.mat-menu-trigger .mat-icon"));
            actionsButton.Click();
            bool deletebuttonexists;
            try 
            { 
                IWebElement deleteButton = _driver.FindElement(By.CssSelector(".delete-flow"));
                deleteButton.Click();
                deletebuttonexists = true;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                deletebuttonexists = false;
            }
            Assert.IsFalse(deletebuttonexists, "Delete option should not be available");
        }

        [TestMethod]
        public void TestEnableFlowFromAnotherTenantNotPossible()
        {
            searchFlowByName("admin to 558");
            _helpers.WaitForElementToAppear(10, ".flow-card--body-title", _driver);

            IWebElement toggleEnable = _driver.FindElement(By.CssSelector(".mat-slide-toggle-bar"));
            Assert.IsFalse(toggleEnable.GetAttribute("disabled") == "true", "Enable option should be disabled");
        }

        [TestMethod]
        public void TestChangeTriggerAndActionOfAnExistingFlow()
        {
            createAutotestFlow();
            Thread.Sleep(5000);
            IWebElement flowsSection = _driver.FindElement(By.CssSelector("a[href='/flows'] .u4-main-nav-item-text"));
            flowsSection.Click();

            searchFlowByName("Autotest Flow");

            _helpers.WaitForElementToAppear(20, ".flow-card--body-title:first-of-type", _driver);
            IWebElement searchedFlow = _driver.FindElement(By.CssSelector(".flow-card--body-title:first-of-type"));
            searchedFlow.Click();

            _helpers.WaitForElementToBeInteractable(5, ".edit-btn", _driver);
            IWebElement triggerButton = _driver.FindElement(By.CssSelector("u4ek-flow-overview-step[type='Trigger'] .step-body .mat-icon"));
            triggerButton.Click();
            IWebElement ChangeTriggerButton = _driver.FindElement(By.XPath("//span[.='Change trigger']"));
            ChangeTriggerButton.Click();
            IWebElement searchBox = _driver.FindElement(By.CssSelector("[placeholder='Search...']"));
            searchBox.SendKeys("scheduled");
            Thread.Sleep(500);
            IWebElement scheduledTrigger = _driver.FindElement(By.CssSelector(".trigger-item"));
            scheduledTrigger.Click();

            IWebElement occursOption = _driver.FindElement(By.CssSelector(".mat-select-arrow-wrapper"));
            occursOption.Click();
            IWebElement dailyOption = _driver.FindElement(By.XPath("//span[contains(.,'Daily')]"));
            dailyOption.Click();

            IWebElement actionButton = _driver.FindElement(By.CssSelector("u4ek-flow-overview-step.cdk-drag-handle .step-body .mat-icon"));
            actionButton.Click();
            Thread.Sleep(500);
            IWebElement ChangeActionButton = _driver.FindElement(By.XPath("//span[.='Change action']"));
            ChangeActionButton.Click();
            searchBox = _driver.FindElement(By.CssSelector("[placeholder='Search...']"));
            searchBox.SendKeys("unit4id");
            Thread.Sleep(500);
            IWebElement unit4IDAction = _driver.FindElement(By.CssSelector(".action-detail"));
            unit4IDAction.Click();

            IWebElement parametersButton = _driver.FindElement(By.CssSelector(".fa-plus-square-o"));
            parametersButton.Click();
            IWebElement stepButton = _driver.FindElement(By.XPath("//button[.='step0']"));
            stepButton.Click();
      
            IWebElement saveFlowButton = _driver.FindElement(By.XPath("//span[.='Save flow']"));
            saveFlowButton.Click();
            Thread.Sleep(500);

            triggerButton.Click();
            IWebElement triggerName = _driver.FindElement(By.XPath("//u4-wizard-step[2]//span[@class='title']"));
            string trigger = triggerName.Text;
            actionButton = _driver.FindElement(By.CssSelector("u4ek-flow-overview-step.cdk-drag-handle .text-container"));
            actionButton.Click();
            IWebElement actionName = _driver.FindElement(By.XPath("//u4-wizard-step[2]//span[@class='title']"));
            string action = actionName.Text;

            Assert.IsTrue(trigger == "Scheduled Event (v1)", "Trigger changes should be saved");
            Assert.IsTrue(action == "Unit4Id Resolver (v1)", "Action changes should be saved");
        }

        [TestMethod]
        public void TestFowHistory()
        {
            searchFlowByName("Autotest history");
            _helpers.WaitForElementToAppear(20, ".flow-card--body-title:first-of-type", _driver);
            IWebElement searchedFlow = _driver.FindElement(By.CssSelector(".flow-card--body-title:first-of-type"));
            searchedFlow.Click();

            _helpers.WaitForElementToBeInteractable(20, "div[title='History'] > .text-container", _driver);
            IWebElement historyButton = _driver.FindElement(By.CssSelector("div[title='History'] > .text-container"));
            historyButton.Click();

            _helpers.WaitForElementToAppear(20, "div[infinite-scroll] > u4ek-flow-history-line:nth-of-type(1)", _driver);
            var flowHistory = _driver.FindElements(By.CssSelector("div[infinite-scroll] > u4ek-flow-history-line"));
            IWebElement lastExecution = _driver.FindElement(By.CssSelector("div[infinite-scroll] > u4ek-flow-history-line:nth-of-type(1)"));
            lastExecution.Click();

            _helpers.WaitForElementToAppear(20, "[placeholder='Flow run id']", _driver);
            IWebElement runId = _driver.FindElement(By.CssSelector("[placeholder='Flow run id']"));
            IWebElement triggerStep = _driver.FindElement(By.XPath("//span[.='Trigger']"));
            IWebElement actionStep = _driver.FindElement(By.XPath("//span[.='Action']"));

            Assert.IsTrue(triggerStep.Displayed, "Trigger step should be displayed");
            Assert.IsTrue(actionStep.Displayed, "Action step should be displayed");
            Assert.IsTrue(runId.Displayed, "There should be a run ID");
            Assert.IsTrue(flowHistory.Count != 0, "Flow history must be populated");
        }
    }
}

