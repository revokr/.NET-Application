using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DOMAIN.Domain;
using SERVER.src.Repo;
using Serilog;

namespace SERVER.src.Service
{
    public class CursaService
    {
        private CursaRepository cursaRepo;

        public CursaService(CursaRepository repo)
        {
            cursaRepo = repo;
        }

        public List<Cursa> getCurse()
        {
            List<Cursa> curse = new List<Cursa>();
            try
            {
                curse = cursaRepo.findAll();
            } catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return curse;
        }

        public string saveParticipantCursa(string cursa_id, string part_id)
        {
            string res = "";
            try
            {
                res = cursaRepo.saveParticipantCursa(cursa_id, part_id);
            } catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return res;
        }
    }
}
