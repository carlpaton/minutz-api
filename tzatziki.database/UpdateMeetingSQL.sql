USE [minutz]
GO

/****** Object: SqlProcedure [app].[createInstanceUser] Script Date: 2017/04/15 6:12:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE app.createMeetingSchema 
 (@tenant NVARCHAR(100))
  AS
  Begin
    DECLARE @sql NVARCHAR(4000) =
'CREATE TABLE' + @tenant +'.[Meeting] (
        [Id] uniqueidentifier NOT NULL,
        [Name] VARCHAR (MAX) NOT NULL,
        [Location] VARCHAR (255) NULL,
        [Date] DATETIME2 NULL,
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
        [Outcome] VARCHAR (255) NULL
)


CREATE TABLE' + @tenant +'.[MeetingAttendee] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [PersonIdentity]
  [Role]

)


CREATE TABLE' + @tenant +'.[MeetingAgenda] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [AgendaText] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [CreatedDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
)


CREATE TABLE' + @tenant +'.[MeetingAction] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [ActionText] VARCHAR (255) NULL,
  [PersonId] uniqueidentifier NULL,
  [DueDate] DATETIME2 NULL,
  [IsComplete] BIT NOT NULL DEFAULT 0,
)


CREATE TABLE' + @tenant +'.[MeetingNote] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [NoteText] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [CreatedDate] DATETIME2 NULL
)


CREATE TABLE' + @tenant +'.[MeetingAttachment] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [FileName] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
  [Date] DATETIME2 NULL,
  [FileData] VARBINARY(MAX) NULL 
)';
EXEC sp_executesql @sql
End
   
