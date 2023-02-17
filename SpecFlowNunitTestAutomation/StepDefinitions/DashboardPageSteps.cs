using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpecFlowNunitTestAutomation.Hooks;
using SpecFlowNunitTestAutomation.Pages;
using SpecFlowNunitTestAutomation.Utils;
using TechTalk.SpecFlow;

namespace SpecFlowNunitTestAutomation.StepDefinitions
{
    [Binding]
    public sealed class DashboardPageSteps
    {
        readonly DashboardPage dashboardPage = new();
        private string StoreSelected;

        [When(@"I click on distribution center change button")]
        public void GivenIClickOnDistributionCenterChangeButton()
        {
            dashboardPage.ClickOnChange();
        }

        [When(@"I select a store named ""([^""]*)""")]
        public void WhenISelectAStoreNamed(string StoreName)
        {
            //Select the store
            dashboardPage.SelectStore(StoreName);
        }

        [When(@"I select a DC named ""([^""]*)""")]
        public void WhenISelectADCNamed(string DCName)
        {
            //Select the DC
            dashboardPage.SelectDC(DCName);
        }


        [Then(@"The store should change to ""([^""]*)""")]
        public void ThenTheStoreShouldChangeTo(string StoreName)
        {
            //Get the store name that is selected from the dashboard page
            string ActualStoreName = dashboardPage.GetStoreNameSelected();

            Assert.AreEqual(StoreName, ActualStoreName, "Store name: " + ActualStoreName + " is not expected. Store name that was selected is: " + StoreName +
                "The store was not changed successfully");
        }

        [When(@"I select a store randomly")]
        public void WhenISelectAStoreRandomly()
        {
            StoreSelected = dashboardPage.SelectStoreRandomly();
            ReporterClass.AddStepLog("----->The store selected randomly is : " + StoreSelected);
        }

        [When(@"I select the store that was selected randomly")]
        public void WhenISelectTheStoreThatWasSelectedRandomly()
        {
            //Select the store
            dashboardPage.SelectStore(StoreSelected);
        }

        [Then(@"The store should change to what was selected randomly")]
        public void ThenTheStoreShouldChangeToWhatWasSelectedRandomly()
        {
            //Get the store name that is selected from the dashboard page
            string ActualStoreName = dashboardPage.GetStoreNameSelected();

            ReporterClass.AddStepLog("----->Actual Store name: " + ActualStoreName + " and Expected store name: " + StoreSelected);

            Assert.AreEqual(StoreSelected, ActualStoreName, "Store name: " + ActualStoreName + " is not expected. Store name that was selected is: " + StoreSelected +
                "The store was not changed successfully");
        }

        [Given(@"I open dashboard page")]
        public void GivenIOpenDasboardPage()
        {
            dashboardPage.OpenDashboardPage();
        }

    }
}
