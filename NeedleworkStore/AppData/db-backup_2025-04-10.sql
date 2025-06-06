USE [master]
GO
/****** Object:  Database [NeedleworkStore]    Script Date: 10.04.2025 8:24:51 ******/
CREATE DATABASE [NeedleworkStore]
 CONTAINMENT = NONE 
ALTER DATABASE [NeedleworkStore] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NeedleworkStore].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NeedleworkStore] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NeedleworkStore] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NeedleworkStore] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NeedleworkStore] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NeedleworkStore] SET ARITHABORT OFF 
GO
ALTER DATABASE [NeedleworkStore] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NeedleworkStore] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NeedleworkStore] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NeedleworkStore] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NeedleworkStore] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NeedleworkStore] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NeedleworkStore] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NeedleworkStore] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NeedleworkStore] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NeedleworkStore] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NeedleworkStore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NeedleworkStore] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NeedleworkStore] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NeedleworkStore] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NeedleworkStore] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NeedleworkStore] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NeedleworkStore] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NeedleworkStore] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [NeedleworkStore] SET  MULTI_USER 
GO
ALTER DATABASE [NeedleworkStore] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NeedleworkStore] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NeedleworkStore] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NeedleworkStore] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NeedleworkStore] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NeedleworkStore] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [NeedleworkStore] SET QUERY_STORE = ON
GO
ALTER DATABASE [NeedleworkStore] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [NeedleworkStore]
GO
/****** Object:  Table [dbo].[AccessoryTypes]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessoryTypes](
	[AccessoryTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AccessoryTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AccessoryTypes] PRIMARY KEY CLUSTERED 
(
	[AccessoryTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssigningStatuses]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssigningStatuses](
	[AssigningStatusID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentStatusID] [int] NOT NULL,
	[ReceivingStatusID] [int] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[OrderID] [int] NOT NULL,
	[ProcessingStatusID] [int] NOT NULL,
 CONSTRAINT [PK_AssigningStatuses] PRIMARY KEY CLUSTERED 
(
	[AssigningStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AvailabilityStatuses]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvailabilityStatuses](
	[AvailabilityStatusID] [int] IDENTITY(1,1) NOT NULL,
	[AvailabilityStatus] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AvailabilityStatuses] PRIMARY KEY CLUSTERED 
(
	[AvailabilityStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carts]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carts](
	[CartID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[QuantityCart] [int] NOT NULL,
	[FormationDate] [datetime] NULL,
 CONSTRAINT [PK_Carts] PRIMARY KEY CLUSTERED 
(
	[CartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[CityID] [int] IDENTITY(1,1) NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Designers]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designers](
	[DesignerID] [int] IDENTITY(1,1) NOT NULL,
	[DesignerName] [nvarchar](50) NOT NULL,
	[CountryID] [int] NOT NULL,
 CONSTRAINT [PK_Designers] PRIMARY KEY CLUSTERED 
(
	[DesignerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Favourities]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Favourities](
	[FavouriteID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[FormationDate] [datetime] NULL,
 CONSTRAINT [PK_Favourities] PRIMARY KEY CLUSTERED 
(
	[FavouriteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NeedleworkTypes]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NeedleworkTypes](
	[NeedleworkTypeID] [int] IDENTITY(1,1) NOT NULL,
	[NeedleworkTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_NeedleworkTypes] PRIMARY KEY CLUSTERED 
(
	[NeedleworkTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderCompositions]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderCompositions](
	[OrderCompositionID] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[OrderID] [int] NOT NULL,
	[OrderPrice] [int] NOT NULL,
 CONSTRAINT [PK_OrderCompositions] PRIMARY KEY CLUSTERED 
(
	[OrderCompositionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[PickUpPointID] [int] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentStatuses]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentStatuses](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentStatus] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PickUpPoints]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PickUpPoints](
	[PickUpPointID] [int] IDENTITY(1,1) NOT NULL,
	[Adress] [nvarchar](70) NULL,
	[CityID] [int] NOT NULL,
 CONSTRAINT [PK_PickUpPoints] PRIMARY KEY CLUSTERED 
(
	[PickUpPointID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcessingStatuses]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessingStatuses](
	[ProcessingStatusID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessingStatus] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ProcessingStatuses] PRIMARY KEY CLUSTERED 
(
	[ProcessingStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductAccessoryTypes]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductAccessoryTypes](
	[ProductAccessoryTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[AccessoryTypeID] [int] NOT NULL,
 CONSTRAINT [PK_ProductAccessoryTypes] PRIMARY KEY CLUSTERED 
(
	[ProductAccessoryTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductNeedleworkTypes]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductNeedleworkTypes](
	[ProductNeedleworkTypesID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[NeedleworkTypeID] [int] NOT NULL,
 CONSTRAINT [PK_ProductNeedleworkTypes] PRIMARY KEY CLUSTERED 
(
	[ProductNeedleworkTypesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](50) NULL,
	[ProductPrice] [int] NULL,
	[Description] [nvarchar](2000) NULL,
	[ProductImage] [nvarchar](50) NULL,
	[DesignerID] [int] NOT NULL,
	[ProductTypeID] [int] NOT NULL,
	[AvailabilityStatusID] [int] NOT NULL,
	[QRLink] [nvarchar](300) NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductStitchTypes]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductStitchTypes](
	[ProductStitchTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[StitchTypeID] [int] NOT NULL,
 CONSTRAINT [PK_ProductStitchTypes] PRIMARY KEY CLUSTERED 
(
	[ProductStitchTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductThemes]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductThemes](
	[ProductThemeID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[ThemeID] [int] NOT NULL,
 CONSTRAINT [PK_ProductThemes] PRIMARY KEY CLUSTERED 
(
	[ProductThemeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductTypes]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductTypes](
	[ProductTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ProductTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ProductTypes] PRIMARY KEY CLUSTERED 
(
	[ProductTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReceivingStatuses]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceivingStatuses](
	[ReceivingStatusID] [int] IDENTITY(1,1) NOT NULL,
	[ReceivingStatus] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ReceivingStatuses] PRIMARY KEY CLUSTERED 
(
	[ReceivingStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[ReviewID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](500) NULL,
	[Rating] [int] NOT NULL,
	[Image] [nvarchar](50) NULL,
	[ReviewDate] [datetime] NULL,
	[ProductID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_Reviews] PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StitchTypes]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StitchTypes](
	[StitchTypeID] [int] IDENTITY(1,1) NOT NULL,
	[StitchTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_StitchTypes] PRIMARY KEY CLUSTERED 
(
	[StitchTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Themes]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Themes](
	[ThemeID] [int] IDENTITY(1,1) NOT NULL,
	[ThemeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Themes] PRIMARY KEY CLUSTERED 
(
	[ThemeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10.04.2025 8:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[UserLastname] [nvarchar](50) NULL,
	[UserPatronymic] [nvarchar](50) NULL,
	[Birthday] [date] NULL,
	[UserPhone] [nvarchar](50) NOT NULL,
	[UserEmail] [nvarchar](50) NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AccessoryTypes] ON 

INSERT [dbo].[AccessoryTypes] ([AccessoryTypeID], [AccessoryTypeName]) VALUES (1, N'Ножницы')
INSERT [dbo].[AccessoryTypes] ([AccessoryTypeID], [AccessoryTypeName]) VALUES (2, N'Иглы')
INSERT [dbo].[AccessoryTypes] ([AccessoryTypeID], [AccessoryTypeName]) VALUES (3, N'Игольницы')
SET IDENTITY_INSERT [dbo].[AccessoryTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[AvailabilityStatuses] ON 

INSERT [dbo].[AvailabilityStatuses] ([AvailabilityStatusID], [AvailabilityStatus]) VALUES (1, N'есть в наличии')
INSERT [dbo].[AvailabilityStatuses] ([AvailabilityStatusID], [AvailabilityStatus]) VALUES (2, N'нет в наличии')
SET IDENTITY_INSERT [dbo].[AvailabilityStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[Cities] ON 

INSERT [dbo].[Cities] ([CityID], [CityName]) VALUES (1, N'Москва')
INSERT [dbo].[Cities] ([CityID], [CityName]) VALUES (2, N'Санкт-Петербург')
INSERT [dbo].[Cities] ([CityID], [CityName]) VALUES (3, N'Уфа')
INSERT [dbo].[Cities] ([CityID], [CityName]) VALUES (4, N'Казань')
INSERT [dbo].[Cities] ([CityID], [CityName]) VALUES (5, N'Самара')
SET IDENTITY_INSERT [dbo].[Cities] OFF
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (1, N'США')
INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (2, N'Россия')
INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (3, N'Индия')
INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (4, N'Китай')
INSERT [dbo].[Countries] ([CountryID], [CountryName]) VALUES (5, N'Германия')
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Designers] ON 

INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (1, N'Dimensions', 1)
INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (2, N'Gamma', 4)
INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (3, N'Prym', 5)
INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (4, N'Pony', 3)
INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (5, N'Owl Forest', 2)
INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (6, N'Miadolla', 2)
INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (7, N'Mill Hill', 1)
INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (8, N'М.П.Студия', 2)
INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (9, N'PANNA', 2)
INSERT [dbo].[Designers] ([DesignerID], [DesignerName], [CountryID]) VALUES (10, N'Mirabilia', 1)
SET IDENTITY_INSERT [dbo].[Designers] OFF
GO
SET IDENTITY_INSERT [dbo].[NeedleworkTypes] ON 

INSERT [dbo].[NeedleworkTypes] ([NeedleworkTypeID], [NeedleworkTypeName]) VALUES (1, N'Вышивка')
INSERT [dbo].[NeedleworkTypes] ([NeedleworkTypeID], [NeedleworkTypeName]) VALUES (2, N'Шитье')
SET IDENTITY_INSERT [dbo].[NeedleworkTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentStatuses] ON 

INSERT [dbo].[PaymentStatuses] ([PaymentID], [PaymentStatus]) VALUES (1, N'Оплачен')
INSERT [dbo].[PaymentStatuses] ([PaymentID], [PaymentStatus]) VALUES (2, N'Не оплачен')
SET IDENTITY_INSERT [dbo].[PaymentStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[PickUpPoints] ON 

INSERT [dbo].[PickUpPoints] ([PickUpPointID], [Adress], [CityID]) VALUES (1, N'Улица Зеленая, д. 15', 1)
INSERT [dbo].[PickUpPoints] ([PickUpPointID], [Adress], [CityID]) VALUES (2, N'Проспект Центральный, д. 123, оф. 90', 1)
INSERT [dbo].[PickUpPoints] ([PickUpPointID], [Adress], [CityID]) VALUES (3, N'Бульвар Мирный, д. 8', 2)
INSERT [dbo].[PickUpPoints] ([PickUpPointID], [Adress], [CityID]) VALUES (4, N'Переулок Тихий, д. 19', 2)
INSERT [dbo].[PickUpPoints] ([PickUpPointID], [Adress], [CityID]) VALUES (5, N'Набережная Речная, д. 45', 3)
INSERT [dbo].[PickUpPoints] ([PickUpPointID], [Adress], [CityID]) VALUES (6, N'Площадь Памятная, д. 21, оф. 18', 4)
INSERT [dbo].[PickUpPoints] ([PickUpPointID], [Adress], [CityID]) VALUES (7, N'Улица Лесная, д. 38', 5)
INSERT [dbo].[PickUpPoints] ([PickUpPointID], [Adress], [CityID]) VALUES (8, N'Проспект Молодежный, д. 95, оф. 48', 1)
INSERT [dbo].[PickUpPoints] ([PickUpPointID], [Adress], [CityID]) VALUES (9, N'Улица Сиреневая, д. 27', 2)
SET IDENTITY_INSERT [dbo].[PickUpPoints] OFF
GO
SET IDENTITY_INSERT [dbo].[ProcessingStatuses] ON 

INSERT [dbo].[ProcessingStatuses] ([ProcessingStatusID], [ProcessingStatus]) VALUES (1, N'Оформлен')
INSERT [dbo].[ProcessingStatuses] ([ProcessingStatusID], [ProcessingStatus]) VALUES (2, N'На сборке')
INSERT [dbo].[ProcessingStatuses] ([ProcessingStatusID], [ProcessingStatus]) VALUES (3, N'Отправлен')
INSERT [dbo].[ProcessingStatuses] ([ProcessingStatusID], [ProcessingStatus]) VALUES (4, N'Доставлен')
SET IDENTITY_INSERT [dbo].[ProcessingStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductAccessoryTypes] ON 

INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (1, 11, 1)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (2, 12, 1)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (3, 13, 1)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (4, 14, 1)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (5, 15, 1)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (6, 16, 2)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (7, 17, 2)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (8, 18, 2)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (9, 19, 2)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (10, 20, 2)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (11, 21, 3)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (12, 22, 3)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (13, 23, 3)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (14, 24, 3)
INSERT [dbo].[ProductAccessoryTypes] ([ProductAccessoryTypeID], [ProductID], [AccessoryTypeID]) VALUES (15, 25, 3)
SET IDENTITY_INSERT [dbo].[ProductAccessoryTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductNeedleworkTypes] ON 

INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (1, 1, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (2, 2, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (3, 3, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (4, 4, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (5, 5, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (6, 6, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (7, 7, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (8, 8, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (9, 9, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (10, 10, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (11, 11, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (12, 12, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (13, 13, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (14, 13, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (15, 14, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (16, 14, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (17, 15, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (18, 15, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (19, 16, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (20, 17, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (21, 17, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (22, 18, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (23, 18, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (24, 19, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (25, 19, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (26, 20, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (27, 20, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (28, 21, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (29, 21, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (30, 22, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (31, 22, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (32, 23, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (33, 23, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (34, 24, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (35, 24, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (36, 25, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (37, 25, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (38, 26, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (39, 27, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (40, 28, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (41, 29, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (42, 30, 2)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (43, 31, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (44, 32, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (45, 33, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (46, 34, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (47, 35, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (48, 36, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (49, 37, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (50, 38, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (51, 39, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (52, 40, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (53, 41, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (54, 42, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (55, 43, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (56, 44, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (57, 45, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (58, 46, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (59, 47, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (60, 48, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (61, 49, 1)
INSERT [dbo].[ProductNeedleworkTypes] ([ProductNeedleworkTypesID], [ProductID], [NeedleworkTypeID]) VALUES (62, 50, 1)
SET IDENTITY_INSERT [dbo].[ProductNeedleworkTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (1, N'Dockside', 1969, N'Набор из серии Gold Collection Petites. В состав набора входят: хлопковое мулине Dimensions, аида 18 белого цвета, игла, схема.', N'01_Dim.jpg', 1, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (2, N'Blissful Moments', 4999, N'Набор из серии Gold Collection. В состав набора входят: хлопковое мулине Dimensions, аида 18 цвета слоновой кости, игла, схема, инструкция.', N'02_Dim.jpg', 1, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (3, N'In The Shadows', 1891, N'В состав набора входят: хлопковое мулине Dimensions, аида 18 черного цвета, игла, схема.', N'03_Dim.jpg', 1, 1, 1, N'dimensions/65219&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (4, N'Joyful Floral', 1575, N'В состав набора входят: хлопковое мулине Dimensions, аида 14 темно-синего цвета, игла, схема.', N'04_Dim.jpg', 1, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (5, N'Golden Ride', 3499, N'В состав набора входят: хлопковое мулине Dimensions, аида 14 цвета слоновой кости, игла, схема.', N'05_Dim.jpg', 1, 1, 1, N'dimensions/35405&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (6, N'Blooms Flower Shop', 3599, N'В состав набора входят: хлопковое мулине Dimensions, аида 14 белого цвета, игла, схема.', N'06_Dim.jpg', 1, 1, 1, N'dimensions/35401&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (7, N'Festive Ride', 3459, N'В состав набора входят: хлопковое мулине Dimensions, аида 14 светло-голубого цвета, игла, схема.', N'07_Dim.jpg', 1, 1, 1, N'dimensions/08992&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (8, N'The Finery Of Nature', 6299, N'В состав набора входят: хлопковое мулине Dimensions, аида 14 черного цвета, игла, схема.', N'08_Dim.jpg', 1, 1, 1, N'dimensions/03824&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (9, N'Wind Moon Fairy', 6799, N'В состав набора входят: хлопковое мулине Dimensions, аида 16 серого цвета, игла, схема.', N'09_Dim.jpg', 1, 1, 1, N'dimensions/35393&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (10, N'Holiday Village', 5849, N'Набор из серии Gold Collection. В набор дополнительно входят пайетки для украшения деревьев и домов.', N'10_Dim.jpg', 1, 1, 1, N'dimensions/08783&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (11, N'Ножницы закройные в блистере 185 мм', 379, N'Используются для раскроя ткани. Металлические с пластиковыми термостойкими ручками, которые не плавятся при соприкосновении с утюгом. Длина лезвия позволяет кроить мелкие детали. Длина лезвия удобна для кроя по прямой. Применяются для кроя плательной, костюмной, пальтовой групп тканей, утеплителей. Ножницы изготовлены из инструментальной стали марки 30x13. Сталь содержит хром , который препятствует образованию ржавчины, и углерод , который придает инструменту дополнительную прочность. Кончики легко разрезают материал, не заминая край; режущие поверхности сходятся идеально.', N'11_Gamma.jpg', 2, 2, 2, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (12, N'Ножницы закройные в блистере 228 мм', 469, N'Используются для раскроя ткани. Металлические с пластиковыми термостойкими ручками, которые не плавятся при соприкосновении с утюгом. Длина лезвия позволяет кроить мелкие детали. Длина лезвия удобна для кроя по прямой. Применяются для кроя плательной, костюмной, пальтовой групп тканей, утеплителей. Ножницы изготовлены из инструментальной стали марки 30x13. Сталь содержит хром , который препятствует образованию ржавчины, и углерод , который придает инструменту дополнительную прочность. Кончики легко разрезают материал, не заминая край; режущие поверхности сходятся идеально.', N'12_Gamma.jpg', 2, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (13, N'Ножницы для вышивки', 933, N'Длина ножниц - 10 см', N'13_Prym.jpg', 3, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (14, N'Ножницы для рукоделия', 1851, N'Длина ножниц - 16,5 см', N'14_Prym.jpg', 3, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (15, N'Ножницы Premium VIT-11 для рукоделия в блистере', 999, N'Изящные, тонкие ножнички VIT-11 для рукоделия с ручками в форме цапелек. Длина 11,3 см', N'15_Gamma.jpg', 2, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (16, N'Иглы для бисера №11', 91, N'Цветные, 6 штук в упаковке', N'16_Pony.jpg', 4, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (17, N'Иглы для синели, глади №22-24-26', 114, N'Цветные, 6 штук в упаковке', N'17_Pony.jpg', 4, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (18, N'Иглы двусторонние с закругленным острием №24', 218, N'В наборе 6 игл', N'18_Gamma.jpg', 2, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (19, N'Иглы гобеленовые острые №26', 87, N'В конверте 25 игл', N'19_Gamma.jpg', 2, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (20, N'Иглы гобеленовые с закругленным острием №26', 91, N'В конверте 25 игл', N'20_Gamma.jpg', 2, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (21, N'Вращающаяся игольница', 810, N'Иглы в комплект не входят', N'21_Prym.jpg', 3, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (22, N'Магнитный держатель Коник', 398, N'Диаметр 25 мм', N'22_OwlForest.jpg', 5, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (23, N'Магнитный держатель Красная смородина', 398, N'Диаметр 25 мм', N'23_OwlForest.jpg', 5, 2, 2, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (24, N'Магнитный держатель Лисички', 398, N'Диаметр 25 мм', N'24_OwlForest.jpg', 5, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (25, N'Магнитный держатель Сапожки', 398, N'Диаметр 25 мм', N'25_OwlForest.jpg', 5, 2, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (26, N'Арома Скат', 438, N'Игрушка-скат из натурального льна с вышивкой станет отличным украшением интерьера. В набор входят ароматические гранулы с ароматом ПЕРСИКА, благодаря чему готовую игрушку можно использовать как саше', N'26_Miadolla.jpg', 6, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (27, N'Олененок Элли', 1413, N'Нитки мулине "Gamma", Швейная игла "Gamma", Нитки швейные "Gamma", Синтепух "Gamma", Подробная инструкция с картинками, Выкройка, Лен, Искусственная замша, Рога из полимерной глины ручной работы, Пряжа (полиэстер), Нос пластиковый с шайбой', N'27_Miadolla.jpg', 6, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (28, N'Олененок Зоуи', 1323, N'Швейная игла "Gamma", Ткань 100% хлопок, Нитки мулине "Gamma", Нитки швейные "Gamma", Синтепух "Gamma", Подробная инструкция с картинками, Выкройка, Длинная игла, Фланель, Ткань вискоза, Пуговицы', N'28_Miadolla.jpg', 6, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (29, N'Мама Кошка и котенок', 1093, N'Нитки мулине "Gamma", Швейная игла "Gamma", Нитки швейные "Gamma", Синтепух "Gamma", Лен, Искусственная замша, Ткань 100% хлопок', N'29_Miadolla.jpg', 6, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (30, N'Лесной олененок', 365, N'Фетровые игрушки с удовольствием станут жить на вашем рюкзаке или ключах в качестве брелока или броши. Также игрушки можно использовать для украшения новогодней елки или собрать из них мобиль для детской кроватки', N'30_Miadolla.jpg', 6, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (31, N'Гирлянда', 854, N'MILL HILL единственный производитель вышивальных наборов на перфорированной бумаге.  Наборы на бумажной основе стирке не подлежат', N'31_MillHill.jpg', 7, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (32, N'Гном с колокольчиком', 1540, N'MILL HILL единственный производитель вышивальных наборов на перфорированной бумаге.  Наборы на бумажной основе стирке не подлежат', N'32_MillHill.jpg', 7, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (33, N'Свитер', 1330, N'MILL HILL единственный производитель вышивальных наборов на перфорированной бумаге.  Наборы на бумажной основе стирке не подлежат', N'33_MillHill.jpg', 7, 1, 1, N'mill_hill/MH18-2431&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (34, N'Нарцисс', 2240, N'MILL HILL единственный производитель вышивальных наборов на перфорированной бумаге.  Наборы на бумажной основе стирке не подлежат', N'34_MillHill.jpg', 7, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (35, N'В лес', 2240, N'MILL HILL единственный производитель вышивальных наборов на перфорированной бумаге.  Наборы на бумажной основе стирке не подлежат', N'35_MillHill.jpg', 7, 1, 1, N'mill_hill/MH14-2326&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (36, N'Подсолнухи в ящичке', 891, N'14*9*6 см (деревянная основа)', N'36_MPStudia.jpg', 8, 1, 1, N'mpstudiya/О-099&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (37, N'Лейка с тюльпанами', 1181, N'14*12*6 см (деревянная основа)', N'37_MPStudia.jpg', 8, 1, 1, N'mpstudiya/О-100&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (38, N'Мухоморчики', 698, N'12*13 см (деревянная основа)', N'38_MPStudia.jpg', 8, 1, 2, N'mpstudiya/О-074&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (39, N'Романтичная эхеверия', 679, N'10 х 9 х 5 (деревянная основа)', N'39_MPStudia.jpg', 8, 1, 1, N'mpstudiya/О-076&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (40, N'Пасхальная корзинка', 592, N'14*13 см (деревянная основа)', N'40_MPStudia.jpg', 8, 1, 1, N'mpstudiya/О-077&gallery')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (41, N'Увидеть Дубай', 989, N'Вышивка комбинируется с элементами аппликации. Особая деталь - объемные волосы у героини', N'41_Panna.jpg', 9, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (42, N'Оберег Лада', 1199, N'Эту вышивку  можно вышить на ткани и оформить в рамку или использовать только водорастворимую основу, чтобы украсить одежду, сумку, полотенце, наволочку или обложку', N'42_Panna.jpg', 9, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (43, N'Новолетие', 1247, N'Эту вышивку  можно вышить на ткани и оформить в рамку или использовать только водорастворимую основу, чтобы украсить одежду, сумку, полотенце, наволочку или обложку', N'43_Panna.jpg', 9, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (44, N'Брошь. Спящий котик', 339, N'4.5*4.5 см', N'44_Panna.jpg', 9, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (45, N'Осьминог', 339, N'Эту вышивку  можно вышить на ткани и оформить в рамку или использовать только водорастворимую основу, чтобы украсить одежду, сумку, полотенце, наволочку или обложку', N'45_Panna.jpg', 9, 1, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (46, N'Офелия', 1456, N'Только схема', N'46_Mirabilia.jpg', 10, 3, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (47, N'Зима', 1256, N'Только схема', N'47_Mirabilia.jpg', 10, 3, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (48, N'Рапунцель', 2520, N'Только схема', N'48_Mirabilia.jpg', 10, 3, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (49, N'Садовая прелюдия', 1781, N'Только схема', N'49_Mirabilia.jpg', 10, 3, 1, N'')
INSERT [dbo].[Products] ([ProductID], [ProductName], [ProductPrice], [Description], [ProductImage], [DesignerID], [ProductTypeID], [AvailabilityStatusID], [QRLink]) VALUES (50, N'Мисс Рождество', 2120, N'Только схема', N'50_Mirabilia.jpg', 10, 3, 1, N'')
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductStitchTypes] ON 

INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (1, 1, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (2, 2, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (3, 3, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (4, 4, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (5, 5, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (6, 6, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (7, 7, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (8, 8, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (9, 9, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (10, 10, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (11, 16, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (12, 17, 3)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (13, 31, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (14, 31, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (15, 32, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (16, 32, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (17, 33, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (18, 33, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (19, 34, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (20, 34, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (21, 35, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (22, 35, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (23, 36, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (24, 37, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (25, 38, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (26, 39, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (27, 40, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (28, 41, 3)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (29, 42, 3)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (30, 43, 3)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (31, 44, 3)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (32, 45, 3)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (33, 46, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (34, 47, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (35, 48, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (36, 49, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (37, 50, 1)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (38, 46, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (39, 47, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (40, 48, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (41, 49, 2)
INSERT [dbo].[ProductStitchTypes] ([ProductStitchTypeID], [ProductID], [StitchTypeID]) VALUES (42, 50, 2)
SET IDENTITY_INSERT [dbo].[ProductStitchTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductThemes] ON 

INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (1, 1, 1)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (2, 1, 2)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (3, 1, 3)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (4, 2, 2)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (5, 2, 3)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (6, 2, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (7, 2, 5)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (8, 2, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (9, 2, 7)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (10, 3, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (11, 4, 5)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (12, 5, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (13, 5, 7)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (14, 6, 3)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (15, 6, 5)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (16, 7, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (17, 7, 7)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (18, 7, 8)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (19, 7, 9)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (20, 8, 2)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (21, 8, 5)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (22, 9, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (23, 9, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (24, 10, 3)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (25, 10, 8)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (26, 10, 9)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (27, 22, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (28, 23, 10)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (29, 24, 10)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (30, 26, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (31, 27, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (32, 28, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (33, 29, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (34, 30, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (35, 30, 8)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (36, 26, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (37, 27, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (38, 28, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (39, 29, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (40, 30, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (41, 31, 9)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (42, 31, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (43, 32, 9)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (44, 32, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (45, 33, 9)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (46, 33, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (47, 32, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (48, 33, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (49, 34, 5)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (50, 35, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (51, 36, 5)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (52, 37, 5)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (53, 39, 5)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (54, 38, 10)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (55, 40, 9)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (56, 36, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (57, 37, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (58, 38, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (59, 39, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (60, 40, 11)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (61, 41, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (62, 42, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (63, 43, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (64, 43, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (65, 44, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (66, 45, 4)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (67, 46, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (68, 47, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (69, 48, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (70, 49, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (71, 50, 6)
INSERT [dbo].[ProductThemes] ([ProductThemeID], [ProductID], [ThemeID]) VALUES (72, 50, 9)
SET IDENTITY_INSERT [dbo].[ProductThemes] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductTypes] ON 

INSERT [dbo].[ProductTypes] ([ProductTypeID], [ProductTypeName]) VALUES (1, N'Набор')
INSERT [dbo].[ProductTypes] ([ProductTypeID], [ProductTypeName]) VALUES (2, N'Аксессуар')
INSERT [dbo].[ProductTypes] ([ProductTypeID], [ProductTypeName]) VALUES (3, N'Печатное издание')
SET IDENTITY_INSERT [dbo].[ProductTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ReceivingStatuses] ON 

INSERT [dbo].[ReceivingStatuses] ([ReceivingStatusID], [ReceivingStatus]) VALUES (1, N'Получен')
INSERT [dbo].[ReceivingStatuses] ([ReceivingStatusID], [ReceivingStatus]) VALUES (2, N'Не получен')
SET IDENTITY_INSERT [dbo].[ReceivingStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Администратор')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Клиент')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (3, N'Гость')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[StitchTypes] ON 

INSERT [dbo].[StitchTypes] ([StitchTypeID], [StitchTypeName]) VALUES (1, N'крестик')
INSERT [dbo].[StitchTypes] ([StitchTypeID], [StitchTypeName]) VALUES (2, N'бисер')
INSERT [dbo].[StitchTypes] ([StitchTypeID], [StitchTypeName]) VALUES (3, N'гладь')
SET IDENTITY_INSERT [dbo].[StitchTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Themes] ON 

INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (1, N'Море')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (2, N'Птицы')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (3, N'Дома')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (4, N'Животные')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (5, N'Цветы')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (6, N'Люди')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (7, N'Машины')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (8, N'Зима')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (9, N'Праздники')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (10, N'Природа')
INSERT [dbo].[Themes] ([ThemeID], [ThemeName]) VALUES (11, N'Игрушки')
SET IDENTITY_INSERT [dbo].[Themes] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Login], [Password], [UserName], [UserLastname], [UserPatronymic], [Birthday], [UserPhone], [UserEmail], [RoleID]) VALUES (1, N'mar', N'Mar1Mar1', N'Мария', N'Петрова', N'Сергеевна', NULL, N'+7 (888) 987-65-43', N'mar@yandex.ru', 1)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [UserName], [UserLastname], [UserPatronymic], [Birthday], [UserPhone], [UserEmail], [RoleID]) VALUES (2, N'olga', N'Olga1Olga1', N'Ольга', N'Кузнецова', N'Дмитриевна', NULL, N'+7 (777) 543-21-09', N'olga@yandex.ru', 2)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [UserName], [UserLastname], [UserPatronymic], [Birthday], [UserPhone], [UserEmail], [RoleID]) VALUES (3, N'anna', N'Anna1Anna1', N'Анна', N'Васильева', N'Петровна', NULL, N'+7 (555) 321-09-87', N'anna@yandex.ru', 2)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [UserName], [UserLastname], [UserPatronymic], [Birthday], [UserPhone], [UserEmail], [RoleID]) VALUES (4, N'e.mih', N'Emih1Emih1', N'Екатерина', N'Семенова', N'Михайловна', NULL, N'+7 (555) 678-91-23', N'emih@yandex.ru', 2)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [UserName], [UserLastname], [UserPatronymic], [Birthday], [UserPhone], [UserEmail], [RoleID]) VALUES (5, N'nat', N'Nat1Nat1', N'Наталья', N'Козлова', N'Игоревна', NULL, N'+7 (777) 456-78-90', N'nat@yandex.ru', 2)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[AssigningStatuses] ADD  CONSTRAINT [DF_AssigningStatuses_PaymentStatusID]  DEFAULT ((1)) FOR [PaymentStatusID]
GO
ALTER TABLE [dbo].[AssigningStatuses] ADD  CONSTRAINT [DF_AssigningStatuses_ReceivingStatusID]  DEFAULT ((1)) FOR [ReceivingStatusID]
GO
ALTER TABLE [dbo].[AssigningStatuses] ADD  CONSTRAINT [DF_AssigningStatuses_ProcessingStatusID]  DEFAULT ((1)) FOR [ProcessingStatusID]
GO
ALTER TABLE [dbo].[Carts] ADD  CONSTRAINT [DF_Carts_QuantityCart]  DEFAULT ((1)) FOR [QuantityCart]
GO
ALTER TABLE [dbo].[OrderCompositions] ADD  CONSTRAINT [DF_OrderCompositions_Quantity]  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Reviews] ADD  CONSTRAINT [DF_Reviews_Rating]  DEFAULT ((5)) FOR [Rating]
GO
ALTER TABLE [dbo].[AssigningStatuses]  WITH CHECK ADD  CONSTRAINT [FK_AssigningStatuses_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[AssigningStatuses] CHECK CONSTRAINT [FK_AssigningStatuses_Orders]
GO
ALTER TABLE [dbo].[AssigningStatuses]  WITH CHECK ADD  CONSTRAINT [FK_AssigningStatuses_PaymentStatuses] FOREIGN KEY([PaymentStatusID])
REFERENCES [dbo].[PaymentStatuses] ([PaymentID])
GO
ALTER TABLE [dbo].[AssigningStatuses] CHECK CONSTRAINT [FK_AssigningStatuses_PaymentStatuses]
GO
ALTER TABLE [dbo].[AssigningStatuses]  WITH CHECK ADD  CONSTRAINT [FK_AssigningStatuses_ProcessingStatuses] FOREIGN KEY([ProcessingStatusID])
REFERENCES [dbo].[ProcessingStatuses] ([ProcessingStatusID])
GO
ALTER TABLE [dbo].[AssigningStatuses] CHECK CONSTRAINT [FK_AssigningStatuses_ProcessingStatuses]
GO
ALTER TABLE [dbo].[AssigningStatuses]  WITH CHECK ADD  CONSTRAINT [FK_AssigningStatuses_ReceivingStatuses] FOREIGN KEY([ReceivingStatusID])
REFERENCES [dbo].[ReceivingStatuses] ([ReceivingStatusID])
GO
ALTER TABLE [dbo].[AssigningStatuses] CHECK CONSTRAINT [FK_AssigningStatuses_ReceivingStatuses]
GO
ALTER TABLE [dbo].[Carts]  WITH CHECK ADD  CONSTRAINT [FK_Carts_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Carts] CHECK CONSTRAINT [FK_Carts_Products]
GO
ALTER TABLE [dbo].[Carts]  WITH CHECK ADD  CONSTRAINT [FK_Carts_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Carts] CHECK CONSTRAINT [FK_Carts_Users]
GO
ALTER TABLE [dbo].[Designers]  WITH CHECK ADD  CONSTRAINT [FK_Designers_Countries] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([CountryID])
GO
ALTER TABLE [dbo].[Designers] CHECK CONSTRAINT [FK_Designers_Countries]
GO
ALTER TABLE [dbo].[Favourities]  WITH CHECK ADD  CONSTRAINT [FK_Favourities_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Favourities] CHECK CONSTRAINT [FK_Favourities_Products]
GO
ALTER TABLE [dbo].[Favourities]  WITH CHECK ADD  CONSTRAINT [FK_Favourities_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Favourities] CHECK CONSTRAINT [FK_Favourities_Users]
GO
ALTER TABLE [dbo].[OrderCompositions]  WITH CHECK ADD  CONSTRAINT [FK_OrderCompositions_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderCompositions] CHECK CONSTRAINT [FK_OrderCompositions_Orders]
GO
ALTER TABLE [dbo].[OrderCompositions]  WITH CHECK ADD  CONSTRAINT [FK_OrderCompositions_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[OrderCompositions] CHECK CONSTRAINT [FK_OrderCompositions_Products]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_PickUpPoints] FOREIGN KEY([PickUpPointID])
REFERENCES [dbo].[PickUpPoints] ([PickUpPointID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_PickUpPoints]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO
ALTER TABLE [dbo].[PickUpPoints]  WITH CHECK ADD  CONSTRAINT [FK_PickUpPoints_Cities] FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[PickUpPoints] CHECK CONSTRAINT [FK_PickUpPoints_Cities]
GO
ALTER TABLE [dbo].[ProductAccessoryTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProductAccessoryTypes_AccessoryTypes] FOREIGN KEY([AccessoryTypeID])
REFERENCES [dbo].[AccessoryTypes] ([AccessoryTypeID])
GO
ALTER TABLE [dbo].[ProductAccessoryTypes] CHECK CONSTRAINT [FK_ProductAccessoryTypes_AccessoryTypes]
GO
ALTER TABLE [dbo].[ProductAccessoryTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProductAccessoryTypes_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[ProductAccessoryTypes] CHECK CONSTRAINT [FK_ProductAccessoryTypes_Products]
GO
ALTER TABLE [dbo].[ProductNeedleworkTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProductNeedleworkTypes_NeedleworkTypes] FOREIGN KEY([NeedleworkTypeID])
REFERENCES [dbo].[NeedleworkTypes] ([NeedleworkTypeID])
GO
ALTER TABLE [dbo].[ProductNeedleworkTypes] CHECK CONSTRAINT [FK_ProductNeedleworkTypes_NeedleworkTypes]
GO
ALTER TABLE [dbo].[ProductNeedleworkTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProductNeedleworkTypes_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[ProductNeedleworkTypes] CHECK CONSTRAINT [FK_ProductNeedleworkTypes_Products]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_AvailabilityStatuses] FOREIGN KEY([AvailabilityStatusID])
REFERENCES [dbo].[AvailabilityStatuses] ([AvailabilityStatusID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_AvailabilityStatuses]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Designers] FOREIGN KEY([DesignerID])
REFERENCES [dbo].[Designers] ([DesignerID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Designers]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_ProductTypes] FOREIGN KEY([ProductTypeID])
REFERENCES [dbo].[ProductTypes] ([ProductTypeID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_ProductTypes]
GO
ALTER TABLE [dbo].[ProductStitchTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProductStitchTypes_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[ProductStitchTypes] CHECK CONSTRAINT [FK_ProductStitchTypes_Products]
GO
ALTER TABLE [dbo].[ProductStitchTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProductStitchTypes_StitchTypes] FOREIGN KEY([StitchTypeID])
REFERENCES [dbo].[StitchTypes] ([StitchTypeID])
GO
ALTER TABLE [dbo].[ProductStitchTypes] CHECK CONSTRAINT [FK_ProductStitchTypes_StitchTypes]
GO
ALTER TABLE [dbo].[ProductThemes]  WITH CHECK ADD  CONSTRAINT [FK_ProductThemes_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[ProductThemes] CHECK CONSTRAINT [FK_ProductThemes_Products]
GO
ALTER TABLE [dbo].[ProductThemes]  WITH CHECK ADD  CONSTRAINT [FK_ProductThemes_Themes] FOREIGN KEY([ThemeID])
REFERENCES [dbo].[Themes] ([ThemeID])
GO
ALTER TABLE [dbo].[ProductThemes] CHECK CONSTRAINT [FK_ProductThemes_Themes]
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [FK_Reviews_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_Reviews_Products]
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [FK_Reviews_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_Reviews_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
USE [master]
GO
ALTER DATABASE [NeedleworkStore] SET  READ_WRITE 
GO
