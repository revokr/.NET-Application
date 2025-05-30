using DOMAIN.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using SERVER;
using Serilog;

namespace SERVER.src.Repo
{
    public class AdminRepository : AdminIRepository
    {

        private SqlConnection connection;
        public AdminRepository(SqlConnection conn)
        {
            connection = conn;

            Log.Logger.Information("BAAAAA");
        }

        public Admin findOne(string id)
        {
            string sql = "SELECT * FROM admin WHERE email = @id"; // Use parameterized query to prevent SQL injection
            
            connection.Open();
            try 
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id); // Safe parameter binding

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Check if there's a result
                    {
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        string parola = reader.GetString(reader.GetOrdinal("parola"));

                        return new Admin(email, parola);
                    }
                }
            } catch (Exception e)
            {
                Log.Error(e.Message);
            }
            connection.Close();

            return null; 
        }

        public Admin findByName(string name)
        {
            return null;
        }

        public List<Admin> findAll()
        {
            string sql = "SELECT * FROM admin"; // Use parameterized query to prevent SQL injection

            try
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                List<Admin> admini = new List<Admin>();
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        string parola = reader.GetString(reader.GetOrdinal("parola"));

                        admini.Add(new Admin(email, parola));
                    }
                }
                connection.Close();
                return admini;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return new List<Admin>();
        }

        public Admin save(Admin entity)
        {
            return null;
        }

        public Admin delete(string id)
        {
            return null;
        }

        public Admin update(Admin entity)
        {
            return null;
        }
    }
}
