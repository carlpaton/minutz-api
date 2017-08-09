# Minutz

This application was created to help people manage thier time and record actions and decitions that have been made while in a meeting. 

## Overall Structure

### Project Information

- Docker
- dotnet core 1.1.2 
- Angular 2
- Auth0 - JWT
- SQL Container

### Controls and Dependancies

- Time Picker: [bootstrap-timepicker](http://jdewit.github.io/bootstrap-timepicker/)
- Date picker [bootstrap-datepicker](https://uxsolutions.github.io/bootstrap-datepicker/?markup=input&format=&weekStart=&startDate=&endDate=&startView=0&minViewMode=0&maxViewMode=4&todayBtn=false&clearBtn=false&language=en&orientation=auto&multidate=&multidateSeparator=&daysOfWeekDisabled=0&daysOfWeekDisabled=6&calendarWeeks=on&autoclose=on&todayHighlight=on&keyboardNavigation=on&forceParse=on&datesDisabled=on&toggleActive=on&defaultViewDate=on#sandbox)
- input [material-design-input-boxes](https://scotch.io/tutorials/google-material-design-input-boxes-in-css3)

### SQL Resources

- SQL Statements [database-sql-server](https://docs.microsoft.com/en-us/sql/t-sql/statements/create-database-sql-server-transact-sql)

### Development Environment

Even though that the project should work in Visual Studio Community, this project was created to function in Visual Studio Code.

#### IDE Considerations

- Visual Studio Code
- Visual Studio 2017
- [sqlectron](https://sqlectron.github.io)
- Microsoft Sql Mangement studio

## Docker 

Install docker from the docker site: [docker for mac](https://www.docker.com/docker-mac). Ensure that your client hass access to 4gig of ram and 2 cpu's.

## Debugging

Ensure that all containers are not running.

View all stopped containers :

	docker ps -a

	
Remove container:

	docker rm {conatinerId}
	
Pull the right Docker images:

	docker pull microsoft/mssql-server-linux

then for .net core:

	docker pull microsoft/aspnetcore:1.1


Open visual studio 2017 and click the debug button, the app will spin up and create the database from the sql container and save the database 
on the development machine. 

Use a TSQL ide to connect to the database server 

- Server Name: 127.0.0.1,1433
- Login: sa
- Password: Password1234

Run the SETUP.sql file found in minutz-database/sql



# Infrastructure

	