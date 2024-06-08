using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumUndetectedChromeDriver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;

namespace SeleniumHelperSupport
{
    public class SeleniumHelper
    {
        public bool EnableDisableImage { get; set; }
        public bool UseExtension { get; set; }
        public bool UseAppMode { get; set; }
        public string ExtensionPath { get; set; }
        public bool UseDebugPort { get; set; }
        public string DebugPort { get; set; }
        public bool UseProfile { get; set; }
        public string ProfilePath { get; set; }
        public int ProxyType { get; set; }
        public string ProxyAddress { get; set; }
        public string[] Arguments { get; set; }
        public bool UseBrowserPath { get; set; }
        public string BrowserPath { get; set; }
        public bool UseDriverPath { get; set; }
        public string DriverPath { get; set; }
        public Dictionary<string, object> Prefs { get; set; }
        public SeleniumHelper()
        {
            EnableDisableImage = false;
            UseExtension = false;
            UseAppMode = false;
            UseBrowserPath = false;
            BrowserPath = string.Empty;
            UseDriverPath = false;
            DriverPath = string.Empty;
            ExtensionPath = string.Empty;
            UseDebugPort = false;
            DebugPort = string.Empty;
            UseProfile = false;
            ProfilePath = string.Empty;
            ProxyType = 0;
            ProxyAddress = string.Empty;
            Arguments = new string[] { };
            Prefs = null;
        }
        public static IWebDriver OpenBrowser(SeleniumHelper seleniumHelper)
        {
            var options = new ChromeOptions();
            ChromeDriverService chromeDriverService = null;
            if (seleniumHelper.UseDriverPath)
            {
                chromeDriverService = ChromeDriverService.CreateDefaultService(seleniumHelper.DriverPath);
            }
            else
            {
                chromeDriverService = ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);
            }
            chromeDriverService.HideCommandPromptWindow = true;
            chromeDriverService.DisableBuildCheck = true;
            options.AddExcludedArgument("enable-automation");
            options.AcceptInsecureCertificates = true;
            AddBrowserArguments(options, seleniumHelper.Arguments);
            AddPreferences(options, seleniumHelper.Prefs);
            if (seleniumHelper.UseBrowserPath)
            {
                options.BinaryLocation = seleniumHelper.BrowserPath;
            }
            if (seleniumHelper.EnableDisableImage)
            {
                options.AddArgument("--blink-settings=imagesEnabled=false");
                options.AddArgument("--disable-images");
            }
            if (seleniumHelper.UseExtension)
            {
                if (seleniumHelper.ExtensionPath.Contains("crx"))
                {
                    options.AddExtension(seleniumHelper.ExtensionPath);
                }
                else
                {
                    options.AddArgument($"--load-extension={seleniumHelper.ExtensionPath}");
                }
            }
            if (seleniumHelper.UseAppMode)
            {
                options.AddArgument("--app=data:,");
            }
            if (seleniumHelper.UseDebugPort)
            {
                options.DebuggerAddress = seleniumHelper.DebugPort;
            }
            if (seleniumHelper.UseProfile)
            {
                if (!Directory.Exists("ProfileChrome\\" + seleniumHelper.ProfilePath))
                {
                    Directory.CreateDirectory("ProfileChrome\\" + seleniumHelper.ProfilePath);
                }
                if (Directory.Exists("ProfileChrome\\" + seleniumHelper.ProfilePath))
                {
                    options.AddArgument("--user-data-dir=" + Directory.GetCurrentDirectory() + "\\ProfileChrome\\" + seleniumHelper.ProfilePath);
                }
            }
            if (!string.IsNullOrEmpty(seleniumHelper.ProxyAddress.Trim()))
            {
                switch (seleniumHelper.ProxyAddress.Split(':').Count())
                {
                    case 1:
                        if (seleniumHelper.ProxyType == 0)
                        {
                            options.AddArgument("--proxy-server= 127.0.0.1:" + seleniumHelper.ProxyAddress);
                        }
                        else
                        {
                            options.AddArgument("--proxy-server= socks5://127.0.0.1:" + seleniumHelper.ProxyAddress);
                        }
                        break;
                    case 2:
                        if (seleniumHelper.ProxyType == 0)
                        {
                            options.AddArgument("--proxy-server= " + seleniumHelper.ProxyAddress);
                        }
                        else
                        {
                            options.AddArgument("--proxy-server= socks5://" + seleniumHelper.ProxyAddress);
                        }
                        break;
                    case 4:
                        AddProxyAuthentication(options, seleniumHelper.ProxyAddress.Split(':')[0], Convert.ToInt32(seleniumHelper.ProxyAddress.Split(':')[1]), seleniumHelper.ProxyAddress.Split(':')[2], seleniumHelper.ProxyAddress.Split(':')[3]);
                        break;
                }
            }
            var driver = new ChromeDriver(chromeDriverService, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(60);
            return driver;
        }
        public static UndetectedChromeDriver OpenUndetectChrome(SeleniumHelper seleniumHelper)
        {
            var options = new ChromeOptions();
            AddBrowserArguments(options, seleniumHelper.Arguments);
            if (seleniumHelper.UseBrowserPath)
            {
                options.BinaryLocation = seleniumHelper.BrowserPath;
            }
            if (seleniumHelper.EnableDisableImage)
            {
                options.AddArgument("--blink-settings=imagesEnabled=false");
                options.AddArgument("--disable-images");
            }
            if (seleniumHelper.UseExtension)
            {
                if (seleniumHelper.ExtensionPath.Contains("crx"))
                {
                    options.AddExtension(seleniumHelper.ExtensionPath);
                }
                else
                {
                    options.AddArgument($"--load-extension={seleniumHelper.ExtensionPath}");
                }
            }
            if (seleniumHelper.UseAppMode)
            {
                options.AddArgument("--app=data:,");
            }
            if (seleniumHelper.UseDebugPort)
            {
                options.DebuggerAddress = seleniumHelper.DebugPort;
            }
            if (seleniumHelper.UseProfile)
            {
                if (!Directory.Exists("ProfileChrome\\" + seleniumHelper.ProfilePath))
                {
                    Directory.CreateDirectory("ProfileChrome\\" + seleniumHelper.ProfilePath);
                }
                if (Directory.Exists("ProfileChrome\\" + seleniumHelper.ProfilePath))
                {
                    options.AddArgument("--user-data-dir=" + Directory.GetCurrentDirectory() + "\\ProfileChrome\\" + seleniumHelper.ProfilePath);
                }
            }
            if (!string.IsNullOrEmpty(seleniumHelper.ProxyAddress.Trim()))
            {
                switch (seleniumHelper.ProxyAddress.Split(':').Count())
                {
                    case 1:
                        if (seleniumHelper.ProxyType == 0)
                        {
                            options.AddArgument("--proxy-server= 127.0.0.1:" + seleniumHelper.ProxyAddress);
                        }
                        else
                        {
                            options.AddArgument("--proxy-server= socks5://127.0.0.1:" + seleniumHelper.ProxyAddress);
                        }
                        break;
                    case 2:
                        if (seleniumHelper.ProxyType == 0)
                        {
                            options.AddArgument("--proxy-server= " + seleniumHelper.ProxyAddress);
                        }
                        else
                        {
                            options.AddArgument("--proxy-server= socks5://" + seleniumHelper.ProxyAddress);
                        }
                        break;
                    case 4:
                        AddProxyAuthentication(options, seleniumHelper.ProxyAddress.Split(':')[0], Convert.ToInt32(seleniumHelper.ProxyAddress.Split(':')[1]), seleniumHelper.ProxyAddress.Split(':')[2], seleniumHelper.ProxyAddress.Split(':')[3]);
                        break;
                }
            }
            UndetectedChromeDriver driver;
            string BaseUrl = $@"{AppDomain.CurrentDomain.BaseDirectory}\chromedriver.exe";
            driver = UndetectedChromeDriver.Create(
                     driverExecutablePath: seleniumHelper.UseDriverPath ? seleniumHelper.DriverPath : BaseUrl,
                     browserExecutablePath: seleniumHelper.UseBrowserPath ? seleniumHelper.BrowserPath : null,
                     //userDataDir: seleniumHelper.UseProfile ? seleniumHelper.ProfilePath : null,
                     options: options,
                     hideCommandPromptWindow: true,
                     prefs: seleniumHelper.Prefs
                 );

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(60);

            return driver;
        }
        private static void AddBrowserArguments(ChromeOptions options, string[] arguments)
        {
            if (arguments != null && arguments.Any())
            {
                options.AddArguments(arguments);
            }
        }
        private static void AddPreferences(ChromeOptions options, Dictionary<string, object> Preference)
        {
            if (Preference != null && Preference.Any())
            {
                foreach (var item in Preference)
                {
                    options.AddUserProfilePreference(item.Key, item.Value);
                }
            }
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
        public static object ExcuteJS(IWebDriver driver, string command)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            return javaScriptExecutor.ExecuteScript(command);
        }
        public static void ClickOptions(IWebDriver driver, int option, string type, string element)
        {
            if (option == 1)
            {
                clickAction(driver, type, element);
            }
            else if (option == 2)
            {
                clickNormal(driver, type, element);
            }
        }
        public static void SendKeysOptions(IWebDriver driver, int option, string type, string element, string text)
        {
            if (option == 1)
            {
                sendTextbyIndex(driver, type, element, text);
            }
            else if (option == 2)
            {
                sendTextNormal(driver, type, element, text);
            }
        }
        public static void clickNormal(IWebDriver driver, string type, string element)
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
        public static void clickWithElementSpecified(IWebDriver driver, string type, string element, int ElementSpecified)
        {
            IList<IWebElement> elements = null;
            switch (type)
            {
                case "xpath":
                    elements = driver.FindElements(By.XPath(element));
                    break;
                case "name":
                    elements = driver.FindElements(By.Name(element));
                    break;
                case "id":
                    elements = driver.FindElements(By.Id(element));
                    break;
                case "css":
                    elements = driver.FindElements(By.CssSelector(element));
                    break;
                case "class":
                    elements = driver.FindElements(By.ClassName(element));
                    break;
            }
            elements[ElementSpecified].Click();
        }
        public static void sendTextNormal(IWebDriver driver, string type, string element, string text)
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
        public static void sendTextbyIndex(IWebDriver driver, string type, string element, string text)
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
        public static int ScrollSmooth(IWebDriver driver, int distance)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy({ top: " + distance + ",behavior: 'smooth'});");
                return 1;
            }
            catch
            {
            }
            return 0;
        }
        public static int CheckExistElementOnScreen(IWebDriver driver, string JSpath)
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(((IJavaScriptExecutor)driver).ExecuteScript($"console.log((x={JSpath}).getBoundingClientRect().top<=0 ? '-1' : (x.getBoundingClientRect().top + x.getBoundingClientRect().height > window.innerHeight ? '1' : '0'));"));
            }
            catch
            {
            }
            return result;
        }
        public static string TakeScreenShotToBase64(IWebDriver driver, string type, IWebDriver element = null)
        {
            string result = "";
            Screenshot screenshot = null;
            try
            {
                switch (type)
                {
                    case "driver":
                        screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                        break;
                    case "element":
                        if (element != null)
                            screenshot = ((ITakesScreenshot)element).GetScreenshot();
                        else
                            throw new ArgumentNullException("element", "The element parameter cannot be null when type is 'element'.");
                        break;
                    default:
                        throw new ArgumentException("Invalid type parameter. It must be either 'driver' or 'element'.");
                }
            }
            catch
            {
                return null;
            }
            return result = screenshot.ToString();
        }

        private const string BackgroundJsTemplate = @"
            var config = {
                mode: 'fixed_servers',
                rules: {
                    singleProxy: {
                        scheme: 'http',
                        host: '{HOST}',
                        port: parseInt('{PORT}')
                    },
                    bypassList: []
                }
            };
            chrome.proxy.settings.set({ value: config, scope: 'regular' }, function() { });
            function callbackFn(details) {
                return {
                    authCredentials: {
                        username: '{USERNAME}',
                        password: '{PASSWORD}'
                    }
                };
            }
            chrome.webRequest.onAuthRequired.addListener(callbackFn, { urls: ['<all_urls>'] }, ['blocking']);";

        private const string ManifestJsonTemplate = @"
        {
            ""version"": ""1.0.0"",
            ""manifest_version"": 2,
            ""name"": ""Chrome Proxy"",
            ""permissions"": [
                ""proxy"", ""tabs"", ""unlimitedStorage"", ""storage"", ""<all_urls>"", ""webRequest"", ""webRequestBlocking""
            ],
            ""background"": {
                ""scripts"": [""background.js""]
            },
            ""minimum_chrome_version"": ""22.0.0""
        }";
        public static void AddProxyAuthentication(ChromeOptions options, string host, int port, string userName, string password)
        {
            string extensionDir = "Plugins";
            if (!Directory.Exists(extensionDir))
            {
                Directory.CreateDirectory(extensionDir);
            }

            string guid = Guid.NewGuid().ToString();
            string manifestPath = Path.Combine(extensionDir, $"manifest_{guid}.json");
            string backgroundScriptPath = Path.Combine(extensionDir, $"background_{guid}.js");
            string extensionZipPath = Path.Combine(extensionDir, $"proxy_auth_plugin_{guid}.zip");

            string configuredManifest = ManifestJsonTemplate;
            string configuredBackgroundJs = ReplaceTemplates(BackgroundJsTemplate, host, port, userName, password);

            File.WriteAllText(manifestPath, configuredManifest);
            File.WriteAllText(backgroundScriptPath, configuredBackgroundJs);

            using (ZipArchive zip = ZipFile.Open(extensionZipPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(manifestPath, "manifest.json");
                zip.CreateEntryFromFile(backgroundScriptPath, "background.js");
            }

            File.Delete(manifestPath);
            File.Delete(backgroundScriptPath);
            options.AddExtension(extensionZipPath);
        }
        private static string ReplaceTemplates(string template, string host, int port, string userName, string password)
        {
            return template.Replace("{HOST}", host)
                .Replace("{PORT}", port.ToString())
                .Replace("{USERNAME}", userName)
                .Replace("{PASSWORD}", password);
        }
    }
}
