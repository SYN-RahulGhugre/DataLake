using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NOAABO;

namespace NOAADAL
{
    public class DatalakeLogDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public void insertDataLog(DataLakeLog datalog)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SpInsertDataLakeLog", conn);
            try
            {
                cmd.CommandType  = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dataset", datalog.dataset);
                cmd.Parameters.AddWithValue("@datsetUrl", datalog.datsetUrl);
                cmd.Parameters.AddWithValue("@lastrundate", datalog.lastrundate);
                cmd.Parameters.AddWithValue("@createddate", datalog.createddate);
                cmd.Parameters.AddWithValue("@iscomplete", datalog.iscomplete);
                cmd.Parameters.AddWithValue("@DataLakeLogDescription", datalog.DataLakeLogDescription);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

    }
}
