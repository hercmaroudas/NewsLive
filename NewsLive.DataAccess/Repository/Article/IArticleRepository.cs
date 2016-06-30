namespace NewsLive.DataAccess.Repository.Article
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticleRepository
    {
        Models.ArticleModel GetArticleById(int articleId);

        Task<Models.ArticleModel> GetArticleByIdAsync(int articleId);

        IEnumerable<Models.ArticleModel> GetAllArticlesByAuthor(int personId);

        Task<IEnumerable<Models.ArticleModel>> GetAllArticlesByAuthorAsync(int personId);

        IEnumerable<Models.ArticleModel> GetAllArticlesByAuthorPaged(int authorId, int numResultsPerPage, int nextPageNum);

        Task<IEnumerable<Models.ArticleModel>> GetAllArticlesByAuthorPagedAsync(int authorId, int numResultsPerPage, int nextPageNum);

        IEnumerable<Models.ArticleModel> GetAllArticlesPaged(int numResultsPerPage, int nextPageNum);

        Task<IEnumerable<Models.ArticleModel>> GetAllArticlesPagedAsync(int numResultsPerPage, int nextPageNum);

        Models.ArticleModel PublishArticle(Models.ArticleModel article);

        Task<Models.ArticleModel> PublishArticleAsync(Models.ArticleModel article);

        bool UpdatePublishedArticle(Models.ArticleModel article);

        Task<bool> UpdatePublishedArticleAsync(Models.ArticleModel article);

        bool DeletePublishedArticle(int articleId);

        Task<bool> DeletePublishedArticleAsync(int articleId);

        IEnumerable<Models.GroupedArticleLikeModel> GetGroupedArticleLikes();

        Task<IEnumerable<Models.GroupedArticleLikeModel>> GetGroupedArticleLikesAsync();
    }
}
