select * from app.Person
select * from app.Instance
-- update app.Person set Role = 'Guest' where id = 2026
-- delete from app.Person where id in (1003,1002)

-- drop login A_59c2bea77c2eab787c175e72
EXECUTE [app].[resetAccount] 
   'A_5a0c7e540d9ef535b8245cb7'
  ,'nathan.lee.ashworth@gmail.com'
  ,'A_5a0c7e540d9ef535b8245cb7'

  drop SCHEMA A_5a0c7e540d9ef535b8245cb7
  drop USER A_5a0c7e540d9ef535b8245cb7
  drop LOGIN A_5a0c7e540d9ef535b8245cb7

update app.Person set InstanceId = null where id = 1002

  delete from app.Instance where id = 1004

  A_5a0c7e540d9ef535b8245cb7