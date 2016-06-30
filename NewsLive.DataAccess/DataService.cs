namespace NewsLive.DataAccess
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataService : IDataService
    {
        private NewsLiveDbContext _dbContext;

        public DataService()
        {
            _dbContext = new NewsLiveDbContext();

            _dbContext.Configuration.LazyLoadingEnabled = true;

            //_dbContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public DataService(NewsLiveDbContext entities)
        {
            _dbContext = entities;
        }

        public Article AddArticle(Article article)
        {
            article = _dbContext.Articles.Add(article);
            _dbContext.SaveChanges();
            return article;
        }

        public async Task<Article> AddArticleAsync(Article article)
        {
            article = _dbContext.Articles.Add(article);
            await _dbContext.SaveChangesAsync();
            return article;
        }

        public ArticleLike AddArticleLike(ArticleLike articleLike)
        {
            articleLike = _dbContext.ArticleLikes.Add(articleLike);
            _dbContext.SaveChanges();
            return articleLike;
        }

        public async Task<ArticleLike> AddArticleLikeAsync(ArticleLike articleLike)
        {
            articleLike = _dbContext.ArticleLikes.Add(articleLike);
            await _dbContext.SaveChangesAsync();
            return articleLike;
        }

        public Comment AddComment(Comment comment)
        {
            comment = _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            comment = _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public int DeleteArticle(int articleId)
        {
            var databaseArticle = GetArticleById(articleId);

            if (databaseArticle == null)
                return 0;

            databaseArticle = _dbContext.Articles.Remove(databaseArticle);

            return (_dbContext.SaveChanges());
        }

        public async Task<int> DeleteArticleAsync(int articleId)
        {
            var databaseArticle = await GetArticleByIdAsync(articleId);

            if (databaseArticle == null)
                return 0;

            databaseArticle = _dbContext.Articles.Remove(databaseArticle);

            return (await _dbContext.SaveChangesAsync());
        }

        public int GetArticleByAuthorCount(int personId)
        {
            return _dbContext.Articles
                .AsNoTracking()
                .Where(a => a.PublishDate.HasValue && a.PersonId == personId).Count();
        }

        public async Task<int> GetArticleByAuthorCountAsync(int personId)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(a => a.PublishDate.HasValue && a.PersonId == personId).CountAsync();
        }

        public Article GetArticleById(int articleId)
        {
            return _dbContext.Articles.Where(a => a.ArticleId == articleId).FirstOrDefault();
        }

        public async Task<Article> GetArticleByIdAsync(int articleId)
        {
            return await _dbContext.Articles
                .Where(a => a.ArticleId == articleId)
                .FirstOrDefaultAsync();
        }

        public int GetArticleCount()
        {
            return _dbContext.Articles.AsNoTracking()
                .Where(a => a.PublishDate.HasValue).Count();
        }

        public async Task<int> GetArticleCountAsync()
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(a => a.PublishDate.HasValue).CountAsync();
        }

        public ArticleLike GetArticleLike(int articleId, int personId)
        {
            return _dbContext.ArticleLikes
                .Where(l => l.ArticleId == articleId && l.PersonId == personId)
                .FirstOrDefault();
        }

        public async Task<ArticleLike> GetArticleLikeAsync(int articleId, int personId)
        {
            return await _dbContext.ArticleLikes
                .Where(l => l.ArticleId == articleId && l.PersonId == personId)
                .FirstOrDefaultAsync();
        }

        public IQueryable<ArticleLike> GetArticleLikes()
        {
            return _dbContext.ArticleLikes;
        }

        public IQueryable<Article> GetArticles(int min = 0, int max = 0)
        {
            if (max == 0 && min == max)
                return _dbContext.Articles
                    .Where(a => a.PublishDate.HasValue)
                    .OrderByDescending(a => a.PublishDate);
            else
            {
                return _dbContext.Articles
                    .AsNoTracking()
                    .Where(a => a.PublishDate.HasValue)
                    .OrderByDescending(a => a.PublishDate)
                    .Skip(() => min)
                    .Take(() => max);
            }
        }

        public IQueryable<Article> GetArticlesByAuthorId(int personId, int min = 0, int max = 0)
        {
            if (max == 0 && min == max)
                return _dbContext.Articles
                    .Where(a => a.PublishDate.HasValue && a.PersonId == personId)
                    .OrderByDescending(a => a.PublishDate);
            else
                return _dbContext.Articles
                    .AsNoTracking()
                    .Where(a => a.PublishDate.HasValue && a.PersonId == personId)
                    .OrderByDescending(a => a.PublishDate)
                    .Skip(() => min)
                    .Take(() => max);
        }

        public Membership GetMembership(string userName, string password)
        {
            return _dbContext.Memberships
                .Where(m => m.UserName == userName && m.Password == password)
                .FirstOrDefault();
        }

        public async Task<Membership> GetMembershipAsync(string userName, string password)
        {
            return await _dbContext.Memberships
                .Where(m => m.UserName == userName && m.Password == password)
                .FirstOrDefaultAsync();
        }

        public EntityState MarkEntityAs<T>(T entity, EntityState entityState) where T : class
        {
            return _dbContext.Entry(entity).State = entityState;
        }

        public int UpdateArticle(Article article)
        {
            var databaseArticle = GetArticleById(article.ArticleId);

            if (databaseArticle == null)
                return 0;

            databaseArticle.Title = article.Title;
            databaseArticle.Body = article.Body;
            databaseArticle.PublishDate = article.PublishDate;

            return _dbContext.SaveChanges();
        }

        public async Task<int> UpdateArticleAsync(Article article)
        {
            var databaseArticle = await GetArticleByIdAsync(article.ArticleId);

            if (databaseArticle == null)
                return 0;

            databaseArticle.Title = article.Title;
            databaseArticle.Body = article.Body;
            databaseArticle.PublishDate = article.PublishDate;

            return await _dbContext.SaveChangesAsync();
        }

        public int UpdateArticleLike(ArticleLike articleLike)
        {
            var databaseArticleLike = GetArticleLike(articleLike.ArticleId, articleLike.PersonId);
            databaseArticleLike.PersonId = articleLike.PersonId;
            databaseArticleLike.IsLiked = articleLike.IsLiked;
            return _dbContext.SaveChanges();
        }

        public async Task<int> UpdateArticleLikeAsync(ArticleLike articleLike)
        {
            var databaseArticleLike = GetArticleLike(articleLike.ArticleId, articleLike.PersonId);
            databaseArticleLike.PersonId = articleLike.PersonId;
            databaseArticleLike.IsLiked = articleLike.IsLiked;
            return await _dbContext.SaveChangesAsync();
        }
    }
}