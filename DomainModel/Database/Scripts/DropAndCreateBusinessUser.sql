USE []
GO

ALTER TABLE [dbo].[BusinessUser] DROP CONSTRAINT [FK_BusinessUser_BusinessType]
GO

ALTER TABLE [dbo].[BusinessUser] DROP CONSTRAINT [DF_BusinessUser_CreatedDate]
GO

/****** Object:  Table [dbo].[BusinessUser]    Script Date: 01/02/2014 17:01:01 ******/
DROP TABLE [dbo].[BusinessUser]
GO

/****** Object:  Table [dbo].[BusinessUser]    Script Date: 01/02/2014 17:01:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BusinessUser](
	[BusinessUserId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[BusinessTypeId] [int] NOT NULL,
	[Address1] [nvarchar](250) NOT NULL,
	[Address2] [nvarchar](250) NULL,
	[City] [nvarchar](250) NOT NULL,
	[Country] [nvarchar](250) NOT NULL,
	[PostCode] [nvarchar](10) NOT NULL,
	[Logo] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_BusinessUser] PRIMARY KEY CLUSTERED 
(
	[BusinessUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BusinessUser] ADD  CONSTRAINT [DF_BusinessUser_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[BusinessUser]  WITH CHECK ADD  CONSTRAINT [FK_BusinessUser_BusinessType] FOREIGN KEY([BusinessTypeId])
REFERENCES [dbo].[BusinessType] ([BusinessTypeId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BusinessUser] CHECK CONSTRAINT [FK_BusinessUser_BusinessType]
GO

