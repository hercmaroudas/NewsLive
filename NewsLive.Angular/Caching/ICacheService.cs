namespace NewsLive.Angular.Caching
{
    using System.Linq;

    using DataAccess.Models;

    public interface ICacheService
    {
        IQueryable<ArticleModel> Articles { get; }
    }
}