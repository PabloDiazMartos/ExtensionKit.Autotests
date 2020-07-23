using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionKit.Autotests
{
    public class Helpers
    {
        public void WaitForElementToAppear(int seconds, string cssSelector, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            var actionContainer = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(cssSelector)));

        }

        public void WaitForElementToBeInteractable(int seconds, string cssSelector, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            var actionContainer = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector(cssSelector)));

        }

        public void Login(string user, string password, IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://u4ek-dev-portal.azurewebsites.net/tenant/admin");
            driver.Manage().Window.Maximize();
            IWebElement userField = driver.FindElement(By.Id("i0116"));
            userField.SendKeys(user);
            IWebElement submitButton = driver.FindElement(By.CssSelector(".btn"));
            WaitForElementToBeInteractable(5, ".btn", driver);
            submitButton.Click();
            WaitForElementToAppear(20, "#i0118", driver);
            IWebElement passwordField = driver.FindElement(By.Id("i0118"));
            passwordField.SendKeys(password);
            IWebElement submitButton2 = driver.FindElement(By.CssSelector(".btn"));
            WaitForElementToBeInteractable(5, ".btn", driver);
            submitButton2.Click();
            IWebElement noStaySigned = driver.FindElement(By.CssSelector("#idBtn_Back"));
            WaitForElementToBeInteractable(5, "#idBtn_Back", driver);
            noStaySigned.Click();
            WaitForElementToAppear(20, "#mat-dialog-0", driver);
            IWebElement understoodButton = driver.FindElement(By.XPath("//span[.='Understood']"));
            understoodButton.Click();
        }
    }
}
