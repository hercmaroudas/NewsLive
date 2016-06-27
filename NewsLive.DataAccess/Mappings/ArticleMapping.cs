namespace NewsLive.DataAccess.Mappings
{
    using Services;

    using System;
    using System.Linq;
    using System.Collections.Generic;

    public static class ArticleMapping
    {
        public static DataAccess.Article ToArticleEntity(this Models.ArticleModel model)
        {
            return new DataAccess.Article()
            {
                PersonId = model.AuthorId,
                ArticleId = model.ArticleId,
                Title = model.Title,
                Body = model.Body,
                PublishDate = model.PublishDate
            };
        }

        public static Models.ArticleModel ToArticleModel(this DataAccess.Article entity)
        {
            return ToArticleModelList(new List<DataAccess.Article>() { entity }, 1).FirstOrDefault();
        }

        public static IEnumerable<Models.ArticleModel> ToArticleModelList(this IEnumerable<DataAccess.Article> entities, int pageCount)
        {
            var modelArticles = entities.Select(entity =>
                new Models.ArticleModel()
                {
                    NumberOfPages = pageCount,
                    AuthorId = entity.PersonId,
                    ArticleId = entity.ArticleId,
                    Title = entity.Title,
                    Body = entity.Body,
                    PublishDate = entity.PublishDate,
                    IsPublished = entity.PublishDate.HasValue,
                    Likes = entity.Likes.ToArticleLikeModelList(),
                    Comments = entity.Comments.ToCommentModelList(),
                    Author = entity.Person.ToPersonModel(),
                });

            return modelArticles;
        }

        public static IEnumerable<Models.ArticleModel> ToPagedArticleModelList
            (this IEnumerable<DataAccess.Article> articleEntities, IPagingService pagingService, int numResultsPerPage, int nextPageNum)
        {
            var numArticles = articleEntities.Count();

            var pagerResult = pagingService
                .CalculatePagingMetric(numArticles, numResultsPerPage, nextPageNum);

            var pagedArticles = articleEntities.
                ToArticleModelList(pagerResult.PageCount)
                .Skip(pagerResult.PageFrom)
                .Take(numResultsPerPage);

            return pagedArticles;
        }
    }
}
