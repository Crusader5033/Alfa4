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
    private readonly TcpListener tcpListener;
    private readonly Dictionary<string, Dictionary<string, string>> messageHistory;

    public Peer(string id, int discoveryPort, int discoveryIntervalSeconds, int tcpPort)
    {
        peerId = id;
        this.discoveryPort = discoveryPort;
        this.discoveryIntervalSeconds = discoveryIntervalSeconds;
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
        udpListener = new UdpListener(discoveryPort);
        udpListener.MessageReceived += HandleDiscoveryResponse;
        tcpConnections = new Dictionary<string, TcpConnection>();
        tcpListener = new TcpListener(IPAddress.Any, tcpPort);
        tcpListener.Start();
        messageHistory = new Dictionary<string, Dictionary<string, string>>();
        StartListeningForMessages();
    }

    public void StartListeningForMessages()
    {
        Thread tcpListenerThread = new Thread(ListenForMessages);
        tcpListenerThread.Start();
    }

    public void ListenForMessages()
    {
        try
        {
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(HandleIncomingMessage, client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred while listening for TCP messages: " + ex.Message);
        }
    }

    public void HandleIncomingMessage(object client)
    {
        TcpClient tcpClient = (TcpClient)client;
        try
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received TCP message: {message}");

            // Parse the incoming TCP message
            var parsedMessage = ParseJsonMessage(message);
            if (parsedMessage.ContainsKey("status") && parsedMessage["status"].ToString() == "ok")
            {
                if (parsedMessage.ContainsKey("messages"))
                {
                    var messages = parsedMessage["messages"] as Dictionary<string, object>;
                    foreach (var kvp in messages)
                    {
                        string messageId = kvp.Key;
                        var messageData = kvp.Value as Dictionary<string, object>;
                        string peerId = messageData["peer_id"].ToString();
                        string msg = messageData["message"].ToString();

                        // Store message in history
                        if (!messageHistory.ContainsKey(peerId))
                        {
                            messageHistory[peerId] = new Dictionary<string, string>();
                        }
                        messageHistory[peerId][messageId] = msg;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred while handling incoming TCP message: " + ex.Message);
        }
        finally
        {
            tcpClient.Close();
        }
    }

    public void StartListening()
    {
        udpListener.StartListening();
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

        HandleHandshake(remoteEP);
    }

    private void HandleHandshake(IPEndPoint remoteEP)
    {
        if (!tcpConnections.ContainsKey(remoteEP.Address.ToString()))
        {
            // Connect to the peer over TCP
            TcpConnection tcpConnection = new TcpConnection(remoteEP.Address.ToString(), 9876);
            tcpConnection.Connect();
            tcpConnections.Add(remoteEP.Address.ToString(), tcpConnection);

            // Send hello message
            tcpConnection.SendMessage($"{{\"command\":\"hello\",\"peer_id\":\"{peerId}\"}}");

            // Prepare and send peer's chat history
            string historyResponse = PrepareHistoryResponse();
            tcpConnection.SendMessage(historyResponse);

            Console.WriteLine($"TCP connection established with peer at {remoteEP.Address}:{tcpConnection.Port}");
        }
    }

    private string PrepareHistoryResponse()
    {
        // Construct history response message
        StringBuilder historyBuilder = new StringBuilder();
        historyBuilder.Append("{\"status\":\"ok\",\"messages\":{");

        foreach (var peerMessages in messageHistory)
        {
            foreach (var kvp in peerMessages.Value)
            {
                string messageId = kvp.Key;
                string msg = kvp.Value;
                historyBuilder.Append($"\"{messageId}\":{{\"peer_id\":\"{peerMessages.Key}\",\"message\":\"{msg}\"}},");
            }
        }

        // Remove the last comma
        if (messageHistory.Count > 0)
        {
            historyBuilder.Remove(historyBuilder.Length - 1, 1);
        }

        historyBuilder.Append("}}");

        return historyBuilder.ToString();
    }

    private Dictionary<string, object> ParseJsonMessage(string message)
    {
        // Here you would parse the JSON message into a dictionary
        // For now, let's just return an empty dictionary
        return new Dictionary<string, object>();
    }

    public void Stop()
    {
        foreach (var connection in tcpConnections.Values)
        {
            connection.Disconnect();
        }
        udpListener.Close();
        tcpListener.Stop();
    }
}
