USE [master]
GO
/****** Object:  Database [CreditApplication]    Script Date: 30/05/2025 01:09:40 ******/
CREATE DATABASE [CreditApplication]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CreditApplication', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\CreditApplication.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CreditApplication_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\CreditApplication_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CreditApplication] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CreditApplication].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CreditApplication] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CreditApplication] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CreditApplication] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CreditApplication] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CreditApplication] SET ARITHABORT OFF 
GO
ALTER DATABASE [CreditApplication] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CreditApplication] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CreditApplication] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CreditApplication] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CreditApplication] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CreditApplication] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CreditApplication] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CreditApplication] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CreditApplication] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CreditApplication] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CreditApplication] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CreditApplication] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CreditApplication] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CreditApplication] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CreditApplication] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CreditApplication] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CreditApplication] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CreditApplication] SET RECOVERY FULL 
GO
ALTER DATABASE [CreditApplication] SET  MULTI_USER 
GO
ALTER DATABASE [CreditApplication] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CreditApplication] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CreditApplication] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CreditApplication] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CreditApplication] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CreditApplication] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CreditApplication', N'ON'
GO
ALTER DATABASE [CreditApplication] SET QUERY_STORE = ON
GO
ALTER DATABASE [CreditApplication] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CreditApplication]
GO
/****** Object:  Schema [21180011]    Script Date: 30/05/2025 01:09:40 ******/
CREATE SCHEMA [21180011]
GO
/****** Object:  Table [21180011].[Accounts]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[Accounts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[ClientID] [int] NULL,
	[PasswordHash] [varbinary](64) NOT NULL,
	[PasswordSalt] [varbinary](128) NOT NULL,
	[Role] [tinyint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedOn_21180011] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [21180011].[ClientAddress]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[ClientAddress](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NULL,
	[City] [nvarchar](50) NOT NULL,
	[StreetNeighbourhood] [nvarchar](50) NOT NULL,
	[Number] [nvarchar](5) NOT NULL,
	[PostCode] [nvarchar](4) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn_21180011] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [21180011].[ClientFinancials]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[ClientFinancials](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[MontlyIncome] [decimal](18, 0) NOT NULL,
	[MontlyExpenses] [decimal](18, 0) NOT NULL,
	[EmploymentType] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn_21180011] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [21180011].[Clients]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[Clients](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EGN] [nvarchar](10) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](10) NOT NULL,
	[IDCardNumber] [nvarchar](20) NOT NULL,
	[IDValidityDate] [date] NOT NULL,
	[IDIssueDate] [date] NOT NULL,
	[IDIssuer] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn_21180011] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [21180011].[Credits]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[Credits](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[CreditAmount] [decimal](18, 0) NOT NULL,
	[CreditBeginDate] [date] NULL,
	[CreditEndDate] [date] NULL,
	[InterestRate] [decimal](10, 2) NULL,
	[CreditPeriod] [int] NULL,
	[TotalCreditAmount] [decimal](18, 0) NULL,
	[MonthlyInstallment] [decimal](18, 0) NULL,
	[Status] [int] NOT NULL,
	[ActivationDate] [date] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn_21180011] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [21180011].[FinancialOperations]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[FinancialOperations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreditID] [int] NOT NULL,
	[PayedOnDate] [date] NULL,
	[PayedAmount] [decimal](18, 0) NULL,
	[OperationType] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn_21180011] [datetime] NOT NULL,
	[RepaymentPlanID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [21180011].[log_21180011]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[log_21180011](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](50) NULL,
	[ActionType] [nvarchar](20) NULL,
	[ActionDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [21180011].[Nomenclature]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[Nomenclature](
	[NomCode] [int] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn_21180011] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[NomCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [CreditApplication].[21180011].[Nomenclature] (
	[NomCode], 
	[Description], 
	[CreatedOn],
	[ModifiedOn_21180011])
VALUES 
(101,N'Очакващ решение', SYSDATETIME(), SYSDATETIME()),
(102,N'Активен', SYSDATETIME(), SYSDATETIME()),
(103,N'Приключен', SYSDATETIME(), SYSDATETIME()),
(104,N'Отхвърлен', SYSDATETIME(), SYSDATETIME()),
(201,N'Усвояване на кредит', SYSDATETIME(), SYSDATETIME()),
(202,N'Вноска по кредит', SYSDATETIME(), SYSDATETIME()),
(203,N'Сторнираща операция', SYSDATETIME(), SYSDATETIME()),
(301,N'На пълен работен ден', SYSDATETIME(), SYSDATETIME()),
(302,N'На половин работен ден', SYSDATETIME(), SYSDATETIME()),
(303,N'Самонаето лице', SYSDATETIME(), SYSDATETIME()),
(304,N'Безработен', SYSDATETIME(), SYSDATETIME()),
(305,N'Пенсионер', SYSDATETIME(), SYSDATETIME()),
(306,N'Учащ', SYSDATETIME(), SYSDATETIME());
GO



/****** Object:  Table [21180011].[RepaymentPlan]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[RepaymentPlan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreditID] [int] NOT NULL,
	[InstallmentNumber] [int] NULL,
	[InstallmentDate] [date] NULL,
	[InstallmentAmount] [decimal](18, 2) NULL,
	[Principal] [decimal](18, 2) NULL,
	[Interest] [decimal](18, 2) NULL,
	[isPaid] [bit] NULL,
	[PayedOnDate] [date] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn_21180011] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [21180011].[Roles]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [21180011].[Roles](
	[ID] [tinyint] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [CreditApplication].[21180011].Roles (
	 [ID], 
	 [RoleName])
VALUES
(3,	'Admin'),
(1,	'Client'),
(2,	'Employee');
GO

ALTER TABLE [21180011].[Accounts] ADD  CONSTRAINT [DF_Accounts_Role]  DEFAULT ((1)) FOR [Role]
GO
ALTER TABLE [21180011].[Accounts] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [21180011].[Accounts] ADD  DEFAULT (sysutcdatetime()) FOR [ModifiedOn_21180011]
GO
ALTER TABLE [21180011].[Accounts] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [21180011].[ClientAddress] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [21180011].[ClientAddress] ADD  DEFAULT (getdate()) FOR [ModifiedOn_21180011]
GO
ALTER TABLE [21180011].[ClientFinancials] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [21180011].[ClientFinancials] ADD  DEFAULT (getdate()) FOR [ModifiedOn_21180011]
GO
ALTER TABLE [21180011].[Clients] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [21180011].[Clients] ADD  DEFAULT (getdate()) FOR [ModifiedOn_21180011]
GO
ALTER TABLE [21180011].[Credits] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [21180011].[Credits] ADD  DEFAULT (getdate()) FOR [ModifiedOn_21180011]
GO
ALTER TABLE [21180011].[FinancialOperations] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [21180011].[FinancialOperations] ADD  DEFAULT (getdate()) FOR [ModifiedOn_21180011]
GO
ALTER TABLE [21180011].[Nomenclature] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [21180011].[Nomenclature] ADD  DEFAULT (getdate()) FOR [ModifiedOn_21180011]
GO
ALTER TABLE [21180011].[RepaymentPlan] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [21180011].[RepaymentPlan] ADD  DEFAULT (getdate()) FOR [ModifiedOn_21180011]
GO
ALTER TABLE [21180011].[Accounts]  WITH NOCHECK ADD  CONSTRAINT [FK_Accounts_Clients] FOREIGN KEY([ClientID])
REFERENCES [21180011].[Clients] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [21180011].[Accounts] NOCHECK CONSTRAINT [FK_Accounts_Clients]
GO
ALTER TABLE [21180011].[ClientAddress]  WITH NOCHECK ADD  CONSTRAINT [FK_ClientAddress_Clients] FOREIGN KEY([ClientID])
REFERENCES [21180011].[Clients] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [21180011].[ClientAddress] NOCHECK CONSTRAINT [FK_ClientAddress_Clients]
GO
ALTER TABLE [21180011].[ClientFinancials]  WITH NOCHECK ADD FOREIGN KEY([EmploymentType])
REFERENCES [21180011].[Nomenclature] ([NomCode])
GO
ALTER TABLE [21180011].[ClientFinancials]  WITH CHECK ADD  CONSTRAINT [FK_ClientFinancials_Clients] FOREIGN KEY([ClientID])
REFERENCES [21180011].[Clients] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [21180011].[ClientFinancials] CHECK CONSTRAINT [FK_ClientFinancials_Clients]
GO
ALTER TABLE [21180011].[FinancialOperations]  WITH NOCHECK ADD FOREIGN KEY([OperationType])
REFERENCES [21180011].[Nomenclature] ([NomCode])
GO
ALTER TABLE [21180011].[FinancialOperations]  WITH NOCHECK ADD  CONSTRAINT [FK_FinancialOperations_Credits] FOREIGN KEY([CreditID])
REFERENCES [21180011].[Credits] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [21180011].[FinancialOperations] NOCHECK CONSTRAINT [FK_FinancialOperations_Credits]
GO
ALTER TABLE [21180011].[RepaymentPlan]  WITH NOCHECK ADD  CONSTRAINT [FK_RepaymentPlan_Credits] FOREIGN KEY([CreditID])
REFERENCES [21180011].[Credits] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [21180011].[RepaymentPlan] NOCHECK CONSTRAINT [FK_RepaymentPlan_Credits]
GO
ALTER TABLE [21180011].[Accounts]  WITH NOCHECK ADD  CONSTRAINT [CHK_Accounts_Role] CHECK  (([Role]=(3) OR [Role]=(2) OR [Role]=(1)))
GO
ALTER TABLE [21180011].[Accounts] NOCHECK CONSTRAINT [CHK_Accounts_Role]
GO
/****** Object:  StoredProcedure [21180011].[sp_CreateRepaymentPlanForCredit]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [21180011].[sp_CreateRepaymentPlanForCredit]
    @CreditID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @CreditAmount       DECIMAL(18,2),
        @InterestRate       DECIMAL(10,2),
        @CreditBeginDate    DATE,
        @CreditPeriod       INT,
        @MonthlyRate        DECIMAL(18,10),
        @MonthlyPayment     DECIMAL(18,2),
        @RemainingBalance   DECIMAL(18,2),
        @InstallmentDate    DATE,
        @Interest           DECIMAL(18,2),
        @Principal          DECIMAL(18,2),
        @Counter            INT;

    SELECT 
        @CreditAmount    = CreditAmount,
        @InterestRate    = InterestRate,
        @CreditBeginDate = CreditBeginDate,
        @CreditPeriod    = CreditPeriod
    FROM [21180011].[Credits]
    WHERE ID = @CreditID;

    DELETE FROM [21180011].[RepaymentPlan]
    WHERE CreditID = @CreditID;

    SET @MonthlyRate = @InterestRate / 12.0;
    IF @MonthlyRate > 0
        SET @MonthlyPayment = @CreditAmount * @MonthlyRate / (1 - POWER(1 + @MonthlyRate, -@CreditPeriod));
    ELSE
        SET @MonthlyPayment = @CreditAmount / @CreditPeriod;

    SET @RemainingBalance = @CreditAmount;
    SET @Counter = 1;

    WHILE @Counter <= @CreditPeriod
    BEGIN
        SET @InstallmentDate = DATEADD(MONTH, @Counter, @CreditBeginDate);
        SET @Interest        = @RemainingBalance * @MonthlyRate;
        SET @Principal       = @MonthlyPayment - @Interest;

        IF @Counter = @CreditPeriod
        BEGIN
            SET @Principal       = @RemainingBalance;
            SET @MonthlyPayment  = @Principal + @Interest;
        END

        INSERT INTO [21180011].[RepaymentPlan]
            (CreditID, InstallmentNumber, InstallmentDate, InstallmentAmount, Principal, Interest, isPaid)
        VALUES
            (
              @CreditID,
              @Counter,
              @InstallmentDate,
              ROUND(@MonthlyPayment, 2),
              ROUND(@Principal,       2),
              ROUND(@Interest,        2),
              0
            );

        SET @RemainingBalance = @RemainingBalance - @Principal;
        SET @Counter = @Counter + 1;
    END
END;
GO
/****** Object:  Trigger [21180011].[trg_21180011_Accounts_Log]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [21180011].[trg_21180011_Accounts_Log]
ON [21180011].[Accounts]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionType NVARCHAR(20) = 
        CASE 
            WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
            WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
            WHEN EXISTS(SELECT * FROM deleted) THEN 'DELETE'
        END;

    INSERT INTO [21180011].[log_21180011] (TableName, ActionType, ActionDate)
    VALUES (
        'Accounts', 
        @ActionType, 
        SYSDATETIME()
    );
END
GO
ALTER TABLE [21180011].[Accounts] ENABLE TRIGGER [trg_21180011_Accounts_Log]
GO
/****** Object:  Trigger [21180011].[trg_21180011_ClientAddress_Log]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [21180011].[trg_21180011_ClientAddress_Log]
ON [21180011].[ClientAddress]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionType NVARCHAR(20) = 
        CASE 
            WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
            WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
            WHEN EXISTS(SELECT * FROM deleted) THEN 'DELETE'
        END;

    INSERT INTO [21180011].[log_21180011] (TableName, ActionType, ActionDate)
    VALUES (
        'ClientAddress', 
        @ActionType, 
        SYSDATETIME()
    );
END
GO
ALTER TABLE [21180011].[ClientAddress] ENABLE TRIGGER [trg_21180011_ClientAddress_Log]
GO
/****** Object:  Trigger [21180011].[trg_21180011_ClientFinancials_Log]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [21180011].[trg_21180011_ClientFinancials_Log]
ON [21180011].[ClientFinancials]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionType NVARCHAR(20) = 
        CASE 
            WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
            WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
            WHEN EXISTS(SELECT * FROM deleted) THEN 'DELETE'
        END;

    INSERT INTO [21180011].[log_21180011] (TableName, ActionType, ActionDate)
    VALUES (
        'ClientFinancials', 
        @ActionType, 
        SYSDATETIME()
    );
END
GO
ALTER TABLE [21180011].[ClientFinancials] ENABLE TRIGGER [trg_21180011_ClientFinancials_Log]
GO
/****** Object:  Trigger [21180011].[trg_21180011_Clients_Log]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [21180011].[trg_21180011_Clients_Log]
ON [21180011].[Clients]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionType NVARCHAR(20) = 
        CASE 
            WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
            WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
            WHEN EXISTS(SELECT * FROM deleted) THEN 'DELETE'
        END;

    INSERT INTO [21180011].[log_21180011] (TableName, ActionType, ActionDate)
    VALUES (
        'Clients', 
        @ActionType, 
        SYSDATETIME()
    );
END
GO
ALTER TABLE [21180011].[Clients] ENABLE TRIGGER [trg_21180011_Clients_Log]
GO
/****** Object:  Trigger [21180011].[trg_21180011_Credits_Log]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [21180011].[trg_21180011_Credits_Log]
ON [21180011].[Credits]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionType NVARCHAR(20) = 
        CASE 
            WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
            WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
            WHEN EXISTS(SELECT * FROM deleted) THEN 'DELETE'
        END;

    INSERT INTO [21180011].[log_21180011] (TableName, ActionType, ActionDate)
    VALUES (
        'Credits', 
        @ActionType, 
        SYSDATETIME()
    );
END
GO
ALTER TABLE [21180011].[Credits] ENABLE TRIGGER [trg_21180011_Credits_Log]
GO
/****** Object:  Trigger [21180011].[trg_Credit_Activate]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [21180011].[trg_Credit_Activate]
ON [21180011].[Credits]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID INT;

    DECLARE cur_New_Active_Credits CURSOR LOCAL FAST_FORWARD FOR
        SELECT i.ID
        FROM inserted i
        JOIN deleted  d ON i.ID = d.ID
        WHERE i.Status = 102
          AND d.Status <> 102;

    OPEN cur_New_Active_Credits;
    FETCH NEXT FROM cur_New_Active_Credits INTO @ID;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC [21180011].[sp_CreateRepaymentPlanForCredit] @CreditID = @ID;

        FETCH NEXT FROM cur_New_Active_Credits INTO @ID;
    END
    CLOSE cur_New_Active_Credits;
    DEALLOCATE cur_New_Active_Credits;
END;
GO
ALTER TABLE [21180011].[Credits] ENABLE TRIGGER [trg_Credit_Activate]
GO
/****** Object:  Trigger [21180011].[trg_21180011_FinancialOperations_Log]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [21180011].[trg_21180011_FinancialOperations_Log]
ON [21180011].[FinancialOperations]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionType NVARCHAR(20) = 
        CASE 
            WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
            WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
            WHEN EXISTS(SELECT * FROM deleted) THEN 'DELETE'
        END;

    INSERT INTO [21180011].[log_21180011] (TableName, ActionType, ActionDate)
    VALUES (
        'FinancialOperations', 
        @ActionType, 
        SYSDATETIME()
    );
END
GO
ALTER TABLE [21180011].[FinancialOperations] ENABLE TRIGGER [trg_21180011_FinancialOperations_Log]
GO
/****** Object:  Trigger [21180011].[trg_21180011_Nomenclature_Log]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [21180011].[trg_21180011_Nomenclature_Log]
ON [21180011].[Nomenclature]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionType NVARCHAR(20) = 
        CASE 
            WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
            WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
            WHEN EXISTS(SELECT * FROM deleted) THEN 'DELETE'
        END;

    INSERT INTO [21180011].[log_21180011] (TableName, ActionType, ActionDate)
    VALUES (
        'Nomenclature', 
        @ActionType, 
        SYSDATETIME()
    );
END
GO
ALTER TABLE [21180011].[Nomenclature] ENABLE TRIGGER [trg_21180011_Nomenclature_Log]
GO
/****** Object:  Trigger [21180011].[trg_21180011_RepaymentPlan_Log]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [21180011].[trg_21180011_RepaymentPlan_Log]
ON [21180011].[RepaymentPlan]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionType NVARCHAR(20) = 
        CASE 
            WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
            WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
            WHEN EXISTS(SELECT * FROM deleted) THEN 'DELETE'
        END;

    INSERT INTO [21180011].[log_21180011] (TableName, ActionType, ActionDate)
    VALUES (
        'RepaymentPlan', 
        @ActionType, 
        SYSDATETIME()
    );
END
GO
ALTER TABLE [21180011].[RepaymentPlan] ENABLE TRIGGER [trg_21180011_RepaymentPlan_Log]
GO
/****** Object:  Trigger [21180011].[trg_RepaymentPlan_OnPayedOnDateUpdate]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   TRIGGER [21180011].[trg_RepaymentPlan_OnPayedOnDateUpdate]
ON [21180011].[RepaymentPlan]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT UPDATE(PayedOnDate)
        RETURN;

    ;WITH ChangedCredits AS (
        SELECT DISTINCT CreditID FROM inserted
        UNION
        SELECT DISTINCT CreditID FROM deleted
    )

    UPDATE C
    SET C.Status = CASE

        WHEN NOT EXISTS (
            SELECT 1
            FROM [21180011].[RepaymentPlan] RP
            WHERE RP.CreditID    = CC.CreditID
              AND RP.PayedOnDate IS NULL
        ) THEN 103
        ELSE 102
    END
    FROM [21180011].[Credits] C
    INNER JOIN ChangedCredits CC
        ON C.ID = CC.CreditID

    WHERE C.Status <> 
        CASE
            WHEN NOT EXISTS (
                SELECT 1
                FROM [21180011].[RepaymentPlan] RP
                WHERE RP.CreditID    = CC.CreditID
                  AND RP.PayedOnDate IS NULL
            ) THEN 103 ELSE 102
        END;
END;
GO
ALTER TABLE [21180011].[RepaymentPlan] ENABLE TRIGGER [trg_RepaymentPlan_OnPayedOnDateUpdate]
GO
/****** Object:  Trigger [21180011].[trg_21180011_Roles_Log]    Script Date: 30/05/2025 01:09:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [21180011].[trg_21180011_Roles_Log]
ON [21180011].[Roles]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionType NVARCHAR(20) = 
        CASE 
            WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
            WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
            WHEN EXISTS(SELECT * FROM deleted) THEN 'DELETE'
        END;

    INSERT INTO [21180011].[log_21180011] (TableName, ActionType, ActionDate)
    VALUES (
        'Roles', 
        @ActionType, 
        SYSDATETIME()
    );
END
GO
ALTER TABLE [21180011].[Roles] ENABLE TRIGGER [trg_21180011_Roles_Log]
GO
USE [master]
GO
ALTER DATABASE [CreditApplication] SET  READ_WRITE 
GO
