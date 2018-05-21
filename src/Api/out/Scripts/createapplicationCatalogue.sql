USE [master]

IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name = '#catalogue#')
BEGIN
DROP DATABASE #catalogue#;
 EXEC ('CREATE DATABASE [#catalogue#]');
END
ELSE
BEGIN
 EXEC ('CREATE DATABASE [#catalogue#]');
END
