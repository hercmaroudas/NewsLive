
namespace NewsLive.Services
{
    public struct PagedMetric
    {
        public int PageCount { get; set; }

        public int PageFrom { get; set; }

        public PagedMetric(int pageCount, int pageFrom)
        {
            PageCount = pageCount;
            PageFrom = pageFrom;
        }
    }
}
