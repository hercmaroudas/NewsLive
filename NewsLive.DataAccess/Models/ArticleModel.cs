namespace NewsLive.DataAccess.Models
{
    using System;
    using System.Collections.Generic;

    public class ArticleModel
    {
        public int ArticleId { get; set; }

        public int NumberOfPages { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime? PublishDate { get; set; }

        public bool IsPublished { get; set; }

        public int AuthorId { get; set; }

        public Models.PersonModel Author { get; set; }

        public IEnumerable<Models.CommentModel> Comments { get; set; }

        public IEnumerable<Models.ArticleLikeModel> ArticleLikes { get; set; }

    }
}
