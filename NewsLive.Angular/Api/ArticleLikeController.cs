namespace NewsLive.Angular.Api
{
    using System.Web.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using DataAccess.Models;
    using DataAccess.Repository.ArticleLike;
    
    public class ArticleLikeController : ApiController
    {
        IArticleLikeRepository _repository;

        public ArticleLikeController(IArticleLikeRepository repository)
        {
            _repository = repository;
        }

        // GET: api/ArticleLike/GetAllArticleLikes
        [HttpGet]
        public IEnumerable<ArticleLikeModel> GetAllArticleLikes()
        {
            return _repository.GetAllArticleLikes();
        }

        [HttpGet]
        public async Task<IEnumerable<ArticleLikeModel>> GetAllArticleLikesAsync()
        {
            return await _repository.GetAllArticleLikesAsync();
        }

        // POST: api/ArticleLike/ToggleLike
        [HttpPost]
        public ArticleLikeModel ToggleLike(ToggleLikeParameters parameter)
        {
            return _repository.ToggleLike(parameter.ArticleId, parameter.PersonId);
        }

        [HttpPost]
        public async Task<ArticleLikeModel> ToggleLikeAsync(ToggleLikeParameters parameter)
        {
            return await _repository.ToggleLikeAsync(parameter.ArticleId, parameter.PersonId);
        }


        public class ToggleLikeParameters
        {
            public int ArticleId { get; set; }
            public int PersonId { get; set; }
        }

    }
}
