USE [BTChallenge]
GO
/****** Object:  User [btc_user]    Script Date: 9/4/2020 3:12:29 PM ******/
CREATE USER [btc_user] FOR LOGIN [btc_user] WITH DEFAULT_SCHEMA=[dbo]
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'btc_user'
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 9/4/2020 3:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[TaskId] [bigint] IDENTITY(1,1) NOT NULL,
	[TaskName] [nvarchar](50) NULL,
	[TaskDate] [datetime] NULL,
	[TaskType] [varchar](10) NULL,
	[TaskProcessed] [bit] NOT NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Tasks] ON 

INSERT [dbo].[Tasks] ([TaskId], [TaskName], [TaskDate], [TaskType], [TaskProcessed]) VALUES (1, N'Notificare', CAST(N'2020-09-04T07:20:00.000' AS DateTime), N'notificare', 0)
INSERT [dbo].[Tasks] ([TaskId], [TaskName], [TaskDate], [TaskType], [TaskProcessed]) VALUES (2, N'cat.gif', CAST(N'2020-09-04T07:21:00.000' AS DateTime), N'fisier', 0)
INSERT [dbo].[Tasks] ([TaskId], [TaskName], [TaskDate], [TaskType], [TaskProcessed]) VALUES (3, N'Plata', CAST(N'2020-09-04T07:22:00.000' AS DateTime), N'plata', 0)
INSERT [dbo].[Tasks] ([TaskId], [TaskName], [TaskDate], [TaskType], [TaskProcessed]) VALUES (4, N'Incasare', CAST(N'2020-09-04T07:24:00.000' AS DateTime), N'incasare', 0)
SET IDENTITY_INSERT [dbo].[Tasks] OFF
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_TaskProcessed]  DEFAULT ((0)) FOR [TaskProcessed]
GO
