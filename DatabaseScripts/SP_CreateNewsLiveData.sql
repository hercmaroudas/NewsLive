USE [NewsLive]
GO

IF OBJECT_ID ( 'SP_CreateNewsLiveData', 'P' ) IS NOT NULL   
    DROP PROCEDURE SP_CreateNewsLiveData;  
GO  
CREATE PROCEDURE SP_CreateNewsLiveData
AS   
SET NOCOUNT ON;
BEGIN TRY  
	BEGIN TRANSACTION
	
	DECLARE @firstName   NVARCHAR(50),
		    @lastName    NVARCHAR(50),
			@userName    NVARCHAR(50),
			@createdOn   DATETIME,
			@roleId      NVARCHAR(max), 
			@title       NVARCHAR(max),
			@body        NVARCHAR(max),
			@comment     NVARCHAR(max),
			@publishDate DATETIME,
			@isLiked     BIT;

    DECLARE @counter           INT = 0,
			@articleId         INT = 0,
			@randomArticleId   INT = 0,
			@randomArticleNum  INT = 0,
			@randomPersonId    INT = 0,
			@commentId         INT = 0,
            @personId          INT = 0,
			@evenNumber        INT = 0,
			@randomCommentLikePersonId INT = 0;
	
	DECLARE person_mock_cursor CURSOR
		FOR SELECT FirstName,
		           LastName, 
				   Email, 
				   CreatedOn,
				   RoleId
			  FROM dbo.PersonMock
		  ORDER BY Id ASC;

	OPEN person_mock_cursor  
	
	FETCH NEXT FROM person_mock_cursor
		INTO @firstName,
			 @lastName,
			 @userName,
			 @createdOn,
			 @roleId
	
	WHILE @@FETCH_STATUS = 0  
    BEGIN		
		-- ( create a person, for each person create a membership and asign a role )
		INSERT INTO dbo.Person (
			FirstName,
			LastName) 
		VALUES (
			@firstName ,
			@lastName)

		SELECT @personId = SCOPE_IDENTITY();

		INSERT INTO dbo.Membership (
			PersonId,
			UserName,
			Password,
			CreateOn,
			LastLoginOn)
		VALUES (
			@personId,
			@userName,
			null,
			@createdOn,
			null)

		INSERT INTO dbo.PersonRole (
			PersonId,
			RoleId)
		VALUES (
			@personId,
			@roleId)
		
		FETCH NEXT FROM person_mock_cursor   
		INTO @firstName, 
			 @lastName, 
			 @userName, 
			 @createdOn,
			 @roleId

	END -- WHILE @@FETCH_STATUS
	
	CLOSE person_mock_cursor;  
	DEALLOCATE person_mock_cursor;

	-- create articles 
	DECLARE person_cursor CURSOR
		FOR SELECT person.PersonId, 
				   person.FirstName,
		           person.LastName, 
				   membership.UserName, 
				   membership.CreateOn,
				   person_role.RoleId
			  FROM dbo.Person person
		INNER JOIN dbo.MemberShip membership on person.PersonId = membership.PersonId
		INNER JOIN dbo.PersonRole person_role on person.PersonId = person_role.PersonId
		     WHERE person.PersonId NOT IN (1, 2, 3)
		  ORDER BY person.PersonId ASC;

	OPEN person_cursor  
	
	FETCH NEXT FROM person_cursor
		INTO @personId,
			 @firstName,
			 @lastName,
			 @userName,
			 @createdOn,
			 @roleId
	
	WHILE @@FETCH_STATUS = 0  
    BEGIN

		-- ( number of articles to insert for person )
		SELECT @randomArticleNum = FLOOR(RAND()*(10-1)+1);

		-- ( newly inserted article to dbo.Article )
		SELECT @articleId = 0;

		-- ( reset counter ) 
		SET @counter = 0;

		WHILE (@counter < @randomArticleNum)
		BEGIN
			-- ( dont insert duplicate articles )
			IF NOT EXISTS (
				SELECT ArticleId 
					FROM dbo.Article 
					WHERE ArticleId = @articleId
					AND PersonId = @personId)
			BEGIN
				-- ( random article id from mock table )
				SELECT @randomArticleId = FLOOR(RAND()*(100-1)+1);
			
				SELECT @title = SUBSTRING(Title, 1, 50),
					   @body = Body,
					   @publishDate = PublishDate,
					   @isLiked = IsLiked
				FROM dbo.ArticleMock
				WHERE Id = @randomArticleId
				  AND Id NOT IN (1, 2, 3);

				INSERT INTO dbo.Article (
					PersonId, 
					Title, 
					Body, 
					PublishDate)
				VALUES (
					@personId,
					@title,
					@body,
					@publishDate)

				
				SET @counter = @counter + 1;
				
			END -- IF NOT EXISTS ( Article )

		END -- WHILE @counter < @randomNumArticles
		
	FETCH NEXT FROM person_cursor   
		INTO @personId,
			 @firstName,
			 @lastName,
			 @userName,
			 @createdOn,
			 @roleId

	END -- WHILE @@FETCH_STATUS
	
	CLOSE person_cursor;  
	DEALLOCATE person_cursor;

	-- ( article likes, comments and comment likes )
	DECLARE article_cursor CURSOR
		FOR SELECT ArticleId, 
				   PersonId, 
				   Title, 
				   Body, 
				   PublishDate
			  FROM dbo.Article
		  ORDER BY ArticleId ASC;

	OPEN article_cursor

	FETCH NEXT FROM article_cursor
		INTO @articleId,
			 @personId,
			 @title,
			 @body,
			 @publishDate

	WHILE @@FETCH_STATUS = 0  
    BEGIN

		DECLARE @likeCount       INT = 0,
			@likeCountMax        INT = 0,
			@commentCount        INT = 0,
			@commentCountMax     INT = 0,
			@commentLikeCount    INT = 0,
			@commentLikeCountMax INT = 0

		--  ( random person id to like the current article to insert article likes )
		SELECT @randomPersonId = FLOOR(RAND()*(1000-1)+1);
		
		SELECT @likeCountMax = FLOOR(RAND()*(1000-1)+1)-1;
				
		WHILE (@likeCount < @likeCountMax)
		BEGIN
			SELECT @evenNumber = (@randomPersonId % 2);

			IF NOT EXISTS (
					SELECT PersonId 
					FROM [NewsLive].[dbo].[Like] 
					WHERE PersonId = @randomPersonId 
					AND ArticleId = @articleId)
			BEGIN
				INSERT INTO [NewsLive].[dbo].[Like] (
					ArticleId,
					PersonId,
					isLiked)
				VALUES (
					@articleId,
					@randomPersonId,
					@evenNumber)

			END

			SELECT @randomPersonId = FLOOR(RAND()*(1000-1)+1);

			SET @likeCount += 1;

		END -- WHILE (@likeCount < @likeCountMax)


		-- ( random person id to on current article to insert comments )
		SELECT @randomPersonId = FLOOR(RAND()*(1000-1)+1);

		-- ( max zero - four comments )
		SELECT @commentCountMax = FLOOR(RAND()*(6-1)+1)-1;

		WHILE (@commentCount < @commentCountMax)
		BEGIN
			SELECT @comment = Comment
				FROM dbo.ArticleMock
				WHERE Id = FLOOR(RAND()*(100-1)+1); 

			INSERT INTO dbo.Comment (
				ArticleId, 
				PersonId, 
				Comment)
			VALUES (
				@articleId,
				@randomPersonId,
				@comment)

			SELECT @commentId = SCOPE_IDENTITY();

			SELECT @randomPersonId = FLOOR(RAND()*(1000-1)+1);

			-- ( random person id to on current article to insert comment likes )
			SELECT @randomCommentLikePersonId = FLOOR(RAND()*(1000-1)+1);
			
			SELECT @commentLikeCountMax = FLOOR(RAND()*(100-1)+1)-1;

			WHILE (@commentLikeCount < @commentLikeCountMax)
			BEGIN

				SELECT @evenNumber = (@randomCommentLikePersonId % 2);

				IF NOT EXISTS  (
					SELECT CommentId 
						FROM dbo.CommentLike 
						WHERE PersonId = @randomCommentLikePersonId 
						AND CommentId = @commentId)
				BEGIN
					-- comment likes
					INSERT INTO dbo.CommentLike (
						CommentId, 
						PersonId, 
						IsLiked)
					VALUES (
						@commentId,
						@randomCommentLikePersonId,
						@evenNumber)

				END
				
				SELECT @randomCommentLikePersonId = FLOOR(RAND()*(1000-1)+1);

				SET @commentLikeCount += 1;

			END -- -- WHILE (@commentLikeCount < @commentLikeCountMax)

			SET @commentCount += 1;

		END -- WHILE (@commentCount < @commentCountMax)


	FETCH NEXT FROM article_cursor   
		INTO @articleId,
			 @personId,
			 @title,
			 @body,
			 @publishDate

	END -- WHILE @@FETCH_STATUS
	
	CLOSE article_cursor;  
	DEALLOCATE article_cursor;	
	  
	COMMIT
END TRY  
BEGIN CATCH  
  -- Determine if an error occurred.  
	IF @@TRANCOUNT > 0  
		ROLLBACK  
	
	IF CURSOR_STATUS('GLOBAL','person_mock_cursor') >= 0 
	BEGIN
		CLOSE person_mock_cursor
		DEALLOCATE person_mock_cursor 
	END

	IF CURSOR_STATUS('GLOBAL','person_cursor') >= 0 
	BEGIN
		CLOSE person_cursor
		DEALLOCATE person_cursor 
	END

	IF CURSOR_STATUS('GLOBAL','article_cursor') >= 0 
	BEGIN
		CLOSE article_cursor
		DEALLOCATE article_cursor
	END  

	-- Return the error information.  
	DECLARE @ErrorMessage  nvarchar(4000),  
	      @ErrorSeverity int;  
  
	SELECT @ErrorMessage = ERROR_MESSAGE(),
           @ErrorSeverity = ERROR_SEVERITY();  
  
	RAISERROR(@ErrorMessage, @ErrorSeverity, 1);

END CATCH;
GO  