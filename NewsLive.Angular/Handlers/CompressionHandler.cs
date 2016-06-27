namespace NewsLive.Angular.Handlers
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Extensions.Compression.Core.Compressors;
    using System.Net.Http.Extensions.Compression.Core.Interfaces;
    using System.Threading;
    using System.Threading.Tasks;

    public class CompressionHandler : DelegatingHandler
    {
        public Collection<ICompressor> Compressors { get; private set; }

        public CompressionHandler()
        {
            Compressors = new Collection<ICompressor>();

            Compressors.Add(new GZipCompressor());
            Compressors.Add(new DeflateCompressor());
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.RequestMessage.Headers.AcceptEncoding != null &&
                response.RequestMessage.Headers.AcceptEncoding.Count > 0)
            {
                var encoding = request.Headers.AcceptEncoding.First();

                var compressor = Compressors.FirstOrDefault(c => c.EncodingType.Equals(encoding.Value, StringComparison.InvariantCultureIgnoreCase));
                if (compressor != null)
                {
                    response.Content = new CompressedContent(response.Content, compressor);
                }
            }

            return response;
        }
    }

    public class CompressedContent : HttpContent
    {
        private readonly HttpContent _content;
        private readonly ICompressor _compression;

        public CompressedContent(HttpContent content, ICompressor compression)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (compression == null)
            {
                throw new ArgumentNullException("encodingType");
            }

            _content = content;
            _compression = compression;

            AddHeaders();
        }
        

        private void AddHeaders()
        {
            foreach (var header in _content.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            Headers.ContentEncoding.Add(_compression.EncodingType);
        }

        protected async override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            using (_content)
            {
                var contentStream = await _content.ReadAsStreamAsync();
                await _compression.Compress(contentStream, stream);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}