-- Articles by a person 
select * from dbo.Article a
	inner join dbo.Person p on a.PersonId = p.PersonId
	where p.PersonId = 1;

-- Roles for people
select p.FirstName, p.LastName, r.Name from dbo.Person p 
inner join dbo.PersonRole pr on p.PersonId = pr.PersonId
	inner join dbo.Role r on pr.RoleId = r.RoleId; 

-- Number of likes plotted against a specific article
SELECT [ArticleId], IsLiked,  COUNT([ArticleId]) LikedCount
  FROM [NewsLive].[dbo].[Like]
  GROUP BY ArticleId, IsLiked
  HAVING IsLiked = 1 and ArticleId = 1;

-- Number of likes plotted against each article
SELECT l.[ArticleId], COUNT(l.[ArticleId]) LikedCount, a.Title
  FROM [NewsLive].[dbo].[Like] l
  INNER JOIN [NewsLive].[dbo].[Article] a ON a.ArticleId = l.ArticleId
  GROUP BY l.ArticleId, l.IsLiked , a.Title
  HAVING l.IsLiked = 1;

-- Number of likes plotted against a each article and the author associated with article
SELECT l.[ArticleId], COUNT(l.[ArticleId]) LikedCount, a.Title, p.PersonId
  FROM [NewsLive].[dbo].[Like] l
  INNER JOIN [NewsLive].[dbo].[Article] a ON a.ArticleId = l.ArticleId
  INNER JOIN [NewsLive].[dbo].[Person] p ON p.PersonId = a.PersonId  
  GROUP BY l.ArticleId, l.IsLiked , a.Title, p.PersonId
  HAVING l.IsLiked = 1
  ORDER BY LikedCount DESC;
