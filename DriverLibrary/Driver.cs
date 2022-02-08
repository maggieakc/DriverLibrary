using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace DriverLibrary
{
    public class Driver
    {
        public static IWebDriver driver;
        public Driver(string driverType)
        {
            switch(driverType.ToUpper())
            {
                case "CHROME":
                    InitializeChromeDriver();
                    break;
                case "FIREFOX":
                    InitializeFirefoxDriver();
                    break;
                case "IE":
                    InitializeIEDriver();
                    break;
                case "INTERNET EXPLORER":
                    InitializeIEDriver();
                    break;
                default:
                    InitializeChromeDriver();
                    break;
            }
        }

        /// <summary>
        /// Initializes an instance of Chrome Driver.
        /// Adding the flags to ignore ssl errors and certificate errors
        /// Starting the instance of Chrome in a maximised window
        /// </summary>
        public void InitializeChromeDriver()
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            ChromeOptions options = new ChromeOptions();

            options.AddArgument("--ignore-ssl-errors=yes");
            options.AddArgument("--ignore-certificate-errors");

            options.AddArgument("--start-maximized");
            options.AddArgument("--headless");
            driver = new ChromeDriver(options);
        }

        public void InitializeFirefoxDriver()
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            FirefoxOptions options = new FirefoxOptions();

            options.AddArgument("--ignore-ssl-errors=yes");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--headless");
            driver = new FirefoxDriver(options);
        }
        
        public void InitializeIEDriver()
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            driver = new InternetExplorerDriver();
        }

        /// <summary>
        /// Go to a specific URL
        /// </summary>
        /// <param name="url">URL to open</param>
        public void GoToUrl(string url)
        {
            Log.Write("Go to URL: " + url);
            driver.Navigate().GoToUrl(url);
            Sleep(2);
        }

        /// <summary>
        /// Returns the current URL
        /// </summary>
        /// <returns>Current URL</returns>
        public string GetUrl()
        {
            return driver.Url;
        }


        /// <summary>
        /// Wait for Element with specified ID to load
        /// </summary>
        /// <param name="id">ID of the expected element</param>
        public void WaitForElementToLoadById(string id)
        {
            WaitUntilElementVisible("WaitForElementToLoadById", id);
        }

        /// <summary>
        /// Waits until element is Visible
        /// </summary>
        /// <param name="selectorType">Selector Type such as ID, ClassName or XPath</param>
        /// <param name="selector">Selector value e.g the id of the element</param>
        /// <param name="timeout">The lenght of time the function should wait</param>
        /// <returns></returns>
        public IWebElement WaitUntilElementVisible(string selectorType, string selector, int timeout = 10)
        {

            string[] selectorList = new string[] { "WaitForElementToLoadById", "WaitForElementToLoadByXPath", "WaitForElementToLoadByClass" };

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));
            //Check if selectorType is valid and conatined within the array
            if (Array.IndexOf(selectorList, selectorType) > -1)
            {
                try
                {
                    switch (selectorType)
                    {
                        case "WaitForElementToLoadById":
                            return wait.Until(ExpectedConditions.ElementIsVisible(By.Id(selector)));
                        case "WaitForElementToLoadByXPath":
                            return wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(selector)));
                        case "WaitForElementToLoadByClass":
                            return wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(selector)));
                        default: break;
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Element with selector type: '" + selectorType + " and  selector value " + selector + "' was not found.");
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns an ID of an element based of class name and index
        /// </summary>
        /// <param name="className">class name of the element which may not be unqiue</param>
        /// <param name="arrayIndex">array index of the element with the specified class name</param>
        /// <returns></returns>
        public string GetIDFromClass(string className, int arrayIndex = 0)
        {
            IWebElement el = driver.FindElements(By.ClassName(className))[arrayIndex];
            return el.GetAttribute("id");
        }

        /// <summary>
        /// Returns the number of elements with the specified className
        /// </summary>
        /// <param name="className">class name of the element</param>
        /// <returns>Number of the elements with the same class name found</returns>
        public int GetElementCountByClass(string className)
        {
            return driver.FindElements(By.ClassName(className)).Count;
        }

        /// <summary>
        /// Sleep for the number of seconds provided
        /// </summary>
        /// <param name="sleepTime">Number of seconds to sleep</param>
        public void Sleep(int sleepTime)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            Thread.Sleep(sleepTime*1000);
            Log.Write("Completed function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
        }

        /// <summary>
        /// Close driver and quit the driver
        /// </summary>
        public void TearDown()
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            driver.Close();
            driver.Quit();
        }

        /// <summary>
        /// Take a screenshot, if exceptionScreen is true the screenshot is saved to the exception folder
        /// </summary>
        /// <param name="name">Screenshot name</param>
        /// <param name="exceptionScreen">name of the exception screenshot</param>
        public void ScreenCapture(string name, bool exceptionScreen = false)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            // save the image
            string path = @"C:\Screenshots";

            if (exceptionScreen)
            {
                path = @"C:\ErrorScreenshots";
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(Path.Combine(path, name + ".png")))
            {
                Log.Write("Screenshot captured: " + name + ".png");
                screenshot.SaveAsFile(Path.Combine(path, name + ".png"), ScreenshotImageFormat.Png);
                Sleep(2);
            }
        }

        /// <summary>
        /// Click on an IWebElement with the provided ID, takes a screenshot if a name is provided, sleeps for longer than a second if a time is provided and clicks a specific index if one is provided
        /// </summary>
        /// <param name="id">ID of the element to be clicked</param>
        /// <param name="screenshotName">Name of the screenshot to be captured, defaults to null if none provided. No screenshot is captured if value is null</param>
        /// <param name="sleepTime">Number of seconds to sleep between actions</param>
        /// <param name="arrayIndex">Index of the element to be clicked, defaults to zero to click first instance of the element</param>
        public void ClickElementByID(string id, string screenshotName = null, int sleepTime = 5, int arrayIndex = 0)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            ClickElement("ClickElementByID", id, screenshotName, sleepTime, arrayIndex);
        }

        /// <summary>
        /// Click on an IWebElement with the provided classname, takes a screenshot if a name is provided, sleeps for longer than a second if a time is provided and clicks a specific index if one is provided
        /// </summary>
        /// <param name="className">Class name of the element to be clicked</param>
        /// <param name="screenshotName">Name of the screenshot to be captured, defaults to null if none provided. No screenshot is captured if value is null</param>
        /// <param name="sleepTime">Number of seconds to sleep between actions</param>
        /// <param name="arrayIndex">Index of the element to be clicked, defaults to zero to click first instance of the element</param>
        public void ClickElementByClass(string className, string screenshotName = null, int sleepTime = 5, int arrayIndex = 0)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            ClickElement("ClickElementByClass", className, screenshotName, sleepTime, arrayIndex);
        }

        /// <summary>
        /// Click on an element using the provided selector type and value. Take a screenshot if a name is provided
        /// </summary>
        /// <param name="selectorType">Selector type to be used, e.g id, xpath, classname</param>
        /// <param name="element"> Value of the chosen selector</param>
        /// <param name="screenshotName">Name of the screenshot to be captured, defaults to null. No screenshot is captured if value is null</param>
        /// <param name="sleepTime">Time to sleep between actions, defaults to 1 second</param>
        /// <param name="arrayIndex">Index of the element to be clicked, defaults to 0 to click the first instance of the element</param>
        private void ClickElement(string selectorType, string element, string screenshotName = null, int sleepTime = 5, int arrayIndex = 0)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            Sleep(sleepTime);

            IWebElement selectedElement = null;
            //All possible types of selector
            string[] selectorList = new string[] { "ClickElementByXpath", "ClickElementByID", "ClickElementByClass" };
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            //Check if selectorType is valid and conatined within the array
            if (Array.IndexOf(selectorList, selectorType) > -1)
            {
                try
                {
                    switch (selectorType)
                    {
                        case "ClickElementByXpath":
                            selectedElement = driver.FindElements(By.XPath(element))[arrayIndex];
                            break;
                        case "ClickElementByID":
                            selectedElement = driver.FindElements(By.Id(element))[arrayIndex];
                            break;
                        case "ClickElementByClass":
                            selectedElement = driver.FindElements(By.ClassName(element))[arrayIndex];
                            break;
                        default: break;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Log.Write("Array index: " + arrayIndex + " is invalid for element: " + element);
                    throw;
                }
                catch (Exception)
                {
                    Log.Write("Unable to select element: " + element + " using: " + selectorType);
                    throw;
                }
            }
            else
            {
                Log.Write("Selection method: " + selectorType + " is not supported in this class");
                throw new MissingMethodException();
            }

            //Do sleep
            Sleep(sleepTime);

            //Click the selected element
            try
            {

                selectedElement.Click();
                Log.Write("Clicked: " + element);
            }
            catch (Exception)
            {
                Log.Write("Unable to click element: " + element);
            }

            //Do sleep
            Sleep(sleepTime);

            //Take screenshot
            if (screenshotName != null)
            {
                try
                {
                    ScreenCapture(screenshotName);
                }
                catch (Exception)
                {
                    Log.Write("Unable to capture screenshot:  " + screenshotName);
                }
            }
        }

        /// <summary>
        /// Return the text attribute of the element with the provided id
        /// </summary>
        /// <param name="id">ID of the element with attribute needed</param>
        /// <returns>Returns text of the element with the provided id</returns>
        public string GetTextFromElementByID(string id)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            Sleep(2);
            IWebElement el = driver.FindElement(By.Id(id));
            return el.Text;
        }


        /// <summary>
        /// Check if an element with the provided id is present
        /// </summary>
        /// <param name="id">ID of the element</param>
        /// <returns>Returns true if element is present, returns false if element is not present</returns>
        public bool IsElementPresentByID(string id)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            try
            {
                return IsElementPresent("ID", id);
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Check if an element with the provided classname is present
        /// </summary>
        /// <param name="classname">Class name of the element</param>
        /// <returns>Returns true if element is present, returns false if element is not present</returns>
        public bool IsElementPresentByClass(string className)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            try
            {
                return IsElementPresent("Class", className);
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Check if an element with the provided selector is present
        /// </summary>
        /// <param name="selectorType">Selector type to be used</param>
        /// <param name="element">Value of the element attribute</param>
        /// <returns>Returns true if element is present, returns false if element is not present</returns>
        private bool IsElementPresent(string selectorType, string element)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            IWebElement selectedElement = null;
            //All possible types of selector
            Sleep(2);
            string[] selectorList = new string[] { "Xpath", "ID", "Class" };

            //Check if selectorType is valid and conatined within the array
            if (Array.IndexOf(selectorList, selectorType) > -1)
            {
                try
                {
                    switch (selectorType)
                    {
                        case "Xpath":
                            selectedElement = driver.FindElement(By.XPath(element));
                            break;
                        case "ID":
                            selectedElement = driver.FindElement(By.Id(element));
                            break;
                        case "Class":
                            selectedElement = driver.FindElement(By.ClassName(element));
                            break;
                        default: break;
                    }
                }

                catch (Exception ex)
                {
                    Log.Write(ex.Message);
                    throw;
                }
            }
            else
            {
                Log.Write("Selection method: " + selectorType + " is not supported in this class");
                throw new MissingMethodException();
            }
            try
            {
                Log.Write("Completed function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
                return selectedElement.Displayed;
            }
            catch (Exception)
            {
                Log.Write("Completed function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
                return false;
            }

        }

        public string GetCellContentByRowAndColumn(int rowIndex)
        {

            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            IWebElement tableBody = driver.FindElements(By.TagName("tbody"))[0];
            IWebElement row = tableBody.FindElements(By.TagName("tr"))[rowIndex];
            IWebElement cell = row.FindElements(By.TagName("td"))[5];
            return cell.Text;
        }

        public int GetRowCount()
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            IWebElement tableBody = driver.FindElements(By.TagName("tbody"))[0];
            return tableBody.FindElements(By.TagName("tr")).Count;
        }

        public void PressDownArrow()
        {
            Actions action = new Actions(driver);
            action.SendKeys(Keys.ArrowDown);
            action.Build().Perform();
        }

        /// <summary>
        /// Send keys to an IWebElement with the provided ID, takes a screenshot if a name is provided, sleeps for longer than a second if a time is provided and sends keys to a specific index if one is provided
        /// </summary>
        /// <param name="id">ID of the element to be sent the keys</param>
        /// <param name="keys"></param>
        /// <param name="screenshotName">Name of the screenshot to be captured, defaults to null if none provided. No screenshot is captured if value is null</param>
        /// <param name="sleepTime">Number of seconds to sleep between actions</param>
        /// <param name="arrayIndex">Index of the element to be sent the keys, defaults to zero to send keys to the first instance of the element</param>
        public void SendKeysToElementByID(string id, string keys, string screenshotName = null, int sleepTime = 5, int arrayIndex = 0)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            SendKeysToElement("SendKeysToElementByID", id, keys, screenshotName, sleepTime, arrayIndex);
            Log.Write("Completed function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
        }

        /// <summary>
        /// Send keys to an element using the provided selector type and value. Take a screenshot if a name is provided
        /// </summary>
        /// <param name="selectorType">Selector type to be used, e.g id, xpath, classname</param>
        /// <param name="element"> Value of the chosen selector</param>
        /// <param name="screenshotName">Name of the screenshot to be captured, defaults to null. No screenshot is captured if value is null</param>
        /// <param name="sleepTime">Time to sleep between actions, defaults to 1 second</param>
        /// <param name="arrayIndex">Index of the element to be sent the keys, defaults to 0 to send keys to the first instance of the element</param>
        private void SendKeysToElement(string selectorType, string element, string keys, string screenshotName = null, int sleepTime = 5, int arrayIndex = 0)
        {
            Log.Write("Starting function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
            Sleep(sleepTime);

            IWebElement selectedElement = null;
            //All possible types of selector
            string[] selectorList = new string[] { "SendKeysToElementByXpath", "SendKeysToElementByID", "SendKeysToElementByClass" };

            //Check if selectorType is valid and conatined within the array
            if (Array.IndexOf(selectorList, selectorType) > -1)
            {
                try
                {
                    switch (selectorType)
                    {
                        case "SendKeysToElementByXpath":
                            selectedElement = driver.FindElements(By.XPath(element))[arrayIndex];
                            break;
                        case "SendKeysToElementByID":
                            selectedElement = driver.FindElements(By.Id(element))[arrayIndex];
                            break;
                        case "SendKeysToElementByClass":
                            selectedElement = driver.FindElements(By.ClassName(element))[arrayIndex];
                            break;
                        default: break;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Log.Write("Array index: " + arrayIndex + " is invalid for element: " + element);
                    throw;
                }
                catch (Exception)
                {
                    Log.Write("Unable to send keys to element: " + element + " using: " + selectorType);
                    throw;
                }
            }
            else
            {
                Log.Write("Selection method: " + selectorType + " is not supported in this class");
                throw new MissingMethodException();
            }

            //Do sleep
            Sleep(sleepTime);

            //Click the selected element
            try
            {
                selectedElement.SendKeys(keys);
                Log.Write("Clicked: " + element);
            }
            catch (Exception)
            {
                Log.Write("Unable to send keys to element: " + element);
            }

            //Do sleep
            Sleep(sleepTime);

            //Take screenshot
            if (screenshotName != null)
            {
                try
                {
                    ScreenCapture(screenshotName);
                }
                catch (Exception)
                {
                    Log.Write("Unable to capture screenshot:  " + screenshotName);
                }
            }
            Log.Write("Completed function: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "-" + MethodBase.GetCurrentMethod());
        }

    }
}
