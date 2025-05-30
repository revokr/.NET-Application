using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DOMAIN.Domain;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Net.Sockets;
using Proiect_MPP.UI;
using Proiect_MPP.src.Network;

namespace Proiect_MPP
{
    public partial class MainForm : Form
    {
        private SqlConnection connection;
        private ILogger<Program> logger;
        private MainWindowController mainWindowController = new MainWindowController();

        private string adminEmail;

        public MainForm(string email)
        {
            InitializeComponent();

            Proxy.Instance().SetController(mainWindowController);

            adminEmail = email;
            this.Text = adminEmail;

            string relativePath = Path.Combine("..", "..", "src", "logs", "app.log");
            string absolutePath = Path.GetFullPath(relativePath); // Resolves to correct full path
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(absolutePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog())
                .BuildServiceProvider();

            logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            loadCurse();

            CCSBox.Items.Add("1000cc");
            CCSBox.Items.Add("700cc");
            CCSBox.Items.Add("500cc");
            CCSBox.Items.Add("250cc");

            mainWindowController.onRefreshRequested += () =>
            {
                this.Invoke(new Action(() =>
                {
                    if (echipaSrchTB.Text != "")
                    {
                        DataTable dtParticipanti = mainWindowController.getDTParticipanti(echipaSrchTB.Text);

                        dataGridView2.DataSource = dtParticipanti;
                    }
                    else
                    {
                        MessageBox.Show("Type a team first!");
                    }
                }));
            };
        }

        private void loadCurse()
        {
            DataTable dtCurse = mainWindowController.getDTCurse();

            dataGridView1.DataSource = dtCurse;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if (echipaSrchTB.Text != "")
            {
                DataTable dtParticipanti = mainWindowController.getDTParticipanti(echipaSrchTB.Text);

                dataGridView2.DataSource = dtParticipanti;
            } else
            {
                MessageBox.Show("Type a team first!");
            }
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            string id = IDBox.Text;
            string cc = (string)CCSBox.SelectedItem;
            string nume = NumeBox.Text;
            string cnp = CNPBox.Text;
            string echipa = EchipaBox.Text;
            string cursa = CursaBox.Text;
            if (id != "" && cc != "" && nume != "" && cnp != "" && echipa != "" && cursa != "")
            {
                mainWindowController.saveParticipant(id, nume, cc, cnp, echipa, cursa);

                IDBox.Clear();
                NumeBox.Clear();
                CNPBox.Clear();
                EchipaBox.Clear();
                CursaBox.Clear();
            }
            else
            {
                MessageBox.Show("Please Fill all the attributes!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
