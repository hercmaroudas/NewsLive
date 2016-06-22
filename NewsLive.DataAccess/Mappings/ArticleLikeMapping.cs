namespace NewsLive.DataAccess.Mappings
{
    using System.Linq;
    using System.Collections.Generic;

    public static class ArticleLikeMapping
    {
        public static DataAccess.Like ToArticleLikeEntity(this Models.ArticleLikeModel model)
        {
            return new DataAccess.Like()
                {
                    PersonId = model.PersonId,
                    ArticleId = model.ArticleId,
                    IsLiked = model.IsLiked
                };
        }

        public static Models.ArticleLikeModel ToArticleLikeModel(this DataAccess.Like entity)
        {
            return new Models.ArticleLikeModel()
                {
                    PersonId = entity.PersonId,
                    ArticleId = entity.ArticleId,
                    IsLiked = entity.IsLiked
                };
        }

        public static IEnumerable<Models.ArticleLikeModel> ToArticleLikeModelList(this IEnumerable<DataAccess.Like> entities)
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
