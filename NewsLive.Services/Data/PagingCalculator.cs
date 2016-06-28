namespace NewsLive.Services
{
    using System;

    public class PagingService : IPagingService
    {
        public PagedMetric CalculatePagingMetric(int repositoryCount, int numResultsPerPage, int nextPageNum)
        {
            var pageCount = (numResultsPerPage == 0) ? 0 : (int)Math.Ceiling((decimal)repositoryCount / numResultsPerPage);

            var pageFrom = (nextPageNum - 1) * numResultsPerPage;

            return new PagedMetric(pageCount, pageFrom);
        }
    }
}
