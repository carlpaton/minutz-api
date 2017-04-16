select * from [app].Person
select * from app.Instance
SELECT name FROM sys.schemas
select t.name, s.name from sys.tables t inner join sys.schemas s on s.schema_id = t.schema_id

exec app.createInstanceSchema @tenant=account_235
exec app.createInstanceUser @tenant=account_235

drop table [account_235].[User]
drop schema [account_235]

delete from app.Instance
update [app].Person set InstanceId = null where id = 11
