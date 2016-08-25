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
    public class GetAlldataDAL
    {

        static string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public DateTime GeLastRunDate()
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            DateTime lastrundt = new DateTime();
            try
            {
                SqlCommand cmd = new SqlCommand  ("select isnull(MAX(lastRundate),0) from DataLakeLog where [IsComplete] =1", conn);
                lastrundt = Convert.ToDateTime(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lastrundt;
        }

    }
}
