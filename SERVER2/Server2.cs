// Server code (Console App)
using System;
using System.Net;
using Serilog;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using SERVER2;
using DOMAIN.Domain;
using SERVER.src.Repo;
using SERVER.src.Service;
using System.Collections.Generic;


public class Server2
{
    static SqlConnection connection;
    static AdminRepository admRepo;
    static public AdminService adminService;

    static CursaRepository cursaRepo;
    static public CursaService cursaSrv;

    static ParticipantRepository partRepo;
    static public ParticipantService partSrv;

    static public HashSet<Admin> admini_logati = new HashSet<Admin>();

    static public List<ServerWorker> serverWorkers = new List<ServerWorker>();

    static void Main()
    {
        string relativePath = Path.Combine("..", "..", "..", "src", "logs", "app.log");
        string absolutePath = Path.GetFullPath(relativePath);
        

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(absolutePath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Application started");
        Log.Logger.Information("ASDASSD");

        try
        {
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", ".."));
            Console.WriteLine(path);
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..")))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            connection = new SqlConnection(connectionString);

            admRepo = new AdminRepository(connection);
            adminService = new AdminService(admRepo);

            cursaRepo = new CursaRepository(connection);
            cursaSrv = new CursaService(cursaRepo);

            partRepo = new ParticipantRepository(connection);
            partSrv = new ParticipantService(partRepo);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            Console.WriteLine(e.Message);
        }


        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Server started on port 5000...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            ServerWorker serverWorker = new ServerWorker(client);
            serverWorkers.Add(serverWorker);
            Thread clientThread = new Thread(() => serverWorker.run());
            clientThread.Start();
        }
    }

    static public void broadcast(string message)
    {
        foreach (ServerWorker worker in serverWorkers)
        {
            worker.sendMessage(message);
        }
    }

}