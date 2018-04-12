using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerFileTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Server.Server server = new Server.Server();
                server.Listen();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
/*
 *    ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 *
 * Aplikacja do przesylu plików klient/serwer
 * klient - jednowątkowo
 * serwer - wielowątkowo
 * możliwość przerwania przesyłania pliku
 * Klasa tcpClient - od strony klienta bezposrednio, od serwera po zaakcpetowaniu połączenia
 * Przesyłanie pliku -> serializacja - 
 * Metoda sendFile - brak kontroli nad nią, nie ma możliwość przerwania przesyłania, niezalecane.
 * 
 * Serializacja pliku - serializcja do formatu binarnego.
 * 
 * jesli jakies pola musza byc zaincjalizowane wartosciami, ktore nie sa serializowane, to zaden konstruktor ich nie ustawi.
 * po deserializacji należy potem wywlać inną metodę, która będzie to robić. 
 * 
 * obiekt zawierający string z nazwą i tablice bajtow z seriwaliozwamym plikiem.
 */