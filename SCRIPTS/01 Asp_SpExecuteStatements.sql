IF EXISTS (SELECT 0 FROM sys.objects WHERE object_id = OBJECT_ID ( N'[dbo].[spExecuteStatements]' ) AND type IN ( N'P', N'PC' ))
  BEGIN
    DROP PROCEDURE dbo.spExecuteStatements;
  END;
GO

CREATE PROCEDURE [dbo].[spExecuteStatements](@Mode AS numeric(18,2), @TableName AS VARCHAR(1000),
@Columns AS VARCHAR(7000), @Values AS VARCHAR(7000), @Condition AS VARCHAR(7000)
) AS

DECLARE @SQlStr AS VARCHAR(7000) = ''
DECLARE @ErrorMessage VARCHAR(7000)

BEGIN TRY
	BEGIN TRANSACTION
		IF @Mode = 0 -- If edition type is an Select
			BEGIN
				IF LEN(@Condition) = 0
					BEGIN
						SET @SQlStr = 'SELECT ' + @Columns + ' FROM ' + @TableName
					END
				ELSE
					BEGIN
						SET @SQlStr = 'SELECT ' + @Columns + ' FROM ' + @TableName + ' WHERE ' + @Condition
					END
				PRINT @SQLStr
				EXECUTE (@SQLStr)
			END 
	
		ELSE IF @Mode = 1 -- If edition type is an addition
			BEGIN
				SET @SQlStr = 'INSERT INTO ' + @TableName + '(' + @Columns + ') VALUES (' + @Values + ')' 
				PRINT @SQLStr
				EXECUTE (@SQLStr)
			END 
		ELSE IF @Mode = 2 -- If edition type is edit
			BEGIN
				IF LEN(@Condition) = 0
					BEGIN
						SET @SQlStr = 'UPDATE ' + @TableName + ' SET ' + @Columns
					END
				ELSE
					BEGIN
						SET @SQlStr = 'UPDATE ' + @TableName + ' SET ' + @Columns + ' WHERE ' + @Condition
					END
				PRINT @SQLStr
				EXECUTE (@SQLStr)
			END
		ELSE IF @Mode = 3 -- If edition type is deletion
			BEGIN
				IF LEN(@Condition) = 0
					BEGIN
						SET @SQlStr = 'DELETE FROM ' + @TableName
					END 
				ELSE
					BEGIN
						SET @SQlStr = 'DELETE FROM ' + @TableName + ' WHERE ' + @Condition
					END 
				PRINT @SQLStr
				EXECUTE (@SQLStr)
			END
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION
	SET @ErrorMessage = Str(@@Error)+ ' '+ ERROR_MESSAGE()
	PRINT 'Error ' + @ErrorMessage
END CATCH
