using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;
using System.Linq;
using System.Xml.Linq;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Virtual_staging_hub
{
    [TestFixture]
    internal class VirtualStagingHubTesting
    {
        IWebDriver driver;
        WebDriverWait wait;


        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(25);
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));


        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();

        }

        [Test]///
        public void FreeEbookRegister()
        {
            driver.Navigate().GoToUrl("https://virtualstaginghub.com/");

            WebDriverWait wait2;
            wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            IWebElement eBookElement = driver.FindElement(By.XPath("//h2[contains(text(), '6 Smart Ways to Convince the Buyer!')]"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView();", eBookElement);

            string randomName = GenerateRandomName();
            IWebElement nameField = driver.FindElement(By.Id("name"));
            nameField.SendKeys(randomName);

            string randomEmail = GenerateRandomEmail();
            IWebElement emailField = driver.FindElement(By.Id("email"));
            emailField.SendKeys(randomEmail);

            IWebElement getItFreeButton = wait2.Until(ElementToBeClickable(By.Id("submit")));
            getItFreeButton.Click();

            wait2.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[class='wpcf7-response-output'][aria-hidden='true']")));
            IWebElement responseOutput = driver.FindElement(By.CssSelector("div[class='wpcf7-response-output'][aria-hidden='true']"));


            Assert.IsTrue(responseOutput.Displayed);
        }

        private string GenerateRandomEmail()
        {
            string baseEmail = "matthewsantoro";
            Random random = new Random();
            int timestamp = random.Next(0, 9999);
            string randomEmail = $"{baseEmail}{timestamp}@gmail.com";

            return randomEmail;
        }
        private string GenerateRandomName()
        {
            List<string> firstNames = new List<string>
            {///
                 "John",
                 "Jane",
                 "Michael",
                 "Emily",
                 "Ben",
                 "Rachel",
                 "Chandler",
                 "Eva",
                 "Dimitar",

            };

            Random random = new Random();
            int index = random.Next(0, firstNames.Count);
            return firstNames[index];
        }


        [Test]
        public void SubmitFiveStarReview()
        {
            // Navigate to the website
            driver.Navigate().GoToUrl("https://virtualstaginghub.com/product/staging/?unapproved=103&moderation-hash=19f2b4d9243e41a7afb9b6b57433b5cd#comment-103");

            IWebElement reviewsButton = driver.FindElement(By.CssSelector("button[aria-controls='reviews-accordion']"));
            reviewsButton.Click();

            IWebElement starsButton = driver.FindElement(By.XPath("//p[@class='stars']//a[@class='star-4']"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView();", starsButton);

            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement starsClickable = wait2.Until(ElementToBeClickable(starsButton));

            js.ExecuteScript("arguments[0].click();", starsClickable);

            IWebElement reviewText = driver.FindElement(By.Id("comment"));
            string randomMessage = GenerateRandomMessage();
            reviewText.SendKeys(randomMessage);

            IWebElement nameField = driver.FindElement(By.Id("author"));
            nameField.SendKeys("Stephan Graham");

            IWebElement emailField = driver.FindElement(By.Id("email"));
            emailField.SendKeys("stephangraham@gmail.com");

            IWebElement submitButton = driver.FindElement(By.Id("submit"));

            js.ExecuteScript("arguments[0].scrollIntoView();", submitButton);

            IWebElement submitClickable = wait2.Until(ElementToBeClickable(submitButton));

            js.ExecuteScript("arguments[0].click();", submitClickable);

            IWebElement approvalMessage = driver.FindElement(By.ClassName("woocommerce-review__awaiting-approval"));
            wait2.Until(ElementIsVisible(By.ClassName("woocommerce-review__awaiting-approval")));
            Assert.IsTrue(approvalMessage.Displayed, "Review submission was not successful.");

            string expectedMessage = "Your review is awaiting approval";
            string actualMessage = approvalMessage.Text.Trim();
            Assert.AreEqual(expectedMessage, actualMessage, "Approval message text is incorrect.");

        }

        private string GenerateRandomMessage()
        {
            //this is made this way because the website doesnt allow the same messages to be entered more than once
            //so i made this random message generator that should never enter the same message twice
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            int length = random.Next(20, 100); 
            string randomMessage = new string(Enumerable.Repeat(characters, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomMessage;
        }
    }
}