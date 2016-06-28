namespace NewsLive.DataAccess
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDataService
    {
        int GetArticleCount();

        int GetArticleByAuthorCount(int personId);

        Membership GetMembership(string userName, string password);

        Article GetArticleById(int articleId);

        IQueryable<Article> GetArticlesByAuthorId(int personId, int min = 0, int max = 0);

        IQueryable<Article> GetArticles(int min = 0, int max = 0);

        Task<IQueryable<Article>> GetArticlesAsync();

        Article AddArticle(Article article);

        int UpdateArticle(Article article);

        int DeleteArticle(int articleId);

        IQueryable<ArticleLike> GetArticleLikes();

        ArticleLike GetArticleLike(int articleId, int personId);

        ArticleLike AddArticleLike(ArticleLike articleLike);

        int UpdateArticleLike(ArticleLike articleLike);

        Comment AddComment(Comment comment);

        EntityState MarkEntityAs<T>(T entity, EntityState entityState) where T : class;

    }
}
