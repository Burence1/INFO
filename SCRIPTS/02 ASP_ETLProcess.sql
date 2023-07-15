IF EXISTS (SELECT 0 FROM sys.objects WHERE object_id=OBJECT_ID(N'[dbo].[ASP_ETLProcess]')AND type IN (N'P', N'PC'))
BEGIN
  DROP PROCEDURE dbo.ASP_ETLProcess;
END;
GO

CREATE PROCEDURE dbo.ASP_ETLProcess
  @mode AS INT, @User VARCHAR(100), @compname VARCHAR(100), @ClientIP VARCHAR(100), @processDesc VARCHAR(500), @etlStatus INT=0,
  @Result VARCHAR(500) OUTPUT
AS

/**VARIABLES**/
Declare @DatabaseName varchar(1000) = ''
Declare @InstitutionName nvarchar(250)
DECLARE @SQL NVARCHAR(Max);
DECLARE @Parameters NVARCHAR(1000);

BEGIN
  BEGIN TRY

  Begin transaction
  	SELECT @DatabaseName = ISNULL(ltrim(rtrim(Client)),''),@InstitutionName = ClientName FROM [Master].[dbo].[DETAILS] with(nolock) WHERE InstitutionCode = '001';

    DECLARE @SystemDate DATE=(select convert(varchar, getdate(), 20));
    --DECLARE @InstitutionName VARCHAR(50) = DB_NAME();


    IF @mode=1
    BEGIN

	 SET @Sql = N'INSERT INTO [dbo].[customer_details]
           ([acno]
           ,[firstName]
           ,[middleName]
           ,[lastName]
           ,[FullName]
           ,[address]
           ,[city]
           ,[personalno]
           ,[Saccomno]
           ,[joiningDate]
           ,[mobileNo]
           ,[gender]
           ,[Employer]
           ,[officer]
           ,[Occupation]
           ,[YEARBIRTH]
           ,[Email]
           ,[MaritalStatus]
           ,[BankAccount]
           ,[BranchId]
           ,[BCode]
           ,[BaseNo])
     select top (20) acno,fname,mname,lname,fullName,address,city,personalno,saccomno,entrydate,telephone,sex,employer,officer,occupation,
	 yearbirth,email,maritalStatus,bankaccount,branchid,bcode,baseno from '+@DatabaseName+'.dbo.customer'
	 
	 EXEC sp_executesql @Sql;

	 select * from dbo.[customer_details]

    END;

	Commit;
  END TRY
  BEGIN CATCH
    SET @Result='0 - Error(s) Occurred: '+ERROR_MESSAGE();

	select @Result
RollBack

    INSERT INTO dbo.ErrorLog_N(ProgModule, Source, Terminal, SaccoUser, ErrorDate, brcode, Error, ErrorNumber,
                               ErrorSeverity, ErrorState, ErrorLine, ErrorProcedure)
    VALUES(
            'Date: '+CONVERT(VARCHAR(MAX), @SystemDate),
            ('Line'+CONVERT(VARCHAR(MAX), ERROR_LINE())+', Source: '+ERROR_PROCEDURE()), @compname, @User, GETDATE(),
            '', ERROR_MESSAGE(), ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_LINE(), ERROR_PROCEDURE()
          );
  END CATCH;
END;