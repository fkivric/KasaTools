USE master
create database KrediPuan
GO
USE [Yonavm_Web_Siparis]
GO
/****** Object:  Table [dbo].[EDevlet_MusteriDurum]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EDevlet_MusteriDurum](
	[Musteri_VolID] [bigint] NULL,
	[Musteri_TicID] [bigint] NULL,
	[Musteri_SipID] [bigint] NULL,
	[islemTarihi] [smalldatetime] NULL,
	[Adress] [bit] NULL,
	[DogumTarihi] [smalldatetime] NULL,
	[icraAcik] [bit] NULL,
	[icraAcikOnemli] [int] NULL,
	[icraAcikOnemsiz] [int] NULL,
	[icraKapali] [bit] NULL,
	[icraKapaliOnemli] [int] NULL,
	[icraKapaliOnemsiz] [int] NULL,
	[icraGizli] [bit] NULL,
	[icraGizliOnemli] [int] NULL,
	[icraGizliOnemsiz] [int] NULL,
	[SSK] [bit] NULL,
	[SGKTopSure] [numeric](18, 2) NULL,
	[SGKsongiris] [numeric](18, 2) NULL,
	[SGKsonPrim] [numeric](18, 2) NULL,
	[dteSonAy] [numeric](18, 2) NULL,
	[SGKYılGun] [smalldatetime] NULL,
	[SGKYılPrim] [numeric](18, 2) NULL,
	[SGKMeslekKodu] [nvarchar](7) NULL,
	[Tapu] [bit] NULL,
	[Mesken] [int] NULL,
	[Tasinmaz] [int] NULL,
	[Mulk] [int] NULL,
	[Arac] [bit] NULL,
	[AracSayi] [int] NULL,
	[Dava] [bit] NULL,
	[DavaOnemli] [int] NULL,
	[DavaOnemsiz] [int] NULL,
	[Emekli] [bit] NULL,
	[EmekliAylik] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EDevlet_RiskGrubu_1]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EDevlet_RiskGrubu_1](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Tanım] [nvarchar](250) NOT NULL,
	[Zorunlu] [bit] NOT NULL,
 CONSTRAINT [PK_EDevlet_RiskGrubu_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EDevlet_RiskGrubu_2]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EDevlet_RiskGrubu_2](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Parent_id] [int] NOT NULL,
	[Tanım] [nvarchar](250) NOT NULL,
	[Risk] [bit] NOT NULL,
 CONSTRAINT [PK_EDevlet_RiskGrubu_2] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EDevlet_RiskGrubu_3]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EDevlet_RiskGrubu_3](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Parent_id] [int] NOT NULL,
	[Tanım] [nvarchar](250) NOT NULL,
	[Risk] [bit] NOT NULL,
 CONSTRAINT [PK_EDevlet_RiskGrubu_3] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EDevlet_RiskGrubu_Puan]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EDevlet_RiskGrubu_Puan](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Risk_id] [int] NOT NULL,
	[Puan] [numeric](18, 2) NOT NULL,
	[Ceza] [bit] NOT NULL,
 CONSTRAINT [PK_EDevlet_RiskGrubu_Puan] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EDevlet_RiskGrubu_Puan_1]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EDevlet_RiskGrubu_Puan_1](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Risk_id] [int] NOT NULL,
	[Puan] [numeric](18, 2) NOT NULL,
	[Ceza] [bit] NOT NULL,
 CONSTRAINT [PK_EDevlet_RiskGrubu_Puan_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EDevlet_RiskGrubu_Puan_2]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EDevlet_RiskGrubu_Puan_2](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Risk_id] [int] NOT NULL,
	[Puan] [numeric](18, 2) NOT NULL,
	[Ceza] [bit] NOT NULL,
 CONSTRAINT [PK_EDevlet_RiskGrubu_Puan_2] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EDevlet_RiskGrubu_Puan_3]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EDevlet_RiskGrubu_Puan_3](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Risk_id] [int] NOT NULL,
	[Puan] [numeric](18, 2) NOT NULL,
	[Ceza] [bit] NOT NULL,
 CONSTRAINT [PK_EDevlet_RiskGrubu_Puan_3] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EDevlet_RiskGrubu_Yetkilendirme]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EDevlet_RiskGrubu_Yetkilendirme](
	[YetkiDepval] [varchar](10) NOT NULL,
	[RiskUrunGurup1] [bit] NOT NULL,
	[RiskUrunGurup2] [bit] NOT NULL,
	[RiskUrunGurup3] [bit] NOT NULL,
	[RiskUrunGurup4] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KrediPuan_Baslik]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KrediPuan_Baslik](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BASLIK] [nvarchar](50) NOT NULL,
	[ZORUNLU] [bit] NOT NULL,
 CONSTRAINT [PK_KrediPuan_Baslik] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KrediPuan_BaslikPuan]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KrediPuan_BaslikPuan](
	[BASLIKID] [int] NOT NULL,
	[PUAN] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KrediPuan_Islem]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KrediPuan_Islem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KIRILIMID] [int] NOT NULL,
	[ISLEMADI] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_KrediPuan_Islem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KrediPuan_IslemPuan]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KrediPuan_IslemPuan](
	[ISLEMID] [int] NOT NULL,
	[PUAN] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KrediPuan_Kirilim]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KrediPuan_Kirilim](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BASLIKID] [int] NOT NULL,
	[KIRILIMADI] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_KrediPuan_Kirilim] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KrediPuan_KirilmPuan]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KrediPuan_KirilmPuan](
	[KIRILIMID] [int] NOT NULL,
	[PUAN] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KrediPuan_RiskUrunGurup]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KrediPuan_RiskUrunGurup](
	[Risk_id] [int] NOT NULL,
	[VOLUID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KrediPuan_RiskUrunGurupPuan]    Script Date: 07/11/2023 16:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KrediPuan_RiskUrunGurupPuan](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Risk_Adi] [varchar](30) NULL,
	[Risk_TutarMin] [numeric](18, 2) NOT NULL,
	[Risk_TutarMax] [numeric](18, 2) NULL,
 CONSTRAINT [PK_KrediPuan_RiskUrunGurup] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[EDevlet_RiskGrubu_1] ON 

INSERT [dbo].[EDevlet_RiskGrubu_1] ([id], [Tanım], [Zorunlu]) VALUES (1, N'İcra', 1)
INSERT [dbo].[EDevlet_RiskGrubu_1] ([id], [Tanım], [Zorunlu]) VALUES (2, N'Ssk', 1)
INSERT [dbo].[EDevlet_RiskGrubu_1] ([id], [Tanım], [Zorunlu]) VALUES (3, N'Tapu Taşınmaz', 1)
INSERT [dbo].[EDevlet_RiskGrubu_1] ([id], [Tanım], [Zorunlu]) VALUES (4, N'Araç', 1)
INSERT [dbo].[EDevlet_RiskGrubu_1] ([id], [Tanım], [Zorunlu]) VALUES (5, N'Dava Bilgisi', 0)
INSERT [dbo].[EDevlet_RiskGrubu_1] ([id], [Tanım], [Zorunlu]) VALUES (6, N'Emekli Bilgisi', 0)
INSERT [dbo].[EDevlet_RiskGrubu_1] ([id], [Tanım], [Zorunlu]) VALUES (7, N'Adres Beyan E-Devlet', 0)
SET IDENTITY_INSERT [dbo].[EDevlet_RiskGrubu_1] OFF
GO
SET IDENTITY_INSERT [dbo].[EDevlet_RiskGrubu_2] ON 

INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (1, 1, N'Acik', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (2, 1, N'Kapalı', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (3, 1, N'Gizli', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (4, 2, N'Çalışıyor', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (5, 2, N'Çalışmıyor', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (6, 3, N'Mesken', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (7, 3, N'Taşınmaz', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (8, 3, N'Beyan', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (9, 4, N'Var', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (10, 4, N'Yok', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (11, 5, N'Var', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (12, 5, N'Yok', 0)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (13, 6, N'Var', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (14, 6, N'Yok', 0)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (15, 7, N'Var', 1)
INSERT [dbo].[EDevlet_RiskGrubu_2] ([id], [Parent_id], [Tanım], [Risk]) VALUES (16, 7, N'Yok', 0)
SET IDENTITY_INSERT [dbo].[EDevlet_RiskGrubu_2] OFF
GO
SET IDENTITY_INSERT [dbo].[EDevlet_RiskGrubu_3] ON 

INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (1, 1, N'Riskli', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (2, 1, N'Önemsiz', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (3, 2, N'Riskli', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (4, 2, N'Önemsiz', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (5, 3, N'Riskli', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (6, 3, N'Önemsiz', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (7, 4, N'', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (8, 4, N'', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (9, 4, N'', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (10, 4, N'', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (11, 4, N'', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (12, 5, N'Resmi Geliri Yok', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (13, 6, N'Var', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (14, 6, N'Yok', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (15, 7, N'Var', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (16, 7, N'Yok', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (17, 8, N'Var', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (18, 8, N'Yok', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (19, 9, N'Var', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (20, 10, N'Yok', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (21, 11, N'Var', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (22, 12, N'Yok', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (23, 13, N'Var', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (24, 14, N'Yok', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (25, 15, N'Var', 0)
INSERT [dbo].[EDevlet_RiskGrubu_3] ([id], [Parent_id], [Tanım], [Risk]) VALUES (26, 16, N'Yok', 0)
SET IDENTITY_INSERT [dbo].[EDevlet_RiskGrubu_3] OFF
GO
SET IDENTITY_INSERT [dbo].[EDevlet_RiskGrubu_Puan_1] ON 

INSERT [dbo].[EDevlet_RiskGrubu_Puan_1] ([id], [Risk_id], [Puan], [Ceza]) VALUES (1, 1, CAST(0.00 AS Numeric(18, 2)), 0)
INSERT [dbo].[EDevlet_RiskGrubu_Puan_1] ([id], [Risk_id], [Puan], [Ceza]) VALUES (2, 2, CAST(0.00 AS Numeric(18, 2)), 0)
INSERT [dbo].[EDevlet_RiskGrubu_Puan_1] ([id], [Risk_id], [Puan], [Ceza]) VALUES (3, 3, CAST(0.00 AS Numeric(18, 2)), 0)
INSERT [dbo].[EDevlet_RiskGrubu_Puan_1] ([id], [Risk_id], [Puan], [Ceza]) VALUES (4, 4, CAST(0.00 AS Numeric(18, 2)), 0)
INSERT [dbo].[EDevlet_RiskGrubu_Puan_1] ([id], [Risk_id], [Puan], [Ceza]) VALUES (5, 5, CAST(0.00 AS Numeric(18, 2)), 0)
INSERT [dbo].[EDevlet_RiskGrubu_Puan_1] ([id], [Risk_id], [Puan], [Ceza]) VALUES (6, 6, CAST(0.00 AS Numeric(18, 2)), 0)
INSERT [dbo].[EDevlet_RiskGrubu_Puan_1] ([id], [Risk_id], [Puan], [Ceza]) VALUES (7, 7, CAST(0.00 AS Numeric(18, 2)), 0)
SET IDENTITY_INSERT [dbo].[EDevlet_RiskGrubu_Puan_1] OFF
GO
SET IDENTITY_INSERT [dbo].[KrediPuan_Baslik] ON 

INSERT [dbo].[KrediPuan_Baslik] ([ID], [BASLIK], [ZORUNLU]) VALUES (1, N'Icra', 1)
INSERT [dbo].[KrediPuan_Baslik] ([ID], [BASLIK], [ZORUNLU]) VALUES (2, N'Ssk', 1)
INSERT [dbo].[KrediPuan_Baslik] ([ID], [BASLIK], [ZORUNLU]) VALUES (3, N'Tapu', 0)
INSERT [dbo].[KrediPuan_Baslik] ([ID], [BASLIK], [ZORUNLU]) VALUES (4, N'Arac', 0)
INSERT [dbo].[KrediPuan_Baslik] ([ID], [BASLIK], [ZORUNLU]) VALUES (5, N'Dava', 0)
INSERT [dbo].[KrediPuan_Baslik] ([ID], [BASLIK], [ZORUNLU]) VALUES (6, N'Emekli', 0)
INSERT [dbo].[KrediPuan_Baslik] ([ID], [BASLIK], [ZORUNLU]) VALUES (7, N'Adres', 0)
SET IDENTITY_INSERT [dbo].[KrediPuan_Baslik] OFF
GO
SET IDENTITY_INSERT [dbo].[KrediPuan_RiskUrunGurupPuan] ON 

INSERT [dbo].[KrediPuan_RiskUrunGurupPuan] ([id], [Risk_Adi], [Risk_TutarMin], [Risk_TutarMax]) VALUES (1, N'Düşük Risk Grubu Ürünler', CAST(40000.00 AS Numeric(18, 2)), CAST(40000.00 AS Numeric(18, 2)))
INSERT [dbo].[KrediPuan_RiskUrunGurupPuan] ([id], [Risk_Adi], [Risk_TutarMin], [Risk_TutarMax]) VALUES (2, N'Orta Risk Grubu', CAST(30000.00 AS Numeric(18, 2)), CAST(30000.00 AS Numeric(18, 2)))
INSERT [dbo].[KrediPuan_RiskUrunGurupPuan] ([id], [Risk_Adi], [Risk_TutarMin], [Risk_TutarMax]) VALUES (3, N'Yüksek Risk Grubu Ürünler', CAST(20000.00 AS Numeric(18, 2)), CAST(20000.00 AS Numeric(18, 2)))
INSERT [dbo].[KrediPuan_RiskUrunGurupPuan] ([id], [Risk_Adi], [Risk_TutarMin], [Risk_TutarMax]) VALUES (4, N'Özel Ürün Risk Grubu', CAST(20000.00 AS Numeric(18, 2)), CAST(100000.00 AS Numeric(18, 2)))
SET IDENTITY_INSERT [dbo].[KrediPuan_RiskUrunGurupPuan] OFF
GO
