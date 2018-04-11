using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferredFile
{
    [Serializable]
    public class TransferredFile
    {
        private string filename;
        private byte[] serializedFile;

        public string FileName { get { return filename; } private set { } }
        public byte[] SerializedFile { get { return serializedFile; } private set { } }

        public TransferredFile(string name, byte[] fileContent)
        {
            filename = name;
            serializedFile = fileContent;
        }
    }
}
