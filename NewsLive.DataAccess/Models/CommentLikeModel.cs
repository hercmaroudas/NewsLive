namespace NewsLive.DataAccess.Models
{
    public class CommentLikeModel
    {
        public int CommentId { get; set; }

        public int? PersonId { get; set; }

        public bool? IsLiked { get; set; }
    }
}
