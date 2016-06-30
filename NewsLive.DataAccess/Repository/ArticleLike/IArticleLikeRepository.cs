namespace NewsLive.DataAccess.Repository.ArticleLike
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IArticleLikeRepository
    {
        IEnumerable<Models.ArticleLikeModel> GetAllArticleLikes();

        Task<IEnumerable<Models.ArticleLikeModel>> GetAllArticleLikesAsync();

        Models.ArticleLikeModel ToggleLike(int articleId, int personId);

        Task<Models.ArticleLikeModel> ToggleLikeAsync(int articleId, int personId);
    }
}
