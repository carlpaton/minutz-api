CREATE SCHEMA [app]
GO

CREATE TABLE app.subscription
(
  Id          INT IDENTITY,
  Name        VARCHAR(200) NOT NULL,
  Description VARCHAR(MAX),
  Term        INT,
  Cost        INT
)
GO

CREATE TABLE app.notificationType
(
  Id   INT IDENTITY,
  Name VARCHAR(200) NOT NULL
)
GO

CREATE TABLE app.notificationRole
(
  Id   INT IDENTITY,
  Name VARCHAR(200) NOT NULL
)
GO

CREATE TABLE app.reminder
(
  Id          INT IDENTITY,
  Name        VARCHAR(200) NOT NULL,
  Description VARCHAR(200) NOT NULL
)
GO

CREATE TABLE app.Person
(
  Id             INT IDENTITY,
  Identityid     VARCHAR(MAX) NOT NULL,
  FirstName      VARCHAR(255),
  LastName       VARCHAR(255),
  FullName       VARCHAR(255),
  ProfilePicture VARCHAR(255),
  Email          VARCHAR(255) NOT NULL,
  Role           VARCHAR(255) NOT NULL,
  Active         BIT          NOT NULL,
  InstanceId     VARCHAR(255)
)
GO


ALTER TABLE app.Person
ADD Related varchar(max);

GO

CREATE TABLE app.Instance
(
  Id                 INT IDENTITY,
  Name               VARCHAR(255) NOT NULL,
  Username           VARCHAR(255) NOT NULL,
  Password           VARCHAR(255) NOT NULL,
  Active             BIT          NOT NULL,
  Type               INT          NOT NULL,
  subscriptionId     INT,
  subscriptionDate   DATETIME2,
  logo               BINARY(1),
  colour             VARCHAR(200),
  style              VARCHAR(200),
  allowInformation   BIT,
  notificationTypeId INT,
  notificationRoleId INT,
  reminderId         INT
)
GO


CREATE TABLE app.EventLog
(
  ID          INT IDENTITY
    PRIMARY KEY,
  EventID     INT,
  LogLevel    NVARCHAR(50),
  Message     VARCHAR(MAX),
  CreatedTime DATETIME2
)
GO

CREATE PROCEDURE [app].[createInstanceSchema](@tenant NVARCHAR(100))
AS
  BEGIN
    DECLARE @sql NVARCHAR(4000) = 'create schema ' + @tenant + ' authorization  dbo';
    EXEC sp_executesql @sql
  END
GO

CREATE PROCEDURE app.createInstanceUser
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

CREATE PROCEDURE [app].[dropSchema]
    @Pattern AS VARCHAR(255)
AS
  BEGIN
    DECLARE @sql AS VARCHAR(MAX)
    SELECT @sql = COALESCE(@sql, '') + 'DROP TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + ']' + CHAR(13) + CHAR(10)
    FROM [INFORMATION_SCHEMA].[TABLES]
    WHERE TABLE_SCHEMA LIKE '%account_%'
    EXEC (@sql)
    --PRINT @sql
  END
GO


CREATE PROCEDURE [app].[resetAccount]
    @schema AS       VARCHAR(255),
    @instanceName AS VARCHAR(255),
    @instanceId AS   VARCHAR(255)
AS
  BEGIN
    DELETE FROM [app].[Instance]
    WHERE Name = @instanceName
    DELETE FROM [app].[Person]
    WHERE InstanceId = @instanceId

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'MeetingNotificationSettings')
      EXEC ('DROP TABLE ' + @schema + '.MeetingNotificationSettings')

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'Meeting')
      EXEC ('DROP TABLE ' + @schema + '.Meeting')

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'MeetingAction')
      EXEC ('DROP TABLE ' + @schema + '.MeetingAction')

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'MeetingAgenda')
      EXEC ('DROP TABLE ' + @schema + '.MeetingAgenda')

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'MeetingAttachment')
      EXEC ('DROP TABLE ' + @schema + '.MeetingAttachment')

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'MeetingAttendee')
      EXEC ('DROP TABLE ' + @schema + '.MeetingAttendee')

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'MeetingNote')
      EXEC ('DROP TABLE ' + @schema + '.MeetingNote')

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'User')
      EXEC ('DROP TABLE ' + @schema + '.[User]')

  END
GO

CREATE PROCEDURE [app].[createMeetingSchema]
  (@tenant NVARCHAR(100))
AS
  BEGIN
    DECLARE @sql NVARCHAR(4000) =
    '
    CREATE TABLE [' + @tenant + '].[Meeting] (
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
				[Status] VARCHAR (255) NULL,
        [MeetingOwnerId] VARCHAR (255) NULL,
        [Outcome] VARCHAR (255) NULL
)

CREATE TABLE [' + @tenant + '].[MeetingAttendee] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [PersonIdentity] VARCHAR (255) NULL,
  [Role] VARCHAR (255) NULL
)

CREATE TABLE [' + @tenant + '].[AvailibleAttendee] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [PersonIdentity] VARCHAR (255) NULL,
  [Status] VARCHAR (255) NULL,
  [Role] VARCHAR (255) NULL
)

CREATE TABLE [' + @tenant + '].[MeetingAgenda] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [AgendaHeading] VARCHAR (255) NULL,
  [AgendaText] VARCHAR (255) NULL,
  [MeetingAttendeeId] VARCHAR (255) NULL,
  [Duration] VARCHAR (255) NULL,
  [CreatedDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
)


CREATE TABLE [' + @tenant + '].[MeetingAction] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [ActionText] VARCHAR (255) NULL,
  [PersonId] uniqueidentifier NULL,
	[CreatedDate] DATETIME2 NULL,
  [DueDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
)

CREATE TABLE [' + @tenant + '].[MinutzAction] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [ActionText] VARCHAR (255) NULL,
  [PersonId] uniqueidentifier NULL,
	[CreatedDate] DATETIME2 NULL,
  [DueDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
)


CREATE TABLE [' + @tenant + '].[MeetingNote] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [NoteText] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [CreatedDate] DATETIME2 NULL
)

CREATE TABLE [' + @tenant + '].[MeetingAttachment] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [FileName] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [Date] DATETIME2 NULL,
  [FileData] VARBINARY(MAX) NULL
)';
    EXEC sp_executesql @sql
  END
GO

