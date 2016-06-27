using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsLive.Services.Compression
{
    public interface IDeflateCompression
    {
        byte[] DeflateByte(byte[] buffer);
    }
}
