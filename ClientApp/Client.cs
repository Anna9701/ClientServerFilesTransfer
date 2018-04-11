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

        public Client()
        {
            serverAddress = "127.0.0.1";
            serverPort = 13000;
        }

        public Client(String address, int port)
        {
            serverAddress = address;
            serverPort = port;
            tcpClient = new TcpClient();
        }

        async public Task ConnectAsync()
        {
            await tcpClient.ConnectAsync(serverAddress, serverPort);
  
        }

        async public Task SendData(byte[] data)
        {
            try
            {
                using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    await networkStream.WriteAsync(data, 0, data.Length);
                }
            } catch (SocketException ex)
            {
                await Console.Error.WriteLineAsync("Error writing to server socket! Discarding.. \n" + ex.Message);
            }
        }

        public void Dispose()
        {
            tcpClient.Close();
        }
    }
}
