SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [app].[createInstanceUser]
  (@tenant NVARCHAR(100))
AS
  BEGIN
    DECLARE @sql NVARCHAR(4000) =
    '
	CREATE TABLE [' + @tenant + '].[MeetingNotificationSettings] (
        [Id] uniqueidentifier NOT NULL,
        [Message] VARCHAR (MAX) NOT NULL,
        [Key] VARCHAR (50) NULL,
        [CreatedDate] DATETIME2 NULL
	)

	CREATE TABLE ' + @tenant + '.[User] (
        [Id] INT IDENTITY (1, 1) NOT NULL,
        [Identity] VARCHAR (MAX) NOT NULL,
        [FirstName]  VARCHAR (255) NULL,
        [LastName]   VARCHAR (255) NULL,
        [FullName]   VARCHAR (255) NULL,
        [ProfilePicture]   VARCHAR (255) NULL,
        [Email]      VARCHAR (255) NOT NULL,
        [Role]       VARCHAR (255) NOT NULL,
        [Active]     BIT           NOT NULL,
    )';
    EXEC sp_executesql @sql
  END
GO
