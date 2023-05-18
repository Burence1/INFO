
CREATE TABLE [dbo].[customer_details](
	[idno] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[acno] [varchar](50) NOT NULL,
	[firstName] [varchar](250) NULL,
	[middleName] [varchar](250) NULL,
	[lastName] [varchar](250) NULL,
	[FullName] [varchar](750) NOT NULL,
	[address] [varchar](500) NULL,
	[city] [varchar](20) NOT NULL,
	[personalno] [nvarchar](150) NOT NULL,
	[Saccomno] [nvarchar](20) NOT NULL,
	[joiningDate] [datetime] NOT NULL,
	[mobileNo] [nvarchar](30) NOT NULL,
	[gender] [varchar](10) NULL,
	[Employer] [nvarchar](35) NOT NULL,
	[officer] [nvarchar](30) NOT NULL,
	[Occupation] [varchar](100) NOT NULL,
	[YEARBIRTH] [datetime] NULL,
	[Email] [varchar](100) NOT NULL,
	[MaritalStatus] [varchar](10) NOT NULL,
	[BankAccount] [varchar](100) NOT NULL,
	[BranchId] [varchar](10) NOT NULL,
	[BCode] [varchar](10) NOT NULL,
	[BaseNo] [varchar](20) NOT NULL
) ON [PRIMARY]
GO


