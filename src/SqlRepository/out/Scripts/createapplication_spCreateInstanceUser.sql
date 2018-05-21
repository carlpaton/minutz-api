USE [#catalogue#]
IF EXISTS(SELECT 1 FROM [#catalogue#].[INFORMATION_SCHEMA].[ROUTINES] WHERE routine_type = 'PROCEDURE' AND SPECIFIC_NAME = 'createInstanceUser')
BEGIN
  EXEC ('DROP PROCEDURE [#schema#].[createInstanceUser];')
END
GO
CREATE PROCEDURE [#schema#].[createInstanceUser] 
 (@tenant nvarchar(100))
  AS
  Begin
    declare @sql nvarchar(4000) =
    '
	CREATE TABLE [' + @tenant +'].[MeetingNotificationSettings] (
        [Id] uniqueidentifier NOT NULL,
        [Message] VARCHAR (MAX) NOT NULL,
        [Key] VARCHAR (50) NULL,
        [CreatedDate] DATETIME2 NULL
	)

	CREATE TABLE ' + @tenant +'.[User] (
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
End