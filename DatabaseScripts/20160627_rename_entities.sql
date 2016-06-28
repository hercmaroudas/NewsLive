-- https://msdn.microsoft.com/en-GB/library/ms188351.aspx

USE NewsLive;  
GO  
EXEC sp_rename 'dbo.Like', 'ArticleLike';  
GO


GO  
EXEC sp_rename 'dbo.Comment.Comment', 'CommentText', 'COLUMN';  
GO 