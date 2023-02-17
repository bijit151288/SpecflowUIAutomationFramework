using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SpecFlowNunitTestAutomation.Utils;

namespace SpecFlowNunitTestAutomation.Pages
{
    class DashboardPage : CommonActionsUtils
    {
        /**Add all the webelements of this page**/
        private static By BtnChangeStore => By.XPath("//input[@id='btnStoreSelector']");
        private static By DrpStoreSelect => By.XPath("//span[@aria-controls='selUserStoreSelector_listbox']//span");
        private static By DrpDCSelect => By.XPath("//span[@aria-controls='selUserDistributionCenterSelector_listbox']//span");
        private static By BtnStoreName(string name) => By.XPath("//li[text()='" + name + "']");
        private static By BtnDCName(string name) => By.XPath("//li[text()='" + name + "']");
        private static By BtnStoreNameByIndex(string Index) => By.XPath("(//li[@data-offset-index='" + Index + "'])[1]");
        private static By TxtStoreName => By.XPath("//section[@id='store-quick-selector-content']//span[contains(@class,'currentValue')]");
        private static By IconLoader => By.XPath("//div[@id='loaderWrapper' and (contains(@style,'block;') or contains(@style,'opacity'))]");
        private static By TopStoreDCName(string name) => By.XPath("//section[@id='store-quick-selector-content']//span[contains(@class,'currentValue') and text()='" + name + "']");
        private static By DrpSideBarMenu(string MenuName) => By.XPath("(//ul[@class='sidebar-menu']//span[text()='" + MenuName + "'])[1]");
        private static By LnkSubMenuImportExport => By.XPath("//span[text()='Import/Export']");
        private static By LnkSubMenuPromotionSetup => By.XPath("//span[text()='Promotion Setup']");
        private static By DrpProfileName => By.XPath("//span[@class='profile-ava']//..//b[@class='caret']");
        private static By BtnLogout => By.XPath("//a[@id='appLogoutButton']");
        private static By DivSideBarClosed => By.XPath("//section[@class='sidebar-closed']");
        private static By Icon_Menu => By.XPath("//i[@class='icon_menu']");
        private static By BtnDashboardItem(string ItemName) => By.XPath("//div[contains(text(),'" + ItemName + "')]");
        private static By BtnDashboardSubItem(string SubItemName) => By.XPath("//div[normalize-space(text())='" + SubItemName + "']");
        private static By BtndashboardStatisticsArrowIcon => By.XPath("//div[@id='dashboardStatisticsToggle']//a[contains(@class,'dashboardStatisticsArrowIcon')]");
        private static By DivDashboardStatisticsTable => By.XPath("//div[@id='dashboardStatistics' and @class='panel-body panel-collapse in']");
        private static By BtndashboardNotification => By.XPath("//div[@href='#dashboardNotification']//h2[@class='pull-right']");
        private static By NotificationGrid => By.XPath("//div[@id='notificationGrid']");
        private static By BtnOrderSubItem(string SubMenuName) => By.XPath("//li[@class='sub-menu']//span[normalize-space(text())='" + SubMenuName + "']");


        /**Page actions: features(behavior) of the page the form of methods**/

        //Verify Dashboard page title
        public bool ValidateDashboardPageTitle()
        {
            //Get the page title and validate
            string DashboardPageTitle = GetPageTitle();

            if (DashboardPageTitle.Equals("ZEUS | Dashboard"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Click on Change button
        public void ClickOnChange()
        {
            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 30);

            ClickElementUsingJS(BtnChangeStore, "Change");

            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 30);
            Thread.Sleep(2000);
        }

        //Select a store from the modal window
        public void SelectStore(string StoreName)
        {
            //Click on store dropdown
            ClickElementUsingJS(DrpStoreSelect, "Select store dropdown");

            Thread.Sleep(2000);

            //Scroll to the option to select
            ScrollToElement(BtnStoreName(StoreName), StoreName);

            Thread.Sleep(1000);

            //Click on the option to select
            ClickElementUsingJS(BtnStoreName(StoreName), StoreName);

            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 15);

            WaitForElementToBeVisible(TopStoreDCName(StoreName), 30);
        }

        //Select a DC
        public void SelectDC(string DCName)
        {
            //Click on DC dropdown
            ClickElementUsingJS(DrpDCSelect, "Select DC dropdown");

            Thread.Sleep(2000);

            //Scroll to the option to select
            ScrollToElement(BtnDCName(DCName), DCName);

            Thread.Sleep(1000);

            //Click on the option to select
            ClickElementUsingJS(BtnDCName(DCName), DCName);

            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 15);

            WaitForElementToBeVisible(TopStoreDCName(DCName), 30);

        }

        //Select a store from the modal window
        public string SelectStoreRandomly()
        {
            //Click on store dropdown
            ClickElementUsingJS(DrpStoreSelect, "Select store dropdown");

            Thread.Sleep(4000);

            //Select a corporate store randomly.
            /*A corporate store range is between 7000 to 7999. data-offset-index should be between 0 to 196
            A franchise store range is between 0001 to 0999. data-offset-index should be between 367 to 470 */

            //Set a data-offset-index randomnly for corporate store.
            //Range should be in between should be between 0 to 196
            string DataOffsetIndex = RandomItemGenerator.RandomNumberGeneration(0, 196);
            //string StoreName = GetTextValue(BtnStoreNameByIndex(DataOffsetIndex), "Store Name");
            string StoreName = GetAttributeValue(BtnStoreNameByIndex(DataOffsetIndex), "Store Name", "innerText");

            //Scroll to the option to select
            ScrollToElement(BtnStoreNameByIndex(DataOffsetIndex), StoreName);

            Thread.Sleep(1000);

            //Click on the option to select
            ClickElementUsingJS(BtnStoreName(StoreName), StoreName);

            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 15);

            WaitForElementToBeVisible(TopStoreDCName(StoreName), 30);

            return StoreName;
        }

        //Get the store name that is selected from the dashboard page
        public string GetStoreNameSelected()
        {
            return GetTextValue(TxtStoreName, "Store Name");
        }

        //Do a logout operation
        public LoginPage Logout()
        {
            ClickElement(DrpProfileName, "Profile name");
            ClickElement(BtnLogout, "Logout");

            return new LoginPage();
        }

        //Click on side menubar option - Dashboard
        public DashboardPage OpenDashboardPage()
        {
            ClickElement(DrpSideBarMenu("Dashboard"), "Dashboard");

            return new DashboardPage();
        }

        public void OpenTheSidebarMenuIfInHiddenState()
        {
            //Check if Sidebar menu is hidden or not
            bool CurrState = IsElementVisible(DivSideBarClosed, 2);

            //If style doesn't have 0px that means the side nav bar menu is hidden, click on the menu icon
            if (CurrState == true)
            {
                //The Sidebar menu is hidden. Click on the icon_menu to open the drawer
                ClickElementUsingJS(Icon_Menu, "Icon Menu");
            }
        }

        //Click on Side menubar option - Setup
        public void OpenSetupMenu()
        {
            ClickElementUsingJS(DrpSideBarMenu("Setup"), "Setup");
            Thread.Sleep(2000);
        }

        public void OpenReportMenu()
        {
            ClickElementUsingJS(DrpSideBarMenu("Report"), "Report");
            Thread.Sleep(2000);
        }

        //Click on Side menubar option - Managed Care
        public void OpenManagedCareMenu()
        {
            ClickElementUsingJS(DrpSideBarMenu("Managed Care"), "Managed Care");
            Thread.Sleep(2000);
        }

        //Click on Sub item menu option - Import/Export
        public void OpenSubItemMenuImportExport()
        {
            ScrollToElement(LnkSubMenuImportExport, "Import/Export");
            ClickElementUsingJS(LnkSubMenuImportExport, "Import/Export");

            Thread.Sleep(2000);
        }

        //Click on Sub item menu option - Promotion Setup
        public void OpenSubItemMenuPromotionSetup()
        {
            ClickElementUsingJS(LnkSubMenuPromotionSetup, "Promotion Setup");

            Thread.Sleep(2000);
        }

        //Click on Side menubar option - InnovationLogs
        public void OpenInnovationLogsMenu()
        {
            ClickElementUsingJS(DrpSideBarMenu("Innovation Logs"), "Innovation Logs");
            Thread.Sleep(2000);
        }

        //Click on Side menubar option - JobTracking
        public void OpenJobTrackingMenu()
        {
            ClickElementUsingJS(DrpSideBarMenu("Job Tracking"), "Job Tracking");
            Thread.Sleep(2000);
        }

        public void ClickOnDashboardItem(string ItemName)
        {
            ClickElement(BtnDashboardItem(ItemName), ItemName);

            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 20);
        }

        public bool VerifyDashboardItemPresence(string ItemName)
        {
            ScrollToElement(BtnDashboardItem(ItemName), ItemName);

            return IsElementVisible(BtnDashboardItem(ItemName), 15);
        }

        public void ClickOnDashboardSubItem(string SubItemName)
        {
            ClickElement(BtnDashboardSubItem(SubItemName), SubItemName);

            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 20);
        }

        public bool VerifyDashboardSubItemPresence(string SubItemName)
        {
            return IsElementVisible(BtnDashboardSubItem(SubItemName), 15);
        }

        public void OpenStatisticsPanel()
        {
            ScrollToElement(BtndashboardStatisticsArrowIcon, "Statistic Panel");

            ClickElement(BtndashboardStatisticsArrowIcon, "Statistic Panel");

            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 20);
        }

        public bool VerifyStatisticsTableIsShown()
        {
            return IsElementVisible(DivDashboardStatisticsTable, 5);
        }

        public void ClickOnNotification()
        {
            ScrollToElement(BtndashboardNotification, "Notification");

            ClickElement(BtndashboardNotification, "Notification");

            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 20);
        }

        public bool NotificationPageShouldOpen()
        {
            return IsElementVisible(NotificationGrid, 5);
        }

        public void ClickOnSubMenuItems(string SubMenuName)
        {
            ClickElement(BtnOrderSubItem(SubMenuName), SubMenuName);

            WaitUntillElementToBeVisible(IconLoader, 5);
            WaitUntillElementToBeInvisible(IconLoader, 20);
        }

    }
}
