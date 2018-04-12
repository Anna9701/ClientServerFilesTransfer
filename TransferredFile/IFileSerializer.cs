using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace TransferredFile
{
    public interface IFileSerializer
    {
        byte[] ParseTransferredFileToBytesArray(TransferredFile transferredFile);
        TransferredFile ParseBytesArrayToTransferredFile(byte[] data);
    }
}
