using System.IO.Compression;
using NUnit.Framework;

namespace SpecFlowNunitTestAutomation.Utils
{
    class FileReaderUtils
    {
        public static string ReadDataFromConfigFile(string key)
        {
            Dictionary<string, string> Configdata = new Dictionary<string, string>();
            string value = null;
            try
            {
                string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + @"\TestData\Config.txt";
                foreach (string data in File.ReadAllLines(filePath))
                {
                    Configdata.Add(data.Split('=')[0].ToLower().Trim(), data.Split('=')[1].ToUpper().TrimStart().TrimEnd());
                }
                value = Configdata[key.ToLower()];
                if (!String.IsNullOrEmpty(value))
                {
                    return value;
                }
                else
                {
                    throw new Exception("Provide Valid Key Property from Config to get the value");
                }
            }
            catch (FileNotFoundException e)
            {
                Assert.Fail("Exception in File Reading. " + e.InnerException);
            }
            return null;
        }

        public static string ReadDataFromTxtfile(string path, string key)
        {
            Dictionary<string, string> Configdata = new Dictionary<string, string>();
            try
            {
                string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + path;
                foreach (string data in File.ReadAllLines(filePath))
                {
                    //var s = "INAGX4 Agatti Island";
                    var firstSpaceIndex = data.IndexOf(" ");
                    var firstString = data.Substring(0, firstSpaceIndex).ToUpper().Trim(); // INAGX4
                    var secondString = data.Substring(firstSpaceIndex + 1).Trim(); // Agatti Island

                    if (!Configdata.ContainsKey(firstString))
                    {
                        Configdata.Add(firstString, secondString);
                    }
                }

                string value = Configdata[key.ToUpper()];
                if (!String.IsNullOrEmpty(value))
                {
                    return value;
                }
                else
                {
                    throw new Exception("Provide Valid Key Property from Config to get the value");
                }
            }
            catch (FileNotFoundException e)
            {
                Assert.Fail("Exception in File Reading. " + e.InnerException);
            }
            return null;
        }

        public static bool VerifyDataPresenceInTxtfile(string path, string StringToVerify)
        {
            try
            {
                string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + path;
                foreach (string data in File.ReadAllLines(filePath))
                    if (data.Contains(StringToVerify))
                    {
                        return true;
                    }
            }
            catch (FileNotFoundException e)
            {
                Assert.Fail("Exception in File Reading. " + e.InnerException);
            }
            return false;
        }

        public static void ClearFolder(string Filepath)
        {
            DirectoryInfo di = new DirectoryInfo(Filepath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public static void ExtractFile(string zipPath, string extractPath)
        {
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }
    }
}
