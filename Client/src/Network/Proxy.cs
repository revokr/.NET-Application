using Azure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Proiect_MPP.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Azure.Core;
using Serilog;
using System.Runtime.Remoting.Messaging;

namespace Proiect_MPP.src.Network
{
    public class Proxy
    {
        private static Proxy instance;  // The single instance of Proxy
        private static readonly object lockObj = new object();  // Lock object for thread-safety
        private TcpClient socket;
        private NetworkStream stream;
        private Queue<string> responses;
        private volatile bool finished;
        private EventWaitHandle _waitHandle;
        private MainWindowController controller;

        // Private constructor to prevent instantiation from outside
        private Proxy(string host, int port)
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            responses = new Queue<string>();
            finished = false;
            StartReader();
        }

        // Public method to get the singleton instance
        public static Proxy Instance(string host = null, int port = 0)
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    if (host == null || port == 0)
                        throw new InvalidOperationException("Proxy must be initialized with host and port first.");

                    instance = new Proxy(host, port);
                }
                return instance;
            }
        }

        // Set controller for UI updates
        public void SetController(MainWindowController controller)
        {
            this.controller = controller;
        }

        // Get the controller
        public MainWindowController GetController()
        {
            return controller;
        }

        public void SendRequest(string request)
        {
            try
            {
                lock (stream)
                {
                    byte[] data = Encoding.UTF8.GetBytes(request); // Append newline 
                    stream.Write(data, 0, data.Length);
                    stream.Flush();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public string ReadResponse()
        {
            string response = null;
            Thread.Sleep(500);
            try
            {
                //_waitHandle.WaitOne();
                lock (responses)
                {
                    response = responses.Dequeue();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //MessageBox.Show(e.Message);
            }
            return response;
        }

        // Close the connection
        public void Close()
        {
            socket.Close();
        }

        // Start the reader thread
        public void StartReader()
        {
            Task.Run(() => run());
        }

        // The reader thread that listens for responses from the server
        private void run()
        {
            while (!finished)
            {
                try
                {
                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                    if (response == "REFRESH_RACERS")
                    {
                        controller.notifyRefresh();
                    }
                    else
                    {
                        lock (responses)
                        {
                            responses.Enqueue(response);
                            //MessageBox.Show("AM BAGAT!");
                        }
                        _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    //log.Error("Reading error " + e);
                }

            }
        }
    }

}

