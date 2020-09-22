USE [LimitBreakPugs]
GO

/****** Object:  Table [dbo].[OverwatchMaps]    Script Date: 9/22/2020 10:24:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OverwatchMaps](
	[ID] [uniqueidentifier] ROWGUIDCOL  NULL,
	[Map_Name] [varchar](100) NULL,
	[Map_Type] [varchar](50) NULL,
	[Map_Enabled] [bit] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OverwatchMaps] ADD  CONSTRAINT [DF_OverwatchMaps_ID]  DEFAULT (newid()) FOR [ID]
GO


