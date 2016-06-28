namespace NewsLive.DataAccess.Repository.ArticleLike
{
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

        public Models.ArticleLikeModel ToggleLike(int articleId, int personId)
        {
            var fetchedALike = _service.GetArticleLike(articleId, personId);
            if (fetchedALike == null)
            {
                var addedLike = _service.AddArticleLike(new DataAccess.ArticleLike()
                {
                    ArticleId = articleId,
                    PersonId = personId,
                    IsLiked = true
                });

                return addedLike == null ? null : addedLike.ToArticleLikeModel();
            }

            fetchedALike.IsLiked = !fetchedALike.IsLiked;

            return (_service.UpdateArticleLike(fetchedALike) > 0)
                ? fetchedALike.ToArticleLikeModel()
                : null;
        }
    }
}
