

CREATE TABLE [dbo].[Parameters](
	[Idxno] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ParamCode] [varchar](50) NULL,
	[ParamName] [nvarchar](100) NOT NULL,
	[ParamCat] [nvarchar](100) NOT NULL,
	[ParamAssoc] [nvarchar](2000) NULL,
	[ItemCode] [varchar](500) NOT NULL,
	[Visible] [int] NOT NULL
) ON [PRIMARY]
GO


