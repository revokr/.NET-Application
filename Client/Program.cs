using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proiect_MPP.src.Network;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proiect_MPP.src.Network;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;

namespace Proiect_MPP
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string relativePath = Path.Combine("..", "..", "src", "logs", "app.log");
            string absolutePath = Path.GetFullPath(relativePath); // Resolves to correct full path
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(absolutePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog())
                .BuildServiceProvider();

            ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            Proxy.Instance("127.0.0.1", 5000);
            Proxy.Instance().StartReader();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
            Log.CloseAndFlush();
        }
    }
}
