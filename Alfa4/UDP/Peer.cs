using System.Net.Sockets;
using System.Net;
using System.Text;

internal class Peer
{
    private string peerId;
    private int discoveryPort;
    private int discoveryIntervalSeconds;
    private  UdpClient udpClient;
    private  UdpListener udpListener;

    public Peer(string id, int port, int intervalSeconds)
    {
        peerId = id;
        discoveryPort = port;
        discoveryIntervalSeconds = intervalSeconds;
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
        udpListener = new UdpListener(discoveryPort);
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
                Thread.Sleep(discoveryIntervalSeconds * 1000);
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
            udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, discoveryPort));
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