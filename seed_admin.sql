USE [Apphia_Website]
GO

-- 1. Create Role
INSERT INTO Roles (Name, Description, IsActive, CreatedByUserId, CreatedDate)
VALUES (N'Admin', N'Administrator', 1, 1, GETDATE())
GO

-- 2. Create Employee
INSERT INTO Employees (EmployeeNumber, LastName, GivenName, MiddleName, Email, IsActive, CreatedByUserId, CreatedDate)
VALUES (N'EMP-001', N'Admin', N'Apphia', N'', N'admin@apphia.com', 1, 1, GETDATE())
GO

-- 3. Create UserAccount
INSERT INTO UserAccounts (UserID, PasswordSalt, Salt, ExpiryDate, EmployeeId, RoleId, IsActive, IsLocked, FailedAttempt, CreatedByUserId, CreatedDate)
VALUES (N'admin@apphia.com', N'z8pZ4a3rPM2GZzh2KTlva6SnFgRn/MaleB4FtXwBat4=', N'e5HZY6LdOl+JueIEKsp8uA==', DATEADD(DAY, 90, GETDATE()), 1, 1, 1, 0, 0, 1, GETDATE())
GO

-- 4. Create SetupSecurityManagement
INSERT INTO SetupSecurityManagements (FailedAttempt, LockTimeOut, IsActive, CreatedByUserId, CreatedDate)
VALUES (5, 15, 1, 1, GETDATE())
GO
