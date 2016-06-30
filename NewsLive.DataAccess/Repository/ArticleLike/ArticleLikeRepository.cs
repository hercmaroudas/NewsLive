namespace NewsLive.DataAccess.Repository.ArticleLike
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using NewsLive.DataAccess.Mappings;
    
    public class ArticleLikeRepository : IArticleLikeRepository
    {
        IDataService _service;

        public ArticleLikeRepository(IDataService service)
        {
            _service = service;
        }

        public IEnumerable<Models.ArticleLikeModel> GetAllArticleLikes()
        {
            return _service.GetArticleLikes().ToArticleLikeModelList();
        }

        public async Task<IEnumerable<Models.ArticleLikeModel>> GetAllArticleLikesAsync()
        {
            return await Task.Run(() => _service.GetArticleLikes().ToArticleLikeModelList());
        }

        public Models.ArticleLikeModel ToggleLike(int articleId, int personId)
        {
            var fetchedLike = _service.GetArticleLike(articleId, personId);
            if (fetchedLike == null)
            {
                var addedLike = _service.AddArticleLike(new DataAccess.ArticleLike()
                {
                    ArticleId = articleId,
                    PersonId = personId,
                    IsLiked = true
                });

                return addedLike == null ? null : addedLike.ToArticleLikeModel();
            }

            fetchedLike.IsLiked = !fetchedLike.IsLiked;

            return (_service.UpdateArticleLike(fetchedLike) > 0)
                ? fetchedLike.ToArticleLikeModel()
                : null;
        }

        public async Task<Models.ArticleLikeModel> ToggleLikeAsync(int articleId, int personId)
        {
            var fetchedLike = await _service.GetArticleLikeAsync(articleId, personId);
            if (fetchedLike == null)
            {
                var addedLike = await _service.AddArticleLikeAsync(new DataAccess.ArticleLike()
                {
                    ArticleId = articleId,
                    PersonId = personId,
                    IsLiked = true
                });

                return addedLike == null 
                    ? null 
                    : addedLike.ToArticleLikeModel();
            }

            fetchedLike.IsLiked = !fetchedLike.IsLiked;

            return (await _service.UpdateArticleLikeAsync(fetchedLike) > 0)
                ? fetchedLike.ToArticleLikeModel()
                : null;
        }
    }
}
