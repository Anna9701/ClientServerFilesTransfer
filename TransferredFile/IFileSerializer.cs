using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace TransferredFile
{
    public interface IFileSerializer
    {
        TransferredFile ReadFromStream(Stream stream);
        void WriteToStream(Stream stream, TransferredFile transferredFile);
    }
}
