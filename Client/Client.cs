using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TransferredFile;

namespace ClientFileTransfer
{
    class Client : IDisposable
    {
        private readonly String serverAddress;
        private readonly int serverPort;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private readonly IFileSerializer serializer;

        public Client()
        {
            serverAddress = "127.0.0.1";
            serverPort = 13000;
            tcpClient = new TcpClient();
            serializer = new FileSerializer();
        }

        public Client(String address, int port)
        {
            serverAddress = address;
            serverPort = port;
            tcpClient = new TcpClient();
            serializer = new FileSerializer();
        }

        async public Task ConnectAsync()
        {
            try
            {
                await tcpClient.ConnectAsync(serverAddress, serverPort);
            } catch (SocketException ex)
            {
                await Console.Error.WriteLineAsync("Error while connecting to server socket! Try again later. \n" + ex.Message);
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }

        public void Connect()
        {
            try
            {
                tcpClient.Connect(serverAddress, serverPort);
                networkStream = tcpClient.GetStream();
            }
            catch (SocketException ex)
            {
                Console.Error.WriteLine("Error while connecting to server socket! Try again later. \n" + ex.Message);
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }

        public void SendData(TransferredFile.TransferredFile data)
        {
            try
            {
                serializer.WriteToStream(networkStream, data);
            } catch (SocketException ex)
            {
                Console.Error.WriteLine("Error writing to server socket! Discarding.. \n" + ex.Message);
                Console.ReadKey();
            }
        }

        async public Task SendDataAsync(byte[] data)
        {
            try
            {
                await networkStream.WriteAsync(data, 0, data.Length);
            }
            catch (SocketException ex)
            {
                await Console.Error.WriteLineAsync("Error writing to server socket! Discarding.. \n" + ex.Message);
                Console.ReadKey();
            }
        }

        public void Dispose()
        {
            networkStream.Close();
            tcpClient.Close();
        }
    }
}
