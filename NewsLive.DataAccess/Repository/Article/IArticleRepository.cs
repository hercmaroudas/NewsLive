namespace NewsLive.DataAccess.Repository.Article
{
    using System.Collections.Generic;

    public interface IArticleRepository
    {
        Models.ArticleModel GetArticle(int articleId);

        IEnumerable<Models.ArticleModel> GetAllArticlesByAuthor(int personId);

        IEnumerable<Models.ArticleModel> GetAllArticlesByAuthorPaged(int authorId, int numResultsPerPage, int currentPageNum);

        IEnumerable<Models.ArticleModel> GetAllArticles();

        IEnumerable<Models.ArticleModel> GetAllArticlesPaged(int numResultsPerPage, int currentPageNum);

        Models.ArticleModel PublishArticle(Models.ArticleModel article);

        bool UpdatePublishedArticle(Models.ArticleModel article);

        bool DeletePublishedArticle(int articleId);

        IEnumerable<Models.GroupedArticleLikeModel> GetGroupedArticleLikes();
    }
}
