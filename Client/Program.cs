using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ClientFileTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client;
            if (args.Length < 2)
                client = new Client();
            else
                client = new Client(args[0], Int32.Parse(args[1]));
            client.Connect();

            FileSenderUtil senderUtil = new FileSenderUtil();
            String path = senderUtil.AskForFilename();

            byte[] data = senderUtil.PrepareBytesMessageFromFile(path);
            byte[] sizeToTransfer = System.Text.ASCIIEncoding.ASCII.GetBytes((sizeof(byte) * data.Length).ToString());

            client.SendData(sizeToTransfer);
            client.SendData(data);

            Console.WriteLine("Press any key to finish execution...");
            Console.ReadKey();
        }
    }
}

/****TODO: 
 * - wyjątki 
 * - async
 * 
 * ****//////

