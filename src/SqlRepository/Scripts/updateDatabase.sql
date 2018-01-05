-- ALTER TABLES

ALTER TABLE app.instance   -- instance is the account table
ADD subscriptionId int NULL
GO
ALTER TABLE app.instance
ADD subscriptionDate DATETIME2 NULL
GO
ALTER TABLE app.instance
ADD logo BINARY NULL
GO
ALTER TABLE app.instance
ADD colour VARCHAR(200) NULL
GO
ALTER TABLE app.instance
ADD style VARCHAR(200) NULL
GO
ALTER TABLE app.instance
ADD allowInformation BIT
GO
ALTER TABLE app.instance
ADD notificationTypeId INT NULL
GO
ALTER TABLE app.instance
ADD notificationRoleId INT NULL
GO
ALTER TABLE app.instance
ADD reminderId INT NULL

-- - id
-- - username
-- - password
-- - type
-- - active
-- - subscription
-- - subscriptionDate
-- - logo - byte[]
-- - colour - string
-- - style - string
-- - allowInformal - bool
-- - notificationTypeId - int
-- - notificationRoleId - int
-- - reminderId


CREATE TABLE app.subscription
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (200) NOT NULL,
    [Description]  VARCHAR (MAX) NULL,
    [Term] INT NULL,
    [Cost] INT  NULL
)
GO

CREATE TABLE app.notificationType
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (200) NOT NULL
)
GO

INSERT INTO app.notificationType VALUES ('app');
INSERT INTO app.notificationType VALUES ('email');

CREATE TABLE app.notificationRole
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (200) NOT NULL
)
GO

INSERT INTO app.notificationRole values('Allow users to set individually');
INSERT INTO app.notificationRole values('Don''t send reminders');
INSERT INTO app.notificationRole values('Send Company-wide reminders as follows');

CREATE TABLE app.reminder
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (200) NOT NULL,
    [Description] VARCHAR (200) NOT NULL
)
GO

INSERT INTO app.reminder VALUES (1,'On Monday');
INSERT INTO app.reminder VALUES (2,'On Tuesday');
INSERT INTO app.reminder VALUES (3,'On Wednesday');
INSERT INTO app.reminder VALUES (4,'On Thursday');
INSERT INTO app.reminder VALUES (5,'On Friday');
INSERT INTO app.reminder VALUES (6,'On Saturday');
INSERT INTO app.reminder VALUES (7,'On Sunday');
INSERT INTO app.reminder VALUES (8,'1 day before the meeting');
INSERT INTO app.reminder VALUES (9,'daily');
INSERT INTO app.reminder VALUES (10,'on the day of the meeting');
