CREATE TABLE app.Instance(
   [Id] int IDENTITY(1,1) PRIMARY KEY,
   [Name] varchar(255) NOT NULL,
   [Username] varchar(255) NOT NULL,
   [Password] varchar(255) NOT NULL,
   [Active] BIT NOT NULL,
   [Type] INT NOT NULL
 )