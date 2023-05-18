IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'ErrorLog_N'))
BEGIN
    
CREATE TABLE [dbo].[ErrorLog_N](
	[IdxNo] [int] IDENTITY(1,1) NOT NULL,
	[ProgModule] [varchar](100) NOT NULL,
	[Source] [varchar](100) NOT NULL,
	[Terminal] [varchar](100) NOT NULL,
	[SaccoUser] [varchar](250) NOT NULL,
	[ErrorDate] [datetime] NOT NULL,
	[brcode] [varchar](3) NOT NULL,
	[Error] [nvarchar](4000) NOT NULL,
	[ErrorNumber] [int] NOT NULL,
	[ErrorSeverity] [int] NOT NULL,
	[ErrorState] [int] NOT NULL,
	[ErrorLine] [int] NOT NULL,
	[ErrorProcedure] [nvarchar](200) NOT NULL,
	[IP] [varchar](50) NOT NULL,
	[CallenderDate] [datetime] NOT NULL
) ON [PRIMARY]
END