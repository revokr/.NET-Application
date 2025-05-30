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
using Serilog;

namespace SERVER.src.Repo
{
    public class ParticipantRepository : ParticipantIRepository
    {
        private SqlConnection connection;

        public ParticipantRepository(SqlConnection conn)
        {
            connection = conn;
        }

        public Participant findOne(string id)
        {
            return null;
        }

        public Participant findByName(string name)
        {
            return null;
        }

        public List<Participant> findAll()
        {
            return null;
        }

        public Participant save(Participant entity)
        {
            string sql = "INSERT INTO participant VALUES(@id, @nume, @cnp, @cc, @echipa)";

            List<String> participanti = new List<string>();
            int res = -1;
            connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", entity.ID);
                cmd.Parameters.AddWithValue("@nume", entity.Nume);
                cmd.Parameters.AddWithValue("@cnp", entity.CNP);
                cmd.Parameters.AddWithValue("@cc", entity.CapacitateMotor);
                cmd.Parameters.AddWithValue("@echipa", entity.echipa);

                res = cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            connection.Close();

            if (res == -1)
            {
                return null;
            }

            return entity;
        }

        public Participant delete(string id)
        {
            return null;
        }

        public Participant update(Participant entity)
        {
            return null;
        }

        public List<Participant> getParticipantiByEchipa(string echipa)
        {
            string sql = "SELECT * FROM participant WHERE echipa = @echipa"; // Use parameterized query to prevent SQL injection

            List<Participant> participanti = new List<Participant>();
            connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@echipa", echipa);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Check if there's a result
                    {
                        string id = reader.GetString(reader.GetOrdinal("id"));
                        string nume = reader.GetString(reader.GetOrdinal("nume"));
                        string cnp = reader.GetString(reader.GetOrdinal("cnp"));
                        string capMotor = reader.GetString(reader.GetOrdinal("capacitatemotor"));

                        participanti.Add(new Participant(id, nume, cnp, capMotor, echipa));
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            connection.Close();

            return participanti;
        }
    }
}
