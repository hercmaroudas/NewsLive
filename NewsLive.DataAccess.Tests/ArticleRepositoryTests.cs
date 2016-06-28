﻿namespace NewsLive.DataAccess.Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;
    using Mappings;
    
    [TestClass]
    public class ArticleRepositoryTests : BaseRepositoryTest
    {
        [TestMethod]
        public void GetArticleTest()
        {
            var article = articleRepository.GetArticleById(1);

            Assert.AreEqual(1, article.ArticleId);
            Assert.AreEqual(1, article.AuthorId);

            Assert.AreEqual("Please feel free to like or comment on my article below!", article.Body);
            Assert.AreEqual("Welcome to John Jack's First News Article", article.Title);
            Assert.AreEqual("John", article.Author.FirstName);
            Assert.AreEqual("Jack", article.Author.LastName);
            Assert.AreEqual(1, article.Author.PersonId);

            Assert.IsTrue(article.Comments.Count() > 0);
            Assert.IsTrue(article.ArticleLikes.Count() > 0);
            Assert.IsTrue(article.PublishDate < DateTime.Now);
        }

        [TestMethod]
        public void GetAllPublishedArticlesTest()
        {
            var numberOfArticles = articleRepository.GetAllArticlesPaged(100, 0).Where(a => a.IsPublished).Count();

            Assert.AreEqual(4, numberOfArticles);
        }

        [TestMethod]
        public void GetArticlesByAuthorTest()
        {
            var numberOfArticles = articleRepository.GetAllArticlesByAuthor(1).Count();

            Assert.AreEqual(2, numberOfArticles);

            var articles = articleRepository.GetAllArticlesByAuthorPaged(1, 1, 1);
        }

        [TestMethod]
        public void GetAllArticlesPagedAndArticlesInDbAreLessThanNumResultsPerPageTest()
        {
            var articles = articleRepository.GetAllArticlesPaged(10, 1);

            int numberOfPages = articles.First().NumberOfPages;
            int numberOfArticles = articles.Count();

            Assert.AreEqual(1, numberOfPages);
            Assert.AreEqual(4, numberOfArticles);
        }

        [TestMethod]
        public void GetAllArticlesPagedOneResultPerPageSmallDatasetTest()
        {
            var articles = articleRepository.GetAllArticlesPaged(1, 1);
            int numberOfPages = articles.First().NumberOfPages;
            int numberOfArticles = articles.Count();

            Assert.AreEqual(4, numberOfPages);
            Assert.AreEqual(1, numberOfArticles);

            // one per page second page
            articles = articleRepository.GetAllArticlesPaged(1, 2);
            numberOfPages = articles.First().NumberOfPages;
            numberOfArticles = articles.Count();

            Assert.AreEqual(4, numberOfPages);
            Assert.AreEqual(1, numberOfArticles);

            // one per page third page
            articles = articleRepository.GetAllArticlesPaged(1, 3);
            numberOfPages = articles.First().NumberOfPages;
            numberOfArticles = articles.Count();

            Assert.AreEqual(4, numberOfPages);
            Assert.AreEqual(1, numberOfArticles);

            // one per page last page
            articles = articleRepository.GetAllArticlesPaged(1, 4);
            numberOfPages = articles.First().NumberOfPages;
            numberOfArticles = articles.Count();

            Assert.AreEqual(4, numberOfPages);
            Assert.AreEqual(1, numberOfArticles);
        }

        [TestMethod]
        public void GetAllArticlesPagedOneResultPerPageLargeDatasetTest()
        {
            var articles = pagedArticleRepository.GetAllArticlesPaged(1, 1);

            int numberOfPages = articles.First().NumberOfPages;
            int numberOfArticles = articles.Count();

            Assert.AreEqual(138, numberOfPages);
            Assert.AreEqual(1, numberOfArticles);

            Assert.AreEqual(166, articles.First().ArticleId);
            Assert.AreEqual(166, articles.Last().ArticleId);

            // one result second page
            articles = pagedArticleRepository.GetAllArticlesPaged(1, 2);

            numberOfPages = articles.First().NumberOfPages;
            numberOfArticles = articles.Count();

            Assert.AreEqual(138, numberOfPages);
            Assert.AreEqual(1, numberOfArticles);

            Assert.AreEqual(164, articles.First().ArticleId);
            Assert.AreEqual(164, articles.Last().ArticleId);

            // one result last page
            articles = pagedArticleRepository.GetAllArticlesPaged(1, 138);

            numberOfPages = articles.First().NumberOfPages;
            numberOfArticles = articles.Count();

            Assert.AreEqual(138, numberOfPages);
            Assert.AreEqual(1, numberOfArticles);

            Assert.AreEqual(1, articles.First().ArticleId);
            Assert.AreEqual(1, articles.Last().ArticleId);
        }

        [TestMethod]
        public void GetAllArticlesPagedAndThereAreManyArticlesWithOddNumberAndLastPageIsValid()
        {
            var articles = pagedArticleRepository.GetAllArticlesPaged(10, 14);

            int numberOfPages = articles.First().NumberOfPages;
            int numberOfArticles = articles.Count();

            Assert.AreEqual(14, numberOfPages);
            Assert.AreEqual(8, numberOfArticles);

            var firstArticle = articles.First();
            var lastArticle = articles.Last();

            Assert.AreEqual(8, firstArticle.ArticleId);
            Assert.AreEqual(1, lastArticle.ArticleId);

            Assert.IsTrue(firstArticle.PublishDate > lastArticle.PublishDate);

        }

        [TestMethod]
        public void GetAllArticlesByAuthorPagedNonPublishedTest()
        {
            var authorId = 0;
            var nextPageNum = 5;
            var numResultsPerPage = 10;

            var numberOfArticles = pagedArticleRepository.GetAllArticlesByAuthorPaged(
                    authorId, numResultsPerPage, nextPageNum).Count();

            Assert.AreEqual(0, numberOfArticles);
        }

        [TestMethod]
        public void GetAllPaublishedArticlesByAuthorPagedTest()
        {
            var authorId = 1;
            var nextPageNum = 5;
            var numResultsPerPage = 10;

            var numberOfArticles = pagedArticleRepository.GetAllArticlesByAuthorPaged(authorId, numResultsPerPage, nextPageNum)
                .Count();

            Assert.AreEqual(10, numberOfArticles);
        }

        [TestMethod]
        public void GetGroupedArticleLikesCheckTest()
        {
            var articles = articleRepository.GetAllArticlesPaged(100, 1);
            var articleLikes = articleLikeRepository.GetAllArticleLikes();
            Assert.IsTrue(articles.Count() == 4, "Actual article count is {0}", articles.Count());
            Assert.IsTrue(articleLikes.Count() == 4, "Actual article like count is {0}", articleLikes.Count());

            var groupedArticles = articleRepository.GetGroupedArticleLikes();

            Assert.IsTrue(groupedArticles.Count() == 2, "Actual grouped article count is {0}", groupedArticles.Count());

            var groupLikeOne = groupedArticles.Where((l, index) => index == 0).FirstOrDefault();
            var groupLikeTwo = groupedArticles.Where((l, index) => index == 1).FirstOrDefault();

            var expectedArticleIdLikesOne = 3;
            var expectedLikesOneCount = 1;
            var expectedArticleTitleLikesOne = "Welcome to Marry Murray's News Article";
            var expectedArticleAuthorNameLikesOne = "Marry";

            var expectedArticleIdLikesTwo = 1;
            var expectedLikesTwoCount = 3;
            var expectedArticleTitleLikesTwo = "Welcome to John Jack's First News Article";
            var expectedArticleAuthorNameLikesTwo = "John";

            Assert.AreEqual(expectedArticleIdLikesOne, groupLikeOne.ArticleId);
            Assert.AreEqual(expectedLikesOneCount, groupLikeOne.LikeCount);
            Assert.AreEqual(expectedArticleTitleLikesOne, groupLikeOne.Article.Title);
            Assert.AreEqual(expectedArticleAuthorNameLikesOne, groupLikeOne.Author.FirstName);

            Assert.AreEqual(expectedArticleIdLikesTwo, groupLikeTwo.ArticleId);
            Assert.AreEqual(expectedLikesTwoCount, groupLikeTwo.LikeCount);
            Assert.AreEqual(expectedArticleTitleLikesTwo, groupLikeTwo.Article.Title);
            Assert.AreEqual(expectedArticleAuthorNameLikesTwo, groupLikeTwo.Author.FirstName);
        }

        [TestMethod]
        public void PublishArticleTest()
        {
            var newArticle = new Models.ArticleModel()
                {
                    Title = "New Article",
                    Body = "New Article Body",
                    PublishDate = DateTime.Now,
                    AuthorId = 1,
                    Author = PeopleEntities.ElementAt(0).ToPersonModel(),
                };

            var expected = new DataAccess.Article()
                {
                    ArticleId = 123,
                    Title = "New Article",
                    Body = "New Article Body",
                    PublishDate = newArticle.PublishDate,
                    PersonId = 1,
                    Person = PeopleEntities.ElementAt(0)
            };

            ArticleDbSetMock.Setup(m => m.Add(It.IsAny<DataAccess.Article>())).Returns(expected);

            var publishedArticle = articleRepository.PublishArticle(newArticle);

            Assert.AreEqual(expected.ArticleId, publishedArticle.ArticleId);
            Assert.AreEqual(expected.Title, publishedArticle.Title);
            Assert.AreEqual(expected.Body, publishedArticle.Body);
            Assert.AreEqual(expected.PublishDate, publishedArticle.PublishDate);
            Assert.IsNotNull(publishedArticle.IsPublished);
            Assert.AreEqual(expected.Person.FirstName, publishedArticle.Author.FirstName);
            Assert.AreEqual(expected.Person.LastName, publishedArticle.Author.LastName);

            ArticleDbSetMock.Verify(m => m.Add(It.IsAny<DataAccess.Article>()), Times.Once());
            NewsLiveDbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void UpdatePublishedArticleTest()
        {
            var dbArticle = articleRepository.GetArticleById(1);
            var publishDate = dbArticle.PublishDate;

            var editedArticle = new Models.ArticleModel()
            {
                ArticleId = 1,
                Title = "Changed Title",
                Body = "Changed Body",
                PublishDate = publishDate.Value
            };

            NewsLiveDbContextMock.Setup(m => m.SaveChanges())
                .Returns(1);

            var updated = articleRepository.UpdatePublishedArticle(editedArticle);
            var dbArticleAfterEdit = articleRepository.GetArticleById(editedArticle.ArticleId);

            NewsLiveDbContextMock.Verify(m => m.SaveChanges(), Times.AtLeastOnce());

            Assert.AreEqual(true, updated);
            Assert.AreEqual(editedArticle.ArticleId, dbArticleAfterEdit.ArticleId);
            Assert.AreEqual(editedArticle.Title, dbArticleAfterEdit.Title);
            Assert.AreEqual(editedArticle.Body, dbArticleAfterEdit.Body);
        }

        [TestMethod]
        public void DeletePublishedArticleTest()
        {
            var removedArticle = articleRepository.GetArticleById(1);
            Assert.IsNotNull(removedArticle);

            ArticleDbSetMock.Setup(m => m.Remove(It.IsAny<DataAccess.Article>()))
                .Returns(removedArticle.ToArticleEntity());

            var isRemoved = articleRepository.DeletePublishedArticle(1);
            Assert.IsTrue(isRemoved);

            ArticleDbSetMock.Verify(m => m.Remove(It.IsAny<DataAccess.Article>()), Times.Once());
            NewsLiveDbContextMock.Verify(m => m.SaveChanges(), Times.AtLeastOnce());
        }
    }
}
