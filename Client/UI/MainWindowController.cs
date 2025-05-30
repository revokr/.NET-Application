using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Proiect_MPP.src.Network;
using DOMAIN.Domain;
namespace Proiect_MPP.UI
{
    public class MainWindowController
    {
        private Proxy proxy = Proxy.Instance();

        public event Action onRefreshRequested;

        public MainWindowController()
        {

        }

        public DataTable getDTCurse()
        {
            proxy.SendRequest("GET_RACES");

            string jsonData = proxy.ReadResponse();
            List<Cursa> curse = JsonConvert.DeserializeObject<List<Cursa>>(jsonData);

            return curseToDataTable(curse);
        }

        private DataTable curseToDataTable(List<Cursa> curse)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Denumire", typeof(string));
            dt.Columns.Add("Cap Motor (CCs)", typeof(string));
            dt.Columns.Add("Max Participanti", typeof(uint));
            dt.Columns.Add("Participanti Curenti", typeof(uint));

            foreach (var cursa in curse)
            {
                dt.Rows.Add(cursa.ID, cursa.Nume, cursa.CapacitateMotor, cursa.NrParticipantiPosibili, cursa.Participanti.Count);
            }

            return dt;
        }

        public DataTable getDTParticipanti(string echipa)
        {
            proxy.SendRequest("GET_RACERS " + echipa);
            string jsonData = proxy.ReadResponse();

            List<Participant> participanti = JsonConvert.DeserializeObject<List<Participant>>(jsonData);
            return participantiToDataTable(participanti);
        }

        private DataTable participantiToDataTable(List<Participant> participanti)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Nume", typeof(string));
            dt.Columns.Add("CNP", typeof(string));
            dt.Columns.Add("CC", typeof(string));
            dt.Columns.Add("Echipa", typeof(string));

            foreach (var participant in participanti)
            {
                dt.Rows.Add(participant.ID, participant.Nume, participant.CNP, participant.CapacitateMotor, participant.Echipa);
            }

            return dt;
        }

        public void saveParticipant(string id, string nume, string cc, string cnp, string echipa, string cursa)
        {
            proxy.SendRequest($"SAVE_RACER {id} {nume} {cnp} {cc} {echipa} {cursa}");
            string response = proxy.ReadResponse();

            MessageBox.Show(response);
        }

        public void notifyRefresh()
        {
            onRefreshRequested?.Invoke();
        }
    }
}
