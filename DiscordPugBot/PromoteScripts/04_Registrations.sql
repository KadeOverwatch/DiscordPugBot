USE [LimitBreakPugs]
GO

/****** Object:  Table [dbo].[Registrations]    Script Date: 9/22/2020 10:24:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Registrations](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Event_ID] [uniqueidentifier] NOT NULL,
	[Player_ID] [uniqueidentifier] NOT NULL,
	[Created_On] [datetime] NOT NULL,
	[Player_Cancelled] [bit] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Registrations] ADD  CONSTRAINT [DF_Registrations_ID]  DEFAULT (newid()) FOR [ID]
GO

ALTER TABLE [dbo].[Registrations] ADD  CONSTRAINT [DF_Registrations_Created_On]  DEFAULT (getdate()) FOR [Created_On]
GO


