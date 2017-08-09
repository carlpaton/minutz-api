USE [minutz]
GO

/****** Object: Table [app].[Instance] Script Date: 2017/04/15 5:11:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [app].[Instance] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (255) NOT NULL,
    [Username] VARCHAR (255) NOT NULL,
    [Password] VARCHAR (255) NOT NULL,
    [Active]   BIT           NOT NULL,
    [Type]     INT           NOT NULL
);


