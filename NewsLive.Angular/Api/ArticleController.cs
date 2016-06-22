namespace NewsLive.Angular.Api
{
    using System.Web.Http;
    using System.Collections.Generic;

    using DataAccess.Models;
    using DataAccess.Repository.Article;

    public class ArticleController : ApiController
    {
        IArticleRepository _repository;
        
        public ArticleController(IArticleRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Article/GetAllArticles
        [HttpGet]
        public IEnumerable<ArticleModel> GetAllArticles()
        {
            return _repository.GetAllArticles();
        }

        // GET: api/Article/GetAllArticlesPaged/10/1
        [HttpGet]
        public IEnumerable<ArticleModel> GetAllArticlesPaged(int numResultsPerPage, int currentPageNum)
        {
            return _repository.GetAllArticlesPaged(numResultsPerPage, currentPageNum);
        }

        public // GET: api/Article/GetAllArticlesByAuthorPaged/1/2/3
        IEnumerable<ArticleModel> GetAllArticlesByAuthorPaged(int authorId, int numResultsPerPage, int currentPageNum)
        {
            return _repository.GetAllArticlesByAuthorPaged(authorId, numResultsPerPage, currentPageNum);
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