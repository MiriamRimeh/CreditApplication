USE [CreditApplication];
GO

-----------------------------------------------
-- 1) Процедура за създаване на план за един кредит
-----------------------------------------------
IF OBJECT_ID('[21180011].[sp_CreateRepaymentPlanForCredit]', 'P') IS NOT NULL
    DROP PROCEDURE [21180011].[sp_CreateRepaymentPlanForCredit];
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

    -- Вземане на параметрите на кредита
    SELECT 
        @CreditAmount    = CreditAmount,
        @InterestRate    = InterestRate,
        @CreditBeginDate = CreditBeginDate,
        @CreditPeriod    = CreditPeriod
    FROM [21180011].[Credits]
    WHERE ID = @CreditID;

    -- Изтриване на стария план (ако има такъв)
    DELETE FROM [21180011].[RepaymentPlan]
    WHERE CreditID = @CreditID;

    -- Изчисляване на анюитетна вноска
    SET @MonthlyRate = @InterestRate / 12.0;
    IF @MonthlyRate > 0
        SET @MonthlyPayment = @CreditAmount * @MonthlyRate / (1 - POWER(1 + @MonthlyRate, -@CreditPeriod));
    ELSE
        SET @MonthlyPayment = @CreditAmount / @CreditPeriod;

    SET @RemainingBalance = @CreditAmount;
    SET @Counter = 1;

    -- Генериране на редове за всяка вноска
    WHILE @Counter <= @CreditPeriod
    BEGIN
        SET @InstallmentDate = DATEADD(MONTH, @Counter, @CreditBeginDate);
        SET @Interest        = @RemainingBalance * @MonthlyRate;
        SET @Principal       = @MonthlyPayment - @Interest;

        IF @Counter = @CreditPeriod
        BEGIN
            -- Корекция на закръглянето при последна вноска
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
-----------------------------------------------
-- 2) Тригер: при нова активация (Статус → 102)
-----------------------------------------------
IF OBJECT_ID('[21180011].[trg_Credit_Activate]', 'TR') IS NOT NULL
    DROP TRIGGER [21180011].[trg_Credit_Activate];
GO

CREATE TRIGGER [21180011].[trg_Credit_Activate]
ON [21180011].[Credits]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID INT;

    -- Курсор само за току-що активираните кредити
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
        -- Генерираме плана само за този @ID
        EXEC [21180011].[sp_CreateRepaymentPlanForCredit] @CreditID = @ID;

        FETCH NEXT FROM cur_New_Active_Credits INTO @ID;
    END
    CLOSE cur_New_Active_Credits;
    DEALLOCATE cur_New_Active_Credits;
END;
GO

-----------------------------------------------
-- 3) (По избор) Процедура с курсор за всички съществуващи кредити
-----------------------------------------------
IF OBJECT_ID('[21180011].[sp_BulkRecreateAllRepaymentPlans]', 'P') IS NOT NULL
    DROP PROCEDURE [21180011].[sp_BulkRecreateAllRepaymentPlans];
GO

CREATE PROCEDURE [21180011].[sp_BulkRecreateAllRepaymentPlans]
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ID INT;

    DECLARE cur_all CURSOR LOCAL FAST_FORWARD FOR
        SELECT ID
        FROM [21180011].[Credits]
        WHERE Status = 102;

    OPEN cur_all;
    FETCH NEXT FROM cur_all INTO @ID;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC [21180011].[sp_CreateRepaymentPlanForCredit] @CreditID = @ID;
        FETCH NEXT FROM cur_all INTO @ID;
    END
    CLOSE cur_all;
    DEALLOCATE cur_all;
END;
GO
