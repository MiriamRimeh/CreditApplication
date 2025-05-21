-------------------------------------------
---- [21180011].Accounts Log Table Trigger
-------------------------------------------
CREATE TRIGGER trg_21180011_Accounts_Log
ON [21180011].Accounts
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


-------------------------------------------
---- [21180011].ClientAddress Log Table Trigger
-------------------------------------------
CREATE TRIGGER trg_21180011_ClientAddress_Log
ON [21180011].ClientAddress
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


-------------------------------------------
---- [21180011].ClientFinancials Log Table Trigger
-------------------------------------------
CREATE TRIGGER trg_21180011_ClientFinancials_Log
ON [21180011].ClientFinancials
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



-------------------------------------------
---- [21180011].Clients Log Table Trigger
-------------------------------------------
CREATE TRIGGER trg_21180011_Clients_Log
ON [21180011].Clients
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


-------------------------------------------
---- [21180011].FinancialOperations Log Table Trigger
-------------------------------------------
CREATE TRIGGER trg_21180011_FinancialOperations_Log
ON [21180011].FinancialOperations
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


-------------------------------------------
---- [21180011].Nomenclature Log Table Trigger
-------------------------------------------
CREATE TRIGGER trg_21180011_Nomenclature_Log
ON [21180011].Nomenclature
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


-------------------------------------------
---- [21180011].RepaymentPlan Log Table Trigger
-------------------------------------------
CREATE TRIGGER trg_21180011_RepaymentPlan_Log
ON [21180011].RepaymentPlan
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



-------------------------------------------
---- [21180011].Roles Log Table Trigger
-------------------------------------------
CREATE TRIGGER trg_21180011_Roles_Log
ON [21180011].Roles
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