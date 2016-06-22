namespace NewsLive.DataAccess
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;

    // TODO: Separate into partial classes 
    public class DataService : IDataService
    {
        NewsLiveDbContext _entities;

        public DataService()
        {
            _entities = new NewsLiveDbContext();

            _entities.Configuration.LazyLoadingEnabled = true;
        }

        public DataService(NewsLiveDbContext entities)
        {
            _entities = entities;
        }

        public Membership GetMembership(string userName, string password)
        {
            return _entities.Memberships
                .Where(m => m.UserName == userName && m.Password == password)
                .FirstOrDefault();
        }

        public IQueryable<Article> GetArticles()
        {
            return _entities.Articles;
        }

        public Article GetArticle(int articleId)
        {
            return _entities.Articles.Where(a => a.ArticleId == articleId).FirstOrDefault();
        }

        public Article AddArticle(Article article)
        {
            article = _entities.Articles.Add(article);
            _entities.SaveChanges();
            return article;
        }

        public int UpdateArticle(Article article)
        {
            var databaseArticle = GetArticle(article.ArticleId);
            if (databaseArticle != null)
            { 
                databaseArticle.Title = article.Title;
                databaseArticle.Body = article.Body;
                databaseArticle.PublishDate = article.PublishDate;
                return _entities.SaveChanges();
            }
            return 0;
        }

        public int DeleteArticle(int articleId)
        {
            var databaseArticle = GetArticle(articleId);
            if (databaseArticle != null)
            {
                databaseArticle = _entities.Articles.Remove(databaseArticle);
                _entities.SaveChanges();
            }
            return Convert.ToInt32(databaseArticle != null);
        }

        public Comment AddComment(Comment comment)
        {
            comment = _entities.Comments.Add(comment);
            _entities.SaveChanges();
            return comment;
        }

        public IQueryable<Like> GetArticleLikes()
        {
            return _entities.Likes;
        }

        public Like GetArticleLike(int articleId, int personId)
        {
            return _entities.Likes.Where(l => l.ArticleId == articleId && l.PersonId == personId).FirstOrDefault();
        }

        public Like AddArticleLike(Like articleLike)
        {
            articleLike = _entities.Likes.Add(articleLike);
            _entities.SaveChanges();
            return articleLike;
        }

        public int UpdateArticleLike(Like articleLike)
        {
            var databaseArticleLike = GetArticleLike(articleLike.ArticleId, articleLike.PersonId);
            databaseArticleLike.PersonId = articleLike.PersonId;
            databaseArticleLike.IsLiked = articleLike.IsLiked;
            return _entities.SaveChanges();
        }

        public EntityState MarkEntityAs<T>(T entity, EntityState entityState) where T : class
        {
            return _entities.Entry(entity).State = entityState;
        }
    }
}
