namespace NewsLive.DataAccess.Models
{
    public class GroupedArticleLikeModel
    {
        public int ArticleId { get; set; }

        public ArticleModel Article { get; set; }

        public int AuthorId { get; set; }

        public PersonModel Author { get; set; }

        public int LikeCount { get; set; }
    }
}
