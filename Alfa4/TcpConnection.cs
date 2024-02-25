using System.Net.Sockets;
using System.Text;

internal class TcpConnection
{
    private TcpClient tcpClient;
    private NetworkStream stream;
    private string ipAddress;

    public TcpConnection(string ipAddress)
    {
        this.ipAddress = ipAddress;
        tcpClient = new TcpClient();
    }

    public void Connect()
    {
        try
        {
            tcpClient.Connect(ipAddress, 9876);
            stream = tcpClient.GetStream();
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
            stream.Close();
            tcpClient.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred while disconnecting TCP connection: " + ex.Message);
        }
    }
}