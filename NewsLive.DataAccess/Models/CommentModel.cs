namespace NewsLive.DataAccess.Models
{
    using System.Collections.Generic;

    public class CommentModel
    {
        public int CommentId { get; set; }

        public int ArticleId { get; set; }

        public int? PersonId { get; set; }

        public string commentText { get; set; }

        public virtual IEnumerable<Models.CommentLikeModel> CommentLikes { get; set; }
    }
}
