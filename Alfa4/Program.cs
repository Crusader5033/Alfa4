using Alfa4.UDP;

namespace Alfa4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string peerId = "dom"; // Změňte podle potřeby

            Peer peer = new Peer(peerId);
            peer.StartDiscovery();
            peer.StartListening();

            // Necháme hlavní vlákno běžet pro kontrolu konzole
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            // Zastavit vlákno a uvolnit zdroje
            peer.Stop();
        }
    }
}