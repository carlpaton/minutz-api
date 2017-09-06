# Minutz

This application was created to help people manage thier time and record actions and decitions that have been made while in a meeting. 

## Overall Structure

There is a Angular 2 UI application the lives in the minutz/src/minutz-web folder, this is developed with VS Code, the API is in the 
minutz/src/minutz-api, you will need to open the minutz.sln file with Visual Studio 2017. The SQL database lives in a Docker container and will 
be spun up on the first run of the application.


### Contribution

We develop using trunk based development so a feature is created for the unit of work then when work is complete a pull request is created using
the Bitbucket UI with the destination branch being master and assign to a differnet developer.

Please understand GIT first before contributing: [Git 101](https://git-scm.com/book/en/v2/Getting-Started-Git-Basics)

In visual studio, select options, then text editor, all languages, then select tabs and set them to 2 for both, indenting select smart.
When writing classes please remove unnessasary using statements.

### Project Information

- Docker
- dotnet core 1.1.2 
- Angular 2
- Auth0 - JWT
- SQL Container
- GIT [Bitbucket]

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

## Debugging the web

Ensure that nodejs is installed on your system to allow for the npm package management system to be availble. 
For more information you can follow the Angular quick start found here: [Angular](https://angular.io/guide/quickstart)

### Recommented Extentions for Visual Studio Code

- Beautify
- c#
- Debugger for Chrome
- Docker
- Intellisense for CSS class names
- npm
- Path Intellisense
- Powershell
- Project Manager
- Python
- vscode-icons
- XML Tools


Open the folder minutz-web in a text editor, Visual Studio Code is prefered.
Open a terminal in that location and type:
 
	npm install 

then when all the denpendacnies are installed then run:

	npm start


## Debugging the api

Ensure that all containers are not running.

View all stopped containers :

	docker ps -a

	
Remove container:

	docker rm {conatinerId}
	
Pull the right Docker images:

	docker pull microsoft/mssql-server-linux

Running sql locally on any platform:

	docker run --name mssql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=password1234$" --cap-add SYS_PTRACE -p 1401:1433 -d microsoft/mssql-server-linux

Now you can use your sql editor and connect using tcp:127.0.0.1,1433 with the username: sa and password: password1234$

Run the SETUP.SQL found in the database folder in the repo, then run this command in powershell:

	docker ps

Will list the running containers, take the container number from the mssql running container, then commit the changes by running:

	docker commit <containernumber> tzatziki/database:1.0

this will create a image with the sql changes in place.

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

## Build 

Using Bitbucket pipelines the docker image will be created for the api and the ui and pushed to the docker hub repository:

- tzatziki/minutz
- tzatziki/minutz-web

The images are taged by the build system with the build number. Builds are automatic so there is no user intervention.

## Deployment

The docker host will be a Ubuntu server running the Docker domain
Ubuntu Server YEO02-NIX0 : 41.185.30.164.

Using Docker Cloud the deployment of the Testing version will be auto deployed.