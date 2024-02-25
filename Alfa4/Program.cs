namespace Alfa4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Získání konfiguračních nastavení
            PeerSettings settings = PeerSettings.LoadFromFile("App.config");

            // Vytvoření instance peeru s použitím konfiguračních nastavení
            Peer peer = new Peer(settings.PeerId, settings.DiscoveryPort, settings.DiscoveryIntervalSeconds, settings.DiscoveryPort);

            // Spuštění metody pro objevování peerů v síti
            peer.StartDiscovery();
            peer.StartListening();

            // Spuštění naslouchání na TCP portu pro přijímání zpráv od ostatních peerů
            peer.StartListeningForMessages();

            // Necháme konzoli běžet, dokud nepřijde stisk klávesy
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            // Ukončení spojení a činnosti peeru
            peer.Stop();
        }
    }
}