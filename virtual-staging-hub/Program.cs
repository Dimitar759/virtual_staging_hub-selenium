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
        Actions actions;


        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(25);
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));

            actions = new Actions(driver);
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

            wait2.Until(ElementIsVisible(By.CssSelector("div[class='wpcf7-response-output'][aria-hidden='true']")));
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

        [Test]
        public void AddToCart()
        {
            driver.Navigate().GoToUrl("https://virtualstaginghub.com/product/staging/");

            try
            {
                IWebElement addToCartButton = driver.FindElement(By.CssSelector(".single_add_to_cart_button"));
                ScrollToElement(driver, addToCartButton);
                addToCartButton.Click();

                WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement viewCartButton = wait2.Until(ElementToBeClickable(By.CssSelector("\"a[href='https://virtualstaginghub.com/cart/'][class='button wc-forward']")));

                viewCartButton.Click();

                IWebElement cartItem = wait2.Until(ElementExists(By.CssSelector(".woocommerce-cart-form__cart-item")));

                if (cartItem.Displayed)
                {
                    Console.WriteLine("Service is in the cart.");
                }
                else
                {
                    Console.WriteLine("Service is not in the cart.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

        }


        private static void ScrollToElement(IWebDriver driver, IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }


        [Test]
        public void Register()
        {
            driver.Navigate().GoToUrl("https://virtualstaginghub.com/");

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

            IWebElement myAccountButton = driver.FindElement(By.CssSelector("a[href='https://prod.virtualstagingplans.com/my-account/']"));
            myAccountButton.Click();

            string randomEmail = GenerateRandomEmail();
            IWebElement emailField = driver.FindElement(By.Name("email"));
            emailField.SendKeys(randomEmail);

            IWebElement registerButton = driver.FindElement(By.Name("register"));
            registerButton.Click();

            IWebElement helloTextElement = driver.FindElement(By.XPath("//*[contains(text(),'Hello')]"));

            bool isHelloTextVisible = helloTextElement.Displayed;

            Assert.IsTrue(isHelloTextVisible, "Expected 'Hello' text to be visible.");



        }

        [Test]
        public void Checkout()
        {
            driver.Navigate().GoToUrl("https://virtualstaginghub.com/product/staging/");

            IWebElement addToCartButton = driver.FindElement(By.CssSelector(".single_add_to_cart_button"));
            addToCartButton.Click();

            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement viewCartButton = wait2.Until(ElementToBeClickable(By.CssSelector("a[href='https://virtualstaginghub.com/cart/'][class='button wc-forward']")));

            viewCartButton.Click();

            IWebElement cartItem = wait2.Until(ElementExists(By.CssSelector(".woocommerce-cart-form__cart-item")));

            IWebElement proceedToCheckout = wait2.Until(ElementExists(By.CssSelector("a[href = 'https://virtualstaginghub.com/checkout/']")));
            proceedToCheckout.Click();

            IWebElement firstNameField = driver.FindElement(By.Id("billing_first_name"));
            firstNameField.SendKeys("John");

            IWebElement lastNameField = driver.FindElement(By.Id("billing_last_name"));
            lastNameField.SendKeys("Doe");

            IWebElement companyNameField = driver.FindElement(By.Id("billing_company"));
            companyNameField.SendKeys("ABC Inc.");

            IWebElement countryDropdown = driver.FindElement(By.Id("billing_country"));
            SelectElement selectCountry = new SelectElement(countryDropdown);
            selectCountry.SelectByText("North Macedonia");

            IWebElement streetAddressField = driver.FindElement(By.Id("billing_address_1"));
            streetAddressField.SendKeys("123 Main Street");

            IWebElement cityField = driver.FindElement(By.Id("billing_city"));
            cityField.SendKeys("Skopje");

            IWebElement stateField = driver.FindElement(By.Id("billing_state"));
            stateField.SendKeys("Skopje");

            IWebElement postcodeField = driver.FindElement(By.Id("billing_postcode"));
            postcodeField.SendKeys("1000");

            IWebElement phoneField = driver.FindElement(By.Id("billing_phone"));
            phoneField.SendKeys("1234567890");

            string randomEmail = GenerateRandomEmail();
            IWebElement emailField = driver.FindElement(By.Id("billing_email"));
            emailField.SendKeys(randomEmail);

            List<IWebElement> paymentMethods = driver.FindElements(By.Id("buttons-container")).ToList();


            IWebElement debitOrCreditCardButton = paymentMethods.FirstOrDefault(el => el.Text.Contains("Debit or Credit Card"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", debitOrCreditCardButton);
            debitOrCreditCardButton.Click();

            IWebElement cardNumberField = driver.FindElement(By.Id("credit-card-number"));
            cardNumberField.Clear();
            cardNumberField.SendKeys("2354 2452 3524 1461");

            IWebElement expiryField = driver.FindElement(By.Id("expiry-date"));
            expiryField.Clear();
            expiryField.SendKeys("03 / 34");

            IWebElement cscField = driver.FindElement(By.Id("credit-card-security"));
            cscField.Clear();
            cscField.SendKeys("233");

            IWebElement payButton = driver.FindElement(By.Id("submit-button"));
            payButton.Click();


        }

        [Test]
        public void OpenBlogPost()
        {
            driver.Navigate().GoToUrl("https://virtualstaginghub.com/");

            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

            IWebElement blogButton = driver.FindElement(By.CssSelector("a[href='https://virtualstaginghub.com/blog/']"));
            wait2.Until(ElementToBeClickable(By.CssSelector("a[href='https://virtualstaginghub.com/blog/']")));
            blogButton.Click();

            IWebElement blogPost = driver.FindElement(By.CssSelector("a[href='https://virtualstaginghub.com/blog/how-to-stage-a-master-bedroom-best-practices/']"));
            wait2.Until(ElementToBeClickable(By.ClassName("b-posts__grid-item")));
            blogPost.Click();

            wait2.Until(UrlToBe("https://virtualstaginghub.com/blog/how-to-stage-a-master-bedroom-best-practices/"));

            string expectedUrl = "https://virtualstaginghub.com/blog/how-to-stage-a-master-bedroom-best-practices/";

            string actualUrl = driver.Url;

            Assert.AreEqual(expectedUrl, actualUrl);
        }
    }
}