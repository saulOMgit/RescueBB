USE [RescueBB]
GO
/****** Object:  Table [dbo].[JugadorPuntaje]    Script Date: 13/09/2021 17:06:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JugadorPuntaje](
	[Nombre] [varchar](50) NOT NULL,
	[Monedas] [int] NULL,
	[Nivel] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
