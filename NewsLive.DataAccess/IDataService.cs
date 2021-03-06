﻿namespace NewsLive.DataAccess
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDataService
    {
        Membership GetMembership(string userName, string password);

        Article GetArticle(int articleId);

        IQueryable<Article> GetArticles();

        Task<IQueryable<Article>> GetArticlesAsync();

        Article AddArticle(Article article);

        int UpdateArticle(Article article);

        int DeleteArticle(int articleId);

        IQueryable<Like> GetArticleLikes();

        Like GetArticleLike(int articleId, int personId);

        Like AddArticleLike(Like articleLike);

        int UpdateArticleLike(Like articleLike);

        Comment AddComment(Comment comment);

        EntityState MarkEntityAs<T>(T entity, EntityState entityState) where T : class;

    }
}
