select * from app.person

select * from A_117330763949240782758.Meeting
select * from A_59c2bea77c2eab787c175e72.meeting

delete  from A_59c2bea77c2eab787c175e72.meeting

    update  A_59c2bea77c2eab787c175e72.meeting set [Name] = 'demo' where id = 'b585ce31-acf3-4f05-a8f5-3359a7952414' 
select * from A_59c2bea77c2eab787c175e72.availibleattendee

-- delete from A_59c2bea77c2eab787c175e72.availibleattendee

-- 429d11df-551c-428a-aa77-38a39be0214b meetingid
-- A_59c2bea77c2eab787c175e72 instance id
-- invite|A_117330763949240782758&ef4d9055-1517-4cf5-8ca4-064565c81f5b
-- reference|A_117330763949240782758&ef4d9055-1517-4cf5-8ca4-064565c81f5b

update app.Person set Related = 'invite|A_117330763949240782758&ef4d9055-1517-4cf5-8ca4-064565c81f5b' where id = 3034

exec [app].[resetAccount]'A_59c2bea77c2eab787c175e72','info@docker.durban','A_59c2bea77c2eab787c175e72';
ALTER DATABASE [MINUTZ-TEST] set single_user with rollback immediate; 
DROP SCHEMA A_59c2bea77c2eab787c175e72;
DROP USER A_59c2bea77c2eab787c175e72;
DROP LOGIN A_59c2bea77c2eab787c175e72;
ALTER DATABASE [MINUTZ-TEST] set MULTI_USER;

-- invite|A_117330763949240782758&ef4d9055-1517-4cf5-8ca4-064565c81f5b