using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TransferredFile
{
    public class FileSerializer : IFileSerializer
    {
        private IFormatter formatter;

        public FileSerializer(IFormatter formatter) => this.formatter = formatter;

        public FileSerializer() => formatter = new BinaryFormatter();


        public FileStream DeserializeFileContent(TransferredFile file)
        {
            using (Stream stream = new MemoryStream(file.SerializedFile))
            {
                return (FileStream)formatter.Deserialize(stream);
            }
        }

        public TransferredFile SerializeFileContent(FileStream file, string name)
        {
            TransferredFile transferredFile;
            Stream destinationStream = new MemoryStream();
            formatter.Serialize(destinationStream, file);
            byte[] serializedFile = ReadFromStream(destinationStream);
 
            transferredFile = new TransferredFile(name, serializedFile);
            destinationStream.Close();
            return transferredFile;
        }

        private byte[] ReadFromStream(Stream stream)
        {
            int PADDING_SIZE = 10;
            byte[] data = new byte[stream.Length + PADDING_SIZE];
            int numBytesToRead = (int)stream.Length;
            int numBytesRead = 0;
            do
            {
                int readed = stream.Read(data, numBytesRead, PADDING_SIZE);
                numBytesRead += readed;
                numBytesToRead -= readed;
            } while (numBytesToRead > 0);
            return data;
        }

        public byte[] ParseTransferredFileToBytesArray (TransferredFile transferredFile)
        {
            using(Stream destinationStrem = new MemoryStream())
            {
                formatter.Serialize(destinationStrem, transferredFile);
                return ReadFromStream(destinationStrem);
            }
        }
        
        public TransferredFile ParseBytesArrayToTransferredFile(byte[] data)
        {
            using(Stream sourceStream = new MemoryStream(data))
            {
                return (TransferredFile)formatter.Deserialize(sourceStream);
            }
        }
    }
}
