namespace NewsLive.DataAccess.Repository.Article
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IArticleRepository
    {
        Models.ArticleModel GetArticleById(int articleId);

        IEnumerable<Models.ArticleModel> GetAllArticlesByAuthor(int personId);

        IEnumerable<Models.ArticleModel> GetAllArticlesByAuthorPaged(int authorId, int numResultsPerPage, int nextPageNum);

        IEnumerable<Models.ArticleModel> GetAllArticlesPaged(int numResultsPerPage, int nextPageNum);

        Models.ArticleModel PublishArticle(Models.ArticleModel article);

        bool UpdatePublishedArticle(Models.ArticleModel article);

        bool DeletePublishedArticle(int articleId);

        IEnumerable<Models.GroupedArticleLikeModel> GetGroupedArticleLikes();
    }
}
