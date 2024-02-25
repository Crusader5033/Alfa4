using Alfa4;
using System.Net.Sockets;
using System.Net;
using System.Text;

internal class Peer
{
    private string peerId;
    private int discoveryPort;
    private int discoveryIntervalSeconds;
    private readonly UdpClient udpClient;
    private readonly UdpListener udpListener;
    private readonly Dictionary<string, TcpConnection> tcpConnections;

    public Peer(string id, int port, int intervalSeconds)
    {
        peerId = id;
        discoveryPort = port;
        discoveryIntervalSeconds = intervalSeconds;
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
        udpListener = new UdpListener(discoveryPort);
        udpListener.MessageReceived += HandleDiscoveryResponse;
        tcpConnections = new Dictionary<string, TcpConnection>();
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

        // Parse the response message
        var response = ParseJsonMessage(message);
        if (response.ContainsKey("messages"))
        {
            // Extract messages from response
            var messages = response["messages"] as Dictionary<string, object>;
            // Handle handshake
            HandleHandshake(messages, remoteEP);
        }
    }

    private void HandleHandshake(Dictionary<string, object> messages, IPEndPoint remoteEP)
    {
        if (!tcpConnections.ContainsKey(remoteEP.Address.ToString()))
        {
            // Connect to the peer over TCP
            TcpConnection tcpConnection = new TcpConnection(remoteEP.Address.ToString());
            tcpConnection.Connect();
            tcpConnections.Add(remoteEP.Address.ToString(), tcpConnection);

            // Send handshake message
            tcpConnection.SendMessage($"{{\"command\":\"hello\",\"peer_id\":\"{peerId}\"}}");

            // Send peer's chat history
            tcpConnection.SendMessage(GetMessageHistory());
        }
    }

    private string GetMessageHistory()
    {
        // Here you would construct your chat history message
        // For now, let's just return a dummy message
        return "{\"status\":\"ok\",\"messages\":{\"1707243010934\":{\"peer_id\":\"molic-peer3\",\"message\":\"pokus\"}}}";
    }

    private Dictionary<string, object> ParseJsonMessage(string message)
    {
        // Here you would parse the JSON message into a dictionary
        // For now, let's just return an empty dictionary
        return new Dictionary<string, object>();
    }

    public void StartListening()
    {
        udpListener.StartListening();
    }

    public void Stop()
    {
        foreach (var connection in tcpConnections.Values)
        {
            connection.Disconnect();
        }
        udpListener.Close();
    }
}
