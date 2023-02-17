using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace SpecFlowNunitTestAutomation.Utils
{
    public class BrowserClass
    {
        //Initialization of selenium webdriver(Make it thread safe)
        public static ThreadLocal<IWebDriver> Driver { get; private set; }

        //Initialize the Thread safe value to Driver refference variable
        public static IWebDriver driver => Driver.Value;
        public static string DownloadPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + @"\TestData\AppDownloads";
        public static string ExtractPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + @"\TestData\ExtractedFiles";

        //Initiate the browser to run
        public static IWebDriver GetBrowserInstanceCreated(string browser)
        {
            switch (browser.ToLower().Trim())
            {
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                    ChromeOptions chromeOptions = new();
                    chromeOptions.AddArguments("--start-maximized");
                    chromeOptions.AddArguments("--disable-extensions");
                    chromeOptions.AddArgument("--ignore-certificate-errors");
                    chromeOptions.AddArguments("--disable-popup-blocking");
                    chromeOptions.AddArguments("--no-sandbox");
                    //chromeOptions.AddArgument("--incognito");
                    chromeOptions.AddArguments("--enable-automation");
                    chromeOptions.AddUserProfilePreference("download.default_directory", DownloadPath);
                    Driver = new ThreadLocal<IWebDriver>(() => new ChromeDriver(ChromeDriverService.CreateDefaultService(), chromeOptions, TimeSpan.FromMinutes(2)));
                    return driver;

                case "chrome_headless":
                    new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                    ChromeOptions options = new();
                    //options.AddArguments("--disable-gpu");
                    options.AddArguments("--disable-extensions");
                    options.AddArguments("--no-sandbox");
                    options.AddArguments("--disable-dev-shm-usage");
                    options.AddArguments("--headless");
                    options.AddArguments("--window-size=1366,768");
                    options.AddArguments("--disable-popup-blocking");
                    options.AddArgument("--ignore-certificate-errors");
                    options.AddArguments("--enable-automation");
                    options.AddUserProfilePreference("download.default_directory", DownloadPath);
                    Driver = new ThreadLocal<IWebDriver>(() => new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(2)));
                    return driver;

                case "firefox":
                case "mozilla firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig(), VersionResolveStrategy.MatchingBrowser);
                    FirefoxOptions firefoxOptions = new();
                    Driver = new ThreadLocal<IWebDriver>(() => new FirefoxDriver(firefoxOptions));
                    return driver;

                default:
                    throw new InvalidOperationException("Unexpected value: " + browser);
            }
        }
    }
}