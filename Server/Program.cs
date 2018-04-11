using System;
using System.Net;
using System.Net.Sockets;

namespace ServerFileTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
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