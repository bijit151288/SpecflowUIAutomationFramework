using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpecFlowNunitTestAutomation.Hooks;
using SpecFlowNunitTestAutomation.Pages;
using SpecFlowNunitTestAutomation.TableData;
using SpecFlowNunitTestAutomation.Utils;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowNunitTestAutomation.StepDefinitions
{
    [Binding]
    public sealed class LoginPageSteps
    {
        LoginPage loginPage = new();
        DashboardPage dashboardPage = new();

        [Given(@"Launch the Zeus application")]
        public void GivenLaunchTheZeusApplication()
        {
            loginPage.LaunchTheZeusApplication();
        }

        [Given(@"I login to the Zeus application with valid credentials")]
        public void GivenILoginToTheZeusApplicationWithValidCredentials()
        {
            string userName = ExcelUtils.ReadDataFromExcel("Username");
            string password = ExcelUtils.ReadDataFromExcel("Password");

            loginPage.DoLogin(userName, password);
        }

        [Given(@"User is not logged in")]
        public void GivenUserIsNotLoggedIn()
        {
            bool LoginPageURL = loginPage.ValidateLoginPageURL();
            if (LoginPageURL == true)
            {
                ReporterClass.AddStepLog("----->User is not logged in");
            }
            else if (LoginPageURL == false)
            {
                ReporterClass.AddStepLog("----->User is already logged in. Do logout");
                dashboardPage.Logout();
            }
        }

        [Given(@"I provide all required fields with valid data")]
        public void GivenIProvideAllRequiredFieldsWithValidData()
        {
            string userName = ExcelUtils.ReadDataFromExcel("Username");
            string password = ExcelUtils.ReadDataFromExcel("Password");

            loginPage.EnterUserName(userName);
            loginPage.EnterPassword(password);
        }

        [When(@"I click on Login")]
        public void WhenIClickOnLogin()
        {
            loginPage.ClickOnLogin();
        }

        [Then(@"User should be redirected to Zeus main page after being prompted with message ""([^""]*)""")]
        public void ThenUserShouldBeRedirectedToZeusMainPageAfterBeingPromptedWithMessage(string message)
        {
            /*bool SettingUpMsgPresence = loginPage.CheckZeusSettingUpMsg();
            if (SettingUpMsgPresence == true)
            {
                ReporterClass.AddStepLog("----->The following message appeared: " + message);
            }
            else if (SettingUpMsgPresence == false)
            {
                Assert.Fail("The following message didn't appear: " + message);
            }*/

            bool DashboardPageTitle = dashboardPage.ValidateDashboardPageTitle();
            if (DashboardPageTitle == true)
            {
                ReporterClass.AddStepLog("----->User is in Dashboard Page");
            }
            else if (DashboardPageTitle == false)
            {
                Assert.Fail("User is not redirected to Dashboard Page. Please check again");
            }
        }

        [Given(@"I provide all required fields with incorrect password")]
        public void GivenIProvideAllRequiredFieldsWithIncorrectPassword()
        {
            string userName = ExcelUtils.ReadDataFromExcel("Username");
            string password = "xyz"; //Inavlid password for the username

            loginPage.EnterUserName(userName);
            loginPage.EnterPassword(password);

            ReporterClass.AddStepLog("----->login with valid Username: " + userName + " and invalid Password: " + password);
        }

        [Then(@"User should be prompted with message ""([^""]*)""")]
        public void ThenUserShouldBePromptedWithMessage(string validationMsg)
        {
            string ActualValidationMsg = loginPage.GetLoginValidationMessage();

            Assert.AreEqual(validationMsg, ActualValidationMsg, "Validation error message: \"" + validationMsg + "\" is not displayed.");
        }

        [Given(@"I provide all required fields with non existing Username and password")]
        public void GivenIProvideAllRequiredFieldsWithNonExistingUsernameAndPassword()
        {
            string userName = "xxyyzz"; //invalid username
            string password = "xyz"; //Inavlid password

            loginPage.EnterUserName(userName);
            loginPage.EnterPassword(password);

            ReporterClass.AddStepLog("----->login with invalid Username: " + userName + " and invalid Password: " + password);
        }

        [Given(@"I don't provide any Username or password")]
        public void GivenIDontProvideAnyUsernameOrPassword()
        {
            string userName = ""; //invalid username
            string password = ""; //Inavlid password

            loginPage.EnterUserName(userName);
            loginPage.EnterPassword(password);

            ReporterClass.AddStepLog("----->login with empty username and password");
        }

        [Then(@"nothing should happen and user stays on login page only")]
        public void ThenNothingShouldHappenAndUserStaysOnLoginPageOnly()
        {
            bool LoginPageURL = loginPage.ValidateLoginPageURL();
            if (LoginPageURL == true)
            {
                ReporterClass.AddStepLog("----->User is not logged in and stays on login page.");
            }
            else if (LoginPageURL == false)
            {
                //ReporterClass.AddStepLog("----->User was logged in with empty username and password provided");
                Assert.Fail("----->User was logged in with empty username and password provided.");
            }
        }

        [When(@"I click Forgot Password\?")]
        public void WhenIClickForgotPassword()
        {
            loginPage.ClickForgotPassword();
        }

        [Then(@"User should be redirected to Forgot Password page")]
        public void ThenUserShouldBeRedirectedToForgotPasswordPage()
        {
            bool FPURL = loginPage.ValidateFPPageURL();
            Assert.True(FPURL, "User is not in Forgot password page");
        }

        [Given(@"I open Forgot Password Page")]
        public void GivenIOpenForgotPasswordPage()
        {
            loginPage.ClickForgotPassword();
        }

        [When(@"I click Reset Password")]
        public void WhenIClickResetPassword()
        {
            loginPage.ClickResetPassword();
        }

        [Then(@"User should receive ""([^""]*)""")]
        public void ThenUserShouldReceive(string message)
        {
            string ActualValidationMsg = loginPage.GetFPValidationMessage();

            Assert.AreEqual(message, ActualValidationMsg, "Validation error message: \"" + message + "\" is not displayed.");
        }

        [Given(@"I provide an invalid Username")]
        public void GivenIProvideAnInvalidUsername()
        {
            string userName = "xyz"; //invalid username
            loginPage.EnterUsernameInFP(userName);
        }

        [Given(@"I provide Valid Username")]
        public void GivenIProvideValidUsername(Table table)
        {
            var record = table.CreateInstance<LoginPageTableData>();

            string FPUsername = record.ValidUsername;

            ReporterClass.AddStepLog("----->Username provided is : " + FPUsername);

            loginPage.EnterUsernameInFP(FPUsername);
        }

        [Then(@"User should be redirected to the Login Page and receive ""([^""]*)""")]
        public void ThenUserShouldBeRedirectedToTheLoginPageAndReceive(string validationMsg)
        {
            //Verify user in in login page now
            bool LoginPageURL = loginPage.ValidateLoginPageURL();

            //Verify the message "Email sent to registered Email" is displayed
            string ActualValidationMsg = loginPage.GetLoginValidationMessage();

            /*Functionally, this results in NUnit storing any failures encountered in the block and reporting all of them 
             * together upon exit from the block.If both asserts failed,then both would be reported.
             * The test itself would terminate at the end of the block if any failures were encountered, 
             * but would continue otherwise.*/

            Assert.Multiple(() =>
              {
                  Assert.True(LoginPageURL, "User is not redirected to login page. The url is not of login page");
                  Assert.AreEqual(validationMsg, ActualValidationMsg, "Validation message: \"" + validationMsg + "\" is not displayed.");
              });

        }

    }
}
