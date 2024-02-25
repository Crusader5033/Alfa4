using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

internal class TcpConnection
{
    private TcpClient tcpClient;
    private NetworkStream stream;
    private string ipAddress;
    private int port;
    private bool isConnected = false;
    private Thread receiveThread;

    public TcpConnection(string ipAddress, int port)
    {
        this.ipAddress = ipAddress;
        this.port = port;
        tcpClient = new TcpClient();
    }

    public void Connect()
    {
        try
        {
            tcpClient.Connect(ipAddress, port);
            stream = tcpClient.GetStream();
            isConnected = true;

            // Spuštění vlákna pro příjem zpráv
            receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred while connecting over TCP: " + ex.Message);
        }
    }

    public void SendMessage(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine($"Sent TCP message: {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred while sending TCP message: " + ex.Message);
        }
    }

    public void Disconnect()
    {
        try
        {
            // Zastavení vlákna pro příjem zpráv
            receiveThread?.Abort();
            stream.Close();
            tcpClient.Close();
            isConnected = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred while disconnecting TCP connection: " + ex.Message);
        }
    }

    private void ReceiveMessages()
    {
        while (isConnected)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received TCP message: {receivedMessage}");
                // Zde můžete provádět další zpracování přijatých zpráv
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while receiving TCP message: " + ex.Message);
                // Ukončení vlákna při chybě
                isConnected = false;
            }
        }
    }
}
