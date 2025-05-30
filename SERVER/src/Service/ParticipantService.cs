using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using DOMAIN.Domain;
using SERVER.src.Repo;

namespace SERVER.src.Service
{
    public class ParticipantService
    {
        private ParticipantRepository partRepo;

        public ParticipantService(ParticipantRepository repo)
        {
            partRepo = repo;
        }

        public List<Participant> getParticipantiByEchipa(string echipa)
        {
            List<Participant> participanti = new List<Participant>();
            try
            {
                participanti = partRepo.getParticipantiByEchipa(echipa);
            } catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return participanti;
        }

        public Participant save(Participant part)
        {
            Participant res = null;
            try
            {
                res = partRepo.save(part);
          
            } catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return res;
        }
    }
}
