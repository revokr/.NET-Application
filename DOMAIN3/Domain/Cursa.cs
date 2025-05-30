using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Domain
{
    public class Cursa
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

        private uint nrParticipantiPosibili;
        public uint NrParticipantiPosibili
        {
            get { return nrParticipantiPosibili; }
            set { nrParticipantiPosibili = value; }
        }

        public List<string> participanti = new List<string>();
        public List<string> Participanti
        {
            get { return participanti; }
            set { participanti = value; }
        }

        public Cursa(string id, string nume, string capacitateMotor, uint nrParticipantiPosibili, List<string> part)
        {
            this.id = id;
            this.nume = nume;
            this.capacitateMotor = capacitateMotor;
            this.nrParticipantiPosibili = nrParticipantiPosibili;
            this.participanti = part;
        }
    }
}
