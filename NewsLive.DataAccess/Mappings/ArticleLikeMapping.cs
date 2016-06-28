namespace NewsLive.DataAccess.Mappings
{
    using System.Linq;
    using System.Collections.Generic;

    public static class ArticleLikeMapping
    {
        public static DataAccess.ArticleLike ToArticleLikeEntity(this Models.ArticleLikeModel model)
        {
            return new DataAccess.ArticleLike()
                {
                    PersonId = model.PersonId,
                    ArticleId = model.ArticleId,
                    IsLiked = model.IsLiked
                };
        }

        public static Models.ArticleLikeModel ToArticleLikeModel(this DataAccess.ArticleLike entity)
        {
            return new Models.ArticleLikeModel()
                {
                    PersonId = entity.PersonId,
                    ArticleId = entity.ArticleId,
                    IsLiked = entity.IsLiked
                };
        }

        public static IEnumerable<Models.ArticleLikeModel> ToArticleLikeModelList(this IEnumerable<DataAccess.ArticleLike> entities)
        {
            var modelLikes = entities.Select(entitiy =>
                new Models.ArticleLikeModel()
                {
                     ArticleId = entitiy.ArticleId,
                     PersonId = entitiy.PersonId,
                     IsLiked = entitiy.IsLiked
                });

            return modelLikes;
        }

    }
}
