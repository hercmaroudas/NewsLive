namespace NewsLive.Angular.Api
{
    using System.Web.Http;
    using System.Threading.Tasks;

    using DataAccess.Models;
    using DataAccess.Repository.Comment;

    public class CommentController : ApiController
    {
        ICommentRepository _repository;

        public CommentController(ICommentRepository repository)
        {
            _repository = repository;
        }

        // PUT: api/Comment/AddComment
        [HttpPut]
        public CommentModel AddComment(CommentModel comment)
        {
            return _repository.AddComment(comment);
        }

        [HttpPut]
        public async Task<CommentModel> AddCommentAsync(CommentModel comment)
        {
            return await _repository.AddCommentAsync(comment);
        }
    }
}
