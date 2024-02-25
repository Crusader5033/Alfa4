//---------------------------------------------------------------------
// Class: UdpListener
// Description: Represents a UDP listener for receiving messages.
//---------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpListener
{
    private UdpClient udpClient;

    //-------------------------------------------------------------
    // Event: MessageReceived
    // Description: Event raised when a message is received.
    // Parameters:
    //      message (string): The received message.
    //      remoteEP (IPEndPoint): The endpoint from which the message originated.
    //-------------------------------------------------------------
    public event Action<string, IPEndPoint> MessageReceived;

    //-------------------------------------------------------------
    // Constructor: UdpListener
    // Description: Initializes a new instance of the UdpListener class.
    // Parameters:
    //      port (int): The port number on which to listen for messages.
    //-------------------------------------------------------------
    public UdpListener(int port)
    {
        udpClient = new UdpClient(port);
        udpClient.EnableBroadcast = true;
    }

    //-------------------------------------------------------------
    // Method: StartListening
    // Description: Starts listening for incoming UDP messages.
    //-------------------------------------------------------------
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

    //-------------------------------------------------------------
    // Method: Close
    // Description: Closes the UDP listener.
    //-------------------------------------------------------------
    public void Close()
    {
        udpClient.Close();
    }
}
