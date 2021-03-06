USE [TAT001]
GO
/****** Object:  Table [dbo].[ACCION]    Script Date: 27/02/2018 12:32:40 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACCION](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DESCCRIPCION] [nvarchar](50) NULL,
	[TIPO] [nchar](1) NOT NULL,
 CONSTRAINT [PK_ACCION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CARPETA]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CARPETA](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URL] [nvarchar](50) NULL,
	[TITULO] [nvarchar](50) NULL,
	[ICON] [nvarchar](20) NULL,
	[ACTIVO] [bit] NULL,
 CONSTRAINT [PK_CARPETA] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CARPETAT]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CARPETAT](
	[SPRAS_ID] [nchar](2) NOT NULL,
	[ID] [int] NOT NULL,
	[TXT50] [nvarchar](50) NULL,
 CONSTRAINT [PK_CARPETAT] PRIMARY KEY CLUSTERED 
(
	[SPRAS_ID] ASC,
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GALL]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GALL](
	[ID] [nvarchar](5) NOT NULL,
	[DESCRIPCION] [nvarchar](60) NULL,
	[ACTIVO] [bit] NULL,
 CONSTRAINT [PK_GALL] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GALLT]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GALLT](
	[SPRAS_ID] [nchar](2) NOT NULL,
	[GALL_ID] [nvarchar](5) NOT NULL,
	[TXT50] [nvarchar](50) NULL,
 CONSTRAINT [PK_GALLT] PRIMARY KEY CLUSTERED 
(
	[SPRAS_ID] ASC,
	[GALL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MIEMBROS]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MIEMBROS](
	[USUARIO_ID] [nvarchar](16) NOT NULL,
	[ROL_ID] [int] NOT NULL,
	[ACTIVO] [bit] NULL,
 CONSTRAINT [PK_MIEMBROS] PRIMARY KEY CLUSTERED 
(
	[USUARIO_ID] ASC,
	[ROL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PAGINA]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAGINA](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URL] [nvarchar](255) NULL,
	[TITULO] [nvarchar](50) NULL,
	[CARPETA_ID] [int] NULL,
	[ICON] [nvarchar](20) NULL,
	[ACTIVO] [bit] NULL,
 CONSTRAINT [PK_PAGINA] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PAGINAT]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAGINAT](
	[SPRAS_ID] [nchar](2) NOT NULL,
	[ID] [int] NOT NULL,
	[TXT50] [nvarchar](50) NULL,
 CONSTRAINT [PK_PAGINAT] PRIMARY KEY CLUSTERED 
(
	[SPRAS_ID] ASC,
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PERMISO_PAGINA]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PERMISO_PAGINA](
	[PAGINA_ID] [int] NOT NULL,
	[ROL_ID] [int] NOT NULL,
	[PERMISO] [bit] NULL,
 CONSTRAINT [PK_PERMISO_PAGINA] PRIMARY KEY CLUSTERED 
(
	[PAGINA_ID] ASC,
	[ROL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PRESUPUESTOH]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PRESUPUESTOH](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ANIO] [nchar](4) NOT NULL,
	[USUARIO_ID] [nvarchar](16) NULL,
	[FECHAC] [datetime] NULL,
 CONSTRAINT [PK_PRESUPUESTOH] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[ANIO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PRESUPUESTOP]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PRESUPUESTOP](
	[ID] [int] NOT NULL,
	[ANIO] [nchar](4) NOT NULL,
	[POS] [int] NOT NULL,
	[MES] [nchar](2) NULL,
	[VERSION] [nvarchar](50) NULL,
	[PAIS] [nvarchar](15) NULL,
	[MONEDA] [nchar](2) NULL,
	[MATERIAL] [nvarchar](18) NULL,
	[BANNER] [nchar](6) NULL,
	[CONCEPTO] [nvarchar](10) NULL,
	[DATA] [nvarchar](50) NULL,
 CONSTRAINT [PK_PRESUPUESTOP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[ANIO] ASC,
	[POS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ROL]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROL](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CLAVE] [nvarchar](10) NULL,
	[NOMBRE] [nvarchar](50) NULL,
	[ACTIVO] [bit] NULL,
 CONSTRAINT [PK_ROL] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SPRAS]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SPRAS](
	[ID] [nchar](2) NOT NULL,
	[DESCRIPCION] [nvarchar](20) NULL,
 CONSTRAINT [PK_SPRAS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TALL]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TALL](
	[ID] [nvarchar](10) NOT NULL,
	[DESCRIPCION] [nvarchar](60) NULL,
	[FECHAI] [date] NULL,
	[FECHAF] [date] NULL,
	[GALL_ID] [nvarchar](5) NULL,
	[ACTIVO] [bit] NULL,
 CONSTRAINT [PK_TALL] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TALLT]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TALLT](
	[SPRAS_ID] [nchar](2) NOT NULL,
	[TALL_ID] [nvarchar](10) NOT NULL,
	[TXT50] [nvarchar](50) NULL,
 CONSTRAINT [PK_TALLT] PRIMARY KEY CLUSTERED 
(
	[SPRAS_ID] ASC,
	[TALL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TSOL]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSOL](
	[ID] [nvarchar](10) NOT NULL,
	[DESCRIPCION] [nvarchar](60) NULL,
	[TSOLR] [nvarchar](10) NULL,
	[RANGO_ID] [nchar](2) NOT NULL,
	[ESTATUS] [nchar](1) NOT NULL,
 CONSTRAINT [PK_TSOL] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TSOLT]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TSOLT](
	[SPRAS_ID] [nchar](2) NOT NULL,
	[TSOL_ID] [nvarchar](10) NOT NULL,
	[TXT020] [nvarchar](20) NULL,
	[TXT50] [nvarchar](50) NULL,
 CONSTRAINT [PK_TSOLT] PRIMARY KEY CLUSTERED 
(
	[SPRAS_ID] ASC,
	[TSOL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[USUARIO]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIO](
	[ID] [nvarchar](16) NOT NULL,
	[PASS] [nvarchar](50) NOT NULL,
	[NOMBRE] [nvarchar](50) NOT NULL,
	[APELLIDO_P] [nvarchar](50) NOT NULL,
	[APELLIDO_M] [nvarchar](50) NULL,
	[EMAIL] [nvarchar](256) NOT NULL,
	[SPRAS_ID] [nchar](2) NOT NULL,
	[ACTIVO] [bit] NULL,
 CONSTRAINT [PK_USUARIO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WORKFH]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WORKFH](
	[ID] [nvarchar](10) NOT NULL,
	[DESCRIPCION] [nvarchar](60) NULL,
	[TSOL_ID] [nvarchar](10) NULL,
	[ESTATUS] [nchar](1) NULL,
	[USUARIO_ID] [nvarchar](16) NULL,
	[FECHAC] [datetime] NULL,
 CONSTRAINT [PK_WORKFH] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WORKFP]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WORKFP](
	[ID] [nvarchar](10) NOT NULL,
	[VERSION] [int] NOT NULL,
	[POS] [int] NOT NULL,
	[AGENTE_ID] [int] NULL,
	[ACCION_ID] [int] NULL,
	[NEXT_STEP] [int] NULL,
	[NS_ACCEPT] [int] NULL,
	[NS_REJECT] [int] NULL,
	[LOOPS] [int] NULL,
	[CONDICION_ID] [int] NULL,
	[NS_CN_ACCEPT] [int] NULL,
	[NS_CN_REJECT] [int] NULL,
	[EMAIL] [nchar](1) NULL,
	[EMAIL_TXT_ID] [int] NULL,
	[EMAIL_INN_ID] [int] NULL,
 CONSTRAINT [PK_WORKFP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[VERSION] ASC,
	[POS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WORKFT]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WORKFT](
	[SPRAS_ID] [nchar](2) NOT NULL,
	[WF_ID] [nvarchar](10) NOT NULL,
	[WF_VERSION] [int] NOT NULL,
	[TXT20] [nvarchar](20) NULL,
	[TXT50] [nvarchar](50) NULL,
 CONSTRAINT [PK_WORKFT] PRIMARY KEY CLUSTERED 
(
	[SPRAS_ID] ASC,
	[WF_ID] ASC,
	[WF_VERSION] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WORKFV]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WORKFV](
	[ID] [nvarchar](10) NOT NULL,
	[VERSION] [int] NOT NULL,
	[DESCRIPCION] [nvarchar](60) NULL,
	[ESTATUS] [nchar](1) NULL,
	[FECHAI] [date] NULL,
	[FECHAF] [date] NULL,
	[USUARIO_ID] [nvarchar](16) NULL,
	[FECHAC] [datetime] NULL,
 CONSTRAINT [PK_WORKV] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[VERSION] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[CARPETAV]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CARPETAV]
AS
SELECT        dbo.USUARIO.ID AS USUARIO_ID, dbo.CARPETA.ID, dbo.CARPETA.URL, dbo.CARPETAT.TXT50, dbo.CARPETA.ICON
FROM            dbo.USUARIO INNER JOIN
                         dbo.MIEMBROS ON dbo.USUARIO.ID = dbo.MIEMBROS.USUARIO_ID INNER JOIN
                         dbo.PERMISO_PAGINA ON dbo.MIEMBROS.ROL_ID = dbo.PERMISO_PAGINA.ROL_ID INNER JOIN
                         dbo.PAGINA ON dbo.PERMISO_PAGINA.PAGINA_ID = dbo.PAGINA.ID INNER JOIN
                         dbo.CARPETA ON dbo.PAGINA.CARPETA_ID = dbo.CARPETA.ID INNER JOIN
                         dbo.CARPETAT ON dbo.CARPETA.ID = dbo.CARPETAT.ID AND dbo.USUARIO.SPRAS_ID = dbo.CARPETAT.SPRAS_ID
WHERE        (dbo.PERMISO_PAGINA.PERMISO = 1)
GROUP BY dbo.CARPETA.ID, dbo.CARPETA.URL, dbo.CARPETAT.TXT50, dbo.CARPETA.ICON, dbo.USUARIO.ID

GO
/****** Object:  View [dbo].[PAGINAV]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PAGINAV]
AS
SELECT        dbo.USUARIO.ID, dbo.PAGINA.ID AS PAGINA_ID, dbo.PAGINA.URL AS PAGINA_URL, dbo.PAGINAT.TXT50, dbo.CARPETA.ID AS CARPETA_ID, 
                         dbo.CARPETA.URL AS CARPETA_URL, dbo.PAGINA.ICON, dbo.PERMISO_PAGINA.PERMISO, dbo.PAGINAT.SPRAS_ID, 
                         dbo.USUARIO.SPRAS_ID AS USUARIO_SPRAS
FROM            dbo.USUARIO INNER JOIN
                         dbo.MIEMBROS ON dbo.USUARIO.ID = dbo.MIEMBROS.USUARIO_ID INNER JOIN
                         dbo.PERMISO_PAGINA ON dbo.MIEMBROS.ROL_ID = dbo.PERMISO_PAGINA.ROL_ID INNER JOIN
                         dbo.PAGINA ON dbo.PERMISO_PAGINA.PAGINA_ID = dbo.PAGINA.ID INNER JOIN
                         dbo.CARPETA ON dbo.PAGINA.CARPETA_ID = dbo.CARPETA.ID INNER JOIN
                         dbo.PAGINAT ON dbo.PAGINA.ID = dbo.PAGINAT.ID AND dbo.USUARIO.SPRAS_ID = dbo.PAGINAT.SPRAS_ID
WHERE        (dbo.PERMISO_PAGINA.PERMISO = 1)

GO
ALTER TABLE [dbo].[CARPETAT]  WITH CHECK ADD  CONSTRAINT [FK_CARPETAT_CARPETA] FOREIGN KEY([ID])
REFERENCES [dbo].[CARPETA] ([ID])
GO
ALTER TABLE [dbo].[CARPETAT] CHECK CONSTRAINT [FK_CARPETAT_CARPETA]
GO
ALTER TABLE [dbo].[CARPETAT]  WITH CHECK ADD  CONSTRAINT [FK_CARPETAT_SPRAS] FOREIGN KEY([SPRAS_ID])
REFERENCES [dbo].[SPRAS] ([ID])
GO
ALTER TABLE [dbo].[CARPETAT] CHECK CONSTRAINT [FK_CARPETAT_SPRAS]
GO
ALTER TABLE [dbo].[GALLT]  WITH CHECK ADD  CONSTRAINT [FK_GALLT_GALL] FOREIGN KEY([GALL_ID])
REFERENCES [dbo].[GALL] ([ID])
GO
ALTER TABLE [dbo].[GALLT] CHECK CONSTRAINT [FK_GALLT_GALL]
GO
ALTER TABLE [dbo].[GALLT]  WITH CHECK ADD  CONSTRAINT [FK_GALLT_SPRAS] FOREIGN KEY([SPRAS_ID])
REFERENCES [dbo].[SPRAS] ([ID])
GO
ALTER TABLE [dbo].[GALLT] CHECK CONSTRAINT [FK_GALLT_SPRAS]
GO
ALTER TABLE [dbo].[MIEMBROS]  WITH CHECK ADD  CONSTRAINT [FK_MIEMBROS_ROL] FOREIGN KEY([ROL_ID])
REFERENCES [dbo].[ROL] ([ID])
GO
ALTER TABLE [dbo].[MIEMBROS] CHECK CONSTRAINT [FK_MIEMBROS_ROL]
GO
ALTER TABLE [dbo].[MIEMBROS]  WITH CHECK ADD  CONSTRAINT [FK_MIEMBROS_USUARIO] FOREIGN KEY([USUARIO_ID])
REFERENCES [dbo].[USUARIO] ([ID])
GO
ALTER TABLE [dbo].[MIEMBROS] CHECK CONSTRAINT [FK_MIEMBROS_USUARIO]
GO
ALTER TABLE [dbo].[PAGINA]  WITH CHECK ADD  CONSTRAINT [FK_PAGINA_CARPETA] FOREIGN KEY([CARPETA_ID])
REFERENCES [dbo].[CARPETA] ([ID])
GO
ALTER TABLE [dbo].[PAGINA] CHECK CONSTRAINT [FK_PAGINA_CARPETA]
GO
ALTER TABLE [dbo].[PAGINAT]  WITH CHECK ADD  CONSTRAINT [FK_PAGINAT_PAGINA] FOREIGN KEY([ID])
REFERENCES [dbo].[PAGINA] ([ID])
GO
ALTER TABLE [dbo].[PAGINAT] CHECK CONSTRAINT [FK_PAGINAT_PAGINA]
GO
ALTER TABLE [dbo].[PAGINAT]  WITH CHECK ADD  CONSTRAINT [FK_PAGINAT_SPRAS] FOREIGN KEY([SPRAS_ID])
REFERENCES [dbo].[SPRAS] ([ID])
GO
ALTER TABLE [dbo].[PAGINAT] CHECK CONSTRAINT [FK_PAGINAT_SPRAS]
GO
ALTER TABLE [dbo].[PERMISO_PAGINA]  WITH CHECK ADD  CONSTRAINT [FK_PERMISO_PAGINA_PAGINA] FOREIGN KEY([PAGINA_ID])
REFERENCES [dbo].[PAGINA] ([ID])
GO
ALTER TABLE [dbo].[PERMISO_PAGINA] CHECK CONSTRAINT [FK_PERMISO_PAGINA_PAGINA]
GO
ALTER TABLE [dbo].[PERMISO_PAGINA]  WITH CHECK ADD  CONSTRAINT [FK_PERMISO_PAGINA_ROL] FOREIGN KEY([ROL_ID])
REFERENCES [dbo].[ROL] ([ID])
GO
ALTER TABLE [dbo].[PERMISO_PAGINA] CHECK CONSTRAINT [FK_PERMISO_PAGINA_ROL]
GO
ALTER TABLE [dbo].[PRESUPUESTOH]  WITH CHECK ADD  CONSTRAINT [FK_PRESUPUESTOH_USUARIO] FOREIGN KEY([USUARIO_ID])
REFERENCES [dbo].[USUARIO] ([ID])
GO
ALTER TABLE [dbo].[PRESUPUESTOH] CHECK CONSTRAINT [FK_PRESUPUESTOH_USUARIO]
GO
ALTER TABLE [dbo].[PRESUPUESTOP]  WITH CHECK ADD  CONSTRAINT [FK_PRESUPUESTOP_PRESUPUESTOH] FOREIGN KEY([ID], [ANIO])
REFERENCES [dbo].[PRESUPUESTOH] ([ID], [ANIO])
GO
ALTER TABLE [dbo].[PRESUPUESTOP] CHECK CONSTRAINT [FK_PRESUPUESTOP_PRESUPUESTOH]
GO
ALTER TABLE [dbo].[TALL]  WITH CHECK ADD  CONSTRAINT [FK_TALL_GALL] FOREIGN KEY([GALL_ID])
REFERENCES [dbo].[GALL] ([ID])
GO
ALTER TABLE [dbo].[TALL] CHECK CONSTRAINT [FK_TALL_GALL]
GO
ALTER TABLE [dbo].[TALLT]  WITH CHECK ADD  CONSTRAINT [FK_TALLT_SPRAS] FOREIGN KEY([SPRAS_ID])
REFERENCES [dbo].[SPRAS] ([ID])
GO
ALTER TABLE [dbo].[TALLT] CHECK CONSTRAINT [FK_TALLT_SPRAS]
GO
ALTER TABLE [dbo].[TALLT]  WITH CHECK ADD  CONSTRAINT [FK_TALLT_TALL] FOREIGN KEY([TALL_ID])
REFERENCES [dbo].[TALL] ([ID])
GO
ALTER TABLE [dbo].[TALLT] CHECK CONSTRAINT [FK_TALLT_TALL]
GO
ALTER TABLE [dbo].[TSOL]  WITH CHECK ADD  CONSTRAINT [FK_TSOL_TSOL] FOREIGN KEY([TSOLR])
REFERENCES [dbo].[TSOL] ([ID])
GO
ALTER TABLE [dbo].[TSOL] CHECK CONSTRAINT [FK_TSOL_TSOL]
GO
ALTER TABLE [dbo].[TSOLT]  WITH CHECK ADD  CONSTRAINT [FK_TSOLT_SPRAS] FOREIGN KEY([SPRAS_ID])
REFERENCES [dbo].[SPRAS] ([ID])
GO
ALTER TABLE [dbo].[TSOLT] CHECK CONSTRAINT [FK_TSOLT_SPRAS]
GO
ALTER TABLE [dbo].[TSOLT]  WITH CHECK ADD  CONSTRAINT [FK_TSOLT_TSOL] FOREIGN KEY([TSOL_ID])
REFERENCES [dbo].[TSOL] ([ID])
GO
ALTER TABLE [dbo].[TSOLT] CHECK CONSTRAINT [FK_TSOLT_TSOL]
GO
ALTER TABLE [dbo].[USUARIO]  WITH CHECK ADD  CONSTRAINT [FK_USUARIO_SPRAS] FOREIGN KEY([SPRAS_ID])
REFERENCES [dbo].[SPRAS] ([ID])
GO
ALTER TABLE [dbo].[USUARIO] CHECK CONSTRAINT [FK_USUARIO_SPRAS]
GO
ALTER TABLE [dbo].[WORKFP]  WITH CHECK ADD  CONSTRAINT [FK_WORKFP_ACCION] FOREIGN KEY([ACCION_ID])
REFERENCES [dbo].[ACCION] ([ID])
GO
ALTER TABLE [dbo].[WORKFP] CHECK CONSTRAINT [FK_WORKFP_ACCION]
GO
ALTER TABLE [dbo].[WORKFP]  WITH CHECK ADD  CONSTRAINT [FK_WORKFP_WORKFV] FOREIGN KEY([ID], [VERSION])
REFERENCES [dbo].[WORKFV] ([ID], [VERSION])
GO
ALTER TABLE [dbo].[WORKFP] CHECK CONSTRAINT [FK_WORKFP_WORKFV]
GO
ALTER TABLE [dbo].[WORKFT]  WITH CHECK ADD  CONSTRAINT [FK_WORKFT_SPRAS] FOREIGN KEY([SPRAS_ID])
REFERENCES [dbo].[SPRAS] ([ID])
GO
ALTER TABLE [dbo].[WORKFT] CHECK CONSTRAINT [FK_WORKFT_SPRAS]
GO
ALTER TABLE [dbo].[WORKFT]  WITH CHECK ADD  CONSTRAINT [FK_WORKFT_WORKFV] FOREIGN KEY([WF_ID], [WF_VERSION])
REFERENCES [dbo].[WORKFV] ([ID], [VERSION])
GO
ALTER TABLE [dbo].[WORKFT] CHECK CONSTRAINT [FK_WORKFT_WORKFV]
GO
ALTER TABLE [dbo].[WORKFV]  WITH CHECK ADD  CONSTRAINT [FK_WORKV_WORKFH] FOREIGN KEY([ID])
REFERENCES [dbo].[WORKFH] ([ID])
GO
ALTER TABLE [dbo].[WORKFV] CHECK CONSTRAINT [FK_WORKV_WORKFH]
GO
/****** Object:  StoredProcedure [dbo].[CSP_CARPETA]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<ROGELIO SÁNCHEZ>
-- Create date: <22-02-2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CSP_CARPETA]
	-- Add the parameters for the stored procedure here
	@ID		nvarchar(16),
	@ACCION int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @ACCION = 1
	BEGIN
		--INSERT USUARIO( ID, PASS, NOMBRE, APELLIDO_P, APELLIDO_M, EMAIL, ACTIVO)
		--VALUES ( @ID, @PASS, @NOMBRE, @APELLIDO_P, @APELLIDO_M, @EMAIL, 'TRUE')
		SELECT C.ID AS CAR_ID, C.URL AS CAR_URL, CT.TXT50 AS CAR_TIT, C.ICON AS ICONO
		FROM USUARIO AS U
		INNER JOIN MIEMBROS AS M
		ON U.ID = M.USUARIO_ID
		INNER JOIN PERMISO_PAGINA AS PP
		ON M.ROL_ID = PP.ROL_ID
		INNER JOIN PAGINA AS P
		ON PP.PAGINA_ID = P.ID
		INNER JOIN CARPETA AS C
		ON p.CARPETA_ID = C.ID
		INNER JOIN CARPETAT AS CT
		ON C.ID = CT.ID
		WHERE U.ID = @ID
		AND PP.PERMISO = 1
		AND CT.SPRAS_ID = U.SPRAS_ID
		group by C.ID, C.URL,  CT.TXT50, C.ICON
	END
	IF @ACCION = 2
	BEGIN 
		SELECT C.ID AS CAR_ID, C.URL AS CAR_URL, CT.TXT50 AS CAR_TIT, C.ICON AS ICONO
		FROM USUARIO AS U
		INNER JOIN MIEMBROS AS M
		ON U.ID = M.USUARIO_ID
		INNER JOIN PERMISO_PAGINA AS PP
		ON M.ROL_ID = PP.ROL_ID
		INNER JOIN PAGINA AS P
		ON PP.PAGINA_ID = P.ID
		INNER JOIN CARPETA AS C
		ON p.CARPETA_ID = C.ID
		INNER JOIN CARPETAT AS CT
		ON C.ID = CT.ID
		WHERE U.ID = @ID
		AND PP.PERMISO = 1
		AND CT.SPRAS_ID = U.SPRAS_ID
		group by C.ID, C.URL,  CT.TXT50, C.ICON
	END
END

GO
/****** Object:  StoredProcedure [dbo].[CSP_PERMISO]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CSP_PERMISO]
	-- Add the parameters for the stored procedure here
	@ID		nvarchar(16),
	@ACCION int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @ACCION = 1
	BEGIN
		--INSERT USUARIO( ID, PASS, NOMBRE, APELLIDO_P, APELLIDO_M, EMAIL, ACTIVO)
		--VALUES ( @ID, @PASS, @NOMBRE, @APELLIDO_P, @APELLIDO_M, @EMAIL, 'TRUE')

		SELECT U.ID, P.ID AS ID_PAG, P.URL, CT.TXT50 AS TITULO, C.ID AS CAR_ID, C.URL AS CAR_URL, P.ICON AS ICONO
		FROM USUARIO AS U 
		INNER JOIN MIEMBROS AS M
		ON U.ID = M.USUARIO_ID
		INNER JOIN PERMISO_PAGINA AS PP
		ON M.ROL_ID = PP.ROL_ID
		INNER JOIN PAGINA AS P
		ON PP.PAGINA_ID = P.ID
		INNER JOIN CARPETA AS C
		ON P.CARPETA_ID = C.ID		
		INNER JOIN PAGINAT AS CT
		ON P.ID = CT.ID
		WHERE U.ID = @ID
		AND PP.PERMISO = 1
		AND CT.SPRAS_ID = U.SPRAS_ID
	END
	IF @ACCION = 2
	BEGIN 
		SELECT U.ID, P.ID AS ID_PAG, P.URL, CT.TXT50 AS TITULO, C.ID AS CAR_ID, C.URL AS CAR_URL, P.ICON AS ICONO
		FROM USUARIO AS U 
		INNER JOIN MIEMBROS AS M
		ON U.ID = M.USUARIO_ID
		INNER JOIN PERMISO_PAGINA AS PP
		ON M.ROL_ID = PP.ROL_ID
		INNER JOIN PAGINA AS P
		ON PP.PAGINA_ID = P.ID
		INNER JOIN CARPETA AS C
		ON P.CARPETA_ID = C.ID		
		INNER JOIN PAGINAT AS CT
		ON P.ID = CT.ID
		WHERE U.ID = @ID
		AND PP.PERMISO = 1
		AND CT.SPRAS_ID = U.SPRAS_ID
	END
END

GO
/****** Object:  StoredProcedure [dbo].[CSP_USUARIO]    Script Date: 27/02/2018 12:32:41 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<ROGELIO SÁNCHEZ>
-- Create date: <22-02-2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CSP_USUARIO]
	-- Add the parameters for the stored procedure here
	@ID		nvarchar(16),
	@PASS nvarchar(50),
	@NOMBRE NVARCHAR(50),
	@APELLIDO_P NVARCHAR(50),
	@APELLIDO_M NVARCHAR(50),
	@EMAIL NVARCHAR(255),
	@SPRAS_ID NCHAR(2),
	@ACTIVO BIT,
	@ACCION int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @ACCION = 1
	BEGIN
		INSERT USUARIO( ID, PASS, NOMBRE, APELLIDO_P, APELLIDO_M, EMAIL, SPRAS_ID, ACTIVO)
		VALUES ( @ID, @PASS, @NOMBRE, @APELLIDO_P, @APELLIDO_M, @EMAIL, @SPRAS_ID,  'TRUE')
	END
	IF @ACCION = 2
	BEGIN 
		SELECT  USUARIO.ID, PASS, USUARIO.NOMBRE, APELLIDO_P, APELLIDO_M, EMAIL, SPRAS_ID, USUARIO.ACTIVO, ROL.ID AS ID_GR, ROL.NOMBRE AS NOMBRE_GR
		FROM USUARIO
		INNER JOIN MIEMBROS
		ON USUARIO.ID = MIEMBROS.USUARIO_ID
		INNER JOIN ROL
		ON MIEMBROS.ROL_ID = ROL.ID
		WHERE USUARIO.ID = @ID
	END
END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[35] 4[28] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "USUARIO"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MIEMBROS"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 118
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PERMISO_PAGINA"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 118
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PAGINA"
            Begin Extent = 
               Top = 120
               Left = 246
               Bottom = 249
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CARPETA"
            Begin Extent = 
               Top = 120
               Left = 454
               Bottom = 249
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CARPETAT"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 250
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
        ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CARPETAV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CARPETAV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CARPETAV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[30] 4[36] 2[19] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "USUARIO"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "MIEMBROS"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 118
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PERMISO_PAGINA"
            Begin Extent = 
               Top = 23
               Left = 525
               Bottom = 152
               Right = 695
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PAGINA"
            Begin Extent = 
               Top = 138
               Left = 235
               Bottom = 267
               Right = 405
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "CARPETA"
            Begin Extent = 
               Top = 151
               Left = 514
               Bottom = 305
               Right = 684
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "PAGINAT"
            Begin Extent = 
               Top = 147
               Left = 34
               Bottom = 303
               Right = 204
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
        ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PAGINAV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PAGINAV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PAGINAV'
GO
