IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME='details' AND C.COLUMN_NAME='Is_Data_wareHouse')
BEGIN
  ALTER TABLE dbo.details ADD Is_Data_wareHouse INT NOT NULL DEFAULT 0;
END;
GO