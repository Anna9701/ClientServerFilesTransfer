using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using TransferredFile;

namespace ClientFileTransfer
{
    class FileSenderUtil
    {
        public String Filename { get; set; }

        public string AskForFilename()
        {
            String path;
            Console.Out.WriteLine("Enter filename (with path): ");
            path = Console.In.ReadLine();
            return path;
        }

        public TransferredFile.TransferredFile GetTransferredFile (String path)
        {
            using (FileStream fileStream = File.OpenRead(path))
            {
                String filename = Path.GetFileName(path);
                byte[] data = File.ReadAllBytes(path);
                return new TransferredFile.TransferredFile(filename, data);
            }
        }
    }
}
