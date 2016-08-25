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
    public class DatasetDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //SqlConnection con = new
        // SqlConnection(ConfigurationManager.ConnectionStrings["Myconstr"].ToString());

        public void InsertDataset(List<Datasets> datsets)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            //SqlCommand dCmd = new SqlCommand("InsertDataset", conn);          
            try
            {
                foreach (var c in datsets)
                {
                    SqlCommand dCmd = new SqlCommand("SpInsertDataset", conn);
                    try
                    {
                        dCmd.CommandType = CommandType.StoredProcedure;
                        dCmd.Parameters.AddWithValue("@uid", c.uid);
                        dCmd.Parameters.AddWithValue("@mindate", c.mindate);
                        dCmd.Parameters.AddWithValue("@maxdate", c.maxdate);
                        dCmd.Parameters.AddWithValue("@name", c.name);
                        //dCmd.Parameters.AddWithValue("@datacovarage",datsets[0].datacoverage);
                        dCmd.Parameters.AddWithValue("@datacoverage", c.datacoverage);
                        dCmd.Parameters.AddWithValue("@id", c.id);
                        dCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        dCmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }       
    }
}
