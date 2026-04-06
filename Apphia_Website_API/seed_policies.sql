-- Seed Policies, Controls, and RolePolicyControls for Admin Role (Id=3)
-- Run this in SSMS against the Apphia database

-- 1. Insert Policies (matching the Policies enum)
SET IDENTITY_INSERT Policies ON;
INSERT INTO Policies (Id, Name, IsActive, CreatedDate) VALUES
(1, 'role', 1, GETDATE()),
(2, 'user_account', 1, GETDATE()),
(3, 'section_formatting', 1, GETDATE()),
(4, 'workflow', 1, GETDATE()),
(5, 'email_recipient', 1, GETDATE()),
(6, 'career_vacancies', 1, GETDATE()),
(7, 'applicants', 1, GETDATE()),
(8, 'contact', 1, GETDATE()),
(9, 'audit_trail', 1, GETDATE()),
(10, 'product', 1, GETDATE()),
(11, 'product_category', 1, GETDATE()),
(12, 'gallery_photo', 1, GETDATE()),
(13, 'gallery_album', 1, GETDATE()),
(14, 'client', 1, GETDATE());
SET IDENTITY_INSERT Policies OFF;

-- 2. Insert Controls (one per policy, all permissions = true)
SET IDENTITY_INSERT Controls ON;
INSERT INTO Controls (Id, [Create], [Read], [Update], [Delete], [Restore], IsActive, CreatedDate) VALUES
(1, 1, 1, 1, 1, 1, 1, GETDATE()),
(2, 1, 1, 1, 1, 1, 1, GETDATE()),
(3, 1, 1, 1, 1, 1, 1, GETDATE()),
(4, 1, 1, 1, 1, 1, 1, GETDATE()),
(5, 1, 1, 1, 1, 1, 1, GETDATE()),
(6, 1, 1, 1, 1, 1, 1, GETDATE()),
(7, 1, 1, 1, 1, 1, 1, GETDATE()),
(8, 1, 1, 1, 1, 1, 1, GETDATE()),
(9, 1, 1, 1, 1, 1, 1, GETDATE()),
(10, 1, 1, 1, 1, 1, 1, GETDATE()),
(11, 1, 1, 1, 1, 1, 1, GETDATE()),
(12, 1, 1, 1, 1, 1, 1, GETDATE()),
(13, 1, 1, 1, 1, 1, 1, GETDATE()),
(14, 1, 1, 1, 1, 1, 1, GETDATE());
SET IDENTITY_INSERT Controls OFF;

-- 3. Insert RolePolicyControls (link Role 3 to each Policy+Control)
INSERT INTO RolePolicyControls (RoleId, PolicyId, ControlId, IsActive, CreatedDate) VALUES
(3, 1, 1, 1, GETDATE()),
(3, 2, 2, 1, GETDATE()),
(3, 3, 3, 1, GETDATE()),
(3, 4, 4, 1, GETDATE()),
(3, 5, 5, 1, GETDATE()),
(3, 6, 6, 1, GETDATE()),
(3, 7, 7, 1, GETDATE()),
(3, 8, 8, 1, GETDATE()),
(3, 9, 9, 1, GETDATE()),
(3, 10, 10, 1, GETDATE()),
(3, 11, 11, 1, GETDATE()),
(3, 12, 12, 1, GETDATE()),
(3, 13, 13, 1, GETDATE()),
(3, 14, 14, 1, GETDATE());

PRINT 'Policies, Controls, and RolePolicyControls seeded successfully!';
