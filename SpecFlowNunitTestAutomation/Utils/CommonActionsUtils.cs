using System.Collections.ObjectModel;
using System.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SpecFlowNunitTestAutomation.Hooks;
using Keys = OpenQA.Selenium.Keys;

namespace SpecFlowNunitTestAutomation.Utils
{
    class CommonActionsUtils : ReporterClass
    {
        [ThreadStatic]
        private static IWebDriver driver;
        public string DEFAULT_BROWSER_NAME = "Chrome";

        public CommonActionsUtils()
        {
            //string browser = FileReaderUtils.ReadDataFromConfigFile("BrowserName");
            string browser = TestContext.Parameters["BrowserName"].ToString();
            if (browser != null)
            {
                if (driver == null || driver.ToString().ToLower().Equals("null"))
                {
                    driver = BrowserClass.GetBrowserInstanceCreated(browser);

                    //To maximize the size of the browser to fit window
                    driver.Manage().Window.Maximize();
                }
            }
            else
            {
                if (driver == null || driver.ToString().ToLower().Equals("null"))
                {
                    driver = BrowserClass.GetBrowserInstanceCreated(DEFAULT_BROWSER_NAME);

                    //To maximize the size of the browser to fit window
                    driver.Manage().Window.Maximize();
                }
            }
        }

        public void LaunchApplication(string url)
        {
            try
            {
                url = url.Trim();

                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(200);

                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                driver.Url = url;
            }
            catch (Exception ex)
            {
                Assert.Fail(TestContext.Parameters["Environment"].ToString() + " environment URL: "
                    + url + " was not opened sucessfully. Please check again. Failed logs: " + ex.Message);
            }
        }

        public void WaitForPageToLoad(int timeoutInSeconds)
        {
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeoutInSeconds);
        }

        public void ClearText(By Element, string ElementName)
        {
            WaitUntillElementToBeVisible(Element, 10);

            try
            {
                SetElementFocus(Element);
                driver.FindElement(Element).Clear();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to clear the text from the input field: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void SendValue(By Element, string ElementName, string value)
        {
            WaitUntillElementToBeVisible(Element, 10);

            try
            {
                SetElementFocus(Element);
                driver.FindElement(Element).SendKeys(value);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to enter the text " + value + " to the input field: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void ClearAndSendValue(By Element, string ElementName, string value)
        {
            ClearText(Element, ElementName);
            SendValue(Element, ElementName, value);
        }

        public void ClickElement(By Element, string ElementName)
        {
            WaitUntillElementToBeVisible(Element, 10);
            WaitUntillElementToBeClickable(Element, 10);
            try
            {
                SetElementFocus(Element);
                driver.FindElement(Element).Click();
            }
            catch (Exception)
            {
                ClickElementUsingJS(Element, ElementName);
            }
        }

        public void ClickElementUsingJS(By Element, string ElementName)
        {
            WaitUntillElementToBeVisible(Element, 10);
            WaitUntillElementToBeClickable(Element, 10);

            try
            {
                IWebElement Button = driver.FindElement(Element);
                string javascript = "arguments[0].click()";
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                jsExecutor.ExecuteScript(javascript, Button);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to click on: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public IWebElement WaitForElementToBeVisible(By Element, int timeoutInSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(Element));
        }

        public void WaitUntillElementToBeVisible(By Element, int timeoutInSeconds)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(Element));
            }
            catch (Exception) { }
        }

        public void WaitUntillElementToExist(By Element, int timeoutInSeconds)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(Element));
            }
            catch (Exception) { }
        }

        public IWebElement WaitForElementToExist(By Element, int timeoutInSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(Element));
        }

        public void WaitForURLtoLoad(string PartialURL, int timeoutInSeconds)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains(PartialURL));
            }
            catch (Exception) { }
        }

        public void WaitUntillElementToBeClickable(By Element, int timeoutInSeconds)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Element));
            }
            catch (Exception) { }
        }

        public void WaitUntillElementToBeInvisible(By Element, int timeoutInSeconds)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(Element));
            }
            catch (Exception) { }
        }

        public IWebElement WaitForElementToBeClickable(By Element, int timeoutInSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Element));
        }

        public int GetSizeOfElements(By Element)
        {
            WaitUntillElementToBeVisible(Element, 10);

            return driver.FindElements(Element).Count;
        }

        public string GetPageTitle()
        {
            return driver.Title;
        }

        public string GetPageURL()
        {
            return driver.Url;
        }

        public bool WaitForDynamicObjectToAppear(By Element)
        {
            int i = 1;
            Thread.Sleep(7000);
            do
            {
                if (driver.FindElements(Element).Count == 1)
                {
                    return true;
                }
                else
                {
                    Thread.Sleep(2000);
                    i++;
                }
            } while (i <= 5);
            return false;
        }

        public static string TakeScreenshotImage(string filePath)
        {
            string screenshotPath = filePath;
            if (!String.IsNullOrEmpty(screenshotPath))
            {
                ITakesScreenshot takeScreenShot = (ITakesScreenshot)driver;
                Screenshot screenshot = takeScreenShot.GetScreenshot();
                screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                return screenshotPath;
            }
            else
            {
                Console.WriteLine("Please provide the screenshot Path");
                return null;
            }
        }

        public static string TakeBase64ScreenshotImage(string filePath)
        {
            string screenshotPath = filePath;

            if (!String.IsNullOrEmpty(screenshotPath))
            {
                ITakesScreenshot takeScreenShot = (ITakesScreenshot)driver;
                Screenshot screenshot = takeScreenShot.GetScreenshot();
                screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                string Base64ImageName = screenshot.AsBase64EncodedString;

                return Base64ImageName;
            }
            else
            {
                Console.WriteLine("Please provide the screenshot Path");
                return null;
            }
        }

        //Convert base64 to Image thumbnail
        public static Image LoadBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        public string GetTextValue(By Element, string ElementName)
        {
            string? TextValue = null;

            WaitUntillElementToBeVisible(Element, 10);

            try
            {
                TextValue = driver.FindElement(Element).Text; ;

            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to find: " + ElementName + ". Failed logs: " + ex.Message);
            }
            return TextValue;
        }

        public string GetAttributeValue(By Element, string ElementName, string attributeName)
        {
            string? ActualAttribute = null;

            WaitUntillElementToBeVisible(Element, 10);

            try
            {
                ActualAttribute = driver.FindElement(Element).GetAttribute(attributeName);

            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to find: " + ElementName + ". Failed logs: " + ex.Message);
            }
            return ActualAttribute;
        }

        public void SelectValueByIndex(By Element, string ElementName, int index)
        {
            WaitUntillElementToBeVisible(Element, 10);

            try
            {
                SetElementFocus(Element);
                SelectElement select = new(driver.FindElement(Element));
                select.SelectByIndex(index);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to select option from the dropdown: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void SelectValueByValue(By Element, string ElementName, string value)
        {
            WaitUntillElementToBeVisible(Element, 10);

            try
            {
                SetElementFocus(Element);
                SelectElement select = new(driver.FindElement(Element));
                select.SelectByValue(value);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to select option from the dropdown: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void SelectValueByVisibleText(By Element, string ElementName, string visibleText)
        {
            WaitUntillElementToBeVisible(Element, 10);

            try
            {
                SetElementFocus(Element);
                SelectElement select = new SelectElement(driver.FindElement(Element));
                select.SelectByText(visibleText.Trim());
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to select option: " + visibleText + " from the dropdown: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public IList<IWebElement> GetAllOptionsElementsInDropDown(By Element, string ElementName)
        {
            WaitUntillElementToBeVisible(Element, 10);

            SelectElement select = new SelectElement(driver.FindElement(Element));
            return select.Options;

        }

        public List<string> GetAllOptionsNamesInDropDown(By Element, string ElementName)
        {
            WaitUntillElementToBeVisible(Element, 10);

            SelectElement select = new SelectElement(driver.FindElement(Element));
            List<string> options = new List<string>();
            foreach (IWebElement optionElement in select.Options)
            {
                options.Add(optionElement.Text);
            }
            return options;
        }

        public void SwitchToAlertsAndAccept()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to get the alert and accept it. Failed logs: " + ex.Message);
            }
        }

        public void ScrollToElement(By Element, string ElementName)
        {
            WaitUntillElementToExist(Element, 10);

            try
            {
                int locationValue = driver.FindElement(Element).Location.Y;
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scroll(0," + locationValue + ");");
                Thread.Sleep(500);
            }
            catch (Exception) { }
        }

        public void ScrollToElementUsingActionClass(By Element, string ElementName)
        {
            WaitUntillElementToExist(Element, 10);

            try
            {
                new Actions(driver).MoveToElement(driver.FindElement(Element)).Perform();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to scroll into : " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void SetElementFocus(By Element)
        {
            IWebElement Button = driver.FindElement(Element);
            string javascript = "arguments[0].focus()";
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript(javascript, Button);

            //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", driver.FindElement(Element));
        }

        public void SetBrowserFocus()
        {
            //((IJavaScriptExecutor)driver).ExecuteScript("window.focus();");

            /*//Store the current window handle
            String currentWindowHandle = driver.CurrentWindowHandle;

            //Switch back to to the window using the handle saved earlier
            driver.SwitchTo().Window(currentWindowHandle);*/

            driver.Manage().Window.Minimize();
            driver.Manage().Window.Maximize();
        }

        public void SwitchWindow()
        {
            string currentWindow = driver.CurrentWindowHandle;
            List<string> windows = new List<string>(driver.WindowHandles);
            foreach (string window in windows)
            {
                driver.SwitchTo().Window(window);
            }
        }

        public void SwitchToMainWindow()
        {
            //driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Window(driver.WindowHandles[0]);
        }

        public bool IsElementDisplayed(By Element)
        {
            return driver.FindElement(Element).Displayed;
        }

        public int FindElementCount(By Element)
        {
            return driver.FindElements(Element).Count();
        }

        /// <summary>
        /// Finds the elements with timeout wait.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> FindElements(By Element)
        {
            ReadOnlyCollection<IWebElement> e = null;
            e = driver.FindElements(Element);

            return e;
        }

        public bool WaitUntilDocumentIsReady(int timeoutInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                Func<IWebDriver, bool> readyCondition = webDriver => (bool)((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");
                wait.Until(readyCondition);

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsElementVisible(By Element, int timeoutInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(Element));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsElementInVisible(By Element, int timeoutInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(Element));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsEelementExist(By Element, int timeoutInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(Element));
                return true;
            }
            catch (TimeoutException te)
            {
                return false;
            }
        }

        public void ScrollIntoView(By Element, string ElementName)
        {
            WaitUntillElementToExist(Element, 10);

            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", Element);
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to scroll into the view : " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void ClearAndSetTextUsingJS(By Element, string ElementName, string TextToWrite)
        {
            try
            {
                //Clear the field
                ClearText(Element, ElementName);

                // set the text
                IWebElement Button = driver.FindElement(Element);
                string javascript = "arguments[0].value='" + TextToWrite + "'";
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                jsExecutor.ExecuteScript(javascript, Button);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to enter text: " + TextToWrite + " into the field: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }
        public void RemoveAttributeUsingJS(IWebElement Element, string AttributeToRemove)
        {
            try
            {
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;

                // Execute Javascript to remove the element
                jsExecutor.ExecuteScript("arguments[0].removeAttribute('" + AttributeToRemove + "')", Element);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to remove attribute: " + AttributeToRemove + ". Failed logs: " + ex.Message);
            }
        }

        //Method to get current page source of the window open
        public bool GetTextFromPageSource(string TextToVerify) => driver.PageSource.Contains(TextToVerify);

        //Method to get current page title
        public string GetTitle() => driver.Title;

        public void DoubleClick(By Element, string ElementName)
        {
            WaitUntillElementToBeVisible(Element, 10);
            WaitUntillElementToBeClickable(Element, 10);

            //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].dblclick();", Element);
            try
            {
                SetElementFocus(Element);
                new Actions(driver).DoubleClick(driver.FindElement(Element)).Build().Perform();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to double click on: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void RightClick(By Element, string ElementName)
        {
            WaitUntillElementToBeVisible(Element, 10);
            WaitUntillElementToBeClickable(Element, 10);

            try
            {
                SetElementFocus(Element);
                new Actions(driver).ContextClick(driver.FindElement(Element)).Build().Perform();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to right click on: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void ActionClick(By Element, string ElementName)
        {
            WaitUntillElementToBeVisible(Element, 10);
            WaitUntillElementToBeClickable(Element, 10);

            try
            {
                SetElementFocus(Element);
                new Actions(driver).MoveToElement(driver.FindElement(Element)).Click().Build().Perform();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to click on: " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void DragAndDrop(By SourceElement, string SourceElementName, By DestinationElement, string DestinationElementName)
        {
            WaitUntillElementToBeVisible(SourceElement, 10);
            WaitUntillElementToBeClickable(SourceElement, 10);

            try
            {
                SetElementFocus(SourceElement);
                new Actions(driver).DragAndDrop(driver.FindElement(SourceElement), driver.FindElement(DestinationElement)).Build().Perform();
                //new Actions(driver).ClickAndHold(driver.FindElement(SourceElement)).MoveToElement(driver.FindElement(DestinationElement)).Release(driver.FindElement(DestinationElement)).Build().Perform();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed in dragging: " + SourceElementName + " to " + DestinationElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void DragAndDropUsingJS(By SourceElement, string SourceElementName, By DestinationElement, string DestinationElementName)
        {
            WaitUntillElementToBeVisible(SourceElement, 10);
            WaitUntillElementToBeClickable(SourceElement, 10);

            try
            {
                IWebElement LocatorFrom = driver.FindElement(SourceElement);
                IWebElement LocatorTo = driver.FindElement(DestinationElement);
                string xto = (LocatorTo.Location.X).ToString();
                string yto = (LocatorTo.Location.Y).ToString();
                ((IJavaScriptExecutor)driver).ExecuteScript("function simulate(f,c,d,e){var b,a=null;for(b in eventMatchers)if(eventMatchers[b].test(c)){a=b;break}if(!a)return!1;document.createEvent?(b=document.createEvent(a),a==\"HTMLEvents\"?b.initEvent(c,!0,!0):b.initMouseEvent(c,!0,!0,document.defaultView,0,d,e,d,e,!1,!1,!1,!1,0,null),f.dispatchEvent(b)):(a=document.createEventObject(),a.detail=0,a.screenX=d,a.screenY=e,a.clientX=d,a.clientY=e,a.ctrlKey=!1,a.altKey=!1,a.shiftKey=!1,a.metaKey=!1,a.button=1,f.fireEvent(\"on\"+c,a));return!0} var eventMatchers={HTMLEvents:/^(?:load|unload|abort|error|select|change|submit|reset|focus|blur|resize|scroll)$/,MouseEvents:/^(?:click|dblclick|mouse(?:down|up|over|move|out))$/}; " +
                "simulate(arguments[0],\"mousedown\",0,0); simulate(arguments[0],\"mousemove\",arguments[1],arguments[2]); simulate(arguments[0],\"mouseup\",arguments[1],arguments[2]); ",
                LocatorFrom, xto, yto);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed in dragging: " + SourceElementName + " to " + DestinationElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void RefreshPage()
        {
            try
            {
                driver.Navigate().Refresh();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to refresh page. Failed logs: " + ex.Message);
            }
        }

        public bool IsElementSelected(By Element)
        {
            return driver.FindElement(Element).Selected;
        }

        public void PressEnter(By Element, string ElementName)
        {
            try
            {
                SetElementFocus(Element);
                driver.FindElement(Element).SendKeys(Keys.Enter);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to simulate keyboard Enter on : " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void PressBackspace(By Element, string ElementName)
        {
            try
            {
                SetElementFocus(Element);
                driver.FindElement(Element).SendKeys(Keys.Backspace);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to simulate keyboard Backspace on : " + ElementName + ". Failed logs: " + ex.Message);
            }
        }

        public void CloseBrowser()
        {
            driver.Quit();
            driver.Dispose();
            //Thread.Sleep(3000);

            driver = null;
        }
    }
}
