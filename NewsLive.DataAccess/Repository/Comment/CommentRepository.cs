namespace NewsLive.DataAccess.Repository.Comment
{
    using NewsLive.DataAccess.Mappings;

    public class CommentRepository : ICommentRepository
    {
        IDataService _service;

        public CommentRepository(IDataService service)
        {
            _service = service;
        }

        public Models.CommentModel AddComment(Models.CommentModel comment)
        {
            var entityComment = comment.ToCommentEntity();

            var savedComment = _service.AddComment(entityComment);

            if (savedComment == null)
                return null;

            return savedComment.ToCommentModel();
        }
    }
}
