using NUnit.Framework;
using OpenQA.Selenium;
using SpecFlowNunitTestAutomation.Hooks;
using SpecFlowNunitTestAutomation.Pages;
using SpecFlowNunitTestAutomation.StepDefinitions;
using SpecFlowNunitTestAutomation.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SpecFlowNunitTestAutomation
{
    class LoginPage : CommonActionsUtils
    {

        /**Add all the webelements of this page**/
        private static By InpUsername => By.XPath("//input[@id='txtUserName']");
        private static By InpPassword => By.XPath("//input[@id='Password']");
        private static By BtnLogin => By.XPath("//button[@id='loginButton']");
        private static By MsgValidation => By.XPath("//div[@class='validation-summary-errors']//li");
        private static By BtnForgotPassword => By.XPath("//a[text()='Forgot Password?']");
        private static By BtnResetPassword => By.XPath("//input[@value='Reset Password']");
        private static By MsgFPValidation => By.XPath("//div[@class='validation-summary-errors text-danger']//li");
        private static By InpFP_Username => By.XPath("//input[@id='Username']");
        private static By EcomLoginFail => By.XPath("//p[text()=\"Username doesn't exist\"]");

        public string? applicationUrl;

        /** page actions: features(behavior) of the page the form of methods:**/

        //Launch the Zeus application based on the environment selected
        public void LaunchTheZeusApplication()
        {
            applicationUrl = ExcelUtils.ReadDataFromExcel("URL");

            LaunchApplication(applicationUrl);

            ReporterClass.AddStepLog("Zeus Application URL  - > " + applicationUrl);
        }

        //Enter Username
        public void EnterUserName(string username)
        {
            ClearAndSendValue(InpUsername, "Username", username);
        }

        //Enter Password
        public void EnterPassword(string pwd)
        {
            ClearAndSendValue(InpPassword, "Password", pwd);
        }

        //Click on Login button
        public DashboardPage ClickOnLogin()
        {
            ClickElement(BtnLogin, "Login");

            //This login method should return Dashboard Page class object.
            return new DashboardPage();
        }

        //Consolidated method to perform login operation
        public DashboardPage DoLogin(string username, string pwd)
        {
            EnterUserName(username);
            EnterPassword(pwd);
            ClickOnLogin();

            //This login method should return Dashboard Page class object.
            return new DashboardPage();
        }

        //Verfify Ecom loggedin successfully
        public bool EcomLoginSuccess()
        {
            bool EcomLoginFailMsg = IsElementVisible(EcomLoginFail, 5);

            return EcomLoginFailMsg;
        }

        //Get page URL and validate its in login page
        public bool ValidateLoginPageURL()
        {
            //Get the page url and validate
            string LoginPageURL = GetPageURL();

            if (LoginPageURL.Contains("Account/Login"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Verify if "Please wait. Zeus is setting up the configurations for you!!" is shown in the page.
        public bool CheckZeusSettingUpMsg()
        {
            bool SettingUpMsgPresence = GetTextFromPageSource("Please wait. Zeus is setting up the configurations for you!!");

            if (SettingUpMsgPresence == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetLoginValidationMessage()
        {
            string LoginValidationMsg = GetTextValue(MsgValidation, "Login Validation message");

            return LoginValidationMsg;
        }

        public void ClickForgotPassword()
        {
            ClickElement(BtnForgotPassword, "Forgot Password");
        }

        public void ClickResetPassword()
        {
            ClickElement(BtnResetPassword, "Reset Password");
        }

        //Get page URL and validate its in Forgot Password page
        public bool ValidateFPPageURL()
        {
            //Get the page url and validate
            string FPURL = GetPageURL();

            if (FPURL.Contains("Account/ForgotPassword"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetFPValidationMessage()
        {
            string FPValidationMsg = GetTextValue(MsgFPValidation, "Forgot Password validation message.");

            return FPValidationMsg;
        }

        public void EnterUsernameInFP(string username)
        {
            SendValue(InpFP_Username, "Username", username);
        }
    }
}
