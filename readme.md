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

## Run the app

    docker run -d -e ASPNETCORE_ENVIRONMENT=Development -p 5000:5000 minutz:1.0

## Run the database

    docker run --name mintz_db -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Password1234' -v C:\Users\dockerdurban\Documents\minutz\tzatziki.database\data:/var/opt/mssql/data  -p 1433:1433 -d microsoft/mssql-server-linux

## Build the required images

### Build running image

    docker build -t dockerdurban/aspnetcore:nodejs-aspnet-1-1-2 -f Dockerfile .

### Build debug image

    docker build -t dockerdurban/aspnetcore:nodejs-aspnet-1-1-2-sdk -f Dockerfile.debug .

### Build the application image

    docker build -t minutz:1.0 .