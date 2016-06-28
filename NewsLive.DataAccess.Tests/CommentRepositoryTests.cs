namespace NewsLive.DataAccess.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class CommentRepositoryTests : BaseRepositoryTest
    {
        [TestMethod]
        public void AddCommentTest()
        {
            var newComment = new Models.CommentModel()
            {
                ArticleId = 1,
                PersonId = 1,
                commentText = "Comment one on Article 1"
            };

            var expected = new DataAccess.Comment()
            {
                CommentId = 123,
                ArticleId = 1,
                PersonId = 1,
                CommentText = "Comment one on Article 1"
            };

            CommentDbSetMock.Setup(m => m.Add(It.IsAny<DataAccess.Comment>()))
                .Returns(expected);

            var addedComment = commentRepository.AddComment(newComment);

            Assert.AreEqual(expected.ArticleId, addedComment.ArticleId);
            Assert.AreEqual(expected.CommentId, addedComment.CommentId);
            Assert.AreEqual(expected.PersonId, addedComment.PersonId);
            Assert.AreEqual(expected.CommentText, addedComment.commentText);

            CommentDbSetMock.Verify(m => m.Add(It.IsAny<DataAccess.Comment>()), Times.Once());
            NewsLiveDbContextMock.Verify(m => m.SaveChanges(), Times.AtLeastOnce());
        }
    }
}
