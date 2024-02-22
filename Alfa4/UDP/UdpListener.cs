using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Alfa4.UDP
{
    internal class UdpListener
    {
        private readonly UdpClient udpClient;
        

        public event Action<string, IPEndPoint> MessageReceived;

        public UdpListener()
        {
            udpClient = new UdpClient(9876);
            udpClient.EnableBroadcast = true;
        }

        public void StartListening()
        {
            try
            {
                while (true)
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = udpClient.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(data);
                    MessageReceived?.Invoke(message, remoteEP);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while listening: " + ex.Message);
            }
        }

        public void Close()
        {
            udpClient.Close();
        }
    }
}