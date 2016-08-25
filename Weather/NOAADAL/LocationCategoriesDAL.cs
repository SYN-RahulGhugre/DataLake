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
    public class LocationCategoriesDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public void InsertLocationcategories(List<LocationCategories> loc_categories)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            try
            {
                foreach (var l in loc_categories)
                {
                    SqlCommand cmd = new SqlCommand("SpInsertLocationcategories", conn); 
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", l.id);
                        cmd.Parameters.AddWithValue("@name", l.name);
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
