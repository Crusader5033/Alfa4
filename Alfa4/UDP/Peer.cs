using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Alfa4.UDP
{
    internal class Peer
    {
        private readonly string peerId;
        private readonly UdpClient udpClient;
        private readonly UdpListener udpListener;


        public Peer(string id)
        {
            peerId = id;
            udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
            udpListener = new UdpListener();
            udpListener.MessageReceived += HandleDiscoveryResponse;
        }

        public void StartDiscovery()
        {
            Thread discoveryThread = new Thread(DiscoverPeers);
            discoveryThread.Start();
        }

        private void DiscoverPeers()
        {
            try
            {
                while (true)
                {
                    SendDiscoveryRequest();
                    Thread.Sleep(5 * 1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred during discovery: " + ex.Message);
            }
        }

        private void SendDiscoveryRequest()
        {
            try
            {
                string message = $"{{\"command\":\"hello\",\"peer_id\":\"{peerId}\"}}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 9876));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while sending discovery request: " + ex.Message);
            }
        }

        private void HandleDiscoveryResponse(string message, IPEndPoint remoteEP)
        {
            Console.WriteLine($"Received response from {remoteEP}: {message}");
        }

        public void StartListening()
        {
            udpListener.StartListening();
        }

        public void Stop()
        {
            udpListener.Close();
        }
    }
}
