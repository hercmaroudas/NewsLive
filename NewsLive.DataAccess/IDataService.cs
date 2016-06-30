namespace NewsLive.DataAccess
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDataService
    {
        Article AddArticle(Article article);

        Task<Article> AddArticleAsync(Article article);

        ArticleLike AddArticleLike(ArticleLike articleLike);

        Task<ArticleLike> AddArticleLikeAsync(ArticleLike articleLike);

        Comment AddComment(Comment comment);

        Task<Comment> AddCommentAsync(Comment comment);

        int DeleteArticle(int articleId);

        Task<int> DeleteArticleAsync(int articleId);

        int GetArticleByAuthorCount(int personId);

        Task<int> GetArticleByAuthorCountAsync(int personId);

        Article GetArticleById(int articleId);

        Task<Article> GetArticleByIdAsync(int articleId);

        int GetArticleCount();

        Task<int> GetArticleCountAsync();

        ArticleLike GetArticleLike(int articleId, int personId);

        Task<ArticleLike> GetArticleLikeAsync(int articleId, int personId);

        IQueryable<ArticleLike> GetArticleLikes();

        IQueryable<Article> GetArticles(int min = 0, int max = 0);

        IQueryable<Article> GetArticlesByAuthorId(int personId, int min = 0, int max = 0);

        Membership GetMembership(string userName, string password);

        Task<Membership> GetMembershipAsync(string userName, string password);

        int UpdateArticle(Article article);

        Task<int> UpdateArticleAsync(Article article);

        int UpdateArticleLike(ArticleLike articleLike);

        Task<int> UpdateArticleLikeAsync(ArticleLike articleLike);

        EntityState MarkEntityAs<T>(T entity, EntityState entityState) where T : class;
    }
}
