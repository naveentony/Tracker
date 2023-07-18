using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.ImportData.Managers
{
    public static class DBHelper
    {
        #region Members

        private static int commandTimeout = 300;

       // private static readonly string connectionString = "Data Source=RICKY;Initial Catalog=WheelTrack;Integrated Security=True;";//ConfigurationManager.AppSettings[name: "DbConnString"].ToString();
       private static readonly string connectionString = "Data Source=RICKY;Initial Catalog=iTrackTest;Integrated Security=True;";
        #endregion

        #region Public Methods


        public static DataSet ExecuteStoredProcedureDS(string resultDataSet, string spName, List<SqlParameter>? parameters = null)
        {
            DataSet result = new DataSet();
            if (!string.IsNullOrEmpty(resultDataSet))
                result = new DataSet(resultDataSet);
            else
                result = new DataSet();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(spName, sqlConn))
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                sqlCommand.Parameters.Add(param);
                            }
                        }
                        using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                        {
                            sqlConn.Open();
                            adapter.Fill(result);
                            return result;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static void ExecuteStoredProcedure(string spName, List<SqlParameter> parameters)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(spName, sqlConn))
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                sqlCommand.Parameters.Add(param);
                            }
                        }
                        sqlConn.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public static DataSet Executetext(string text)
        {
            DataSet result = new DataSet();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(text, sqlConn))
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                        sqlCommand.CommandType = CommandType.Text;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                        {
                            sqlConn.Open();
                            adapter.Fill(result);
                            return result;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
   

}
