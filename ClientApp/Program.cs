using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ClientFileTransfer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Client client;
            if (args.Length < 2)
            {
                client = new Client();
            }
            else
            {
                client = new Client(args[0], Int32.Parse(args[1]));
            }
            await client.ConnectAsync();
            //byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes("Hello server");
            //await client.SendData(data);
            Console.WriteLine("Press any key to finish execution...");
            Console.ReadKey();
        }
    }
}
