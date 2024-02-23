namespace Alfa4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string configFile = "App.config"; // Změňte podle potřeby
            PeerSettings settings = PeerSettings.LoadFromFile(configFile);
            Peer peer = new Peer(settings.PeerId, settings.DiscoveryPort, settings.DiscoveryIntervalSeconds);
            peer.StartDiscovery();
            peer.StartListening();

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            // Zastavit vlákno a uvolnit zdroje
            peer.Stop();
        }
    }
}