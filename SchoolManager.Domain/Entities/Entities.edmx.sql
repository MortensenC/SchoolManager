
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/12/2017 20:39:29
-- Generated from EDMX file: D:\Visual Studio Projects\SchoolManager\SchoolManager.Domain\Entities\Entities.edmx
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
IF OBJECT_ID(N'[dbo].[FK_RoleUsers_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleUsers] DROP CONSTRAINT [FK_RoleUsers_User];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleUsers_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleUsers] DROP CONSTRAINT [FK_RoleUsers_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_BookUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Books] DROP CONSTRAINT [FK_BookUser];
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
    [Title] nvarchar(max)  NOT NULL,
    [Link] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Author] nvarchar(max)  NULL,
    [Highlighted] bit  NOT NULL,
    [User_Id] int  NULL
);
GO

-- Creating table 'Galleries'
CREATE TABLE [dbo].[Galleries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
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
    [PostsCategoryId] int  NOT NULL,
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

-- Creating table 'PostsCategories'
CREATE TABLE [dbo].[PostsCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
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

-- Creating primary key on [Id] in table 'PostsCategories'
ALTER TABLE [dbo].[PostsCategories]
ADD CONSTRAINT [PK_PostsCategories]
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
ADD CONSTRAINT [FK_RoleUsers_User]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Roles_Id] in table 'RoleUsers'
ALTER TABLE [dbo].[RoleUsers]
ADD CONSTRAINT [FK_RoleUsers_Role]
    FOREIGN KEY ([Roles_Id])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUsers_Role'
CREATE INDEX [IX_FK_RoleUsers_Role]
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

-- Creating foreign key on [PostsCategoryId] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_PostPostsCategory]
    FOREIGN KEY ([PostsCategoryId])
    REFERENCES [dbo].[PostsCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostPostsCategory'
CREATE INDEX [IX_FK_PostPostsCategory]
ON [dbo].[Posts]
    ([PostsCategoryId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------