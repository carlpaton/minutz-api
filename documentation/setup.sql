CREATE DATABASE minutz
GO

USE minutz;
GO

CREATE SCHEMA app
GO

CREATE TABLE app.subscription
(
  Id          int identity,
  Name        varchar(200) not null,
  Description varchar(max),
  Term        int,
  Cost        int
)
GO

CREATE table app.notificationRole
(
  Id   int identity,
  Name varchar(200) not null
)
go

CREATE table app.reminder
(
  Id          int identity,
  Name        varchar(200) not null,
  Description varchar(200) not null
)
go

CREATE table app.Person
(
  Id             int identity,
  Identityid     varchar(max) not null,
  FirstName      varchar(255),
  LastName       varchar(255),
  FullName       varchar(255),
  ProfilePicture varchar(255),
  Email          varchar(255) not null,
  Role           varchar(255) not null,
  Active         bit          not null,
  InstanceId     varchar(255),
  Related        varchar(max)
)
go

CREATE table app.Instance
(
  Id                 int identity,
  Name               varchar(255) not null,
  Username           varchar(255) not null,
  Password           varchar(255) not null,
  Active             bit          not null,
  Type               int          not null,
  subscriptionId     int,
  subscriptionDate   datetime2,
  logo               binary(1),
  colour             varchar(200),
  style              varchar(200),
  allowInformation   bit,
  notificationTypeId int,
  notificationRoleId int,
  reminderId         int,
  company            varchar(200)
)
go

CREATE table app.EventLog
(
  ID          int identity
    primary key,
  EventID     int,
  LogLevel    nvarchar(50),
  Message     varchar(max),
  CreatedTime datetime2
)
go


CREATE PROCEDURE [app].[createUser]
    @Identity       NVARCHAR(100),
    @Email          NVARCHAR(100),
    @Username       NVARCHAR(100),
    @PasswordString NVARCHAR(100)
AS

  DECLARE @createloginSQL nvarchar(4000) = 'CREATE LOGIN ' + @username + ' WITH PASSWORD = ''' + @PasswordString +
                                           ''' ';
  EXEC sys.sp_executesql @createloginSQL

  DECLARE @createloginuserSQL nvarchar(4000) = 'CREATE USER ' + @username + ' FOR LOGIN ' + @username + ' ';
  EXEC sys.sp_executesql @createloginuserSQL
  --CREATE USER AbolrousHazem FOR LOGIN AbolrousHazem;

  DECLARE @createSchemaSQL nvarchar(4000) = 'CREATE SCHEMA ' + @username + ' authorization  ' + @username + ' ';
  EXEC sp_executesql @createSchemaSQL

  INSERT into [minutz].[app].[Instance] ([Name], [Username], [Password], [Active], [Type])
  VALUES (@Email, @Username, @PasswordString, 1, 1);

  UPDATE [minutz].[app] . [Person]
  SET [InstanceId] = @Username,
      [Role]       = 'User'
  WHERE Identityid = @Identity

  DECLARE @sql NVARCHAR(MAX) =
  '
    CREATE TABLE [' + @username + '].[Meeting] (
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

    CREATE TABLE [' + @username + '].[Meeting_Audit] (
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
        [Outcome] VARCHAR (255) NULL,
        [Email] VARCHAR (255) NULL,
        [Role] VARCHAR (255) NULL,
        [Action] VARCHAR (255) NULL,
        [ChangedBy] VARCHAR (255) NULL,
        [AuditDate] DATETIME2 NOT NULL
)

CREATE TABLE [' + @username + '].[MeetingAttendee] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [PersonIdentity] VARCHAR (255) NULL,
  [Email] VARCHAR (255) NULL,
  [Status] VARCHAR (255) NULL,
  [Role] VARCHAR (255) NULL
)

CREATE TABLE [' + @username + '].[MeetingAttendee_Audit] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [PersonIdentity] VARCHAR (255) NULL,
  [Email] VARCHAR (255) NULL,
  [Status] VARCHAR (255) NULL,
  [Role] VARCHAR (255) NULL,
  [Action] VARCHAR (255) NULL,
  [ChangedBy] VARCHAR (255) NULL,
  [AuditDate] DATETIME2 NOT NULL
)

CREATE TABLE [' + @username + '].[AvailibleAttendee] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [PersonIdentity] VARCHAR (255) NULL,
  [Email] VARCHAR (255) NULL,
  [Status] VARCHAR (255) NULL,
  [Role] VARCHAR (255) NULL
)

CREATE TABLE [' + @username + '].[AvailibleAttendee_Audit] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [PersonIdentity] VARCHAR (255) NULL,
  [Email] VARCHAR (255) NULL,
  [Status] VARCHAR (255) NULL,
  [Role] VARCHAR (255) NULL,
  [Action] VARCHAR (255) NULL,
  [ChangedBy] VARCHAR (255) NULL,
  [AuditDate] DATETIME2 NOT NULL
)

CREATE TABLE [' + @username + '].[MeetingAgenda] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [AgendaHeading] VARCHAR (255) NULL,
  [AgendaText] VARCHAR (255) NULL,
  [MeetingAttendeeId] VARCHAR (255) NULL,
  [Duration] VARCHAR (255) NULL,
  [CreatedDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
)

CREATE TABLE [' + @username + '].[MeetingAgenda_Audit] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [AgendaHeading] VARCHAR (255) NULL,
  [AgendaText] VARCHAR (255) NULL,
  [MeetingAttendeeId] VARCHAR (255) NULL,
  [Duration] VARCHAR (255) NULL,
  [CreatedDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
  [Action] VARCHAR (255) NULL,
  [ChangedBy] VARCHAR (255) NULL,
  [AuditDate] DATETIME2 NOT NULL
)

CREATE TABLE [' + @username + '].[MeetingAction] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [ActionText] VARCHAR (255) NULL,
  [PersonId] uniqueidentifier NULL,
	[CreatedDate] DATETIME2 NULL,
  [DueDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0
)

CREATE TABLE [' + @username + '].[MeetingAction_Audit] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [ActionText] VARCHAR (255) NULL,
  [PersonId] uniqueidentifier NULL,
	[CreatedDate] DATETIME2 NULL,
  [DueDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
  [Action] VARCHAR (255) NULL,
  [ChangedBy] VARCHAR (255) NULL,
  [AuditDate] DATETIME2 NOT NULL
)

CREATE TABLE [' + @username + '].[MinutzAction] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [ActionText] VARCHAR (255) NULL,
  [PersonId] uniqueidentifier NULL,
	[CreatedDate] DATETIME2 NULL,
  [DueDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0
)

CREATE TABLE [' + @username + '].[MinutzAction_Audit] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [ActionText] VARCHAR (255) NULL,
  [PersonId] uniqueidentifier NULL,
	[CreatedDate] DATETIME2 NULL,
  [DueDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
  [Action] VARCHAR (255) NULL,
  [ChangedBy] VARCHAR (255) NULL,
  [AuditDate] DATETIME2 NOT NULL
)

CREATE TABLE [' + @username + '].[MeetingNote] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [NoteText] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [CreatedDate] DATETIME2 NULL
)

CREATE TABLE [' + @username + '].[MeetingNote_Audit] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [NoteText] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [CreatedDate] DATETIME2 NULL,
  [Action] VARCHAR (255) NULL,
  [ChangedBy] VARCHAR (255) NULL,
  [AuditDate] DATETIME2 NOT NULL
)

CREATE TABLE [' + @username + '].[MeetingAttachment] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [FileName] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [Date] DATETIME2 NULL,
  [FileData] VARBINARY(MAX) NULL
)

CREATE TABLE [' + @username + '].[MeetingAttachment_Audit] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [FileName] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [Date] DATETIME2 NULL,
  [FileData] VARBINARY(MAX) NULL,
  [Action] VARCHAR (255) NULL,
  [ChangedBy] VARCHAR (255) NULL,
  [AuditDate] DATETIME2 NOT NULL
)
CREATE TABLE [' + @username + '].[MinutzDecision] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [DescisionText] VARCHAR (max) NULL,
  [Descisioncomment] VARCHAR (max) NULL,
  [AgendaId] VARCHAR (max) NULL,
  [PersonId] uniqueidentifier NULL,
  [CreatedDate] DATETIME2 NULL,
  [IsOverturned] BIT NULL
)

CREATE TABLE [' + @username + '].[MinutzDecision_Audit] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [DescisionText] VARCHAR (max) NULL,
  [Descisioncomment] VARCHAR (max) NULL,
  [AgendaId] VARCHAR (max) NULL,
  [PersonId] uniqueidentifier NULL,
  [CreatedDate] DATETIME2 NULL,
  [IsOverturned] BIT NULL,
  [Action] VARCHAR (255) NULL,
  [ChangedBy] VARCHAR (255) NULL,
  [AuditDate] DATETIME2 NOT NULL
)

';
  EXEC sp_executesql @sql
go

CREATE PROCEDURE [app].[spCreateUserAndSchema]
    @Identity                NVARCHAR(100),
    @Email                   NVARCHAR(100),
    @tenant                  NVARCHAR(100),
    @EncryptedPasswordString NVARCHAR(100),
    @PasswordString          NVARCHAR(100)
AS

-- DECLARE @tenant NVARCHAR(MAX);
-- SET @tenant = 'foo';

-- DECLARE @PasswordString NVARCHAR(MAX);
-- SET @PasswordString = 'bGRQX5Tqzd';

  INSERT into [minutz].[app].[Instance] ([Name], [Username], [Password], [Active], [Type])
  VALUES (@Email, @tenant, @EncryptedPasswordString, 1, 1);

  UPDATE [minutz].[app] . [Person]
  SET [InstanceId] = @tenant,
      [Role]       = 'User'
  WHERE Email = @Email

  DECLARE @createloginSQL nvarchar(4000) = 'CREATE LOGIN ' + @tenant + ' WITH PASSWORD = ''' + @PasswordString + ''' ';
  EXEC sys.sp_executesql @createloginSQL

  DECLARE @createloginuserSQL nvarchar(4000) = 'CREATE USER ' + @tenant + ' FOR LOGIN ' + @tenant + ' ';
  EXEC sys.sp_executesql @createloginuserSQL

  DECLARE @createSchemaSQL nvarchar(1000) = 'CREATE SCHEMA ' + @tenant + ' ';
  EXEC sp_executesql @createSchemaSQL

  DECLARE @assigPermissionsSchemaSQL nvarchar(4000) =
  'GRANT SELECT, UPDATE, DELETE, INSERT ON SCHEMA :: ' + @tenant + ' TO ' + @tenant + ' ';
  EXEC sp_executesql @assigPermissionsSchemaSQL

  DECLARE @createMeetingTablesSql NVARCHAR(max) = '
CREATE TABLE [' + @tenant + '].[Meeting] (
[Id] uniqueidentifier NOT NULL,
[Name] VARCHAR (MAX) NOT NULL,
[Location] VARCHAR (255) NULL,
[Date] DATETIME2 NULL,
[UpdatedDate] DATETIME2 NULL,
[Time] VARCHAR (255) NULL,
[Duration] INT NULL,
[IsReacurance] BIT NULL,
[IsPrivate] BIT NULL,
[ReacuranceType] VARCHAR (255) NULL,
[IsLocked] BIT NULL,
[IsFormal] BIT NULL,
[TimeZone] VARCHAR (255) NULL,
[Tag] VARCHAR (255) NULL,
[Purpose] VARCHAR (255) NULL,
[Status] VARCHAR (255) NULL,
[MeetingOwnerId] VARCHAR (255) NULL,[RecurrenceData] VARCHAR (MAX) NULL,
[Outcome] VARCHAR (255) NULL
)

CREATE TABLE [' + @tenant + '].[Meeting_Audit]
(
[Id] uniqueidentifier NOT NULL,
[Name] VARCHAR (MAX) NOT NULL,
[Location] VARCHAR (255) NULL,
[Date] DATETIME2 NULL,
[UpdatedDate] DATETIME2 NULL,
[Time] VARCHAR (255) NULL,
[Duration] INT NULL,
[IsReacurance] BIT NULL,
[IsPrivate] BIT NULL,
[ReacuranceType] VARCHAR (255) NULL,
[IsLocked] BIT NULL,
[IsFormal] BIT NULL,
[TimeZone] VARCHAR (255) NULL,
[Tag] VARCHAR (255) NULL,
[Purpose] VARCHAR (255) NULL,
[Status] VARCHAR (255) NULL,
[MeetingOwnerId] VARCHAR (255) NULL,
[Outcome] VARCHAR (255) NULL,
[Email] VARCHAR (255) NULL,
[Role] VARCHAR (255) NULL,
[Action] VARCHAR (255) NULL,
[ChangedBy] VARCHAR (255) NULL,
[AuditDate] DATETIME2 NOT NULL
)';
  EXEC sp_executesql @createMeetingTablesSql

  DECLARE @createMeetingAttendeeTablesSql NVARCHAR(max) = '
CREATE TABLE [' + @tenant + '].[MeetingAttendee]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[PersonIdentity] VARCHAR (255) NULL,
[Email] VARCHAR (255) NULL,
[Status] VARCHAR (255) NULL,
[Role] VARCHAR (255) NULL
)

CREATE TABLE [' + @tenant + ' ].[MeetingAttendee_Audit]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[PersonIdentity] VARCHAR (255) NULL,
[Email] VARCHAR (255) NULL,
[Status] VARCHAR (255) NULL,
[Role] VARCHAR (255) NULL,
[Action] VARCHAR (255) NULL,
[ChangedBy] VARCHAR (255) NULL,
[AuditDate] DATETIME2 NOT NULL
)';
  EXEC sp_executesql @createMeetingAttendeeTablesSql

  DECLARE @createAvailibleAttendeeTablesSql NVARCHAR(max) = '
CREATE TABLE [' + @tenant + '].[AvailibleAttendee]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[PersonIdentity] VARCHAR (255) NULL,
[Email] VARCHAR (255) NULL,
[Status] VARCHAR (255) NULL,
[Role] VARCHAR (255) NULL
)

CREATE TABLE [' + @tenant + '].[AvailibleAttendee_Audit]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[PersonIdentity] VARCHAR (255) NULL,
[Email] VARCHAR (255) NULL,
[Status] VARCHAR (255) NULL,
[Role] VARCHAR (255) NULL,
[Action] VARCHAR (255) NULL,
[ChangedBy] VARCHAR (255) NULL,
[AuditDate] DATETIME2 NOT NULL
)';
  EXEC sp_executesql @createAvailibleAttendeeTablesSql

  DECLARE @createMeetingAgendaTablesSql NVARCHAR(max) = '
CREATE TABLE [' + @tenant + '].[MeetingAgenda]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[AgendaHeading] VARCHAR (255) NULL,
[AgendaText] VARCHAR (255) NULL,
[MeetingAttendeeId] VARCHAR (255) NULL,
[Duration] VARCHAR (255) NULL,
[CreatedDate] DATETIME2 NULL,
[IsComplete] BIT NOT NULL DEFAULT 0,
[Order] INT NULL
)

CREATE TABLE [' + @tenant + '].[MeetingAgenda_Audit]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[AgendaHeading] VARCHAR (255) NULL,
[AgendaText] VARCHAR (255) NULL,
[MeetingAttendeeId] VARCHAR (255) NULL,
[Duration] VARCHAR (255) NULL,
[CreatedDate] DATETIME2 NULL,
[IsComplete] BIT NOT NULL DEFAULT 0,
[Order] INT NULL,
[Action] VARCHAR (255) NULL,
[ChangedBy] VARCHAR (255) NULL,
[AuditDate] DATETIME2 NOT NULL
)';
  EXEC sp_executesql @createMeetingAgendaTablesSql

  DECLARE @createMeetingActionTablesSql NVARCHAR(max) = '
CREATE TABLE [' + @tenant + '].[MeetingAction]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[ActionText] VARCHAR (255) NULL,
[PersonId] uniqueidentifier NULL,
[CreatedDate] DATETIME2 NULL,
[DueDate] DATETIME2 NULL,
[IsComplete] BIT NOT NULL DEFAULT 0,
[Order] INT NULL
)

CREATE TABLE [' + @tenant + '].[MeetingAction_Audit]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[ActionText] VARCHAR (255) NULL,
[PersonId] uniqueidentifier NULL,
[CreatedDate] DATETIME2 NULL,
[DueDate] DATETIME2 NULL,
[IsComplete] BIT NOT NULL DEFAULT 0,
[Order] INT NULL,
[Action] VARCHAR (255) NULL,
[ChangedBy] VARCHAR (255) NULL,
[AuditDate] DATETIME2 NOT NULL
)';
  EXEC sp_executesql @createMeetingActionTablesSql

  DECLARE @createMinutzActionTablesSql NVARCHAR(max) = '
CREATE TABLE [' + @tenant + '].[MinutzAction]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[ActionText] VARCHAR (255) NULL,
[PersonId] uniqueidentifier NULL,
[CreatedDate] DATETIME2 NULL,
[DueDate] DATETIME2 NULL,
[IsComplete] BIT NOT NULL DEFAULT 0,
[Order] INT NULL
)

CREATE TABLE [' + @tenant + '].[MinutzAction_Audit]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[ActionText] VARCHAR (255) NULL,
[PersonId] uniqueidentifier NULL,
[CreatedDate] DATETIME2 NULL,
[DueDate] DATETIME2 NULL,
[IsComplete] BIT NOT NULL DEFAULT 0,
[Order] INT NULL,
[Action] VARCHAR (255) NULL,
[ChangedBy] VARCHAR (255) NULL,
[AuditDate] DATETIME2 NOT NULL
)';
  EXEC sp_executesql @createMinutzActionTablesSql

  DECLARE @createMeetingNoteTablesSql NVARCHAR(max) = '
CREATE TABLE [' + @tenant + '].[MeetingNote]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[NoteText] VARCHAR (255) NULL,
[MeetingAttendeeId] uniqueidentifier NULL,
[CreatedDate] DATETIME2 NULL,
[Order] INT NULL
)

CREATE TABLE [' + @tenant + '].[MeetingNote_Audit]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[NoteText] VARCHAR (255) NULL,
[MeetingAttendeeId] uniqueidentifier NULL,
[CreatedDate] DATETIME2 NULL,
[Action] VARCHAR (255) NULL,
[Order] INT NULL,
[ChangedBy] VARCHAR (255) NULL,
[AuditDate] DATETIME2 NOT NULL
)';
  EXEC sp_executesql @createMeetingNoteTablesSql

  DECLARE @createMeetingAttachmentTablesSql NVARCHAR(max) = '
CREATE TABLE [' + @tenant + '].[MeetingAttachment]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[FileName] VARCHAR (255) NULL,
[MeetingAttendeeId] uniqueidentifier NULL,
[Date] DATETIME2 NULL,
[FileData] VARBINARY(MAX) NULL,
[Order] INT NULL
)

CREATE TABLE [' + @tenant + '].[MeetingAttachment_Audit]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[FileName] VARCHAR (255) NULL,
[MeetingAttendeeId] uniqueidentifier NULL,
[Date] DATETIME2 NULL,
[FileData] VARBINARY(MAX) NULL,
[Order] INT NULL,
[Action] VARCHAR (255) NULL,
[ChangedBy] VARCHAR (255) NULL,
[AuditDate] DATETIME2 NOT NULL
)';
  EXEC sp_executesql @createMeetingAttachmentTablesSql

  DECLARE @createMinutzDecisionTablesSql NVARCHAR(max) = '
CREATE TABLE [' + @tenant + '].[MinutzDecision]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[DescisionText] VARCHAR (max) NULL,
[Descisioncomment] VARCHAR (max) NULL,
[AgendaId] VARCHAR (max) NULL,
[PersonId] uniqueidentifier NULL,
[Order] INT NULL,
[CreatedDate] DATETIME2 NULL,
[IsOverturned] BIT NULL
)

CREATE TABLE [' + @tenant + '].[MinutzDecision_Audit]
(
[Id] uniqueidentifier NOT NULL,
[ReferanceId] uniqueidentifier NOT NULL,
[DescisionText] VARCHAR (max) NULL,
[Descisioncomment] VARCHAR (max) NULL,
[AgendaId] VARCHAR (max) NULL,
[PersonId] uniqueidentifier NULL,
[CreatedDate] DATETIME2 NULL,
[IsOverturned] BIT NULL,
[Order] INT NULL,
[ChangedBy] VARCHAR (255) NULL,
[AuditDate] DATETIME2 NOT NULL
)
';
  EXEC sp_executesql @createMinutzDecisionTablesSql
go

