
## Controls and Dependancies

- Time Picker: [bootstrap-timepicker](http://jdewit.github.io/bootstrap-timepicker/)

- Date picker [bootstrap-datepicker](https://uxsolutions.github.io/bootstrap-datepicker/?markup=input&format=&weekStart=&startDate=&endDate=&startView=0&minViewMode=0&maxViewMode=4&todayBtn=false&clearBtn=false&language=en&orientation=auto&multidate=&multidateSeparator=&daysOfWeekDisabled=0&daysOfWeekDisabled=6&calendarWeeks=on&autoclose=on&todayHighlight=on&keyboardNavigation=on&forceParse=on&datesDisabled=on&toggleActive=on&defaultViewDate=on#sandbox)

- input [material-design-input-boxes](https://scotch.io/tutorials/google-material-design-input-boxes-in-css3)

## Environment Variables

- SQLCONNECTION=Server=tcp:127.0.0.1,1433;User ID=sa;pwd=Password1234;database=minutz;
- ASPNETCORE_ENVIRONMENT=Development

## Database 

Install docker and run:

	docker run --name mintz_db -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Password1234' -v C:\Users\dockerdurban\Documents\minutz\tzatziki.database\data:/var/opt/mssql/data  -p 1433:1433 -d microsoft/mssql-server-linux

Then logon and run the SETUP.sql found in the database folder

## Resources 

SQL Statements


- [database-sql-server](https://docs.microsoft.com/en-us/sql/t-sql/statements/create-database-sql-server-transact-sql)