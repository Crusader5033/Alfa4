
//---------------------------------------------------------------------
// Class: TcpConnection
// Description: Represents a TCP connection to a remote host.
//------
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

internal class TcpConnection
{
    // Member variables

    private TcpClient tcpClient;
    private NetworkStream stream;
    private string ipAddress;
    private int port;
    private bool isConnected = false;
    private Thread receiveThread;

    //-------------------------------------------------------------
    // Property: Port
    // Description: Gets or sets the port number of the TCP connection.
    //-------------------------------------------------------------
    public int Port { get => port; set => port = value; }


    //-------------------------------------------------------------
    // Constructor: TcpConnection
    // Description: Initializes a new instance of the TcpConnection class.
    // Parameters:
    //      ipAddress (string): The IP address of the remote host.
    //      port (int): The port number of the remote host.
    //-------------------------------------------------------------
    public TcpConnection(string ipAddress, int port)
    {
        this.ipAddress = ipAddress;
        this.port = port;
        tcpClient = new TcpClient();
    }
    //-------------------------------------------------------------
    // Method: Connect
    // Description: Connects to the remote host.
    //-------------------------------------------------------------
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
            throw ex;
        }
    }
    //-------------------------------------------------------------
    // Method: SendMessage
    // Description: Sends a message to the remote host.
    // Parameters:
    //      message (string): The message to send.
    //-------------------------------------------------------------

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
            throw ex;
        }
    }
    //-------------------------------------------------------------
    // Method: Disconnect
    // Description: Disconnects from the remote host.
    //-------------------------------------------------------------
    public void Disconnect()
    {
        try
        {
            receiveThread?.Abort();
            stream.Close();
            tcpClient.Close();
            isConnected = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //-------------------------------------------------------------
    // Method: ReceiveMessages
    // Description: Receives messages from the remote host.
    //-------------------------------------------------------------

    public void ReceiveMessages()
    {
        while (isConnected)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received TCP message: {receivedMessage}");
            }
            catch (Exception ex)
            {
                throw ex;
                // Ukončení vlákna při chybě
                isConnected = false;
            }
        }
    }
}
