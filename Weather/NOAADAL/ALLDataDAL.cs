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
    public class ALLDataDAL
    {

        static string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public void InsertAllData( List <ALLData> data)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            foreach (var d in data)
            {
                SqlCommand cmd = new SqlCommand("SpInsertAllData", conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@date", d.date);
                    cmd.Parameters.AddWithValue("@datatype", d.datatype);
                    cmd.Parameters.AddWithValue("@stations", d.stations);
                    cmd.Parameters.AddWithValue("@attributes", d.attributes);
                    cmd.Parameters.AddWithValue("@value", d.value);                    
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }
    }
}
