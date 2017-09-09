USE [#catalogue#]
IF EXISTS(SELECT 1 FROM [sys].[schemas] WHERE name = '#schema#')
BEGIN
 DROP SCHEMA [app];
 EXEC ('CREATE SCHEMA [#schema#] AUTHORIZATION [dbo]');
END
ELSE
BEGIN
 EXEC ('CREATE SCHEMA [#schema#] AUTHORIZATION [dbo]');
END