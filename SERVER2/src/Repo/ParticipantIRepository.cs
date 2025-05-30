using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOMAIN.Domain;
namespace SERVER.src.Repo
{
    public interface ParticipantIRepository : Repository<string, Participant>
    {
        List<Participant> getParticipantiByEchipa(string echipa);
    }
}
