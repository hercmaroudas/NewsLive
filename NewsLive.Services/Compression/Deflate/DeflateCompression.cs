namespace NewsLive.Services.Compression
{
    using System.IO;

    public class DeflateCompression : IDeflateCompression
    {
        public byte[] DeflateByte(byte[] buffer)
        {
            if (buffer == null)
            {
                return null;
            }

            using (var output = new MemoryStream())
            {
                using (var compressor = new Ionic.Zlib.DeflateStream(
                    output, Ionic.Zlib.CompressionMode.Compress,
                    Ionic.Zlib.CompressionLevel.BestSpeed))
                {
                    compressor.Write(buffer, 0, buffer.Length);
                }

                return output.ToArray();
            }
        }
    }
}
