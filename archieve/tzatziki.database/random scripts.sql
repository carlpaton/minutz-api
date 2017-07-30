USE [minutz]
GO

DECLARE @schema NVARCHAR(200) = 'account_564c3f6b0efd43f8b52b3253bc55e67e'
DECLARE @meetingId NVARCHAR(200) = '3BBE9F7F-C505-CEDB-1C32-45E8D2055945'
DECLARE @OldMeetingId NVARCHAR(200)
DECLARE @meetingSQLId NVARCHAR(200) = 'SELECT TOP 1 Id FROM ['+ @schema +'].[Meeting] WHERE Id = ''' + @meetingId + ''''
DECLARE @meetingSQL NVARCHAR(200) = 'SELECT TOP 1 * FROM ['+ @schema +'].[Meeting] WHERE Id = ''' + @meetingId + ''''
SET @OldMeetingId = NULL 
EXEC @OldMeetingId = @meetingSQLId
SELECT @OldMeetingId

IF EXISTS( EXEC (@meetingSQL))
SELECT 'yes'
ELSE
Select 'no'

GO
DECLARE @OldMeetingId NVARCHAR(200)
EXEC @OldMeetingId= [app].[CheckIfMeetingExists]@schema='account_564c3f6b0efd43f8b52b3253bc55e67e',@meetingId='3BBE9F7F-C505-CEDB-1C32-45E8D2055945', @OldMeetingId OUTPUT
SELECT @OldMeetingId AS MeetingId
GO

DROP PROCEDURE [app].[CheckIfMeetingExists]
CREATE PROCEDURE [app].[CheckIfMeetingExists](@schema nvarchar(100),@meetingId nvarchar(250),@OUTPARAM VARCHAR(20) OUT)
  AS
  Begin
   declare @sql nvarchar(4000) = '@OUTPARAM = SELECT TOP 1 * FROM [' + @schema +'].[Meeting] WHERE Id = ''' + @meetingId + ''''
   EXEC @OUTPARAM = sp_executesql @sql
END

Declare @ID varchar(200)    
EXECUTE  [dbo].[test] '3BBE9F7F-C505-CEDB-1C32-45E8D2055945','account_564c3f6b0efd43f8b52b3253bc55e67e',@ID OUTPUT   
SELECT @ID

DROP Procedure [dbo].[test]

Create Procedure [dbo].[test]
@Id varchar(200),
@schema varchar(200),
@output varchar(200) Output   
As  
Begin  
declare @sql nvarchar(4000) = 'SELECT TOP 1 * FROM [' + @schema +'].[Meeting] WHERE Id = ''' + @Id + ''''
   SELECT @ = EXEC @output = ( @sql)
-- SELECT @output = Id from [account_564c3f6b0efd43f8b52b3253bc55e67e].[Meeting] where  Id = @Id   
Return;
END  

SELECT * FROM [account_564c3f6b0efd43f8b52b3253bc55e67e].[MeetingAgenda] WHERE  ReferanceId = '3bbe9f7f-c505-cedb-1c32-45e8d2055945'
select * from [account_564c3f6b0efd43f8b52b3253bc55e67e].[Meeting]

INSERT INTO [account_564c3f6b0efd43f8b52b3253bc55e67e].[Meeting]
           ([Id]
           ,[Name]
           ,[Location]
           ,[Date]
           ,[UpdatedDate]
           ,[Time]
           ,[Duration]
           ,[IsReacurance]
           ,[IsPrivate]
           ,[ReacuranceType]
           ,[IsLocked]
           ,[IsFormal]
           ,[TimeZone]
           ,[Tag]
           ,[Purpose]
           ,[MeetingOwnerId]
           ,[Outcome])
     VALUES
           (<Id, uniqueidentifier,>
           ,<Name, varchar(max),>
           ,<Location, varchar(255),>
           ,<Date, datetime2(7),>
           ,<UpdatedDate, datetime2(7),>
           ,<Time, varchar(255),>
           ,<Duration, int,>
           ,<IsReacurance, bit,>
           ,<IsPrivate, bit,>
           ,<ReacuranceType, varchar(255),>
           ,<IsLocked, bit,>
           ,<IsFormal, bit,>
           ,<TimeZone, varchar(255),>
           ,<Tag, varchar(255),>
           ,<Purpose, varchar(255),>
           ,<MeetingOwnerId, varchar(255),>
           ,<Outcome, varchar(255),>)
GO

update [account_564c3f6b0efd43f8b52b3253bc55e67e].[MeetingAgenda] set ReferanceId ='D37FE467-2394-475F-A2AD-1BE50B29522F'

--delete from [account_564c3f6b0efd43f8b52b3253bc55e67e].[Meeting] WHERE Id in( Select top  10 Id from  [account_564c3f6b0efd43f8b52b3253bc55e67e].[Meeting])