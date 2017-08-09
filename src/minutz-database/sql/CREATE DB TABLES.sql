-- CREATE

CREATE SCHEMA [app]
GO

CREATE TABLE [app].[Instance](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Username] [varchar](255) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
	[Type] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE TABLE [app].[Person](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Identityid] [varchar](max) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[LastName] [varchar](255) NULL,
	[FullName] [varchar](255) NULL,
	[Email] [varchar](255) NOT NULL,
	[Role] [varchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
	[InstanceId] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

CREATE PROCEDURE [app].[createMeetingSchema] 
 (@tenant NVARCHAR(100))
  AS
  Begin
    DECLARE @sql NVARCHAR(4000) =
'CREATE TABLE [' + @tenant +'].[Meeting] (
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

CREATE PROCEDURE [app].[createInstanceSchema] 
 (@tenant nvarchar(100))
  AS
  Begin
    declare @sql nvarchar(4000) = 'create schema ' + @tenant + ' authorization  dbo';

    EXEC sp_executesql @sql

  End
GO

CREATE PROCEDURE [app].[createInstanceUser] 
 (@tenant nvarchar(100))
  AS
  Begin
    declare @sql nvarchar(4000) =
    'CREATE TABLE ' + @tenant +'.[User] (
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
GO