namespace NewsLive.DataAccess.Repository.Article
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Services;
    using NewsLive.DataAccess.Models;
    using NewsLive.DataAccess.Mappings;

    public class ArticleRepository : IArticleRepository
    {
        IDataService _dataSrvice;
        IPagingService _pagingService;

        const int defaultPageCount = 1;

        public ArticleRepository(IDataService dataService, IPagingService pagingService)
        {
            _dataSrvice = dataService;
            _pagingService = pagingService;
        }

        public IEnumerable<Models.ArticleModel> GetAllArticles()
        {
            return _dataSrvice.GetArticles().ToArticleModelList(defaultPageCount);
        }
        public IEnumerable<Models.ArticleModel> GetAllArticlesByAuthor(int authorId)
        {
            return _dataSrvice.GetArticles()
                .Where(a => a.PersonId == authorId).ToArticleModelList(defaultPageCount);
        }

        public IEnumerable<Models.ArticleModel> GetAllArticlesByAuthorPaged(int authorId, int numResultsPerPage, int nextPageNum)
        {
            var authorArticles = _dataSrvice.GetArticles()
                .Where(a => a.PersonId == authorId)
                .OrderByDescending(a => a.PublishDate);

            return authorArticles.ToPagedArticleModelList(_pagingService, numResultsPerPage, nextPageNum);
        }

        public IEnumerable<Models.ArticleModel> GetAllArticlesPaged(int numResultsPerPage, int nextPageNum)
        {
            return _dataSrvice.GetArticles()
                .OrderByDescending(a => a.PublishDate)
                .ToPagedArticleModelList(_pagingService, numResultsPerPage, nextPageNum);
        }

        public async Task<IEnumerable<Models.ArticleModel>> GetAllArticlesPagedAsync(int numResultsPerPage, int nextPageNum)
        {
            return await Task.Run(() => _dataSrvice.GetArticlesAsync().Result
                .OrderByDescending(a => a.PublishDate)
                .ToPagedArticleModelList(_pagingService, numResultsPerPage, nextPageNum));
        }

        public Models.ArticleModel GetArticle(int articleId)
        {
            var entity = _dataSrvice.GetArticle(articleId);

            if (entity == null)
                return null;

            return entity.ToArticleModel();
        }

        public Models.ArticleModel PublishArticle(Models.ArticleModel article)
        {
            var entityArticle = article.ToArticleEntity();

            var savedArticle = _dataSrvice.AddArticle(entityArticle);

            if (savedArticle == null)
                return null;

            return savedArticle.ToArticleModel();
        }

        public bool UpdatePublishedArticle(Models.ArticleModel article)
        {
            var existingArticle = GetArticle(article.ArticleId);

            if (existingArticle == null)
                return false;

            var updatedArticle = new DataAccess.Article
            {
                ArticleId = article.ArticleId,
                Title = article.Title,
                Body = article.Body,
                PublishDate = DateTime.Now
            };

            if (_dataSrvice.UpdateArticle(updatedArticle) <= 0)
                return false;

            if (article.Comments != null && article.Comments.Count() > 0 && !string.IsNullOrWhiteSpace(article.Comments.FirstOrDefault().CommentContent))
            {
                var updatedComment = _dataSrvice.AddComment(new DataAccess.Comment()
                {
                    PersonId = article.AuthorId,
                    ArticleId = article.ArticleId,
                    Comment1 = article.Comments.FirstOrDefault().CommentContent
                });

                return (updatedComment != null);
            }

            return true;

        }

        public bool DeletePublishedArticle(int articleId)
        {
            return Convert.ToBoolean(_dataSrvice.DeleteArticle(articleId));
            // ( To test deletions without deleting )
            // return Convert.ToBoolean((_service.GetArticle(articleId)!=null));
        }

        /*
        Example Output: 
        -----------------------------------------------------------------------------
        ArticleId	LikedCount  PersonId    Title
        -----------------------------------------------------------------------------
        1           2           2           Welcome to John Jacks First News Article
        3           1           1           Welcome to Marry Marray's News Article
        -----------------------------------------------------------------------------
        */
        public IEnumerable<GroupedArticleLikeModel> GetGroupedArticleLikes()
        {
            var articles = _dataSrvice.GetArticles();
            var articleLikes = _dataSrvice.GetArticleLikes();

            var groupedArticleLikes = articles
                .Join(articleLikes, article => article.ArticleId, like => like.ArticleId,
                    (article, like) => new
                    {
                        PersonId = article.PersonId,
                        Person = article.Person,
                        ArticleId = article.ArticleId,
                        Article = article,
                        Title = article.Title,
                    })
                    .GroupBy(prod => prod.ArticleId).
                        Select(likesGroup => new GroupedArticleLikeModel()
                        {
                            ArticleId = likesGroup.Key,
                            LikeCount = likesGroup.Count(),
                            Article = likesGroup.FirstOrDefault().Article.ToArticleModel(),
                            Author = likesGroup.FirstOrDefault().Person.ToPersonModel(),
                            AuthorId = likesGroup.FirstOrDefault().PersonId
                        });

            return groupedArticleLikes;
        }

       
    }
}
