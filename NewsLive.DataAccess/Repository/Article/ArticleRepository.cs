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
        IDataService _dataService;
        IPagingService _pagingService;

        const int defaultPageCount = 1;

        public ArticleRepository(IDataService dataService, IPagingService pagingService)
        {
            _dataService = dataService;
            _pagingService = pagingService;
        }

        public IEnumerable<Models.ArticleModel> GetAllArticlesByAuthor(int personId)
        {
            return _dataService.GetArticlesByAuthorId(personId)
                .ToArticleModelList(defaultPageCount);
        }

        public async Task<IEnumerable<Models.ArticleModel>> GetAllArticlesByAuthorAsync(int personId)
        {
            return await Task.Run(() => _dataService.GetArticlesByAuthorId(personId).ToArticleModelList(defaultPageCount));
        }

        public IEnumerable<Models.ArticleModel> GetAllArticlesByAuthorPaged(int authorId, int numResultsPerPage, int nextPageNum)
        {
            var numArticles = _dataService.GetArticleByAuthorCount(authorId);

            var result = _pagingService.CalculatePagingMetric(numArticles, numResultsPerPage, nextPageNum);

            var min = result.PageFrom;
            var max = numResultsPerPage;

            var articles = _dataService.GetArticlesByAuthorId(authorId, min, max);

            return articles.ToArticleModelList(result.PageCount);
        }

        public async Task<IEnumerable<Models.ArticleModel>> GetAllArticlesByAuthorPagedAsync(int authorId, int numResultsPerPage, int nextPageNum)
        {
            var numArticles = await _dataService.GetArticleByAuthorCountAsync(authorId);

            var result = _pagingService.CalculatePagingMetric(numArticles, numResultsPerPage, nextPageNum);

            var min = result.PageFrom;
            var max = numResultsPerPage;

            var articles = await Task.Run(() => _dataService.GetArticlesByAuthorId(authorId, min, max));

            return articles.ToArticleModelList(result.PageCount);
        }

        public IEnumerable<Models.ArticleModel> GetAllArticlesPaged(int numResultsPerPage, int nextPageNum)
        {
            var numArticles = _dataService.GetArticleCount();

            var result = _pagingService.CalculatePagingMetric(numArticles, numResultsPerPage, nextPageNum);

            var min = result.PageFrom;
            var max = numResultsPerPage;

            var articles = _dataService.GetArticles(min, max);

            return articles.ToArticleModelList(result.PageCount);
        }

        public async Task<IEnumerable<Models.ArticleModel>> GetAllArticlesPagedAsync(int numResultsPerPage, int nextPageNum)
        {
            var numArticles = await _dataService.GetArticleCountAsync();

            var result = _pagingService.CalculatePagingMetric(numArticles, numResultsPerPage, nextPageNum);

            var min = result.PageFrom;
            var max = numResultsPerPage;

            var articles = await Task.Run(() => _dataService.GetArticles(min, max));

            return articles.ToArticleModelList(result.PageCount);
        }

        public Models.ArticleModel GetArticleById(int articleId)
        {
            var entity = _dataService.GetArticleById(articleId);

            if (entity == null)
                return null;

            return entity.ToArticleModel();
        }

        public async Task<Models.ArticleModel> GetArticleByIdAsync(int articleId)
        {
            var entity = await _dataService.GetArticleByIdAsync(articleId);

            if (entity == null)
                return null;

            return entity.ToArticleModel();
        }

        public Models.ArticleModel PublishArticle(Models.ArticleModel article)
        {
            var entityArticle = article.ToArticleEntity();

            var savedArticle = _dataService.AddArticle(entityArticle);

            if (savedArticle == null)
                return null;

            return savedArticle.ToArticleModel();
        }

        public async Task<Models.ArticleModel> PublishArticleAsync(Models.ArticleModel article)
        {
            var entityArticle = article.ToArticleEntity();

            var savedArticle = await _dataService.AddArticleAsync(entityArticle);

            if (savedArticle == null)
                return null;

            return savedArticle.ToArticleModel();
        }

        public bool UpdatePublishedArticle(Models.ArticleModel article)
        {
            var existingArticle = GetArticleById(article.ArticleId);

            if (existingArticle == null)
                return false;

            var updatedArticle = new DataAccess.Article
            {
                ArticleId = article.ArticleId,
                Title = article.Title,
                Body = article.Body,
                PublishDate = DateTime.Now
            };

            if (_dataService.UpdateArticle(updatedArticle) <= 0)
                return false;

            if (article.Comments != null && article.Comments.Count() > 0 && !string.IsNullOrWhiteSpace(article.Comments.FirstOrDefault().commentText))
            {
                var updatedComment = _dataService.AddComment(new DataAccess.Comment()
                {
                    PersonId = article.AuthorId,
                    ArticleId = article.ArticleId,
                    CommentText = article.Comments.FirstOrDefault().commentText
                });

                return (updatedComment != null);
            }

            return true;

        }

        public async Task<bool> UpdatePublishedArticleAsync(Models.ArticleModel article)
        {
            var existingArticle = await GetArticleByIdAsync(article.ArticleId);

            if (existingArticle == null)
                return false;

            var updatedArticle = new DataAccess.Article
            {
                ArticleId = article.ArticleId,
                Title = article.Title,
                Body = article.Body,
                PublishDate = DateTime.Now
            };

            if (await _dataService.UpdateArticleAsync(updatedArticle) <= 0)
                return false;

            if (article.Comments != null && article.Comments.Count() > 0 && !string.IsNullOrWhiteSpace(article.Comments.FirstOrDefault().commentText))
            {
                var updatedComment = await _dataService.AddCommentAsync(new DataAccess.Comment()
                {
                    PersonId = article.AuthorId,
                    ArticleId = article.ArticleId,
                    CommentText = article.Comments.FirstOrDefault().commentText
                });

                return (updatedComment != null);
            }

            return true;
        }


        public bool DeletePublishedArticle(int articleId)
        {
            return Convert.ToBoolean(_dataService.DeleteArticle(articleId));
            // ( To test deletions without deleting )
            // return Convert.ToBoolean((_service.GetArticle(articleId)!=null));
        }

        public async Task<bool> DeletePublishedArticleAsync(int articleId)
        {
            var deleted = await _dataService.DeleteArticleAsync(articleId);

            return Convert.ToBoolean(deleted);
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
            var articles = _dataService.GetArticles();
            var articleLikes = _dataService.GetArticleLikes();

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

        public async Task<IEnumerable<Models.GroupedArticleLikeModel>> GetGroupedArticleLikesAsync()
        {
            return await Task.Run(() => GetGroupedArticleLikes());
        }
    }
}
