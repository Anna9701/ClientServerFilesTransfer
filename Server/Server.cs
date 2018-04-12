﻿using System;
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
                Console.Write("Waiting for a connection... ");
                TcpClient client = server.AcceptTcpClient();
                Thread clinetThread = new Thread(() => HandleClient(client));
                clinetThread.Start();
            }
        }



        void HandleClient(TcpClient client)
        {
            
            byte[] bytes = new Byte[sizeof(int)];
            Console.WriteLine("Connected!");
            using (NetworkStream stream = client.GetStream())
            {
                int readed = stream.Read(bytes, 0, bytes.Length);
                int size = Int32.Parse(System.Text.Encoding.ASCII.GetString(bytes, 0, readed));
                Console.WriteLine("Received: {0}", size);
                bytes = new Byte[size];
                readed = 0;
                do
                {
                    readed = stream.Read(bytes, readed, size - readed);
                } while (readed != size);
                SaveReceivedFile(bytes); 
            }
            client.Close();
        }

        private void SaveReceivedFile(byte[] receivedData) 
        {
            TransferredFile.TransferredFile file = serializer.ParseBytesArrayToTransferredFile(receivedData);
            System.IO.File.WriteAllBytes(file.FileName, file.SerializedFile);
        }

        public void Dispose()
        {
            server.Stop();
        }
    }
}