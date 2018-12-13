using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _08_ExUDP
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continuer = true;

            while (continuer)
            {
                Console.WriteLine("\n Entrez un message : ");
                string message = Console.ReadLine();

                //Sérialisation du message en tableau de bytes.
                byte[] msg = Encoding.Default.GetBytes(message);

                UdpClient udpClient = new UdpClient();

                //La méthode Send envoie un message UDP.
                udpClient.Send(msg, msg.Length, "127.0.0.1", 5035);

                udpClient.Close();

                Console.WriteLine("\nContinuer ? (O/N)");
                continuer = (Console.ReadKey().Key == ConsoleKey.O);
            }
        }
    }
}
