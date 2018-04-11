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
        private readonly IFileSerializer serializer;

        public String Filename { get; set; }

        public FileSenderUtil() => serializer = new FileSerializer();

        public FileSenderUtil(IFileSerializer fileSerializer) => serializer = fileSerializer;

        public string AskForFilename()
        {
            String path;
            System.Console.Out.WriteLine("Enter filename (with path): ");
            path = Console.In.ReadLine();
            return path;
        }

        private TransferredFile.TransferredFile PrepareTransferredFile (String path)
        {
            using (FileStream fileStream = File.OpenRead(path))
            {
                String filename = Path.GetFileName(path);
                return serializer.SerializeFileContent(fileStream, filename);
            }
        }

        public byte[] PrepareBytesMessageFromFile(String path)
        {
            TransferredFile.TransferredFile transferredFile = PrepareTransferredFile(path);
            return serializer.ParseTransferredFileToBytesArray(transferredFile);
        }
    }
}
