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
    public class LocationsDAL
    {

        static string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public void InsertLocations(List<Locations> locations)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            foreach (var l in locations)
            {
                SqlCommand cmd = new SqlCommand("SpInsertLocations", conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mindate", l.mindate);
                    cmd.Parameters.AddWithValue("@maxdate", l.maxdate);
                    cmd.Parameters.AddWithValue("@name", l.name);
                    cmd.Parameters.AddWithValue("@datacoverage", l.datacoverage);
                    cmd.Parameters.AddWithValue("@id", l.id);
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
