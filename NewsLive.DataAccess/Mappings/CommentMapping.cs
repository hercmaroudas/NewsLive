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
                CommentText = model.commentText
            };
        }

        public static Models.CommentModel ToCommentModel(this DataAccess.Comment entity)
        {
            return new Models.CommentModel()
            {
                ArticleId = entity.ArticleId,
                CommentId = entity.CommentId,
                PersonId = entity.PersonId,
                commentText = entity.CommentText
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
                    commentText = x.CommentText,
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
