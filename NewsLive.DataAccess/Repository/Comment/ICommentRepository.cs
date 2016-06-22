
namespace NewsLive.DataAccess.Repository.Comment
{
    public interface ICommentRepository
    {
        Models.CommentModel AddComment(Models.CommentModel comment);
    }
}
