namespace NewsLive.DataAccess.Tests
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using NewsLive.DataAccess.Repository.Article;
    using NewsLive.DataAccess.Repository.ArticleLike;
    using NewsLive.DataAccess.Repository.Comment;
    using NewsLive.DataAccess.Repository.Membership;

    [TestClass]
    public class BaseRepositoryTest
    {
        protected static IDataService dataService;
        protected static IDataService pagedDataService;

        protected static IMembershipRepository membershipRepository;
        protected static IArticleRepository articleRepository;
        protected static IArticleRepository pagedArticleRepository;
        protected static ICommentRepository commentRepository;
        protected static IArticleLikeRepository articleLikeRepository;

        protected static Mock<NewsLiveDbContext> PagingNewsLiveDbContextMock;
        protected static Mock<NewsLiveDbContext> NewsLiveDbContextMock;

        protected static Mock<DbSet<Membership>> MembershipDbSetMock;
        protected static Mock<DbSet<Article>> ArticleDbSetMock;
        protected static Mock<DbSet<Article>> PagedArticleDbSetMock;
        protected static Mock<DbSet<Comment>> CommentDbSetMock;
        protected static Mock<DbSet<Like>> ArticleLikeDbSetMock;

        protected static IQueryable<Membership> MembershipEntities;
        protected static IQueryable<Person> PeopleEntities;
        protected static IQueryable<Article> ArticleEntities;
        protected static IQueryable<Article> PagingArticleEntities;
        protected static IQueryable<Comment> CommentEntities;
        protected static IQueryable<Like> ArticleLikeEntities;

        protected static ICollection<Role> PublisherMembershipRole;
        protected static ICollection<Role> EmployeeMembershipRole;
        protected static ICollection<Comment> ArticleOneComments;
        protected static ICollection<Like> ArticleOneLikes;
        protected static ICollection<Like> ArticleFourLikes;
        protected static ICollection<CommentLike> ArticleOneCommentOneLikes;


        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            Setup();
            
            // Inject db context mocking our database entities 
            dataService = new DataService(NewsLiveDbContextMock.Object);
            pagedDataService = new DataService(PagingNewsLiveDbContextMock.Object);

            // MembershipRepository now contains our data service with mocked entities
            membershipRepository = new MembershipRepository(dataService);

            // CommentRepository now contains our data service with mocked entities
            commentRepository = new CommentRepository(dataService);

            // ArticleRepository now contains our data service with mocked entities
            articleRepository = new ArticleRepository(dataService);
            // ArticleRepository (Paged Articles) now contains our data service with mocked entities
            pagedArticleRepository = new ArticleRepository(pagedDataService);

            // ArticleLikeRepository now contains our data service with mocked entities
            articleLikeRepository = new ArticleLikeRepository(dataService);
        }


        public static void Setup()
        {
            NewsLiveDbContextMock = new Mock<NewsLiveDbContext>();
            PagingNewsLiveDbContextMock = new Mock<NewsLiveDbContext>();

            SetupMockMembershipTestData();
            MembershipDbSetMock = GetQueryableMockDbSet(MembershipEntities.ToList());
            NewsLiveDbContextMock.Setup(m => m.Memberships).Returns(MembershipDbSetMock.Object);

            SetupMockArticleTestData();
            SetupArticleDbSetMock();
            NewsLiveDbContextMock.Setup(m => m.Articles).Returns(ArticleDbSetMock.Object);
            PagingNewsLiveDbContextMock.Setup(m => m.Articles).Returns(PagedArticleDbSetMock.Object);

            SetupMockCommentTestData();
            CommentDbSetMock = GetQueryableMockDbSet(CommentEntities.ToList());
            NewsLiveDbContextMock.Setup(m => m.Comments).Returns(CommentDbSetMock.Object);

            SetupMockArticleLikeTestData();
            ArticleLikeDbSetMock = GetQueryableMockDbSet(ArticleLikeEntities.ToList());
            NewsLiveDbContextMock.Setup(m => m.Likes).Returns(ArticleLikeDbSetMock.Object);
        }

        protected static void SetupMockMembershipTestData()
        {
            PublisherMembershipRole = new List<Role>
            {
                new Role { RoleId = 1, Name = "Publisher" }
            };

            EmployeeMembershipRole = new List<Role>
            {
                new Role { RoleId = 2, Name = "Employee" }
            };

            PeopleEntities = new List<Person>
            {
                new Person { PersonId = 1, FirstName = "John", LastName = "Jack", Roles = PublisherMembershipRole },
                new Person { PersonId = 2, FirstName = "Marry", LastName = "Murray", Roles = PublisherMembershipRole },
                new Person { PersonId = 3, FirstName = "Fred", LastName = "Flint", Roles = EmployeeMembershipRole }
            }
            .AsQueryable();

            MembershipEntities = new List<Membership>
            {
                new Membership { PersonId = 1, Person = PeopleEntities.ElementAt(0), UserName = "john.jack@news.co.uk", Password = "password", LastLoginOn = DateTime.Now, CreateOn = DateTime.Now },
                new Membership { PersonId = 2, Person = PeopleEntities.ElementAt(1), UserName = "mary.murray@news.com", Password = "password", LastLoginOn = DateTime.Now, CreateOn = DateTime.Now },
                new Membership { PersonId = 3, Person = PeopleEntities.ElementAt(2), UserName = "fred.flint@news.com", Password = "password", LastLoginOn = DateTime.Now, CreateOn = DateTime.Now },
            }
            .AsQueryable();

            PeopleEntities.ElementAt(0).Membership = MembershipEntities.ElementAt(0);
            PeopleEntities.ElementAt(1).Membership = MembershipEntities.ElementAt(1);
            PeopleEntities.ElementAt(2).Membership = MembershipEntities.ElementAt(2);
        }

        protected static void SetupMockArticleTestData()
        {
            ArticleEntities = new List<Article>
            {
                new Article { ArticleId = 1, Title = "Welcome to John Jack's First News Article", Body = "Please feel free to like or comment on my article below!", PersonId = 1,  Person = PeopleEntities.ElementAt(0), PublishDate = DateTime.Now },
                new Article { ArticleId = 2, Title = "Welcome to John Jack's Second News Article", Body = "This is my second news article, in a series of two articles.", PersonId = 1, Person = PeopleEntities.ElementAt(0), PublishDate = DateTime.Now },
                new Article { ArticleId = 3, Title = "Welcome to Marry Murray's News Article", Body = "Please feel free to like or comment on my article below!", PersonId = 2, Person = PeopleEntities.ElementAt(1), PublishDate = DateTime.Now },
                new Article { ArticleId = 4, Title = "Welcome to Fred Flint's News Article", Body = "Please feel free to like or comment on my article below!", PersonId = 3, Person = PeopleEntities.ElementAt(2), PublishDate = DateTime.Now },
            }
            .AsQueryable();

            ArticleOneComments = new List<Comment>
            {
                new Comment { PersonId = 2, ArticleId = 1, CommentId = 1, Comment1 = "Hey John I see you fancy your own post hey? :P", Article = ArticleEntities.ElementAt(0) },
                new Comment { PersonId = 1, ArticleId = 1, CommentId = 2, Comment1 = "Hey Mary of course I do, I mean what a talent I am. :)", Article = ArticleEntities.ElementAt(0) },
            };

            ArticleOneLikes = new List<Like>
            {
                new Like { ArticleId = 1, PersonId = 1, IsLiked = true, Article = ArticleEntities.ElementAt(0), Person = PeopleEntities.ElementAt(0) },
                new Like { ArticleId = 1, PersonId = 2, IsLiked = true, Article = ArticleEntities.ElementAt(0), Person = PeopleEntities.ElementAt(1) },
                new Like { ArticleId = 1, PersonId = 3, IsLiked = true, Article = ArticleEntities.ElementAt(0), Person = PeopleEntities.ElementAt(2) }
            };

            ArticleFourLikes = new List<Like>
            {
                new Like { ArticleId = 3, PersonId = 2, IsLiked = true, Article = ArticleEntities.ElementAt(3), Person = PeopleEntities.ElementAt(1) }
            };

            ArticleOneCommentOneLikes = new List<CommentLike>
            {
                new CommentLike { CommentId = 2, PersonId = 2, IsLiked = true, Comment = ArticleOneComments.ElementAt(1), Person = PeopleEntities.ElementAt(1) },
            };

            ArticleEntities.ElementAt(0).Likes = ArticleOneLikes;
            ArticleEntities.ElementAt(3).Likes = ArticleFourLikes;
            ArticleEntities.ElementAt(0).Comments = ArticleOneComments;
            ArticleEntities.ElementAt(0).Comments.ElementAt(0).CommentLikes = ArticleOneCommentOneLikes;
        }

        protected static void SetupMockCommentTestData()
        {
            CommentEntities = ArticleOneComments.Select(c => new DataAccess.Comment
                {
                    CommentId = c.CommentId,    
                    ArticleId = c.ArticleId,
                    PersonId = c.PersonId, 
                    Comment1 = c.Comment1 
                })
                .AsQueryable(); 
        }

        protected static void SetupMockArticleLikeTestData()
        {
            ArticleLikeEntities = 
                    ArticleOneLikes.Select(c => new DataAccess.Like
                    {
                        ArticleId = c.ArticleId,
                        PersonId = c.PersonId,
                        IsLiked = c.IsLiked,
                        Article = c.Article,
                        Person = c.Person
                    })
                .Union(
                    ArticleFourLikes.Select(c => new DataAccess.Like
                    {
                        ArticleId = c.ArticleId,
                        PersonId = c.PersonId,
                        IsLiked = c.IsLiked,
                        Article = c.Article,
                        Person = c.Person
                    }))
                    .AsQueryable();
        }

        protected static void SetupArticleDbSetMock()
        {
            const int numArticleToCreate = 167;

            ArticleDbSetMock = GetQueryableMockDbSet(ArticleEntities.ToList());

            CreatePlentyArticleEntities(numArticleToCreate);
            PagedArticleDbSetMock = GetQueryableMockDbSet(PagingArticleEntities.ToList());
        }

        protected static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((e) => sourceList.Add(e));
            dbSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((e) => sourceList.Remove(e));

            return dbSet;
        }

        protected static void CreatePlentyArticleEntities(int numberOfArticles)
        {
            Person authorOne = PeopleEntities.ElementAt(0);
            Person authorTwo = PeopleEntities.ElementAt(1);

            List<Article> articles = new List<Article>();
            for (int articleCount = 1; articleCount < numberOfArticles + 1; articleCount++)
            {
                articles.Add(new Article()
                {
                    ArticleId = articleCount,
                    Title = string.Format("Title {0}", articleCount),
                    Body = string.Format("Body {0}", articleCount),
                    PublishDate = articleCount < 111 
                        ? DateTime.Now 
                        : (articleCount % 2) == 0 
                            ? DateTime.Now 
                            : new DateTime?(), 
                    PersonId = articleCount < 111 
                        ? authorOne.PersonId 
                        : authorTwo.PersonId,
                    Person = articleCount < 111 
                        ? authorOne 
                        : authorTwo
                });
            }

            PagingArticleEntities = articles.AsQueryable();
        }
    }
}
