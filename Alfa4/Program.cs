namespace Alfa4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string configFile = "App.config"; // Změňte podle potřeby
            PeerSettings settings = PeerSettings.LoadFromFile(configFile);

            // Vytvoření instance peeru s načtenými nastaveními
            Peer peer = new Peer(settings.PeerId, settings.DiscoveryPort, settings.DiscoveryIntervalSeconds);
            peer.StartDiscovery();
            peer.StartListening();

            

            // Zastavit vlákno a uvolnit zdroje
            peer.Stop();
        }
    }
}