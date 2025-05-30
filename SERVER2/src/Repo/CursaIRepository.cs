using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOMAIN.Domain;
namespace SERVER.src.Repo
{
    public interface CursaIRepository : Repository<string, Cursa>
    {
        List<string> findParticipantiCursa(SqlConnection con, string id);

        string saveParticipantCursa(string part_id, string cursa_id);
    }
}
