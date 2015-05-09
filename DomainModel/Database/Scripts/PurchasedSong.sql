USE [_Test]
GO

/****** Object:  Table [dbo].[PurchasedSong]    Script Date: 27/09/2014 14:45:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PurchasedSong](
	[PurchasedSongId] [int] IDENTITY(1,1) NOT NULL,
	[PlaylistSongId] [int] NOT NULL,
	[Cost] [money] NULL,
	[Credits] [int] NULL,
	[DatePurchased] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_PurchasedSong] PRIMARY KEY CLUSTERED 
(
	[PurchasedSongId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PurchasedSong] ADD  CONSTRAINT [DF_PurchasedSong_Credits]  DEFAULT ((0)) FOR [Credits]
GO

ALTER TABLE [dbo].[PurchasedSong] ADD  CONSTRAINT [DF_PurchasedSong_DatePurchased]  DEFAULT (getdate()) FOR [DatePurchased]
GO

ALTER TABLE [dbo].[PurchasedSong]  WITH CHECK ADD  CONSTRAINT [FK_PurchasedSong_PlaylistSong] FOREIGN KEY([PlaylistSongId])
REFERENCES [dbo].[PlaylistSong] ([PlaylistSongId])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[PurchasedSong] CHECK CONSTRAINT [FK_PurchasedSong_PlaylistSong]
GO

