namespace NewsLive.Services
{
    using System;

    public class PagingService : IPagingService
    {
        public PagedMetric CalculatePagingMetric(int repositoryCount, int numResultsPerPage, int nextPageNum)
        {
            var pageCount = (int)Math.Ceiling((decimal)repositoryCount / numResultsPerPage);

            var pageFrom = (nextPageNum - 1) * numResultsPerPage;

            return new PagedMetric(pageCount, pageFrom);
        }
    }
}
