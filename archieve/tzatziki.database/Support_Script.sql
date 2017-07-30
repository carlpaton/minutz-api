USE [minutz]
--GET THE TABLES ON THE SERVER 
SELECT t.name, s.name FROM sys.tables t INNER JOIN sys.schemas s ON s.schema_id = t.schema_id

--GET THE SCHEMA's FROM THE SERVER
SELECT [name] FROM sys.schemas

--DROP SCHEMA
DROP SCHEMA [account_235]

--GET ALL THE STORED PROCEDURES 
SELECT * FROM information_schema.routines WHERE routine_type = 'PROCEDURE'

--DROP  STORED PROCEDURES 
DROP PROCEDURE [app].[createMeetingSchema]

--CALLING STREDPROCEDURES
EXEC app.createInstanceSchema @tenant=account_235
EXEC app.createInstanceUser @tenant=account_235
EXEC app.createMeetingSchema @tenant=account_9309628653564ed19e9d0d44f2580b06

CREATE PROCEDURE [app].[createMeetingSchema] 
--DROP TABLES
DROP TABLE [account_9309628653564ed19e9d0d44f2580b06].[User]

select * from  [account_f41a450e715d44918e9f67ecfc0d9da8].[Meeting]
select * from  [account_f41a450e715d44918e9f67ecfc0d9da8].[MeetingAttendee] where ReferenceId '63e7c7e1-db80-9e64-028f-e2c75921ada0'
select * from  [account_f41a450e715d44918e9f67ecfc0d9da8].[MeetingAgenda] where ReferanceId = '63e7c7e1-db80-9e64-028f-e2c75921ada0'
DROP TABLE [account_f41a450e715d44918e9f67ecfc0d9da8].[MeetingAttachment]
DROP TABLE [account_f41a450e715d44918e9f67ecfc0d9da8].[MeetingNote]
DROP TABLE [account_f41a450e715d44918e9f67ecfc0d9da8].[MeetingAction]

select * from  [account_f41a450e715d44918e9f67ecfc0d9da8].[MeetingAgenda]
INSERT INTO [account_f41a450e715d44918e9f67ecfc0d9da8].[MeetingAgenda] VALUES (
        '488ce342-4a44-95f3-299a-2a57b5bcd786',
        'c5f72937-d3b0-03a6-15a2-b873e0ba282a',
        'bar',
        '',
        'meetingattendeeId',
        '0',
        '0001/01/01 12:00:00 AM',
        0
        )


--DELETE FROM TABLE

DELETE FROM app.Instance

app.deleteMeetingSchema 

-- 12	google-oauth2|117785067536320807071	Lee-Roy Ashworth	NULL	NULL	leeroya@gmail.com	Admin	1	
-- f41a450e-715d-4491-8e9f-67ecfc0d9da8
-- drop table [account_8da9e136e5784b17b436018cdeb1ebba].[User]

--UPDATE TABLE
UPDATE [app].Person SET InstanceId = null WHERE id = 11

--f41a450e-715d-4491-8e9f-67ecfc0d9da8
--MINE 31FB6E5D-3ADC-14BD-1FB8-6C1A4228D063
SELECT * FROM app.Instance
SELECT * FROM [app].Person
SELECT * FROM [account_f41a450e715d44918e9f67ecfc0d9da8].[User]
--SELECT FROM TABLE
UPDATE [account_f41a450e715d44918e9f67ecfc0d9da8].[Meeting] SET MeetingOwnerId = 'google-oauth2|117785067536320807071' WHERE Id = 1
SELECT * FROM [account_f41a450e715d44918e9f67ecfc0d9da8].[Meeting]
SELECT * FROM [account_9309628653564ed19e9d0d44f2580b06].[User]
SELECT * FROM [app].Person
SELECT * FROM app.Instance
SELECT * FROM account_235.Meeting

SELECT * FROM [account_f41a450e715d44918e9f67ecfc0d9da8].[Meeting]
SELECT * FROM [account_f41a450e715d44918e9f67ecfc0d9da8].[MeetingAgenda]

SELECT * FROM [account_235].[Meeting] WHERE  Id = '5423f6fb-b764-4f27-bb63-26b04c72cba6'
--DELETE
DELETE FROM account_235.Meeting WHERE Id = '0F5301C5-6B2A-43DA-9CCB-772C27E0B5BA'

IF EXISTS (SELECT 1 
           FROM [database].INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_TYPE='BASE TABLE' 
           AND TABLE_NAME='Person' AND 
           TABLE_SCHEMA = 'app1' )
   SELECT 1 AS res ELSE SELECT 0 AS res;
END

CREATE PROCEDURE app.createMeetingSchema 
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
  [AgendaText] VARCHAR (255) NULL,
  [MeetingAttendeeId] uniqueidentifier NULL,
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


INSERT INTO [account_235].[Meeting] VALUES (
        'e577cfad-0f93-480f-b7e6-92dada35bd61',
        '8da9e136-e578-4b17-b436-018cdeb1ebba',
        '',
        '',
        '6/2/2017 7:28:18 PM',
        '6/2/2017 7:28:18 PM',
        '',
        '0',
         0,
         0,
         '',
         0,
         0,
         '',
        '',
        '',
        ''
        )



BEGIN
CREATE DATABASE minutz
END;
GO 
USE minutz
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE NAME='minutz' AND xtype='U')
BEGIN
	CREATE TABLE Demo
	(
		id INT NULL,
		name VARCHAR(200) NULL,
	)
END

SELECT * FROM Demo
INSERT INTO Demo values(1,'Lee');
SELECT * FROM Demo

