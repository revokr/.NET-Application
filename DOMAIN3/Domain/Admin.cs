using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Domain
{
     public class Admin
    {
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string parola;
        public string Parola
        {
            get { return parola; }
            set { parola = value; }
        }

        public Admin(string email, string parola)
        {
            this.email = email;
            this.parola = parola;
        }

    }
}
