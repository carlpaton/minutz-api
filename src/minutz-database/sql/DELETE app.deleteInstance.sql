

CREATE PROCEDURE app.deleteMeetingSchema 
 (@tenant NVARCHAR(100),@userIdentity NVARCHAR(100),@instanceId NVARCHAR(100))
  AS
  Begin
    DECLARE @sql NVARCHAR(4000) =
'DROP TABLE  [' + @tenant +'].[Meeting]
DROP TABLE  [' + @tenant +'].[MeetingAttendee]
DROP TABLE  [' + @tenant +'].[MeetingAgenda]
DROP TABLE  [' + @tenant +'].[MeetingAttachment]
DROP TABLE  [' + @tenant +']..[MeetingNote]
DROP TABLE  [' + @tenant +'].[MeetingAction]
DELETE FROM sys.schemas WHERE [name] =''' + @tenant +'''
DELETE FROM [app].[Instances] WHERE [name] =''' + @instanceId +'''
UPDATE [app].[Person] SET [InstanceId] ='' WHERE [Identityid] =''' + @userIdentity +'''
';
EXEC sp_executesql @sql
End