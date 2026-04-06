-- Migration: Update ApiCommands table to match SSCGI notification API pattern
-- Run this on the Apphia_Website database

-- Step 1: Drop the old columns from BaseModel that are no longer needed
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ApiCommands') AND name = 'Command')
BEGIN
    -- Drop old columns
    ALTER TABLE ApiCommands DROP COLUMN Command;
    ALTER TABLE ApiCommands DROP COLUMN Description;
    ALTER TABLE ApiCommands DROP COLUMN Status;
    ALTER TABLE ApiCommands DROP COLUMN CreatedByUserId;
    ALTER TABLE ApiCommands DROP COLUMN CreatedDate;
    ALTER TABLE ApiCommands DROP COLUMN UpdatedByUserId;
    ALTER TABLE ApiCommands DROP COLUMN UpdatedDate;
    ALTER TABLE ApiCommands DROP COLUMN DeletedByUserId;
    ALTER TABLE ApiCommands DROP COLUMN DeletedDate;
    ALTER TABLE ApiCommands DROP COLUMN RestoredByUserId;
    ALTER TABLE ApiCommands DROP COLUMN RestoredDate;

    -- Add new columns
    ALTER TABLE ApiCommands ADD Code NVARCHAR(MAX) NULL;
    ALTER TABLE ApiCommands ADD Name NVARCHAR(MAX) NULL;
    ALTER TABLE ApiCommands ADD Value NVARCHAR(MAX) NULL;

    PRINT 'ApiCommands table migrated successfully.';
END
ELSE
BEGIN
    PRINT 'ApiCommands table already has the new schema.';
END
GO

-- Step 2: Seed notification API config (replace placeholders with SSCGI values)
-- Run: SELECT * FROM [SSCGI_Website].[dbo].[ApiCommands] WHERE IsActive = 1
-- Then replace the placeholder values below

IF NOT EXISTS (SELECT 1 FROM ApiCommands WHERE Name = 'BaseUrl')
BEGIN
    INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'BaseUrl', '<<COPY FROM SSCGI>>', 1);
    INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'ClientId', '<<COPY FROM SSCGI>>', 1);
    INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'ClientKey', '<<COPY FROM SSCGI>>', 1);
    INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'Token', '', 1);
    INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'Expired', '2000-01-01', 1);
    PRINT 'ApiCommands seeded.';
END
GO
