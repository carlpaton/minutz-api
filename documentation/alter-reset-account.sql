SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [app].[resetAccount]
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

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'AvailibleAttendee')
      EXEC ('DROP TABLE ' + @schema + '.AvailibleAttendee')

    IF EXISTS(SELECT 1
              FROM [INFORMATION_SCHEMA].[TABLES]
              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'MinutzAction')
      EXEC ('DROP TABLE ' + @schema + '.MinutzAction')

    -- DROP LOGIN @schema
  END

GO
