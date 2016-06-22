namespace NewsLive.DataAccess.Models
{
    public class ArticleLikeModel
    {
        public int ArticleId { get; set; }

        public int PersonId { get; set; }

        public bool? IsLiked { get; set; }

    }
}
