CREATE DATABASE payement;
USE [payement]
GO

/****** Object:  Table [dbo].[ResumptionTable]    Script Date: 1/11/2017 1:19:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ResumptionTable](
	[id] [int] PRIMARY KEY IDENTITY(1,1),
	[ResumptionCookie] [nchar](500) NOT NULL,
	[ConvoId] [nchar](50) NOT NULL,

) 

GO


