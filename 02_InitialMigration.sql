IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [ApiCommands] (
    [Id] int NOT NULL IDENTITY,
    [Command] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_ApiCommands] PRIMARY KEY ([Id])
);

CREATE TABLE [ApplicantAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_ApplicantAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [Approvals] (
    [Id] int NOT NULL IDENTITY,
    [Token] nvarchar(max) NOT NULL,
    [Module] nvarchar(max) NOT NULL,
    [TransactionId] int NOT NULL,
    [ExpirationDate] datetime2 NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Approvals] PRIMARY KEY ([Id])
);

CREATE TABLE [CareerVacancies] (
    [Id] int NOT NULL IDENTITY,
    [HasContentChanges] bit NULL,
    [IsOnWorkflow] bit NULL,
    [JobTitle] nvarchar(max) NOT NULL,
    [ShortDescription] nvarchar(max) NOT NULL,
    [FullDescription] nvarchar(max) NOT NULL,
    [LookingFor] nvarchar(max) NOT NULL,
    [Qualifications] nvarchar(max) NOT NULL,
    [Status] bit NULL,
    [Thumbnail] nvarchar(max) NULL,
    [Category] nvarchar(max) NOT NULL,
    [CategoryId] nvarchar(450) NOT NULL,
    [JobType] nvarchar(max) NOT NULL,
    [Slug] nvarchar(max) NOT NULL,
    [YearsOfExperience] nvarchar(max) NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_CareerVacancies] PRIMARY KEY ([Id])
);

CREATE TABLE [CareerVacancyAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_CareerVacancyAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [CareerVacancyVersions] (
    [Id] int NOT NULL IDENTITY,
    [CareerVacancyId] int NOT NULL,
    [ApprovalStatus] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_CareerVacancyVersions] PRIMARY KEY ([Id])
);

CREATE TABLE [CareerVacancyVersionWorkflowApprovers] (
    [Id] int NOT NULL IDENTITY,
    [CareerVacancyVersionWorkflowId] int NOT NULL,
    [ApproverName] nvarchar(max) NULL,
    [EmailAddress] nvarchar(max) NULL,
    [ApprovalOrder] int NULL,
    [ApprovalStatus] nvarchar(max) NULL,
    [ApprovalDate] datetime2 NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_CareerVacancyVersionWorkflowApprovers] PRIMARY KEY ([Id])
);

CREATE TABLE [CareerVacancyVersionWorkflows] (
    [Id] int NOT NULL IDENTITY,
    [CareerVacancyVersionId] int NOT NULL,
    [WorkflowId] int NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_CareerVacancyVersionWorkflows] PRIMARY KEY ([Id])
);

CREATE TABLE [ClientAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_ClientAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [Clients] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Logo] nvarchar(max) NULL,
    [SortOrder] int NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
);

CREATE TABLE [ContactAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_ContactAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [Contacts] (
    [Id] int NOT NULL IDENTITY,
    [firstName] nvarchar(max) NULL,
    [lastName] nvarchar(max) NULL,
    [email] nvarchar(max) NULL,
    [contactNo] nvarchar(max) NULL,
    [inquiry] nvarchar(max) NULL,
    [message] nvarchar(max) NULL,
    [cookieAccepted] bit NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([Id])
);

CREATE TABLE [Controls] (
    [Id] int NOT NULL IDENTITY,
    [Create] bit NOT NULL,
    [Read] bit NOT NULL,
    [Update] bit NOT NULL,
    [Delete] bit NOT NULL,
    [Restore] bit NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Controls] PRIMARY KEY ([Id])
);

CREATE TABLE [DashboardCountries] (
    [Id] int NOT NULL IDENTITY,
    [Domain] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [Count] int NOT NULL,
    [GeneratedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_DashboardCountries] PRIMARY KEY ([Id])
);

CREATE TABLE [DashboardDevices] (
    [Id] int NOT NULL IDENTITY,
    [Domain] nvarchar(max) NOT NULL,
    [Device] nvarchar(max) NOT NULL,
    [Count] int NOT NULL,
    [GeneratedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_DashboardDevices] PRIMARY KEY ([Id])
);

CREATE TABLE [DashboardEngagements] (
    [Id] int NOT NULL IDENTITY,
    [Domain] nvarchar(max) NOT NULL,
    [TotalTime] int NOT NULL,
    [ActiveTime] int NOT NULL,
    [GeneratedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_DashboardEngagements] PRIMARY KEY ([Id])
);

CREATE TABLE [DashboardPages] (
    [Id] int NOT NULL IDENTITY,
    [Domain] nvarchar(max) NOT NULL,
    [Url] nvarchar(max) NOT NULL,
    [Count] int NOT NULL,
    [GeneratedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_DashboardPages] PRIMARY KEY ([Id])
);

CREATE TABLE [EmailRecipientAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_EmailRecipientAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [EmailRecipients] (
    [Id] int NOT NULL IDENTITY,
    [email] nvarchar(max) NOT NULL,
    [segment] nvarchar(max) NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_EmailRecipients] PRIMARY KEY ([Id])
);

CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [EmployeeNumber] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [GivenName] nvarchar(max) NOT NULL,
    [MiddleName] nvarchar(max) NULL,
    [Email] nvarchar(max) NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
);

CREATE TABLE [GalleryAlbumAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_GalleryAlbumAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [GalleryAlbums] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Thumbnail] nvarchar(max) NULL,
    [SortOrder] int NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_GalleryAlbums] PRIMARY KEY ([Id])
);

CREATE TABLE [GalleryPhotoAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_GalleryPhotoAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [PasswordResetRequests] (
    [Id] int NOT NULL IDENTITY,
    [UserAccountId] int NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [ExpirationDate] datetime2 NOT NULL,
    [IsUsed] bit NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_PasswordResetRequests] PRIMARY KEY ([Id])
);

CREATE TABLE [Policies] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Policies] PRIMARY KEY ([Id])
);

CREATE TABLE [ProductAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_ProductAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [ProductCategories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [SortOrder] int NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_ProductCategories] PRIMARY KEY ([Id])
);

CREATE TABLE [ProductCategoryAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_ProductCategoryAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [RoleAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_RoleAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [RolePolicyControls] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [PolicyId] int NOT NULL,
    [ControlId] int NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_RolePolicyControls] PRIMARY KEY ([Id])
);

CREATE TABLE [Roles] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

CREATE TABLE [SectionFormattingAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_SectionFormattingAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [SectionFormattings] (
    [Id] int NOT NULL IDENTITY,
    [HasContentChanges] bit NULL,
    [IsOnWorkflow] bit NULL,
    [Tab] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [TabOrder] int NULL,
    [Html] nvarchar(max) NULL,
    [Css] nvarchar(max) NULL,
    [Js] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_SectionFormattings] PRIMARY KEY ([Id])
);

CREATE TABLE [SectionFormattingVersions] (
    [Id] int NOT NULL IDENTITY,
    [SectionFormattingId] int NOT NULL,
    [Html] nvarchar(max) NULL,
    [Css] nvarchar(max) NULL,
    [Js] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [ApprovalStatus] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_SectionFormattingVersions] PRIMARY KEY ([Id])
);

CREATE TABLE [SectionFormattingVersionWorkflowApprovers] (
    [Id] int NOT NULL IDENTITY,
    [SectionFormattingVersionWorkflowId] int NOT NULL,
    [ApproverName] nvarchar(max) NULL,
    [EmailAddress] nvarchar(max) NULL,
    [ApprovalOrder] int NULL,
    [ApprovalStatus] nvarchar(max) NULL,
    [ApprovalDate] datetime2 NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_SectionFormattingVersionWorkflowApprovers] PRIMARY KEY ([Id])
);

CREATE TABLE [SectionFormattingVersionWorkflows] (
    [Id] int NOT NULL IDENTITY,
    [SectionFormattingVersionId] int NOT NULL,
    [WorkflowId] int NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_SectionFormattingVersionWorkflows] PRIMARY KEY ([Id])
);

CREATE TABLE [SetupSecurityAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_SetupSecurityAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [SetupSecurityManagements] (
    [Id] int NOT NULL IDENTITY,
    [FailedAttempt] int NULL,
    [LockTimeOut] int NULL,
    [IsDisableTimeOut] bit NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_SetupSecurityManagements] PRIMARY KEY ([Id])
);

CREATE TABLE [TempPasswords] (
    [Id] int NOT NULL IDENTITY,
    [tempPassword] nvarchar(max) NOT NULL,
    [UserID] int NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_TempPasswords] PRIMARY KEY ([Id])
);

CREATE TABLE [UserAccountAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_UserAccountAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [UserAccounts] (
    [Id] int NOT NULL IDENTITY,
    [UserID] nvarchar(max) NOT NULL,
    [PasswordSalt] nvarchar(max) NOT NULL,
    [Salt] nvarchar(max) NOT NULL,
    [ExpiryDate] datetime2 NOT NULL,
    [IsLocked] bit NULL,
    [RequiredPasswordChange] bit NULL,
    [FailedAttempt] int NOT NULL,
    [LockedDate] datetime2 NULL,
    [EmployeeId] int NOT NULL,
    [RoleId] int NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_UserAccounts] PRIMARY KEY ([Id])
);

CREATE TABLE [WorkflowApprovers] (
    [Id] int NOT NULL IDENTITY,
    [WorkflowId] int NOT NULL,
    [ApproverName] nvarchar(max) NULL,
    [EmailAddress] nvarchar(max) NULL,
    [ApprovalOrder] int NULL,
    [PermanentDelete] bit NOT NULL,
    [PermanentDeletedByUserId] int NULL,
    [PermanentDeletedDate] datetime2 NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_WorkflowApprovers] PRIMARY KEY ([Id])
);

CREATE TABLE [WorkflowAudits] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [ActionByUserId] nvarchar(max) NOT NULL,
    [ActionByEmail] nvarchar(max) NOT NULL,
    [ActionByRoleId] nvarchar(max) NOT NULL,
    [ActionByRole] nvarchar(max) NOT NULL,
    [ActionByName] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_WorkflowAudits] PRIMARY KEY ([Id])
);

CREATE TABLE [Workflows] (
    [Id] int NOT NULL IDENTITY,
    [WorkflowName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [ApprovalType] nvarchar(max) NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Workflows] PRIMARY KEY ([Id])
);

CREATE TABLE [Applicants] (
    [Id] int NOT NULL IDENTITY,
    [CareerVacancyId] int NOT NULL,
    [AboutUs] nvarchar(max) NOT NULL,
    [GivenName] nvarchar(max) NOT NULL,
    [MiddleName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [CurrentAddress] nvarchar(max) NOT NULL,
    [PermanentAddress] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [ContactNumber] nvarchar(max) NOT NULL,
    [Resume] nvarchar(max) NOT NULL,
    [Disability] bit NOT NULL,
    [Crime] bit NOT NULL,
    [Travel] bit NOT NULL,
    [Disease] bit NOT NULL,
    [Resign] bit NOT NULL,
    [cookieAccepted] bit NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Applicants] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Applicants_Employees_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Employees] ([Id]),
    CONSTRAINT [FK_Applicants_Employees_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [Employees] ([Id]),
    CONSTRAINT [FK_Applicants_Employees_RestoredByUserId] FOREIGN KEY ([RestoredByUserId]) REFERENCES [Employees] ([Id]),
    CONSTRAINT [FK_Applicants_Employees_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [Employees] ([Id])
);

CREATE TABLE [GalleryPhotos] (
    [Id] int NOT NULL IDENTITY,
    [Image] nvarchar(max) NOT NULL,
    [Caption] nvarchar(max) NULL,
    [SortOrder] int NULL,
    [GalleryAlbumId] int NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_GalleryPhotos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GalleryPhotos_GalleryAlbums_GalleryAlbumId] FOREIGN KEY ([GalleryAlbumId]) REFERENCES [GalleryAlbums] ([Id])
);

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Image] nvarchar(max) NULL,
    [ProductCategoryId] int NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_ProductCategories_ProductCategoryId] FOREIGN KEY ([ProductCategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ApplicantCharacterReferences] (
    [Id] int NOT NULL IDENTITY,
    [ApplicantId] int NOT NULL,
    [Name] nvarchar(max) NULL,
    [Company] nvarchar(max) NULL,
    [ContactNumber] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_ApplicantCharacterReferences] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApplicantCharacterReferences_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ApplicantEducationExperiences] (
    [Id] int NOT NULL IDENTITY,
    [ApplicantId] int NOT NULL,
    [SchoolName] nvarchar(max) NULL,
    [Degree] nvarchar(max) NULL,
    [FieldOfStudy] nvarchar(max) NULL,
    [StartDate] nvarchar(max) NULL,
    [EndDate] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_ApplicantEducationExperiences] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApplicantEducationExperiences_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ApplicantWorkExperiences] (
    [Id] int NOT NULL IDENTITY,
    [ApplicantId] int NOT NULL,
    [JobTitle] nvarchar(max) NULL,
    [CompanyName] nvarchar(max) NULL,
    [StartDate] nvarchar(max) NULL,
    [EndDate] nvarchar(max) NULL,
    [CurrentWork] bit NOT NULL,
    [IsActive] bit NULL,
    [CreatedByUserId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedByUserId] int NULL,
    [UpdatedDate] datetime2 NULL,
    [DeletedByUserId] int NULL,
    [DeletedDate] datetime2 NULL,
    [RestoredByUserId] int NULL,
    [RestoredDate] datetime2 NULL,
    CONSTRAINT [PK_ApplicantWorkExperiences] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApplicantWorkExperiences_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_ApplicantCharacterReferences_ApplicantId] ON [ApplicantCharacterReferences] ([ApplicantId]);

CREATE INDEX [IX_ApplicantEducationExperiences_ApplicantId] ON [ApplicantEducationExperiences] ([ApplicantId]);

CREATE INDEX [IX_Applicants_CreatedByUserId] ON [Applicants] ([CreatedByUserId]);

CREATE INDEX [IX_Applicants_DeletedByUserId] ON [Applicants] ([DeletedByUserId]);

CREATE INDEX [IX_Applicants_RestoredByUserId] ON [Applicants] ([RestoredByUserId]);

CREATE INDEX [IX_Applicants_UpdatedByUserId] ON [Applicants] ([UpdatedByUserId]);

CREATE INDEX [IX_ApplicantWorkExperiences_ApplicantId] ON [ApplicantWorkExperiences] ([ApplicantId]);

CREATE UNIQUE INDEX [IX_CareerVacancies_CategoryId] ON [CareerVacancies] ([CategoryId]);

CREATE INDEX [IX_GalleryPhotos_GalleryAlbumId] ON [GalleryPhotos] ([GalleryAlbumId]);

CREATE INDEX [IX_Products_ProductCategoryId] ON [Products] ([ProductCategoryId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260311014844_InitialCreate', N'9.0.5');

COMMIT;
GO

