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
    public class StationsDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public void Insertstations(List<Stations> stations)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            foreach (var s in stations)
            {
                SqlCommand cmd = new SqlCommand("SpInsertstations", conn);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@elevation", s.elevation);
                    cmd.Parameters.AddWithValue("@mindate", s.mindate);
                    cmd.Parameters.AddWithValue("@maxdate", s.maxdate);
                    cmd.Parameters.AddWithValue("@latitude", s.latitude);
                    cmd.Parameters.AddWithValue("@name", s.name);
                    cmd.Parameters.AddWithValue("@datacoverage", s.datacoverage);
                    cmd.Parameters.AddWithValue("@id", s.id);
                    cmd.Parameters.AddWithValue("@elevationUnit", s.elevationUnit);
                    cmd.Parameters.AddWithValue("@longitude", s.longitude);
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
