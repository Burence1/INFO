IF EXISTS (SELECT 0 FROM sys.objects WHERE object_id=OBJECT_ID(N'[dbo].[ASP_ETLProcess]')AND type IN (N'P', N'PC'))
BEGIN
  DROP PROCEDURE dbo.ASP_ETLProcess;
END;
GO

CREATE PROCEDURE dbo.ASP_ETLProcess
  @mode AS           INT, @User VARCHAR(100), @compname VARCHAR(100), @ClientIP VARCHAR(100), @Language VARCHAR(10),
  @BackupDescription VARCHAR(500), @AlertMsgNo VARCHAR(500) ='', @eodStatus INT=0, @nitro VARCHAR(500) ='',
  @MandatoryItems    INT OUTPUT, @Result VARCHAR(500) OUTPUT
AS
BEGIN
  BEGIN TRY
    --DECLARE @SystemDate DATE=(dbo.GetSystemDate(1, dbo.GetSetupItem('HQBRANCHID'), GETDATE()));
    DECLARE @InstitutionName VARCHAR(50) =DB_NAME();

    --SET @SystemDate=ISNULL(@SystemDate, GETDATE());

    --DECLARE @Prevsysdate VARCHAR(50) =(SELECT TOP(1)sysdate FROM dbo.SystemData(NOLOCK)ORDER BY code);
    --DECLARE @Nextsysdate DATE =dbo.GetNextprocDate(@Prevsysdate);
    DECLARE @SQL NVARCHAR(1000);
    DECLARE @Parameters NVARCHAR(1000);

    --DECLARE @BranchName VARCHAR(50) =(SELECT  BranchName FROM DBO.Branches (NOLOCK) WHERE BranchCode=@TxnBranchId)

    -- Perform EOD Checks that happen before EOD.
    IF @mode=1
    BEGIN
     
    END;

  END TRY
  BEGIN CATCH
    SET @Result='0 - Error(s) Occurred: '+ERROR_MESSAGE();

    INSERT INTO dbo.ErrorLog_N(ProgModule, Source, Terminal, SaccoUser, ErrorDate, brcode, Error, ErrorNumber,
                               ErrorSeverity, ErrorState, ErrorLine, ErrorProcedure)
    VALUES(
            'Date: '+CONVERT(VARCHAR(MAX), @SystemDate),
            ('Line'+CONVERT(VARCHAR(MAX), ERROR_LINE())+', Source: '+ERROR_PROCEDURE()), @compname, @User, GETDATE(),
            @brcode, ERROR_MESSAGE(), ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_LINE(), ERROR_PROCEDURE()
          );
  END CATCH;
END;