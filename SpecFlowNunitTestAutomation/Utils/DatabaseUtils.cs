using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpecFlowNunitTestAutomation.Hooks;

namespace SpecFlowNunitTestAutomation.Utils
{
    public class DatabaseUtils
    {
        private string? connetionString;
        SqlConnection? cnn;
        public SqlCommand? command;
        SqlDataAdapter adapter = new();

        public void OpenConnection(string server, string database, string username, string password)
        {
            try
            {
                connetionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password}";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                ReporterClass.AddStepLog("----->DB Connection Established!");
            }
            catch (SqlException ex)
            {
                ReporterClass.AddFailedStepLog("----->Cannot connect to database server. Connection not established: " + ex.Message);
            }
        }

        //Used to execute UPDATE command that will not return any data
        public void ExecuteUpdateQuery(string sql)
        {
            try
            {
                command = new SqlCommand(sql, cnn);

                adapter.UpdateCommand = new SqlCommand(sql, cnn);
                adapter.UpdateCommand.ExecuteNonQuery();

                ReporterClass.AddStepLog("----->UPDATE query executed.");
            }
            catch (SqlException ex)
            {
                ReporterClass.AddFailedStepLog("----->Update query not executed sucessfully: "+ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                //Close connection
                cnn.Close();

                ReporterClass.AddStepLog("----->DB connection closed successfully!");
            }
            catch (SqlException ex)
            {
                ReporterClass.AddFailedStepLog("----->DB Connection not closed sucessfully: " + ex.Message);
            }
        }
    }

}
