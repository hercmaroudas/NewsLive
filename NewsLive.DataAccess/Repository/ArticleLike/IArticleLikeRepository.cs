namespace NewsLive.DataAccess.Repository.ArticleLike
{
    using System.Collections.Generic;

    public interface IArticleLikeRepository
    {
        IEnumerable<Models.ArticleLikeModel> GetAllArticleLikes();

        Models.ArticleLikeModel ToggleLike(int articleId, int personId);
    }
}
