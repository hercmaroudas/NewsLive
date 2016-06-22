namespace NewsLive.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.Linq;

    public static class CommentMapping
    {
        public static DataAccess.Comment ToCommentEntity(this Models.CommentModel model)
        {
            return new DataAccess.Comment()
            {
                ArticleId = model.ArticleId,
                CommentId = model.CommentId,
                PersonId = model.PersonId,
                Comment1 = model.CommentContent
            };
        }

        public static Models.CommentModel ToCommentModel(this DataAccess.Comment entity)
        {
            return new Models.CommentModel()
            {
                ArticleId = entity.ArticleId,
                CommentId = entity.CommentId,
                PersonId = entity.PersonId,
                CommentContent = entity.Comment1
            };
        }

        public static IEnumerable<Models.CommentModel> ToCommentModelList(this IEnumerable<DataAccess.Comment> entities)
        {
            var modelComments = entities.Select(x =>
                new Models.CommentModel()
                {
                    ArticleId = x.ArticleId,
                    CommentId = x.CommentId,
                    PersonId = x.PersonId,
                    CommentContent = x.Comment1,
                    CommentLikes = x.CommentLikes.Select(l =>
                        new Models.CommentLikeModel()
                        {
                            CommentId = l.CommentId,
                            PersonId = l.PersonId,
                            IsLiked = l.IsLiked

                        })});

            return modelComments;
        }
    }
}
