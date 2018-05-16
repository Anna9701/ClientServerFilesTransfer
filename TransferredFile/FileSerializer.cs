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

        public void WriteToStream(Stream stream, TransferredFile transferredFile)
        {
            formatter.Serialize(stream, transferredFile);
        }

        public TransferredFile ReadFromStream(Stream stream)
        {
            return (TransferredFile) formatter.Deserialize(stream);
        }
    }
}
