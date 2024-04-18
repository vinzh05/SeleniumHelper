SeleniumSupport Library
=======================

Enhance your Selenium experience with streamlined functionality!

Description
-----------

SeleniumSupport is a robust library created to simplify and enhance your Selenium-based automation tasks. It introduces user-friendly methods and configurations to streamline browser manipulation and interaction. Key features include disabling images for speed, using browser extensions, managing multiple profiles, and proxy configurations. With SeleniumSupport, developers can automate more efficiently, reducing the boilerplate code typically associated with Selenium.

Getting Started
---------------

### Dependencies

-   Ensure you have Selenium WebDriver installed.
-   Compatible with major operating systems (e.g., Windows, macOS, Linux).

### Installing

-   Download the library from the provided repository link.
-   Include the library in your project directory.
-   Download on nuget https://www.nuget.org/packages/Selenium.SeleniumHelper

### Executing program

Execute your Selenium scripts with enhanced capabilities:
-   Usage:
-   
    ```csharp
    var helper = new SeleniumHelper();
    var driver = SeleniumHelper.OpenBrowser(helper);
    // Your automation code here

Detailed Usage Guide
--------------------

1. `OpenBrowser(SeleniumHelper seleniumHelper)`

-   Purpose: Initializes and returns a `ChromeDriver` instance configured with options specified in the `SeleniumHelper` instance.
-   Usage:

    ```csharp
    var helper = new SeleniumHelper();
    helper.DisableImages = true; // Example setting
    IWebDriver driver = SeleniumHelper.OpenBrowser(helper);

2. `WaitElement(IWebDriver driver, string type, string element, int repeat, int timeDelay)`

-   Purpose: Waits for an element to become available on the web page, retrying a specified number of times.
-   Usage:

    ```csharp
    bool isElementFound = SeleniumHelper.WaitElement(driver, "id", "submit-button", 5, 1);

3. `CheckPageSource(IWebDriver driver, int timeWait, string text)`

-   Purpose: Checks if a specific text is present in the page source within a specified time period.
-   Usage:

    ```csharp
    bool isTextFound = SeleniumHelper.CheckPageSource(driver, 3, "Welcome");

4. `ExcuteJS(IWebDriver driver, string command)`

-   Purpose: Executes JavaScript on the current page.
-   Usage:

    ```csharp
    SeleniumHelper.ExcuteJS(driver, "alert('Hello World');");

### Tab and Window Handling Methods

1.  `OpenNewTab(IWebDriver driver)`:

-   Opens a new browser tab.
-   Example:

    ```csharp
    SeleniumHelper.OpenNewTab(driver);

2.  `SwitchtoFirstTab(IWebDriver driver)`, `SwitchtoLastTab(IWebDriver driver)`, `SwitchtoCloseFirstTab(IWebDriver driver)`, `SwitchtoCloseLastTab(IWebDriver driver)`:

-   Manage browser tabs by switching to or closing the first or last tab.
-   Example:

    ```csharp
    SeleniumHelper.SwitchtoFirstTab(driver);
    SeleniumHelper.CloseDriver(driver);

### Additional Utilities

1.  `CheckPageSource(IWebDriver driver, int timeWait, string text)`:

-   Checks if a specific text is present in the page source within a specified time period.
-   Example:

    ```csharp
    bool isTextFound = SeleniumHelper.CheckPageSource(driver, 3, "Welcome");

2.  `AddCookieIntoChrome(IWebDriver driver, string cookie, string domain)`:

-   Adds a cookie into Chrome.
-   Example:

    ```csharp
    SeleniumHelper.AddCookieIntoChrome(driver, "sessionId=abc123", "example");

3.  `GetCookieFromChrome(IWebDriver driver)`:
-   Retrieves cookies from Chrome.
-   Example:

    ```csharp
    string cookies = SeleniumHelper.GetCookieFromChrome(driver);


Authors
-------

-   BUI QUANG VINH
-   Contact: [BUI QUANG VINH](https://www.facebook.com/Vinzh05)

Version History
---------------

-   1.0.1
    -   Initial release.

License
-------

This project is licensed under the MIT License - see the LICENSE.md file for details.

Donations
---------

Support the ongoing development and improvement of SeleniumSupport:

-   Donate via [Binance](https://www.binance.com/) Address: TRvXSzxnjDWTTi1fnouqjFvb2YNzG5RqsZ 
-   Donate via [Momo](https://momo.vn/) Address: 0974602103
-   Donate via [Paypal](https://www.paypal.com/) Address: quangvinhb167@gmail.com
-   Support on [Telegram](https://t.me/Vinzh05)
-   Support on [Facebook](https://www.facebook.com/Vinzh05)
