using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUnit.Framework;

namespace SpecFlowNunitTestAutomation.Utils
{
    class ExcelUtils
    {
        static FileStream fileStream;
        static IWorkbook workBook;

        public static string ReadDataFromExcel(String columnName)
        {
            string pathOfExcelFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + @"\TestData\TestData.xls";
            string testData;

            //open file and returns as Stream
            /**(FileShare.Read) this is given to make the excel file readonly 
            i.e. it wont affect during parallel execution**/
            fileStream = new FileStream(pathOfExcelFile, FileMode.Open, FileAccess.Read, FileShare.Read);
           
            string extension = pathOfExcelFile.Split(".")[1].Trim();
            if (extension.Equals("xls"))
            {
                workBook = new HSSFWorkbook(fileStream);
            }
            else
            {
                workBook = new XSSFWorkbook(fileStream);
            }
            //ISheet sheet = workBook.GetSheet(FileReaderUtils.ReadDataFromConfigFile("Environment"));

            //To read params from test.runsetting file
            ISheet sheet = workBook.GetSheet(TestContext.Parameters["Environment"].ToString());
            IRow rowobj;
            int cell = -1;
            //  int row = -1;
            rowobj = sheet.GetRow(0);
            int Columncount = rowobj.PhysicalNumberOfCells;
            for (int i = 0; i <= Columncount - 1; i++)
            {
                if (rowobj.GetCell(i).ToString().Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    cell = i;
                    break;
                }
            }
            rowobj = sheet.GetRow(1);
            testData = rowobj.GetCell(cell).ToString().Trim();
            return testData;
        }

        public static string ReadDataFromExcel(String columnName, int rowNum)
        {
            string pathOfExcelFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + @"\TestData\TestData.xls";
            string testData;

            //open file and returns as Stream
            /**(FileShare.Read) this is given to make the excel file readonly 
            i.e. it wont affect during parallel execution**/
            fileStream = new FileStream(pathOfExcelFile, FileMode.Open, FileAccess.Read, FileShare.Read);

            string extension = pathOfExcelFile.Split(".")[1].Trim();
            if (extension.Equals("xls"))
            {
                workBook = new HSSFWorkbook(fileStream);
            }
            else
            {
                workBook = new XSSFWorkbook(fileStream);
            }
            //ISheet sheet = workBook.GetSheet(FileReaderUtils.ReadDataFromConfigFile("Environment"));

            //To read params from test.runsetting file
            ISheet sheet = workBook.GetSheet(TestContext.Parameters["Environment"].ToString());
            IRow rowobj;
            int cell = -1;
            //  int row = -1;
            rowobj = sheet.GetRow(0);
            int Columncount = rowobj.PhysicalNumberOfCells;
            for (int i = 0; i <= Columncount - 1; i++)
            {
                if (rowobj.GetCell(i).ToString().Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    cell = i;
                    break;
                }
            }
            rowobj = sheet.GetRow(rowNum);
            testData = rowobj.GetCell(cell).ToString().Trim();
            return testData;
        }

        public static string ReadDataFromExcel(string rowName, string columnName)
        {
            string pathOfExcelFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + @"\TestData\ScenarioLevelTestData.xls";   
            string testData = "";

            //open file and returns as Stream
            /**(FileShare.Read) this is given to make the excel file readonly 
            i.e. it wont affect during parallel execution**/
            fileStream = new FileStream(pathOfExcelFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            
            string extension = pathOfExcelFile.Split(".")[1].Trim();
            if (extension.Equals("xls"))
            {
                workBook = new HSSFWorkbook(fileStream);
            }
            else
            {
                workBook = new XSSFWorkbook(fileStream);
            }
            //ISheet sheet = workBook.GetSheet(FileReaderUtils.ReadDataFromConfigFile("Environment"));
            ISheet sheet = workBook.GetSheet(TestContext.Parameters["Environment"].ToString());
            IRow rowobj;
            int cell = -1;
            int row = -1;
            int rowCount = sheet.PhysicalNumberOfRows;
            for (int j = 0; j <= rowCount - 1; j++)
            {
                if (sheet.GetRow(j).GetCell(0).ToString().Trim().Equals(rowName, StringComparison.OrdinalIgnoreCase))
                {
                    row = j;
                    break;
                }
            }
            rowobj = sheet.GetRow(0);
            int Columncount = rowobj.PhysicalNumberOfCells;
            for (int i = 0; i <= Columncount - 1; i++)
            {
                if (rowobj.GetCell(i).ToString().Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    cell = i;
                    break;
                }
            }
            rowobj = sheet.GetRow(row);
            if (!String.IsNullOrEmpty(rowobj.GetCell(cell).ToString()))
            {
                testData = rowobj.GetCell(cell).ToString().Trim();
            }
            return testData;
        }
    }
}
