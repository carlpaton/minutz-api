

CREATE TABLE [Meeting] (
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
GO;

CREATE TABLE [MeetingAttendee] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
  [PersonIdentity]
  [Role]

)
GO;

CREATE TABLE [MeetingAgenda] (
  [Id] uniqueidentifier NOT NULL,
  [MeetingId] uniqueidentifier NOT NULL,
)
GO;

CREATE TABLE [MeetingAction] (
  [Id] uniqueidentifier NOT NULL,
  [MeetingId] uniqueidentifier NOT NULL,
)
GO;

CREATE TABLE [MeetingNote] (
  [Id] uniqueidentifier NOT NULL,
  [MeetingId] uniqueidentifier NOT NULL,
)
GO;

CREATE TABLE [MeetingAttachment] (
  [Id] uniqueidentifier NOT NULL,
  [ReferanceId] uniqueidentifier NOT NULL,
)
GO;
   
