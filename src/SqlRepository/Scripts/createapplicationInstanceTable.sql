USE [#catalogue#]
IF EXISTS(SELECT 1 FROM [#catalogue#].[INFORMATION_SCHEMA].[TABLES] WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME = 'Instance')
BEGIN
DROP TABLE [#schema#].[Instance];
CREATE TABLE [#schema#].[Instance] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (255) NOT NULL,
    [Username] VARCHAR (255) NOT NULL,
    [Password] VARCHAR (255) NOT NULL,
    [Active]   BIT           NOT NULL,
    [Type]     INT           NOT NULL
);
END
ELSE
BEGIN
CREATE TABLE [#schema#].[Instance] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (255) NOT NULL,
    [Username] VARCHAR (255) NOT NULL,
    [Password] VARCHAR (255) NOT NULL,
    [Active]   BIT           NOT NULL,
    [Type]     INT           NOT NULL
);
END
