using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientFileTransfer
{
    class Client : IDisposable
    {
        private readonly String serverAddress;
        private readonly int serverPort;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        public Client()
        {
            serverAddress = "127.0.0.1";
            serverPort = 13000;
            tcpClient = new TcpClient();
        }

        public Client(String address, int port)
        {
            serverAddress = address;
            serverPort = port;
            tcpClient = new TcpClient();
        }

        async public Task ConnectAsync()
        {
            try
            {
                await tcpClient.ConnectAsync(serverAddress, serverPort);
            } catch (SocketException ex)
            {
                await Console.Error.WriteLineAsync("Error while connecting to server socket! Try again later. \n" + ex.Message);
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
                Environment.Exit(-1);
            }
        }

        public void SendData(byte[] data)
        {
            try
            {
                networkStream.Write(data, 0, data.Length);
            } catch (SocketException ex)
            {
                Console.Error.WriteLine("Error writing to server socket! Discarding.. \n" + ex.Message);
            }
        }

        public void Dispose()
        {
            networkStream.Close();
            tcpClient.Close();
        }
    }
}
