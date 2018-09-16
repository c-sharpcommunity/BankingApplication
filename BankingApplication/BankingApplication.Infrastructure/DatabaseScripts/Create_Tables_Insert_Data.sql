CREATE DATABASE [BankingApplication];
GO

USE [BankingApplication];  
GO

CREATE TABLE [dbo].[Account](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [varchar](50) NULL,
	[AccountNumber] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Balance] [decimal](18, 0) NULL,
	[Address] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[RowVersion] [timestamp] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Transaction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ToAccount] [int] NULL,
	[FromAccount] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[Amount] [decimal](18, 0) NULL,
	[Type] [int] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Account] FOREIGN KEY([ToAccount])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Account]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Account1] FOREIGN KEY([FromAccount])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Account1]
GO

-- Default Password: 1234abc
INSERT INTO [Account] (LoginName, AccountNumber, Balance, [Address], CreatedDate, [Password]) VALUES 
('Tester01', '0011', 0, 'address1', '2012-06-18 10:34:09.000', '64ad3fb166ddb41a2ca24f1803b8b722' ),
('Tester02', '1122', 0, 'address2', '2012-06-18 10:34:09.000', '64ad3fb166ddb41a2ca24f1803b8b722' ),
('Tester03', '2233', 0, 'address3', '2012-06-18 10:34:09.000', '64ad3fb166ddb41a2ca24f1803b8b722' ),
('Tester04', '3344', 0, 'address4', '2012-06-18 10:34:09.000', '64ad3fb166ddb41a2ca24f1803b8b722' )