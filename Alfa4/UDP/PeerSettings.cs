using System.Xml;

public class PeerSettings
{
    public string PeerId { get; set; }
    public int DiscoveryPort { get; set; }
    public int DiscoveryIntervalSeconds { get; set; }

    public static PeerSettings LoadFromFile(string filePath)
    {
        PeerSettings settings = new PeerSettings();
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode peerNode = doc.SelectSingleNode("/configuration/peerSettings");

            settings.PeerId = peerNode.SelectSingleNode("peerId").InnerText;
            settings.DiscoveryPort = int.Parse(peerNode.SelectSingleNode("discoveryPort").InnerText);
            settings.DiscoveryIntervalSeconds = int.Parse(peerNode.SelectSingleNode("discoveryIntervalSeconds").InnerText);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading peer settings: " + ex.Message);
        }
        return settings;
    }
}
