namespace NewsLive.DataAccess.Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MembershipRepositoryTests : BaseRepositoryTest
    {
        [TestMethod]
        public void LoginTest()
        {
            Assert.IsNull(membershipRepository.Login(string.Empty, string.Empty));
        }

        [TestMethod]
        public void LoginUnsuccessfulTest()
        {
            Assert.IsNull(membershipRepository.Login("Marry", string.Empty));
        }

        [TestMethod]
        public void LoginSuccessfulTest()
        {
            var membership = membershipRepository.Login("mary.murray@news.com", "password");

            Assert.AreEqual(2, membership.PersonId);
            Assert.AreEqual(2, membership.Person.PersonId);

            Assert.AreEqual("mary.murray@news.com", membership.UserName);
            
            Assert.AreEqual("Marry", membership.Person.FirstName);
            Assert.AreEqual("Murray", membership.Person.LastName);

            Assert.IsTrue(membership.CreateOn < DateTime.Now);

            Assert.AreEqual("Publisher", membership.Person.Roles.First().Name);

            Assert.AreEqual(true, membership.Person.IsPublisher);
        }

    }
}
