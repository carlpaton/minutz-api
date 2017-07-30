USE [minutz]
GO

/****** Object: Table [app].[Person] Script Date: 2017/04/14 10:48:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [app].[Person] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Identityid] VARCHAR (MAX) NOT NULL,
    [FirstName]  VARCHAR (255) NULL,
    [LastName]   VARCHAR (255) NULL,
    [FullName]   VARCHAR (255) NULL,
    [Email]      VARCHAR (255) NOT NULL,
    [Role]       VARCHAR (255) NOT NULL,
    [Active]     BIT           NOT NULL,
    [InstanceId] VARCHAR (255) NULL
);


