USE [minutz]
GO

/****** Object: SqlProcedure [app].[createInstanceSchema] Script Date: 2017/04/15 5:13:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE app.createInstanceSchema 
 (@tenant nvarchar(100))
  AS
  Begin
    declare @sql nvarchar(4000) = 'create schema ' + @tenant + ' authorization  dbo';

    EXEC sp_executesql @sql

  End
