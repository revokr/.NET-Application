using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using DOMAIN.Domain;
using SERVER2;
using SERVER.src.Service;
using Azure;

namespace SERVER2
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
                    string request = Encoding.UTF8.GetString(buffer, 0, byteCount).Trim();
                    Console.WriteLine($"Received: {request}");

                    if (request != "")
                    {
                        if (request.StartsWith("LOGIN"))
                        {
                            string email = request.Split(' ')[1];
                            string password = request.Split(' ')[2];
                            Console.WriteLine("Email: " + email + ", password: " + password);

                            Admin adm = Server2.adminService.validateAdmin(email, password);

                            //bool is_active = Server2.admini_logati.Contains(adm);
                            if (adm == null || (adm != null && Server2.admini_logati.Contains(adm)))
                            {
                                // Echo back the message
                                string response = "FAILED\r\n";
                                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                                stream.Write(responseBytes, 0, responseBytes.Length);
                            }
                            else
                            {
                                // Echo back the message
                                string response = "SUCCESS\r\n";
                                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                                stream.Write(responseBytes, 0, responseBytes.Length);
                                Server2.admini_logati.Add(adm);
                            }
                        }
                        else if (request.StartsWith("GET_RACES"))
                        {
                            List<Cursa> curse = Server2.cursaSrv.getCurse();

                            string serialized = JsonConvert.SerializeObject(curse);
                            serialized += "\r\n";
                            byte[] jsonData = Encoding.UTF8.GetBytes(serialized);
                            stream.Write(jsonData, 0, jsonData.Length);
                        }
                        else if (request.StartsWith("GET_RACERS"))
                        {
                            string echipa = request.Split(' ')[1];
                            List<Participant> participanti = Server2.partSrv.getParticipantiByEchipa(echipa);

                            string serialized = JsonConvert.SerializeObject(participanti);
                            serialized += "\r\n";
                            byte[] jsonData = Encoding.UTF8.GetBytes(serialized);
                            stream.Write(jsonData, 0, jsonData.Length);
                        }
                        else if (request.StartsWith("ADD_RACER"))
                        {
                            var splitted = request.Split(' ');
                            Participant part = new Participant(splitted[1], splitted[2], splitted[3], splitted[4], splitted[5]);
                            var res = Server2.partSrv.save(part);
                            Server2.cursaSrv.saveParticipantCursa(splitted[6], part.ID);

                            if (res == null)
                            {
                                string response = "FAILED\r\n";
                                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                                stream.Write(responseBytes, 0, responseBytes.Length);
                            }
                            else if (res != null)
                            {
                                string response = "SUCCESS\r\n";
                                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                                stream.Write(responseBytes, 0, responseBytes.Length);
                                Server2.broadcast("REFRESH_RACERS\r\n");
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
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
