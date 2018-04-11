using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace TransferredFile
{
    public interface IFileSerializer
    {
        FileStream DeserializeFileContent(TransferredFile file);
        TransferredFile SerializeFileContent(FileStream file, string name);
        byte[] ParseTransferredFileToBytesArray(TransferredFile transferredFile);
        TransferredFile ParseBytesArrayToTransferredFile(byte[] data);
    }
}
