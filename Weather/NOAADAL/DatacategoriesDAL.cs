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
    public class DatacategoriesDAL
    {

        static string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public void InsertDataCategories(List<Datacategories> categories)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            try
            {
                foreach (var d in categories)
                {
                    SqlCommand cmd = new SqlCommand("SpInsertDatacategories", conn);
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", d.id);
                        cmd.Parameters.AddWithValue("@name", d.name);
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
