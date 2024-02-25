//---------------------------------------------------------------------
// Class: PeerSettings
// Description: Represents the configuration settings for a peer.
//---------------------------------------------------------------------

using System.Xml;

public class PeerSettings
{
    //-------------------------------------------------------------
    // Property: PeerId
    // Description: Gets or sets the unique identifier of the peer.
    //-------------------------------------------------------------
    public string PeerId { get; set; }

    //-------------------------------------------------------------
    // Property: DiscoveryPort
    // Description: Gets or sets the port used for peer discovery.
    //-------------------------------------------------------------
    public int DiscoveryPort { get; set; }

    //-------------------------------------------------------------
    // Property: DiscoveryIntervalSeconds
    // Description: Gets or sets the interval (in seconds) for peer discovery.
    //-------------------------------------------------------------
    public int DiscoveryIntervalSeconds { get; set; }

    //-------------------------------------------------------------
    // Method: LoadFromFile
    // Description: Loads peer settings from a specified XML file.
    // Parameters:
    //      filePath (string): Path to the XML file containing settings.
    // Returns:
    //      PeerSettings: Instance of PeerSettings loaded from the file.
    //-------------------------------------------------------------
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
