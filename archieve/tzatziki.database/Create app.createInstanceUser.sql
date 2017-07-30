USE [minutz]
GO

/****** Object: SqlProcedure [app].[createInstanceUser] Script Date: 2017/04/15 6:12:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE app.createInstanceUser 
 (@tenant nvarchar(100))
  AS
  Begin
    declare @sql nvarchar(4000) =
    'CREATE TABLE' + @tenant +'.[User] (
        [Id] INT IDENTITY (1, 1) NOT NULL,
        [Identity] VARCHAR (MAX) NOT NULL,
        [FirstName]  VARCHAR (255) NULL,
        [LastName]   VARCHAR (255) NULL,
        [FullName]   VARCHAR (255) NULL,
        [Email]      VARCHAR (255) NOT NULL,
        [Role]       VARCHAR (255) NOT NULL,
        [Active]     BIT           NOT NULL,
    )';
 EXEC sp_executesql @sql
End
