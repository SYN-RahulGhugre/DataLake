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
    public class DataTypesDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public void InsertDatatype(List<DataTypes> datatypes)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            foreach (var d in datatypes)
            {
                SqlCommand cmd = new SqlCommand("SpInsertDataTypes", conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mindate", d.mindate);
                    cmd.Parameters.AddWithValue("@maxdate", d.maxdate);
                    cmd.Parameters.AddWithValue("@name", d.name);
                    cmd.Parameters.AddWithValue("@datacoverage", d.datacoverage);
                    cmd.Parameters.AddWithValue("@id", d.id);
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
