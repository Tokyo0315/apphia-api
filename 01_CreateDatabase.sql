-- =============================================
-- Apphia Website Database Setup
-- Step 1: Create Database, Login, and User
-- Server: localhost\SQLEXPRESS
-- =============================================

-- 1. Create the database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Apphia_Website')
BEGIN
    CREATE DATABASE [Apphia_Website];
    PRINT 'Database [Apphia_Website] created successfully.';
END
ELSE
BEGIN
    PRINT 'Database [Apphia_Website] already exists.';
END
GO

-- 2. Create SQL Login (server-level)
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'apphia_admin')
BEGIN
    CREATE LOGIN [apphia_admin] WITH PASSWORD = 'Apph1a_2026db', DEFAULT_DATABASE = [Apphia_Website], CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF;
    PRINT 'Login [apphia_admin] created successfully.';
END
ELSE
BEGIN
    PRINT 'Login [apphia_admin] already exists.';
END
GO

-- 3. Create Database User mapped to Login
USE [Apphia_Website];
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'apphia_admin')
BEGIN
    CREATE USER [apphia_admin] FOR LOGIN [apphia_admin];
    PRINT 'User [apphia_admin] created successfully.';
END
ELSE
BEGIN
    PRINT 'User [apphia_admin] already exists.';
END
GO

-- 4. Grant db_owner role to the user (full access to this database only)
ALTER ROLE [db_owner] ADD MEMBER [apphia_admin];
PRINT 'User [apphia_admin] added to db_owner role.';
GO

PRINT '========================================';
PRINT 'Database setup complete!';
PRINT 'Server:   localhost\SQLEXPRESS';
PRINT 'Database: Apphia_Website';
PRINT 'Login:    apphia_admin';
PRINT 'Password: Apph1a_2026db';
PRINT '========================================';
GO
