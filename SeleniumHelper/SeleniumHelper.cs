using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SeleniumSupport
{
    public class SeleniumHelper
    {
        public static Point GetPointFromIndexPosition(int indexPos, int column, int row)
        {
            Point result = default(Point);
            while (indexPos >= column * row)
            {
                indexPos -= column * row;
            }
            switch (row)
            {
                case 1:
                    result.Y = 0;
                    break;
                case 2:
                    if (indexPos < column)
                    {
                        result.Y = 0;
                    }
                    else if (indexPos < column * 2)
                    {
                        int num = indexPos / column;
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 2;
                        indexPos -= column;
                    }
                    break;
                case 3:
                    if (indexPos < column)
                    {
                        result.Y = 0;
                    }
                    else if (indexPos < column * 2)
                    {
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 3;
                        indexPos -= column;
                    }
                    else if (indexPos < column * 3)
                    {
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 3 * 2;
                        indexPos -= column * 2;
                    }
                    break;
                case 4:
                    if (indexPos < column)
                    {
                        result.Y = 0;
                    }
                    else if (indexPos < column * 2)
                    {
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 4;
                        indexPos -= column;
                    }
                    else if (indexPos < column * 3)
                    {
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 4 * 2;
                        indexPos -= column * 2;
                    }
                    else if (indexPos < column * 4)
                    {
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 4 * 3;
                        indexPos -= column * 3;
                    }
                    break;
                case 5:
                    if (indexPos < column)
                    {
                        result.Y = 0;
                    }
                    else if (indexPos < column * 2)
                    {
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 5;
                        indexPos -= column;
                    }
                    else if (indexPos < column * 3)
                    {
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 5 * 2;
                        indexPos -= column * 2;
                    }
                    else if (indexPos < column * 4)
                    {
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 5 * 3;
                        indexPos -= column * 3;
                    }
                    else
                    {
                        result.Y = Screen.PrimaryScreen.WorkingArea.Height / 5 * 4;
                        indexPos -= column * 4;
                    }
                    break;
            }
            int num2 = Screen.PrimaryScreen.WorkingArea.Width / column;
            result.X = indexPos * num2 - 10;
            return result;
        }
        public static Point GetSizeChrome(int column, int row)
        {
            int x = Screen.PrimaryScreen.WorkingArea.Width / column + 15;
            int y = Screen.PrimaryScreen.WorkingArea.Height / row + 10;
            return new Point(x, y);
        }
        public static ChromeDriver OpenChrome(int indexPos, bool DisableImage, bool Extension, bool App, string nameExtension, bool debugPort, int Port, int TypeProxy, string Proxyaddress, Point Size, Point Position)
        {
            ChromeOptions Option = new ChromeOptions();
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);
            chromeDriverService.HideCommandPromptWindow = true;
            chromeDriverService.DisableBuildCheck = true;
            Option.AddExcludedArgument("enable-automation");
            Option.AddArguments(new string[]
            {
                "--allow-running-insecure-content",
                "--start-maximized",
                "--disable-3d-apis",
                "--disable-blink-features=\"BlockCredentialedSubresources\"",
                "--disable-blink-features=AutomationControlled",
                "--disable-features=IsolateOrigins,site-per-process",
                "--disable-site-isolation-trials",
                "--disable-field-trial-config",
                "--disable-features=NetworkService,NetworkServiceInProcess",
                "--disable-features=TranslateUI,BlinkGenPropertyTrees",
                "--disable-features=NotificationTriggers",
                "--disable-features=AudioServiceOutOfProcess",
                "--disable-session-crashed-bubble",
                "--disable-impl-side-painting",
                "--safebrowsing-disable-auto-update",
                "--metrics-recording-only",
                "--disable-xss-auditor",
                "--disable-client-side-phishing-detection",
                "--disable-accelerated-jpeg-decoding",
                "--disable-accelerated-2d-canvas",
                "--disable-background-timer-throttling",
                "--disable-breakpad",
                "--disable-component-extensions-with-background-pages",
                "--disable-ipc-flooding-protection",
                "--disable-logging",
                "--dns-prefetch-disable",
                "--disable-features=VizDisplayCompositor",
                "--disable-background-networking",
                "--disable-bundled-ppapi-flash",
                "--disable-client-side-phishing-detection",
                "--disable-default-apps",
                "--disable-hang-monitor",
                "--disable-prompt-on-repost",
                "--disable-sync",
                "--disable-webgl",
                "--disable-gpu",
                "--disable-software-rasterizer",
                "--disable-dev-shm-usage",
                "--disable-web-security",
                "--disable-rtc-smoothness-algorithm",
                "--disable-webrtc-hw-decoding",
                "--disable-webrtc-multiple-routes",
                "--disable-webrtc-hw-vp8-encoding",
                "--disable-notifications",
                "--disable-popup-blocking",
                "--mute-audio",
                "--enable-blink-features=ShadowDOMV0",
                "--no-sandbox",
                "--enforce-webrtc-ip-permission-check",
                "--force-webrtc-ip-handling-policy",
                "--ignore-certificate-errors",
                "--force-device-scale-factor=1",
                $"--window-size={Size.X},{Size.Y}",
                $"--window-position={Position.X},{Position.Y}",
            });
            if (DisableImage)
            {
                Option.AddArgument("--blink-settings=imagesEnabled=false");
                Option.AddArgument("--disable-images");
            }
            if (Extension)
            {
                if (nameExtension.Contains("crx"))
                {
                    Option.AddExtension(nameExtension);
                }
                else
                {
                    Option.AddArgument($"--load-extension={nameExtension}");
                }
            }
            if (App)
            {
                Option.AddArgument("--app=data:,");
            }
            if (debugPort)
            {
                Option.DebuggerAddress = $"127.0.0.1:{Port}";
            }
            if (!string.IsNullOrEmpty(Proxyaddress.Trim()))
            {
                switch (Proxyaddress.Split(':').Count())
                {
                    case 1:
                        if (TypeProxy == 0)
                        {
                            Option.AddArgument("--proxy-server= 127.0.0.1:" + Proxyaddress);
                        }
                        else
                        {
                            Option.AddArgument("--proxy-server= socks5://127.0.0.1:" + Proxyaddress);
                        }
                        break;
                    case 2:
                        if (TypeProxy == 0)
                        {
                            Option.AddArgument("--proxy-server= " + Proxyaddress);
                        }
                        else
                        {
                            Option.AddArgument("--proxy-server= socks5://" + Proxyaddress);
                        }
                        break;
                    case 4:
                        if (TypeProxy == 0)
                        {
                            Option.AddArgument("--proxy-server= " + Proxyaddress.Split(':')[0] + ":" + Proxyaddress.Split(':')[1]);
                            Option.AddExtension(@"Extension\Proxy-Auto-Auth.crx");
                        }
                        else
                        {
                            Option.AddArgument("--proxy-server= socks5://" + Proxyaddress.Split(':')[0] + ":" + Proxyaddress.Split(':')[1]);
                            Option.AddExtension(@"Extension\Proxy-Auto-Auth.crx");
                        }
                        break;
                }
            }
            var driver = new ChromeDriver(chromeDriverService, Option);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(60);
            return driver;
        }
        public static bool WaitElement(IWebDriver driver, string type, string element, int repeat, int timeDelay)
        {
            for (int i = 0; i < repeat; i++)
            {
                try
                {
                    switch (type)
                    {

                        case "xpath":
                            driver.FindElement(By.XPath(element));
                            break;
                        case "name":
                            driver.FindElement(By.Name(element));
                            break;
                        case "id":
                            driver.FindElement(By.Id(element));
                            break;
                        case "css":
                            driver.FindElement(By.CssSelector(element));
                            break;
                        case "class":
                            driver.FindElement(By.ClassName(element));
                            break;
                    }
                    return true;
                }
                catch
                {
                    Thread.Sleep(TimeSpan.FromSeconds(timeDelay));
                }
            }
            return false;
        }
        public static bool CheckPageSource(IWebDriver driver, int timeWait, string text)
        {
            while (timeWait > 0)
            {
                string source = driver.PageSource;
                if (source.Contains(text))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
                timeWait -= 1;
            }
            if (timeWait > 0)
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }
        public static void ExcuteJS(IWebDriver driver, string command)
        {
            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
            ex.ExecuteScript(command);
        }
        public static void ClickOptions(IWebDriver driver, int option, string type, string element)
        {
            if (option == 1)
            {
                clickAction(driver, type, element);
            }
            else if (option == 2)
            {
                clickBot(driver, type, element);
            }
        }
        public static void SendKeysOptions(IWebDriver driver, int option, string type, string element, string text)
        {
            if (option == 1)
            {
                sendTextbyText(driver, type, element, text);
            }
            else if (option == 2)
            {
                sendTextRobot(driver, type, element, text);
            }
        }
        public static void clickBot(IWebDriver driver, string type, string element)
        {
            switch (type)
            {

                case "xpath":
                    driver.FindElement(By.XPath(element)).Click();
                    break;
                case "name":
                    driver.FindElement(By.Name(element)).Click();
                    break;
                case "id":
                    driver.FindElement(By.Id(element)).Click();
                    break;
                case "css":
                    driver.FindElement(By.CssSelector(element)).Click();
                    break;
                case "class":
                    driver.FindElement(By.ClassName(element)).Click();
                    break;
            }
        }
        public static void clickAction(IWebDriver driver, string type, string element)
        {
            Actions action = new Actions(driver);
            switch (type)
            {
                case "xpath":
                    action.MoveToElement(driver.FindElement(By.XPath(element))).Click().Build().Perform();
                    break;
                case "name":
                    action.MoveToElement(driver.FindElement(By.Name(element))).Click().Build().Perform();
                    break;
                case "id":
                    action.MoveToElement(driver.FindElement(By.Id(element))).Click().Build().Perform();
                    break;
                case "css":
                    action.MoveToElement(driver.FindElement(By.CssSelector(element))).Click().Build().Perform();
                    break;
                case "class":
                    action.MoveToElement(driver.FindElement(By.ClassName(element))).Click().Build().Perform();
                    break;
            }
        }
        public static void sendTextRobot(IWebDriver driver, string type, string element, string text)
        {
            switch (type)
            {

                case "xpath":
                    driver.FindElement(By.XPath(element)).SendKeys(text);
                    break;
                case "name":
                    driver.FindElement(By.Name(element)).SendKeys(text);
                    break;
                case "id":
                    driver.FindElement(By.Id(element)).SendKeys(text);
                    break;
                case "css":
                    driver.FindElement(By.CssSelector(element)).SendKeys(text);
                    break;
                case "class":
                    driver.FindElement(By.ClassName(element)).SendKeys(text);
                    break;
            }
        }
        public static void sendTextbyText(IWebDriver driver, string type, string element, string text)
        {
            Random re = new Random();
            IWebElement ele = null;
            switch (type)
            {

                case "xpath":
                    ele = driver.FindElement(By.XPath(element));
                    break;
                case "name":
                    ele = driver.FindElement(By.Name(element));
                    break;
                case "id":
                    ele = driver.FindElement(By.Id(element));
                    break;
                case "css":
                    ele = driver.FindElement(By.CssSelector(element));
                    break;
                case "class":
                    ele = driver.FindElement(By.ClassName(element));
                    break;
            }
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                String s = new StringBuilder().Append(c).ToString();
                ele.SendKeys(s);
                Thread.Sleep(re.Next(80, 200));
            }
        }
        public static void OpenNewTab(IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
        }
        public static void SwitchtoFirstTab(IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.First());
        }
        public static void SwitchtoLastTab(IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }
        public static void SwitchtoCloseFirstTab(IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.First()).Close();
        }
        public static void SwitchtoCloseLastTab(IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last()).Close();
        }
        public static void SwitchToTabByName(IWebDriver driver, string tabTitle)
        {
            foreach (var windowHandle in driver.WindowHandles)
            {
                driver.SwitchTo().Window(windowHandle);
                if (driver.Title.Contains(tabTitle))
                {
                    return;
                }
            }
        }
        public static bool CheckChromeClosed(IWebDriver driver)
        {
            bool result = true;
            try
            {
                _ = driver.Title;
                result = false;
            }
            catch (Exception)
            {
            }
            return result;
        }
        public static void CloseDriver(IWebDriver driver)
        {
            try
            {
                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                    driver.Dispose();
                }
            }
            catch
            {
            }
        }
        public static string GetPageSource(IWebDriver driver)
        {
            try
            {
                return driver.PageSource;
            }
            catch (Exception)
            {
            }
            return "";
        }
        public static int GetIndexOfPossitionApp(ref List<int> lstPossition)
        {
            int result = 0;
            lock (lstPossition)
            {
                for (int i = 0; i < lstPossition.Count; i++)
                {
                    if (lstPossition[i] == 0)
                    {
                        result = i;
                        lstPossition[i] = 1;
                        break;
                    }
                }
            }
            return result;
        }
        public static void TimeWaitForSearchingElement(int time, IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(time);
        }
        public static void TimeWaitForLoadingPage(int time, IWebDriver driver)
        {
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(time);
        }
        public static void GotoUrl(string Web, IWebDriver driver)
        {
            driver.Navigate().GoToUrl(Web);
        }
        public static void Refresh(IWebDriver driver)
        {
            driver.Navigate().Refresh();
        }
        public static void KillProcessChromeDriver()
        {
            Process[] processesByName = Process.GetProcessesByName("chromedriver");
            Process[] array = processesByName;
            Process[] array2 = array;
            foreach (Process process in array2)
            {
                process.Kill();
            }
        }
        public static int AddCookieIntoChrome(IWebDriver driver, string cookie, string domain)
        {
            domain = $".{domain}.com";
            try
            {
                string[] array = cookie.Split(';');
                string[] array2 = array;
                string[] array3 = array2;
                foreach (string text in array3)
                {
                    if (text.Trim() != "")
                    {
                        string[] array4 = text.Split('=');
                        if (array4.Count() > 1 && array4[0].Trim() != "")
                        {
                            Cookie cookie2 = new Cookie(array4[0].Trim(), text.Substring(text.IndexOf('=') + 1).Trim(), domain, "/", DateTime.Now.AddDays(10.0));
                            driver.Manage().Cookies.AddCookie(cookie2);
                        }
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static string GetCookieFromChrome(IWebDriver driver)
        {
            string text = "";
            string domain = "facebook";
            try
            {
                Cookie[] array = driver.Manage().Cookies.AllCookies.ToArray();
                Cookie[] array2 = array;
                Cookie[] array3 = array2;
                foreach (Cookie cookie in array3)
                {
                    if (cookie.Domain.Contains(domain))
                    {
                        text = text + cookie.Name + "=" + cookie.Value + ";";
                    }
                }
            }
            catch (Exception)
            {
            }
            return text;
        }
        public static int OpenNewTab(IWebDriver driver, string url, bool switchToLastTab = true)
        {
            try
            {
                driver.ExecuteJavaScript("window.open('" + url + "', '_blank').focus();");
                if (switchToLastTab)
                {
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int CloseCurrentTab(IWebDriver driver)
        {
            try
            {
                driver.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int SelectValue(IWebDriver driver, string typeAttribute, string element, string value)
        {
            bool flag = false;
            try
            {
                switch (typeAttribute)
                {
                    case "id":
                        new SelectElement(driver.FindElement(By.Id(element))).SelectByValue(value);
                        break;
                    case "name":
                        new SelectElement(driver.FindElement(By.Name(element))).SelectByValue(value);
                        break;
                    case "xpath":
                        new SelectElement(driver.FindElement(By.XPath(element))).SelectByValue(value);
                        break;
                    case "css":
                        new SelectElement(driver.FindElement(By.CssSelector(element))).SelectByValue(value);
                        break;
                }
                flag = true;
            }
            catch (Exception)
            {
            }
            return flag ? 1 : 0;
        }
        public static bool CheckExistElement(IWebDriver driver, string type, string element)
        {
            bool elementCount = false;

            switch (type)
            {

                case "xpath":
                    elementCount = driver.FindElements(By.XPath(element)).Any();
                    break;
                case "name":
                    elementCount = driver.FindElements(By.Name(element)).Any();
                    break;
                case "id":
                    elementCount = driver.FindElements(By.Id(element)).Any();
                    break;
                case "css":
                    elementCount = driver.FindElements(By.CssSelector(element)).Any();
                    break;
                case "class":
                    elementCount = driver.FindElements(By.ClassName(element)).Any();
                    break;
            }
            return elementCount;
        }
        public static string GetAttribute(IWebDriver driver, string type, string element, string Attribute)
        {
            string GetAttribute = "";
            switch (type)
            {

                case "xpath":
                    GetAttribute = driver.FindElement(By.XPath(element)).GetAttribute(Attribute);
                    break;
                case "name":
                    GetAttribute = driver.FindElement(By.Name(element)).GetAttribute(Attribute);
                    break;
                case "id":
                    GetAttribute = driver.FindElement(By.Id(element)).GetAttribute(Attribute);
                    break;
                case "css":
                    GetAttribute = driver.FindElement(By.CssSelector(element)).GetAttribute(Attribute);
                    break;
                case "class":
                    GetAttribute = driver.FindElement(By.ClassName(element)).GetAttribute(Attribute);
                    break;
            }
            return GetAttribute;
        }
        public static string GetText(IWebDriver driver, string type, string element)
        {
            string GetAttribute = "";
            switch (type)
            {

                case "xpath":
                    GetAttribute = driver.FindElement(By.XPath(element)).Text;
                    break;
                case "name":
                    GetAttribute = driver.FindElement(By.Name(element)).Text;
                    break;
                case "id":
                    GetAttribute = driver.FindElement(By.Id(element)).Text;
                    break;
                case "css":
                    GetAttribute = driver.FindElement(By.CssSelector(element)).Text;
                    break;
                case "class":
                    GetAttribute = driver.FindElement(By.ClassName(element)).Text;
                    break;
            }
            return GetAttribute;
        }
        public static void ClearText(IWebDriver driver, string type, string element)
        {
            switch (type)
            {

                case "xpath":
                    driver.FindElement(By.XPath(element)).Clear();
                    break;
                case "name":
                    driver.FindElement(By.Name(element)).Clear();
                    break;
                case "id":
                    driver.FindElement(By.Id(element)).Clear();
                    break;
                case "css":
                    driver.FindElement(By.CssSelector(element)).Clear();
                    break;
                case "class":
                    driver.FindElement(By.ClassName(element)).Clear();
                    break;
            }
        }
        public static int ScrollSmooth(IWebDriver driver, string JSpath)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(JSpath + ".scrollIntoView({ behavior: 'smooth', block: 'center'});");
                return 1;
            }
            catch
            {
            }
            return 0;
        }
        public static int ScrollSmoothIfNotExistOnScreen(IWebDriver driver, string JSpath)
        {
            try
            {
                if (CheckExistElementOnScreen(driver, JSpath) != 0)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript(JSpath + ".scrollIntoView({ behavior: 'smooth', block: 'center'});");
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public static int CheckExistElementOnScreen(IWebDriver driver, string JSpath)
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(((IJavaScriptExecutor)driver).ExecuteScript("var check='';x=" + JSpath + ";if(x.getBoundingClientRect().top<=0) check='-1'; else if(x.getBoundingClientRect().top+x.getBoundingClientRect().height>window.innerHeight) check='1'; else check='0'; return check;"));
            }
            catch
            {
                return 0;
            }
            return result;
        }
    }
}
