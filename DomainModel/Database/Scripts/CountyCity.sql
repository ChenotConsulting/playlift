USE [TRM_Test]
GO

/****** Object:  Table [dbo].[CountyCity]    Script Date: 04/11/2014 17:44:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CountyCity](
	[CountyCityId] [int] IDENTITY(1,1) NOT NULL,
	[County] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CountyCity] PRIMARY KEY CLUSTERED 
(
	[CountyCityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

