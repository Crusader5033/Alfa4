//---------------------------------------------------------------------
// Namespace: Alfa4
// Description: Namespace containing classes for peer communication.
//---------------------------------------------------------------------

namespace Alfa4
{
    //-------------------------------------------------------------
    // Class: Program
    // Description: Entry point of the peer communication program.
    //-------------------------------------------------------------
    internal class Program
    {
        //---------------------------------------------------------
        // Method: Main
        // Description: Main method to start the peer application.
        // Parameters:
        //      args (string[]): Command-line arguments.
        //---------------------------------------------------------
        static void Main(string[] args)
        {
            // Get configuration settings
            PeerSettings settings = PeerSettings.LoadFromFile("App.config");

            // Create a peer instance using configuration settings
            Peer peer = new Peer(settings.PeerId, settings.DiscoveryPort, settings.DiscoveryIntervalSeconds, settings.DiscoveryPort);

            // Start peer discovery process
            peer.StartDiscovery();
            peer.StartListening();  // Start listening for other peers

            // Start listening on TCP port to receive messages from other peers
            peer.StartListeningForMessages();

            // Keep the console running until a key is pressed
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            // Terminate peer connections and activities
            peer.Stop();
        }
    }
}
