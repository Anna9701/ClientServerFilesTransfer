using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransferredFile;

namespace Server
{
    class Server : IDisposable
    {
        private static String LocalHostAddress => "127.0.0.1";
        private static int DefaultPort => 13000;
        private readonly IFileSerializer serializer;

        private const int SIZE = 256;
        private const int LENGHT_OF_BUFFER_FOR_TRANSFER_SIZE = 32;

        private readonly Int32 port;
        private readonly IPAddress serverAddress;
        private TcpListener server;

        public Server() : this(LocalHostAddress, DefaultPort)
        {
        }

        public Server(String address, int port)
        {
            this.port = port;
            serverAddress = IPAddress.Parse(address);
            server = new TcpListener(serverAddress, port);
            serializer = new FileSerializer();
            server.Start();
        }

        public void Listen()
        {
            while(true)
            {
                Console.WriteLine("Waiting for a connection... ");
                TcpClient client = server.AcceptTcpClient();
                Thread clinetThread = new Thread(() => HandleClient(client));
                clinetThread.Start();
            }
        }



        void HandleClient(TcpClient client)
        {
            
            byte[] bytes = new Byte[LENGHT_OF_BUFFER_FOR_TRANSFER_SIZE];
            Console.WriteLine("Connected!");
            NetworkStream stream = client.GetStream();
            
            bool breaked = false;
            int readed = stream.Read(bytes, 0, bytes.Length);
            int size = Int32.Parse(Encoding.ASCII.GetString(bytes, 0, readed));
            Console.WriteLine("Size of file to transfer is: {0}. If you want to break transfer, press Q", size);
            bytes = new Byte[size];
            readed = 0;
            do
            {
                try
                {
                    readed += stream.Read(bytes, readed, size - readed);
                    Console.Out.WriteLine(String.Format("Download {0}%. If you want to break press Q, ENTER to continue", readed / size * 100));
                    ConsoleKeyInfo consoleKey = Console.ReadKey();
                    if (consoleKey.Key.Equals(ConsoleKey.Q))
                    {
                        breaked = true;
                        break;
                    }
                } catch (System.IO.IOException ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    breaked = true;
                    break;
                }
            } while (readed != size);

            if (!breaked)
                SaveReceivedFile(bytes);

            stream.Close();
            client.Close();          
        }

        private void SaveReceivedFile(byte[] receivedData) 
        {
            TransferredFile.TransferredFile file = serializer.ParseBytesArrayToTransferredFile(receivedData);
            System.IO.File.WriteAllBytes(file.FileName, file.SerializedFile);
            Console.WriteLine("File successful copied");
        }

        public void Dispose()
        {
            server.Stop();
        }
    }
}
