namespace NewsLive.DataAccess.Repository.Comment
{
    using System.Threading.Tasks;

    public interface ICommentRepository
    {
        Models.CommentModel AddComment(Models.CommentModel comment);

        Task<Models.CommentModel> AddCommentAsync(Models.CommentModel comment);
    }
}
