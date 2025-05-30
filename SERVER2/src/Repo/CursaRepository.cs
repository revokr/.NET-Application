using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using DOMAIN.Domain;
using System.Drawing;
using Serilog;

namespace SERVER.src.Repo
{
    public class CursaRepository : CursaIRepository
    {   
        private SqlConnection connection;
        public CursaRepository(SqlConnection conn)
        {
            connection = conn;
        }

        public Cursa findOne(string id)
        {
            return null;
        }

        public Cursa findByName(string name)
        {
            return null;
        }

        public List<Cursa> findAll()
        {
            string sql = "SELECT * FROM cursa ORDER BY capacitatemotor"; // Use parameterized query to prevent SQL injection

            List<Cursa> curse = new List<Cursa>();
            try
            {
                //connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Check if there's a result
                    {
                        string id = reader.GetString(reader.GetOrdinal("id"));
                        string denumire = reader.GetString(reader.GetOrdinal("nume"));
                        string capMotor = reader.GetString(reader.GetOrdinal("capacitatemotor"));
                        int maxPart = reader.GetInt32(reader.GetOrdinal("maxparticipanti"));

                        List<string> participanti = findParticipantiCursa(connection, id);
                        curse.Add(new Cursa(id, denumire, capMotor, (uint)maxPart, participanti));
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            connection.Close();

            return curse;
        }

        public Cursa save(Cursa entity)
        {
            return null;
        }

        public Cursa delete(string id)
        {
            return null;
        }

        public Cursa update(Cursa entity)
        {
            return null;
        }

        public List<string> findParticipantiCursa(SqlConnection con, string id)
        {
            string sql = "SELECT * FROM curse_participanti WHERE cursa_id = @id";
            
            List<String> participanti = new List<string>();
            try {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id); // Safe parameter binding

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Check if there's a result
                    {
                        string cursa = reader.GetString(reader.GetOrdinal("cursa_id"));
                        string part = reader.GetString(reader.GetOrdinal("participant_id"));

                        participanti.Add(part);
                    }
                }

            } catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return participanti;
        }

        public string saveParticipantCursa(string id_cursa, string id_participant)
        {
            string sql = "INSERT INTO curse_participanti(cursa_id, participant_id) VALUES(@cursaid, @partid)";

            int res = -1;
            connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@cursaid", id_cursa);
                cmd.Parameters.AddWithValue("@partid", id_participant);

                res = cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            connection.Close();

            if (res == -1)
            {
                return "";
            }

            return id_participant;
        }
    }
}
