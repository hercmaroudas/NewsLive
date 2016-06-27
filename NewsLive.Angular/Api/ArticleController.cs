namespace NewsLive.Angular.Api
{
    using System.Web.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Caching;
    using DataAccess.Models;
    using DataAccess.Repository.Article;

    public class ArticleController : ApiController
    {
        IArticleRepository _repository;
        ICacheService _cacheService;
        
        public ArticleController(IArticleRepository repository, ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        // GET: api/Article/GetAllArticles
        [HttpGet]
        public IEnumerable<ArticleModel> GetAllArticles()
        {
            return _repository.GetAllArticles();
        }

        // GET: api/Article/GetAllArticlesPaged/10/1
        [HttpGet]
        public IEnumerable<ArticleModel> GetAllArticlesPaged(int numResultsPerPage, int nextPageNum)
        {
            return _repository.GetAllArticlesPaged(numResultsPerPage, nextPageNum);
        }

        [HttpGet]
        public async Task<IEnumerable<ArticleModel>> GetAllArticlesPagedAsync(int numResultsPerPage, int nextPageNum)
        {
            return await _repository.GetAllArticlesPagedAsync(numResultsPerPage, nextPageNum);
        }

        // GET: api/Article/GetAllArticlesByAuthorPaged/1/2/3
        [HttpGet]
        public IEnumerable<ArticleModel> GetAllArticlesByAuthorPaged(int authorId, int numResultsPerPage, int nextPageNum)
        {
            return _repository.GetAllArticlesByAuthorPaged(authorId, numResultsPerPage, nextPageNum);
        }

        // GET: api/Article/GetGroupedArticleLikes
        [HttpGet]
        public IEnumerable<GroupedArticleLikeModel> GetGroupedArticleLikes()
        {
            return _repository.GetGroupedArticleLikes();
        }

        // GET: api/Article/GetArticle/5
        [HttpGet]
        public ArticleModel GetArticle(int articleId)
        {
            return _repository.GetArticle(articleId);
        }

        // GET: api/Article/GetAllArticlesByAuthor/5
        public IEnumerable<ArticleModel> GetAllArticlesByAuthor(int authorId)
        {
            return _repository.GetAllArticlesByAuthor(authorId);
        }

        // POST: api/Article/PublishArticle
        [HttpPost]
        public ArticleModel PublishArticle(ArticleModel article)
        {
            return _repository.PublishArticle(article);
        }

        // PUT: api/Article/UpdatePublishedArticle
        [HttpPut]
        public bool UpdatePublishedArticle(ArticleModel article)
        {
            return _repository.UpdatePublishedArticle(article);
        }

        // DELETE: api/Article/DeletePublishedArticle/5
        [HttpDelete]
        public bool DeletePublishedArticle(int articleId)
        {
            return _repository.DeletePublishedArticle(articleId);
        }
    }
}