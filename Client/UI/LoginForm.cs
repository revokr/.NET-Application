using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Windows.Forms;
using Serilog;
using Microsoft.Data.SqlClient;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Proiect_MPP.src.Network;

using DOMAIN.Domain;

namespace Proiect_MPP
{
    public partial class LoginForm : Form
    {

        private SqlConnection connection;
        private ILogger<Program> logger;

        public LoginForm()
        {
            string relativePath = Path.Combine("..", "..", "src", "logs", "app.log");
            string absolutePath = Path.GetFullPath(relativePath); // Resolves to correct full path
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(absolutePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog())
                .BuildServiceProvider();

            logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //byte[] data = Encoding.UTF8.GetBytes("LOGIN " + emailTextBox.Text + " " + passwordTextBox.Text);
            //stream.Write(data, 0, data.Length);
            Proxy.Instance().SendRequest("LOGIN " + emailTextBox.Text + " " + passwordTextBox.Text);

            //byte[] responseBuffer = new byte[1024];
            //int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
            string response = Proxy.Instance().ReadResponse();
            //MessageBox.Show(response);

            if (response == "SUCCESS")
            {
                MainForm mainForm = new MainForm(emailTextBox.Text);
                mainForm.Show();
                this.Hide();
            } else
            {
                MessageBox.Show("LOGIN FAILED!");
                logger.LogError("LOGIN FAILED!");
            }
        }

    }
}
