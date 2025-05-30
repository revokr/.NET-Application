using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using DOMAIN.Domain;
using SERVER;
using SERVER.src.Service;
using Azure;
namespace SERVER
{
    public class ServerWorker
    {
        private TcpClient client;
        private NetworkStream stream;

        public ServerWorker(TcpClient str)
        {
            this.client = str;
            this.stream = this.client.GetStream();
            Console.WriteLine("Client Connected!");
        }

        public void run()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int byteCount;

                while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    Console.WriteLine($"Received: {request}");

                    if (request.StartsWith("LOGIN"))
                    {
                        string email = request.Split(" ")[1];
                        string password = request.Split(" ")[2];
                        Console.WriteLine("Email: " + email + ", password: " + password);

                        Admin adm = Server.adminService.validateAdmin(email, password);

                        if (adm == null || (adm != null && Server.admini_logati.Contains(adm)))
                        {
                            // Echo back the message
                            string response = "FAILED";
                            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }
                        else
                        {
                            // Echo back the message
                            string response = "SUCCESS";
                            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                            stream.Write(responseBytes, 0, responseBytes.Length);
                            Server.admini_logati.Add(adm);
                        }
                    }
                    else if (request.StartsWith("GET_RACES"))
                    {
                        List<Cursa> curse = Server.cursaSrv.getCurse();

                        string serialized = JsonConvert.SerializeObject(curse);

                        byte[] jsonData = Encoding.UTF8.GetBytes(serialized);
                        stream.Write(jsonData, 0, jsonData.Length);
                    }
                    else if (request.StartsWith("GET_RACERS"))
                    {
                        string echipa = request.Split(" ")[1];
                        List<Participant> participanti = Server.partSrv.getParticipantiByEchipa(echipa);

                        string serialized = JsonConvert.SerializeObject(participanti);
                        byte[] jsonData = Encoding.UTF8.GetBytes(serialized);
                        stream.Write(jsonData, 0, jsonData.Length);
                    }
                    else if (request.StartsWith("SAVE_RACER"))
                    {
                        var splitted = request.Split(" ");
                        Participant part = new Participant(splitted[1], splitted[2], splitted[3], splitted[4], splitted[5]);
                        var res = Server.partSrv.save(part);
                        Server.cursaSrv.saveParticipantCursa(splitted[6], part.ID);

                        if (res == null)
                        {
                            string response = "FAILED";
                            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }
                        else if (res != null)
                        {
                            string response = "SUCCESS";
                            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                            stream.Write(responseBytes, 0, responseBytes.Length);
                            Server.broadcast("REFRESH_RACERS");
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Client disconnected (IO): {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Cleaning up client connection...");

                try { stream?.Close(); } catch { }
                try { client?.Close(); } catch { }
            }
        }

        public void sendMessage(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            stream.Write(messageBytes, 0, messageBytes.Length);
        }
    }
}
