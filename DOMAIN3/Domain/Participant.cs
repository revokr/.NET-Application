using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Domain
{
    public class Participant
    {
        private string id;
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private string nume;
        public string Nume
        {
            get { return nume; }
            set { nume = value; }
        }

        private string capacitateMotor;
        public string CapacitateMotor
        {
            get { return capacitateMotor; }
            set { capacitateMotor = value; }
        }

        private string cnp;
        public string CNP
        {
            get { return cnp; }
            set { cnp = value; }
        }

        public string echipa;
        public string Echipa
        {
            get { return echipa; }
            set { echipa = value; }
        }

        public Participant(string id, string name, string CNP, string capacitateMotor, string echipa)
        {
            this.id = id;
            this.nume = name;
            this.CNP = CNP;
            this.capacitateMotor = capacitateMotor;
            this.echipa = echipa;
        }
    }
}
