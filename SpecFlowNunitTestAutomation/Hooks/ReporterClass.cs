
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using SpecFlowNunitTestAutomation.Utils;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.UnitTestProvider;

//[assembly: Parallelizable(ParallelScope.Fixtures)]

/*This attribute is optional. If it is not specified,
NUnit uses the processor count or 2, whichever is greater.
example, on a four processor machine the default value is 4.*/
//[assembly: LevelOfParallelism(2)]

namespace SpecFlowNunitTestAutomation.Hooks
{
    [Binding]
    class ReporterClass
    {
        private static ExtentHtmlReporter htmlReporter;
        private static ExtentReports extentReports;
        static string reportCompleteFilePath;
        static string reporterNameTimeStamp;
        [ThreadStatic]
        private static ExtentTest featureName;
        [ThreadStatic]
        private static ExtentTest scenarioName;
        [ThreadStatic]
        static ExtentTest stepName;
        public static ThreadLocal<ExtentTest> stepThreadLocal = new();
        public static ThreadLocal<ExtentTest> featureThreadLocal = new();
        private static CommonActionsUtils cmnActions = new();

        public static void SetStepThreadLocal(ExtentTest stepThread)
        {
            stepThreadLocal.Value = stepThread;
        }

        public static ThreadLocal<ExtentTest> GetStepThreadLocal()
        {
            return stepThreadLocal;
        }

        [BeforeTestRun(Order = 1)]
        public static void CreateExtentHtmlReporter()
        {
            //string browser = FileReaderUtils.ReadDataFromConfigFile("Environment");
            string environment = TestContext.Parameters["Environment"].ToString();

            //string browser = FileReaderUtils.ReadDataFromConfigFile("BrowserName");
            string browser = TestContext.Parameters["BrowserName"].ToString();

            string JiraTktNum = TestContext.Parameters["JiraTicketNum"].ToString().ToUpper();

            //Initialize Extent report before test starts

            reporterNameTimeStamp = DateTime.Now.ToString("dd_MMM_yyyy_HH_mm_ss");
            //reportCompleteFilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent + @"\Reports\TestReport\Report_" + reporterNameTimeStamp + @"\";

            //reportCompleteFilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent + @"\Reports\TestReport\";
            //reportCompleteFilePath = @"C:\AutomatedTestResults\TestReport\";

            reportCompleteFilePath = @"C:\AutomatedTestResults\TestReport_" + JiraTktNum + "_" + environment.ToUpper() + "_" + reporterNameTimeStamp + @"\";
            Console.WriteLine("Path of the Report File - > " + reportCompleteFilePath);

            htmlReporter = new ExtentHtmlReporter(reportCompleteFilePath);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            htmlReporter.Config.ReportName = "Test Automation Report";

            extentReports = new ExtentReports();
            extentReports.AttachReporter(htmlReporter);

            //Get current OS/Version and Hostname
            OperatingSystem os = Environment.OSVersion;

            string OS_Version = os.VersionString.ToString();
            string OS_Platform = os.Platform.ToString();
            string Hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");

            //Call the AddSysInfo to provide OS info and hostname
            extentReports.AddSystemInfo("Operating System", OS_Version);
            extentReports.AddSystemInfo("Platform", OS_Platform);
            extentReports.AddSystemInfo("Hostname", Hostname);

            //Call the AddSystemInfo to provide browser info
            extentReports.AddSystemInfo("Browser", browser.ToUpper());

            //Call the AddSystemInfo to provide Environment info
            extentReports.AddSystemInfo("Environment", environment.ToUpper());
        }

        [BeforeFeature]
        public static void CreateFeature(FeatureContext featureContext)
        {
            featureName = extentReports.CreateTest<Feature>(featureContext.FeatureInfo.Title, featureContext.FeatureInfo.Description);
        }

        [BeforeScenario]
        public static void CreateScenario(ScenarioContext scenarioContext)
        {
            scenarioName = featureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Tags.First() + ":<br />"
                + scenarioContext.ScenarioInfo.Title).AssignCategory(scenarioContext.ScenarioInfo.Tags.Last());
        }

        [BeforeStep]
        public static void createStep(ScenarioContext scenarioContext)
        {
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            switch (stepType)
            {
                case "Given":
                    stepName = scenarioName.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text);
                    SetStepThreadLocal(stepName);
                    break;
                case "When":
                    stepName = scenarioName.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text);
                    SetStepThreadLocal(stepName);
                    break;
                case "Then":
                    stepName = scenarioName.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text);
                    SetStepThreadLocal(stepName);
                    break;
                case "And":
                    stepName = scenarioName.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text);
                    SetStepThreadLocal(stepName);
                    break;
            }
        }

        [AfterStep]
        public static void CreateStepsWithScenario(ScenarioContext scenarioContext)
        {
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            if (scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.OK)
            {
                switch (stepType)
                {
                    case "Given":
                        stepName.Pass("Step Passed");
                        break;
                    case "When":
                        stepName.Pass("Step Passed");
                        break;
                    case "Then":
                        stepName.Pass("Step Passed");
                        break;
                    case "And":
                        stepName.Pass("Step Passed");
                        break;
                }
            }
            else if (scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError)
            {
                string filePathToSaveScreenshots = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent + @"\Reports\Screenshots\ScreenshotImage" + DateTime.Now.ToString("ddMMHHmmss") + ".png";
                switch (stepType)
                {
                    //Use <small> or <p> tags based on your convenient but you have to keep your exception message inside some HTML tag as I have done below.
                    case "Given":
                        stepName.Fail("Step Failed:" + "<small>" + scenarioContext.TestError.Message.Replace("<", "&#60").Replace(">", "&#62") + "</small>").AddScreenCaptureFromBase64String(CommonActionsUtils.TakeBase64ScreenshotImage(filePathToSaveScreenshots), "Failed Image");
                        break;
                    case "When":
                        stepName.Fail("Step Failed:" + "<small>" + scenarioContext.TestError.Message.Replace("<", "&#60").Replace(">", "&#62") + "</small>").AddScreenCaptureFromBase64String(CommonActionsUtils.TakeBase64ScreenshotImage(filePathToSaveScreenshots), "Failed Image");
                        break;
                    case "Then":
                        stepName.Fail("Step Failed:" + "<small>" + scenarioContext.TestError.Message.Replace("<", "&#60").Replace(">", "&#62") + "</small>").AddScreenCaptureFromBase64String(CommonActionsUtils.TakeBase64ScreenshotImage(filePathToSaveScreenshots), "Failed Image");
                        break;
                    case "And":
                        stepName.Fail("Step Failed:" + "<small>" + scenarioContext.TestError.Message.Replace("<", "&#60").Replace(">", "&#62") + "</small>").AddScreenCaptureFromBase64String(CommonActionsUtils.TakeBase64ScreenshotImage(filePathToSaveScreenshots), "Failed Image");
                        break;
                }
            }
            else if (scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.Skipped)
            {
                switch (stepType)
                {
                    //Use <small> or <p> tags based on your convenient but you have to keep your exception message inside some HTML tag as I have done below.
                    case "Given":
                        stepName.Skip("Step Skipped:");
                        break;
                    case "When":
                        stepName.Skip("Step Skipped:");
                        break;
                    case "Then":
                        stepName.Skip("Step Skipped:");
                        break;
                    case "And":
                        stepName.Skip("Step Skipped:");
                        break;
                }
            }
        }

        public static void AddStepLog(string passedDescription)
        {
            GetStepThreadLocal().Value.Pass(passedDescription);
        }

        public static void AddFailedStepLog(string failedDescription)
        {
            GetStepThreadLocal().Value.Fail(failedDescription);
            Assert.Fail();
        }

        public static void AddSkippedStepLog(string skippedDescription)
        {
            GetStepThreadLocal().Value.Skip(skippedDescription);
            Assert.Ignore();
        }

        [AfterScenario(Order = 2)]
        public static void Closedriver(ScenarioContext scenarioContext)
        {
            cmnActions.CloseBrowser();
        }

        [AfterTestRun]
        public static void AfterTestReporterFlush()
        {
            string cleanUpFilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + @"\CleanupProcess.bat";
            try
            {
                extentReports.Flush();

                System.Diagnostics.Process process = new();
                process.StartInfo.UseShellExecute = true;

                //Open the test result report html file
                process.StartInfo.FileName = reportCompleteFilePath + "index.html";
                process.Start();

                //Clean all browser exe files created.
                process.StartInfo.FileName = cleanUpFilePath;
                process.Start();

                System.Diagnostics.Process[] allChromeProccess = System.Diagnostics.Process.GetProcessesByName("chromedriver.exe");
                string s = allChromeProccess[0].ProcessName;
                foreach (System.Diagnostics.Process chromeprocess in allChromeProccess)
                {
                    chromeprocess.Kill();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }
    }
}
