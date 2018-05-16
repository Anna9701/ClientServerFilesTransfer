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

            TransferredFile.TransferredFile transferredFile = serializer.ReadFromStream(stream);
            System.IO.File.WriteAllBytes(transferredFile.FileName, transferredFile.SerializedFile);
            Console.WriteLine("File successful copied");
            stream.Close();
            client.Close();          
        }

        public void Dispose()
        {
            server.Stop();
        }
    }
}
