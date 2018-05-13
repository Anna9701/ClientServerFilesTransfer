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


        private byte[] ReadFromStream(Stream stream)
        {
            int numBytesToRead = (int)stream.Length;
            byte[] data = new byte[numBytesToRead];
            int readed = 0;
            stream.Seek(0, SeekOrigin.Begin);
            do
            {
                readed += stream.Read(data, readed, numBytesToRead - readed);
            } while (readed != numBytesToRead);
            return data;
        }

        public byte[] ParseTransferredFileToBytesArray (TransferredFile transferredFile)
        {
            using (Stream destinationStrem = new MemoryStream())
            {
                formatter.Serialize(destinationStrem, transferredFile);
                return ReadFromStream(destinationStrem);
            }

        }
        
        public TransferredFile ParseBytesArrayToTransferredFile(byte[] data)
        {
            using(Stream sourceStream = new MemoryStream(data))
            {
                sourceStream.Seek(0, SeekOrigin.Begin);
                return (TransferredFile)formatter.Deserialize(sourceStream);
            }
        }
    }
}
