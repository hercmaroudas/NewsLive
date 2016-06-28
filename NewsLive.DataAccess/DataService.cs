namespace NewsLive.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataService : IDataService
    {
        NewsLiveDbContext _dbContext;

        public DataService()
        {
            _dbContext = new NewsLiveDbContext();

            _dbContext.Configuration.LazyLoadingEnabled = true;

           // _dbContext.Database.Log = s => Debug.WriteLine(s);
        }

        public DataService(NewsLiveDbContext entities)
        {
            _dbContext = entities;
        }

        public Membership GetMembership(string userName, string password)
        {
            return _dbContext.Memberships
                .Where(m => m.UserName == userName && m.Password == password)
                .FirstOrDefault();
        }

        public IQueryable<Article> GetArticles(int min = 0, int max = 0)
        {
            if (max == 0 && min == max)
                return _dbContext.Articles
                    .Where(a => a.PublishDate.HasValue)
                    .OrderByDescending(a => a.PublishDate);
            else
                return _dbContext.Articles
                    .AsNoTracking()
                    .Where(a => a.PublishDate.HasValue)
                    .OrderByDescending(a => a.PublishDate)
                    .Skip(() => min)
                    .Take(() => max);
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

        public async Task<IQueryable<Article>> GetArticlesAsync()
        {
            return await Task.Run(() => _dbContext.Articles);
        }

        public Article GetArticleById(int articleId)
        {
            return _dbContext.Articles.Where(a => a.ArticleId == articleId).FirstOrDefault();
        }

        public Article AddArticle(Article article)
        {
            article = _dbContext.Articles.Add(article);
            _dbContext.SaveChanges();
            return article;
        }

        public int UpdateArticle(Article article)
        {
            var databaseArticle = GetArticleById(article.ArticleId);
            if (databaseArticle != null)
            { 
                databaseArticle.Title = article.Title;
                databaseArticle.Body = article.Body;
                databaseArticle.PublishDate = article.PublishDate;
                return _dbContext.SaveChanges();
            }
            return 0;
        }

        public int DeleteArticle(int articleId)
        {
            var databaseArticle = GetArticleById(articleId);
            if (databaseArticle != null)
            {
                databaseArticle = _dbContext.Articles.Remove(databaseArticle);
                _dbContext.SaveChanges();
            }
            return Convert.ToInt32(databaseArticle != null);
        }

        public Comment AddComment(Comment comment)
        {
            comment = _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        public IQueryable<ArticleLike> GetArticleLikes()
        {
            return _dbContext.ArticleLikes;
        }

        public ArticleLike GetArticleLike(int articleId, int personId)
        {
            return _dbContext.ArticleLikes.Where(l => l.ArticleId == articleId && l.PersonId == personId).FirstOrDefault();
        }

        public ArticleLike AddArticleLike(ArticleLike articleLike)
        {
            articleLike = _dbContext.ArticleLikes.Add(articleLike);
            _dbContext.SaveChanges();
            return articleLike;
        }

        public int UpdateArticleLike(ArticleLike articleLike)
        {
            var databaseArticleLike = GetArticleLike(articleLike.ArticleId, articleLike.PersonId);
            databaseArticleLike.PersonId = articleLike.PersonId;
            databaseArticleLike.IsLiked = articleLike.IsLiked;
            return _dbContext.SaveChanges();
        }

        public int GetArticleCount()
        {
            return _dbContext.Articles.AsNoTracking()
                .Where(a => a.PublishDate.HasValue).Count();
        }

        public int GetArticleByAuthorCount(int personId)
        {
            return _dbContext.Articles
                .AsNoTracking()
                .Where(a => a.PublishDate.HasValue && a.PersonId == personId).Count();
        }

        public EntityState MarkEntityAs<T>(T entity, EntityState entityState) where T : class
        {
            return _dbContext.Entry(entity).State = entityState;
        }
    }
}
