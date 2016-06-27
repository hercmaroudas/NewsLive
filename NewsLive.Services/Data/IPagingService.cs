namespace NewsLive.Services
{
    public interface IPagingService
    {
        PagedMetric CalculatePagingMetric(int repositoryCount, int numResultsPerPage, int nextPageNum);
    }
}
