USE [LimitBreakPugs]
GO

/****** Object:  Table [dbo].[Events]    Script Date: 9/22/2020 10:24:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Events](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Scheduled_Date] [datetime] NOT NULL,
	[Created_On] [datetime] NOT NULL,
	[Discord_Message_ID] [varchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Events] ADD  CONSTRAINT [DF_Events_ID]  DEFAULT (newid()) FOR [ID]
GO

ALTER TABLE [dbo].[Events] ADD  CONSTRAINT [DF_Events_Created_On]  DEFAULT (getdate()) FOR [Created_On]
GO


