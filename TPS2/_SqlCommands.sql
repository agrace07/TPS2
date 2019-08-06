CREATE TABLE [dbo].[CD_State] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [StateCd]   VARCHAR (2)  NOT NULL,
    [StateName] VARCHAR (50) NOT NULL
);

CREATE TABLE [dbo].[ClientRequest] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [IdSkills]          INT            NULL,
    [IdEducation]       INT            NULL,
    [EducationRequired] INT            NULL,
    [StartingSalary]    INT            NULL,
    [IdLocation]        INT            NULL,
    [RequestorID]       NVARCHAR (128) NULL,
    [Complete]          INT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Employee] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [AspNetUserId]     NVARCHAR (128) NOT NULL,
    [FirstName]        NVARCHAR (50)  NOT NULL,
    [LastName]         NVARCHAR (50)  NOT NULL,
    [AddressID]        INT            NOT NULL,
    [Relocate]         INT            NOT NULL,
    [ExperienceID]     INT            NULL,
    [ResumeLocation]   VARCHAR (MAX)  NULL,
    [PictureLocation]  VARCHAR (MAX)  NULL,
    [AvailabilityDate] DATETIME       NULL,
    [PhoneNumber]      NCHAR (10)     NULL,
    [Expired]          DATETIME       NULL
);

CREATE TABLE [dbo].[EmployeeAddress] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [AddressLine1] VARCHAR (50) NOT NULL,
    [AddressLine2] VARCHAR (50) NULL,
    [City]         VARCHAR (50) NOT NULL,
    [Zip]          NCHAR (5)    NOT NULL,
    [StateCD]      VARCHAR (2)  NOT NULL
);

CREATE TABLE [dbo].[RequestAddress] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [AddressLine1] VARCHAR (50) NOT NULL,
    [AddressLine2] VARCHAR (50) NULL,
    [City]         VARCHAR (50) NOT NULL,
    [Zip]          NCHAR (5)    NOT NULL,
    [StateCD]      VARCHAR (2)  NOT NULL
);

CREATE TABLE [dbo].[RequestMatch] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [RequestId]    INT            NOT NULL,
    [AspNetUserId] NVARCHAR (128) NOT NULL,
    [WasSelected]  INT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[RequestSkills] (
    [RequestId] INT NOT NULL,
    [SkillId]   INT NOT NULL,
    [Required]  INT NULL,
    PRIMARY KEY CLUSTERED ([RequestId] ASC, [SkillId] ASC)
);

CREATE TABLE [dbo].[Skills] (
    [Id]   INT          NOT NULL,
    [Name] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE PROCEDURE [dbo].[InsertClientRequest]	
	@EducationLevel int,
	@EducationRequired int,
	@StartingSalary nchar(7),
	@AddressLine1 varchar(50),
	@AddressLine2 varchar(50),
	@City varchar(50),
	@Zip nchar(5),
	@State varchar(2),
	@Telecommute int,
	@RequestorID nvarchar(128),
	@ReturnVal  int OUTPUT

AS
BEGIN
	INSERT INTO RequestAddress (AddressLine1, AddressLine2, City, Zip, StateCD)
	VALUES 
	(@AddressLine1, 
	@AddressLine2, 
	@City, 
	@Zip,
	(SELECT StateCd from CD_State where StateCd = @State));
	
	INSERT INTO ClientRequest (IdEducation, EducationRequired, StartingSalary, IdLocation, RequestorID)
	values (@EducationLevel, @EducationRequired, @StartingSalary, SCOPE_IDENTITY(), @RequestorID)
	
	SELECT @ReturnVal = @@IDENTITY
END

CREATE PROCEDURE [dbo].[InsertClientRequestSkills]	
	@RequestId INT,
	@SkillId INT,
	@Required INT

AS
BEGIN
	INSERT INTO RequestSkills (RequestId, SkillId, [Required])
	VALUES (@RequestId, @SkillId, @Required)
END

CREATE PROCEDURE [dbo].[MatchRequest]
	@AspNetUserId nvarchar(128),
	@RequestId INT
AS
	INSERT INTO RequestMatch (RequestId, AspNetUserId)
	VALUES (@RequestId, @AspNetUserId)

CREATE PROCEDURE [dbo].[UpdateEmployeeInfo]
	@FirstName nvarchar(10),
	@LastName nvarchar(10), 
	@AspNetUserId nvarchar(50), 
	@Relocate int,
	@AvailabilityDate dateTime,
	@PhoneNumber nchar(10),
	@AddressLine1 varchar(50),
	@AddressLine2 varchar(50),
	@City varchar(50),
	@Zip nchar(5),
	@State varchar(2)
AS
BEGIN
DECLARE @RECORDS int;
SELECT @RECORDS = COUNT(*) FROM Employee WHERE AspNetUserId = @AspNetUserId
IF @RECORDS > 0
	UPDATE Employee
	SET Expired = GETDATE()
	WHERE AspNetUserId = @AspNetUserId
	AND Expired IS NULL;

	INSERT INTO EmployeeAddress (AddressLine1, AddressLine2, City, Zip, StateCD)
	VALUES 
	(@AddressLine1, 
	@AddressLine2, 
	@City, 
	@Zip,
	(SELECT StateCd from CD_State where StateCd = @State));
	
	INSERT INTO Employee (FirstName, LastName, AspNetUserId, AddressID, Relocate, AvailabilityDate, PhoneNumber)
	values (@FirstName, @LastName, @AspNetUserId, SCOPE_IDENTITY(), @Relocate, @AvailabilityDate, @PhoneNumber)
END