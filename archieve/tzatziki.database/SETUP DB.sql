USE [master]
IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'minutz')
BEGIN
 ALTER DATABASE minutz set single_user with rollback immediate;
END
GO
--USE [master]
--DROP DATABASE minutz ;

IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'minutz')
BEGIN
DROP DATABASE minutz;
 EXEC ('CREATE DATABASE [minutz]');
END
ELSE
BEGIN
 EXEC ('CREATE DATABASE [minutz]');
END
GO

USE [minutz]
IF EXISTS(SELECT 1 FROM [sys].[schemas] WHERE name = 'app')
BEGIN
 DROP SCHEMA [app];
 EXEC ('CREATE SCHEMA [app] AUTHORIZATION [dbo]');
END
ELSE
BEGIN
 EXEC ('CREATE SCHEMA [app] AUTHORIZATION [dbo]');
END
GO

USE [minutz]
IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'Person')
BEGIN
DROP TABLE [app].[Person];

CREATE TABLE [app].[Person] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Identityid] VARCHAR (MAX) NOT NULL,
    [FirstName]  VARCHAR (255) NULL,
    [LastName]   VARCHAR (255) NULL,
    [FullName]   VARCHAR (255) NULL,
    [ProfilePicture]   VARCHAR (255) NULL,
    [Email]      VARCHAR (255) NOT NULL,
    [Role]       VARCHAR (255) NOT NULL,
    [Active]     BIT           NOT NULL,
    [InstanceId] VARCHAR (255) NULL
)
--app.Personapp.Person
END
ELSE
BEGIN
CREATE TABLE [app].[Person] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Identityid] VARCHAR (MAX) NOT NULL,
    [FirstName]  VARCHAR (255) NULL,
    [LastName]   VARCHAR (255) NULL,
    [FullName]   VARCHAR (255) NULL,
    [ProfilePicture]   VARCHAR (255) NULL,    
    [Email]      VARCHAR (255) NOT NULL,
    [Role]       VARCHAR (255) NOT NULL,
    [Active]     BIT           NOT NULL,
    [InstanceId] VARCHAR (255) NULL
)
END
GO

USE [minutz]
IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'Instance')
BEGIN
DROP TABLE [app].[Instance];
CREATE TABLE [app].[Instance] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (255) NOT NULL,
    [Username] VARCHAR (255) NOT NULL,
    [Password] VARCHAR (255) NOT NULL,
    [Active]   BIT           NOT NULL,
    [Type]     INT           NOT NULL
);
END
ELSE
BEGIN
CREATE TABLE [app].[Instance] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (255) NOT NULL,
    [Username] VARCHAR (255) NOT NULL,
    [Password] VARCHAR (255) NOT NULL,
    [Active]   BIT           NOT NULL,
    [Type]     INT           NOT NULL
);
END
GO


USE [minutz]
IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[ROUTINES] WHERE routine_type = 'PROCEDURE' AND SPECIFIC_NAME = 'createInstanceSchema')
BEGIN
	EXEC ('DROP PROCEDURE [app].[createInstanceSchema];');
END
GO
CREATE PROCEDURE [app].[createInstanceSchema] (@tenant nvarchar(100))
  AS
  BEGIN
    DECLARE @sql nvarchar(4000) = 'create schema ' + @tenant + ' authorization  dbo';
    EXEC sp_executesql @sql
   END
GO

USE [minutz]
IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[ROUTINES] WHERE routine_type = 'PROCEDURE' AND SPECIFIC_NAME = 'createInstanceUser')
BEGIN
  EXEC ('DROP PROCEDURE [app].[createInstanceUser];')
END
GO
CREATE PROCEDURE app.createInstanceUser 
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
GO
USE [minutz]
IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[ROUTINES] WHERE routine_type = 'PROCEDURE' AND SPECIFIC_NAME = 'createMeetingSchema')
BEGIN
  EXEC ('DROP PROCEDURE [app].[createMeetingSchema];')
END
GO
CREATE PROCEDURE [app].[createMeetingSchema] 
 (@tenant NVARCHAR(100))
  AS
  Begin
    DECLARE @sql NVARCHAR(4000) =
'
CREATE TABLE [' + @tenant +'].[Meeting] (
        [Id] uniqueidentifier NOT NULL,
        [Name] VARCHAR (MAX) NOT NULL,
        [Location] VARCHAR (255) NULL,
        [Date] DATETIME2 NULL,
		[UpdatedDate] DATETIME2 NULL,
        [Time] VARCHAR (255) NULL,
        [Duration] INT  NULL,
        [IsReacurance] BIT  NULL,
        [IsPrivate] BIT  NULL,
        [ReacuranceType] VARCHAR (255) NULL,
        [IsLocked] BIT  NULL,
        [IsFormal] BIT  NULL,
        [TimeZone] VARCHAR (255) NULL,
        [Tag] VARCHAR (255) NULL,
        [Purpose] VARCHAR (255) NULL,
        [MeetingOwnerId] VARCHAR (255) NULL,
        [Outcome] VARCHAR (255) NULL
)

CREATE TABLE [' + @tenant +'].[MeetingAttendee] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [PersonIdentity] VARCHAR (255) NULL,
  [Role] VARCHAR (255) NULL

)


CREATE TABLE [' + @tenant +'].[MeetingAgenda] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [AgendaHeading] VARCHAR (255) NULL,
  [AgendaText] VARCHAR (255) NULL,
  [MeetingAttendeeId] VARCHAR (255) NULL,
  [Duration] VARCHAR (255) NULL,
  [CreatedDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
)


CREATE TABLE [' + @tenant +'].[MeetingAction] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [ActionText] VARCHAR (255) NULL,
  [PersonId] uniqueidentifier NULL,
  [DueDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
)


CREATE TABLE [' + @tenant +'].[MeetingNote] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [NoteText] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [CreatedDate] DATETIME2 NULL
)


CREATE TABLE [' + @tenant +'].[MeetingAttachment] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [FileName] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [Date] DATETIME2 NULL,
  [FileData] VARBINARY(MAX) NULL 
)';
    EXEC sp_executesql @sql
  End
GO

USE [minutz]
IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[ROUTINES] WHERE routine_type = 'PROCEDURE' AND SPECIFIC_NAME = 'dropSchema')
BEGIN
EXEC ('DROP PROCEDURE [app].[dropSchema];')
END
GO
CREATE PROCEDURE [app].[dropSchema]
   @Pattern AS varchar(255)
 AS
BEGIN
    DECLARE @sql AS varchar(max)
    SELECT @sql = COALESCE(@sql, '') + 'DROP TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + ']' + CHAR(13) + CHAR(10)
    FROM [minutz].[INFORMATION_SCHEMA].[TABLES]
    WHERE  TABLE_SCHEMA LIKE '%account_%'
	EXEC (@sql)
	--PRINT @sql
END
GO
EXEC [app].[dropSchema] @Pattern = N'account_';
GO

USE [minutz]
IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[ROUTINES] WHERE routine_type = 'PROCEDURE' AND SPECIFIC_NAME = 'resetAccount')
BEGIN
EXEC ('DROP PROCEDURE [app].[resetAccount];')
END
GO

CREATE PROCEDURE [app].[resetAccount]
   @schema AS varchar(255),
   @instanceName AS varchar(255),
   @instanceId AS VARCHAR(255)
 AS
BEGIN
DELETE FROM [minutz].[app].[Instance] WHERE Name = @instanceName
DELETE FROM [minutz].[app].[Person] WHERE InstanceId = @instanceId

IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'MeetingNotificationSettings')
EXEC ('DROP TABLE ' + @schema + '.MeetingNotificationSettings')

IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'Meeting')
EXEC ('DROP TABLE ' + @schema + '.Meeting')

IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'MeetingAction')
EXEC ('DROP TABLE ' + @schema + '.MeetingAction')

IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'MeetingAgenda')
EXEC ('DROP TABLE ' + @schema + '.MeetingAgenda')

IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'MeetingAttachment')
EXEC ('DROP TABLE ' + @schema + '.MeetingAttachment')

IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'MeetingAttendee')
EXEC ('DROP TABLE ' + @schema + '.MeetingAttendee')

IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'MeetingNote')
EXEC ('DROP TABLE ' + @schema + '.MeetingNote')

IF EXISTS(SELECT 1 FROM [minutz].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'User')
EXEC ('DROP TABLE [minutz].[' + @schema + '].[User]')

END
