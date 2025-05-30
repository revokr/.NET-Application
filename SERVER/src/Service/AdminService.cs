using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SERVER.src.Repo;
using DOMAIN.Domain;
using Serilog;

namespace SERVER.src.Service
{
    public class AdminService
    {
        private AdminRepository admRepo;

        public AdminService(AdminRepository repo)
        {
            admRepo = repo;
        }

        public Admin validateAdmin(string email, string parola)
        {
            Admin adm = null;
            try
            {
                adm = admRepo.findOne(email);
            } catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }

            if (adm.Parola == parola)
            {
                return adm;
            }

            return null;
        }
    }
}
