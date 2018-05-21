USE [#catalogue#]
IF EXISTS(SELECT 1 FROM [#catalogue#].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'Person')
BEGIN
DROP TABLE [#schema#].[Person];

CREATE TABLE [#schema#].[Person] (
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
ELSE
BEGIN
CREATE TABLE [#schema#].[Person] (
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