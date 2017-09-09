USE [#catalogue#]
IF EXISTS(SELECT 1 FROM [sys].[schemas] WHERE name = '#schema#')
BEGIN
 DROP SCHEMA [app];
 EXEC ('CREATE SCHEMA [app] AUTHORIZATION [#schema#]');
END
ELSE
BEGIN
 EXEC ('CREATE SCHEMA [app] AUTHORIZATION [#schema#]');
END