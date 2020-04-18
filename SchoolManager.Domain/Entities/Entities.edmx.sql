
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/17/2020 13:54:58
-- Generated from EDMX file: C:\Users\ayelen\Documents\GitHub\SchoolManager\SchoolManager.Domain\Entities\Entities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SchoolManager];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posts] DROP CONSTRAINT [FK_UserPost];
GO
IF OBJECT_ID(N'[dbo].[FK_PostReply]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Replies] DROP CONSTRAINT [FK_PostReply];
GO
IF OBJECT_ID(N'[dbo].[FK_UserReply]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Replies] DROP CONSTRAINT [FK_UserReply];
GO
IF OBJECT_ID(N'[dbo].[FK_UserClassroom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserClassroom];
GO
IF OBJECT_ID(N'[dbo].[FK_ClassroomPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posts] DROP CONSTRAINT [FK_ClassroomPost];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleUsers_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleUsers] DROP CONSTRAINT [FK_RoleUsers_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleUsers_Roles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleUsers] DROP CONSTRAINT [FK_RoleUsers_Roles];
GO
IF OBJECT_ID(N'[dbo].[FK_BookUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Books] DROP CONSTRAINT [FK_BookUser];
GO
IF OBJECT_ID(N'[dbo].[FK_EventUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Events] DROP CONSTRAINT [FK_EventUser];
GO
IF OBJECT_ID(N'[dbo].[FK_ClassroomEvent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Events] DROP CONSTRAINT [FK_ClassroomEvent];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Books]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Books];
GO
IF OBJECT_ID(N'[dbo].[Galleries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Galleries];
GO
IF OBJECT_ID(N'[dbo].[Posts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posts];
GO
IF OBJECT_ID(N'[dbo].[Replies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Replies];
GO
IF OBJECT_ID(N'[dbo].[Classrooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Classrooms];
GO
IF OBJECT_ID(N'[dbo].[RegistrationRequest]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RegistrationRequest];
GO
IF OBJECT_ID(N'[dbo].[ASPStateTempApplications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ASPStateTempApplications];
GO
IF OBJECT_ID(N'[dbo].[ASPStateTempSessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ASPStateTempSessions];
GO
IF OBJECT_ID(N'[dbo].[SystemConfigurations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemConfigurations];
GO
IF OBJECT_ID(N'[dbo].[Events]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Events];
GO
IF OBJECT_ID(N'[dbo].[RoleUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleUsers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] smallint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Surname] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [Username] nvarchar(max)  NOT NULL,
    [SharePhone] bit  NOT NULL,
    [ShareEmail] bit  NOT NULL,
    [DadId] int  NULL,
    [MomId] int  NULL,
    [Birthdate] nvarchar(max)  NULL,
    [Teaches] nvarchar(max)  NULL,
    [Subject] int  NULL,
    [YearOfPromotion] int  NULL,
    [Classroom_Id] int  NULL
);
GO

-- Creating table 'Books'
CREATE TABLE [dbo].[Books] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NULL,
    [Link] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Author] nvarchar(max)  NULL,
    [Highlighted] bit  NULL,
    [Year] nvarchar(max)  NULL,
    [Origin] nvarchar(max)  NULL,
    [Editorial] nvarchar(max)  NULL,
    [User_Id] int  NULL
);
GO

-- Creating table 'Galleries'
CREATE TABLE [dbo].[Galleries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [CreationDate] datetime  NULL,
    [UpdateDate] datetime  NULL,
    [FlickrAlbum] nvarchar(max)  NULL
);
GO

-- Creating table 'Posts'
CREATE TABLE [dbo].[Posts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Category] int  NOT NULL,
    [UserId] int  NOT NULL,
    [FlickrAlbum] nvarchar(max)  NULL,
    [Public] bit  NOT NULL,
    [Subject] int  NULL,
    [YouTubeVideo] nvarchar(max)  NULL,
    [Classroom_Id] int  NULL
);
GO

-- Creating table 'Replies'
CREATE TABLE [dbo].[Replies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [PostId] int  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Classrooms'
CREATE TABLE [dbo].[Classrooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RegistrationRequest'
CREATE TABLE [dbo].[RegistrationRequest] (
    [Id] int  NOT NULL,
    [Level] varchar(50)  NULL,
    [Year] varchar(2)  NULL,
    [Turn] varchar(20)  NULL,
    [Lastnames] varchar(50)  NULL,
    [Firstnames] varchar(50)  NULL,
    [Birthday] datetime  NULL,
    [Birthplace] varchar(50)  NULL,
    [DNI] varchar(12)  NOT NULL,
    [Sex] varchar(1)  NULL,
    [AdressStreet] varchar(50)  NULL,
    [AdressNumber] int  NULL,
    [AdressFloor] varchar(10)  NULL,
    [AdressDpto] varchar(10)  NULL,
    [Localidad] varchar(50)  NULL,
    [PostalCode] int  NULL,
    [FamilyPhone] varchar(50)  NULL,
    [CellPhone] varchar(50)  NULL,
    [OtherPhones] varchar(100)  NULL,
    [EmailFather] varchar(100)  NULL,
    [EmailMother] varchar(100)  NULL,
    [HMB] tinyint  NULL,
    [HMITS] tinyint  NULL,
    [BDB] varchar(50)  NULL,
    [Language] varchar(50)  NULL,
    [OtherSchool] bit  NULL,
    [Distrito] varchar(50)  NULL,
    [OtherSchoolLevel] varchar(50)  NULL,
    [NameSchool] varchar(50)  NULL,
    [NumberSchool] bigint  NULL,
    [Private] bit  NULL,
    [SocialName] varchar(50)  NULL,
    [AffiliateSocialNumber] varchar(50)  NULL,
    [MothersName] varchar(50)  NULL,
    [MothersProfession] varchar(100)  NULL,
    [MothersNationality] varchar(50)  NULL,
    [MothersLevelInstruction] varchar(50)  NULL,
    [MothersLevelInstructionCompleted] bit  NULL,
    [MothersUntilYearLevel] varchar(20)  NULL,
    [MothersLive] bit  NULL,
    [MothersKindOfDocument] varchar(10)  NULL,
    [MothersDocumentNumber] bigint  NULL,
    [MothersAddressStreet] varchar(100)  NULL,
    [MothersAddressNumber] varchar(10)  NULL,
    [MothersAddressFloor] varchar(10)  NULL,
    [MothersAddressTower] varchar(10)  NULL,
    [MothersAddressDpto] varchar(5)  NULL,
    [MothersLocalidad] varchar(50)  NULL,
    [MothersPostalCode] bigint  NULL,
    [MothersPhone] varchar(50)  NULL,
    [MothersWorkPhone] varchar(50)  NULL,
    [FathersName] varchar(50)  NULL,
    [FathersProfession] varchar(100)  NULL,
    [FathersNationality] varchar(50)  NULL,
    [FathersLevelInstruction] varchar(50)  NULL,
    [FathersLevelInstructionCompleted] bit  NULL,
    [FathersUntilYearLevel] varchar(20)  NULL,
    [FathersLive] bit  NULL,
    [FathersKindofDocument] varchar(10)  NULL,
    [FathersDocumentNumber] bigint  NULL,
    [FathersAddressStreet] varchar(100)  NULL,
    [FathersAddressNumber] varchar(10)  NULL,
    [FathersAddressFloor] varchar(10)  NULL,
    [FathersAddressTower] varchar(10)  NULL,
    [FathersAddressDpto] varchar(5)  NULL,
    [FathersLocalidad] varchar(50)  NULL,
    [FathersPostalCode] bigint  NULL,
    [FathersPhone] varchar(50)  NULL,
    [FathersWorkPhone] varchar(50)  NULL,
    [TutorName] varchar(50)  NULL,
    [TutorProfession] varchar(100)  NULL,
    [TutorNationality] varchar(50)  NULL,
    [TutorLevelInstruction] varchar(50)  NULL,
    [TutorLevelInstructionCompleted] bit  NULL,
    [TutorUntilYearLevel] varchar(20)  NULL,
    [TutorLive] bit  NULL,
    [TutorKindOfDocument] varchar(10)  NULL,
    [TutorDocumentNumber] bigint  NULL,
    [TutorAddressStreet] varchar(100)  NULL,
    [TutorAddressNumber] varchar(10)  NULL,
    [TutorAddressFloor] varchar(10)  NULL,
    [TutorAddressTower] varchar(10)  NULL,
    [TutorAddressDpto] varchar(5)  NULL,
    [TutorLocalidad] varchar(50)  NULL,
    [TutorPostalCode] bigint  NULL,
    [TutorPhone] varchar(50)  NULL,
    [TutorWorkPhone] varchar(50)  NULL,
    [OtherPersonsName] varchar(100)  NULL,
    [PersonKindDocument] varchar(10)  NULL,
    [PersonDocumentNumber] bigint  NULL,
    [YearRequest] int  NOT NULL,
    [Status] varchar(50)  NULL,
    [HealthCompleted] bit  NULL,
    [HealthUpdated] bit  NULL,
    [OtherLevel] varchar(50)  NULL
);
GO

-- Creating table 'ASPStateTempApplications'
CREATE TABLE [dbo].[ASPStateTempApplications] (
    [AppId] int  NOT NULL,
    [AppName] char(280)  NOT NULL
);
GO

-- Creating table 'ASPStateTempSessions'
CREATE TABLE [dbo].[ASPStateTempSessions] (
    [SessionId] nvarchar(88)  NOT NULL,
    [Created] datetime  NOT NULL,
    [Expires] datetime  NOT NULL,
    [LockDate] datetime  NOT NULL,
    [LockDateLocal] datetime  NOT NULL,
    [LockCookie] int  NOT NULL,
    [Timeout] int  NOT NULL,
    [Locked] bit  NOT NULL,
    [SessionItemShort] varbinary(7000)  NULL,
    [SessionItemLong] varbinary(max)  NULL,
    [Flags] int  NOT NULL
);
GO

-- Creating table 'SystemConfigurations'
CREATE TABLE [dbo].[SystemConfigurations] (
    [Key] varchar(100)  NOT NULL,
    [Value] varchar(300)  NULL,
    [DefaultValue] varchar(300)  NULL,
    [DisplayName] varchar(100)  NULL,
    [Description] varchar(300)  NULL,
    [Group] varchar(50)  NULL,
    [isInherited] bit  NULL,
    [customData] varchar(300)  NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [start] datetime  NOT NULL,
    [end] datetime  NOT NULL,
    [title] nvarchar(max)  NOT NULL,
    [description] nvarchar(max)  NULL,
    [url] nvarchar(max)  NULL,
    [backgroundColor] nvarchar(max)  NULL,
    [borderColor] nvarchar(max)  NULL,
    [textColor] nvarchar(max)  NULL,
    [extendedProps] nvarchar(max)  NULL,
    [userId] int  NOT NULL,
    [classroomId] int  NULL,
    [students] nvarchar(max)  NULL
);
GO

-- Creating table 'RoleUsers'
CREATE TABLE [dbo].[RoleUsers] (
    [Users_Id] int  NOT NULL,
    [Roles_Id] smallint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Books'
ALTER TABLE [dbo].[Books]
ADD CONSTRAINT [PK_Books]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Galleries'
ALTER TABLE [dbo].[Galleries]
ADD CONSTRAINT [PK_Galleries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [PK_Posts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Replies'
ALTER TABLE [dbo].[Replies]
ADD CONSTRAINT [PK_Replies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Classrooms'
ALTER TABLE [dbo].[Classrooms]
ADD CONSTRAINT [PK_Classrooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RegistrationRequest'
ALTER TABLE [dbo].[RegistrationRequest]
ADD CONSTRAINT [PK_RegistrationRequest]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AppId] in table 'ASPStateTempApplications'
ALTER TABLE [dbo].[ASPStateTempApplications]
ADD CONSTRAINT [PK_ASPStateTempApplications]
    PRIMARY KEY CLUSTERED ([AppId] ASC);
GO

-- Creating primary key on [SessionId] in table 'ASPStateTempSessions'
ALTER TABLE [dbo].[ASPStateTempSessions]
ADD CONSTRAINT [PK_ASPStateTempSessions]
    PRIMARY KEY CLUSTERED ([SessionId] ASC);
GO

-- Creating primary key on [Key] in table 'SystemConfigurations'
ALTER TABLE [dbo].[SystemConfigurations]
ADD CONSTRAINT [PK_SystemConfigurations]
    PRIMARY KEY CLUSTERED ([Key] ASC);
GO

-- Creating primary key on [Id] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [PK_Events]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Users_Id], [Roles_Id] in table 'RoleUsers'
ALTER TABLE [dbo].[RoleUsers]
ADD CONSTRAINT [PK_RoleUsers]
    PRIMARY KEY CLUSTERED ([Users_Id], [Roles_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_UserPost]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPost'
CREATE INDEX [IX_FK_UserPost]
ON [dbo].[Posts]
    ([UserId]);
GO

-- Creating foreign key on [PostId] in table 'Replies'
ALTER TABLE [dbo].[Replies]
ADD CONSTRAINT [FK_PostReply]
    FOREIGN KEY ([PostId])
    REFERENCES [dbo].[Posts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostReply'
CREATE INDEX [IX_FK_PostReply]
ON [dbo].[Replies]
    ([PostId]);
GO

-- Creating foreign key on [UserId] in table 'Replies'
ALTER TABLE [dbo].[Replies]
ADD CONSTRAINT [FK_UserReply]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserReply'
CREATE INDEX [IX_FK_UserReply]
ON [dbo].[Replies]
    ([UserId]);
GO

-- Creating foreign key on [Classroom_Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserClassroom]
    FOREIGN KEY ([Classroom_Id])
    REFERENCES [dbo].[Classrooms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserClassroom'
CREATE INDEX [IX_FK_UserClassroom]
ON [dbo].[Users]
    ([Classroom_Id]);
GO

-- Creating foreign key on [Classroom_Id] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_ClassroomPost]
    FOREIGN KEY ([Classroom_Id])
    REFERENCES [dbo].[Classrooms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClassroomPost'
CREATE INDEX [IX_FK_ClassroomPost]
ON [dbo].[Posts]
    ([Classroom_Id]);
GO

-- Creating foreign key on [Users_Id] in table 'RoleUsers'
ALTER TABLE [dbo].[RoleUsers]
ADD CONSTRAINT [FK_RoleUsers_Users]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Roles_Id] in table 'RoleUsers'
ALTER TABLE [dbo].[RoleUsers]
ADD CONSTRAINT [FK_RoleUsers_Roles]
    FOREIGN KEY ([Roles_Id])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUsers_Roles'
CREATE INDEX [IX_FK_RoleUsers_Roles]
ON [dbo].[RoleUsers]
    ([Roles_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Books'
ALTER TABLE [dbo].[Books]
ADD CONSTRAINT [FK_BookUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookUser'
CREATE INDEX [IX_FK_BookUser]
ON [dbo].[Books]
    ([User_Id]);
GO

-- Creating foreign key on [userId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_EventUser]
    FOREIGN KEY ([userId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventUser'
CREATE INDEX [IX_FK_EventUser]
ON [dbo].[Events]
    ([userId]);
GO

-- Creating foreign key on [classroomId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_ClassroomEvent]
    FOREIGN KEY ([classroomId])
    REFERENCES [dbo].[Classrooms]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClassroomEvent'
CREATE INDEX [IX_FK_ClassroomEvent]
ON [dbo].[Events]
    ([classroomId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------