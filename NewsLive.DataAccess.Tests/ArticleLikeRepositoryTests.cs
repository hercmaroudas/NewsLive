namespace NewsLive.DataAccess.Tests
{
    using Moq;

    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ArticleLikeRepositoryTests : BaseRepositoryTest
    {
        [TestMethod]
        public void GetAllArticleLikesTest()
        {
            var numArticleLikes = articleLikeRepository.GetAllArticleLikes().Count();

            Assert.AreEqual(4, numArticleLikes);
        }

        [TestMethod]
        public void ToggleArticleLikeAndArticleDoesNotExist()
        {
            var newLike = new DataAccess.Like()
            { ArticleId = 10, PersonId = 3 };

            var nullLike = articleLikeRepository.ToggleLike(newLike.ArticleId, newLike.PersonId);
            Assert.IsNull(nullLike);
        }

        [TestMethod]
        public void ToggleArticleLikeAndLikeDoesNotExistTest()
        {
            var newLike = new DataAccess.Like()
                { ArticleId = 2, PersonId = 3, IsLiked = true };

            ArticleLikeDbSetMock.Setup(m => m.Add(It.IsAny<Like>()))
                .Returns(newLike);

            var addedLike = articleLikeRepository.ToggleLike(newLike.ArticleId, newLike.PersonId);

            ArticleLikeDbSetMock.Verify(m => m.Add(It.IsAny<DataAccess.Like>()), Times.AtLeastOnce());
            NewsLiveDbContextMock.Verify(m => m.SaveChanges(), Times.AtLeastOnce());

            Assert.IsNotNull(addedLike.IsLiked);
            Assert.IsTrue(addedLike.IsLiked.Value);
        }

        [TestMethod]
        public void ToggleArticleLikeAndLikeDoesExistTest()
        {
            var articleLike = new DataAccess.Like()
                { ArticleId = 1, PersonId = 2, IsLiked = true };

            NewsLiveDbContextMock.Setup(m => m.SaveChanges())
                .Returns(1);
            ArticleLikeDbSetMock.Setup(m => m.Add(It.IsAny<Like>()))
                .Returns(articleLike);

            var updatedLike = articleLikeRepository.ToggleLike(articleLike.ArticleId, articleLike.PersonId);

            NewsLiveDbContextMock.Verify(m => m.SaveChanges(), Times.AtLeastOnce());

            Assert.IsFalse(updatedLike.IsLiked.Value);
        }
    }
}
