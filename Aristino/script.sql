USE [master]
GO
/****** Object:  Database [AristinoDB]    Script Date: 8/29/2023 1:54:58 PM ******/
CREATE DATABASE [AristinoDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AristinoDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AristinoDB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AristinoDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AristinoDB_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [AristinoDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AristinoDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AristinoDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AristinoDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AristinoDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AristinoDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AristinoDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [AristinoDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [AristinoDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AristinoDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AristinoDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AristinoDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AristinoDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AristinoDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AristinoDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AristinoDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AristinoDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [AristinoDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AristinoDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AristinoDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AristinoDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AristinoDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AristinoDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AristinoDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AristinoDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AristinoDB] SET  MULTI_USER 
GO
ALTER DATABASE [AristinoDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AristinoDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AristinoDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AristinoDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AristinoDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AristinoDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AristinoDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [AristinoDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [AristinoDB]
GO
/****** Object:  Table [dbo].[AboutUs]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AboutUs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ShopAddress] [nvarchar](500) NULL,
	[City] [nvarchar](100) NULL,
	[Province] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
 CONSTRAINT [PK__AboutUs__3214EC27E85396A4] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Passwords] [nvarchar](100) NOT NULL,
	[RoleID] [int] NOT NULL,
	[AccountStatus] [bit] NOT NULL,
	[CreatedDate] [date] NULL,
	[Email] [nvarchar](100) NULL,
	[CustomerID] [int] NULL,
 CONSTRAINT [PK__Accounts__349DA586FC9DB95F] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AristinoPolicy]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AristinoPolicy](
	[PolicyID] [int] IDENTITY(1,1) NOT NULL,
	[PolicyTitle] [nvarchar](200) NULL,
	[PolicyContent] [nvarchar](max) NULL,
 CONSTRAINT [PK__Aristino__2E1339442946A942] PRIMARY KEY CLUSTERED 
(
	[PolicyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blog]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blog](
	[BlogId] [int] IDENTITY(1,1) NOT NULL,
	[BlogTitle] [nvarchar](200) NOT NULL,
	[BlogContent] [nvarchar](max) NOT NULL,
	[SourceName] [nvarchar](50) NULL,
	[SourceLink] [nvarchar](max) NULL,
	[PostDate] [nvarchar](50) NULL,
	[Thumbnail] [nvarchar](max) NULL,
	[BlogLike] [int] NULL,
	[BlogViews] [int] NULL,
	[CustomerId] [int] NULL,
 CONSTRAINT [PK__Blog__54379E3060FF1FCC] PRIMARY KEY CLUSTERED 
(
	[BlogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogComments]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogComments](
	[BlogCommentId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[Title] [nvarchar](50) NULL,
	[CommentContent] [nvarchar](max) NULL,
	[BlogId] [int] NULL,
	[CommentDate] [nvarchar](100) NULL,
 CONSTRAINT [PK__BlogComm__5F60E1D1DA8B8884] PRIMARY KEY CLUSTERED 
(
	[BlogCommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[CustomerId] [int] NULL,
	[TotalPrice] [nvarchar](max) NULL,
	[Quantity] [int] NULL,
	[Size] [nvarchar](10) NULL,
	[Color] [nvarchar](10) NULL,
 CONSTRAINT [PK__Cart__51BCD7B77F98446F] PRIMARY KEY CLUSTERED 
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
	[CategoryDescription] [nvarchar](50) NULL,
	[Avatar] [nvarchar](max) NULL,
 CONSTRAINT [PK__Categori__19093A2B10841C7E] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Color]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Color](
	[ColorID] [int] IDENTITY(1,1) NOT NULL,
	[ColorName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Color__8DA7676D085592C2] PRIMARY KEY CLUSTERED 
(
	[ColorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CommentedDate] [nvarchar](50) NOT NULL,
	[StarRating] [int] NULL,
	[CommentContent] [nvarchar](300) NULL,
	[ProductId] [int] NULL,
 CONSTRAINT [PK__Comments__C3B4DFCA6F26A0F8] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomersID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Dob] [date] NULL,
	[CustomerAddress] [nvarchar](100) NULL,
	[PhoneNumber] [int] NULL,
	[Email] [nvarchar](50) NULL,
	[CreatedDate] [date] NULL,
	[PostalCode] [int] NULL,
	[Country] [nvarchar](20) NULL,
	[Avatar] [nvarchar](max) NULL,
	[GenderId] [int] NULL,
	[StatusId] [int] NULL,
	[CardNumber] [nvarchar](16) NULL,
	[CardOwner] [nvarchar](50) NULL,
	[ExpiredDate] [nvarchar](50) NULL,
	[CVC] [int] NULL,
 CONSTRAINT [PK__Customer__EB5B581EB5DDC761] PRIMARY KEY CLUSTERED 
(
	[CustomersID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiscountBanners]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiscountBanners](
	[DiscountID] [int] IDENTITY(1,1) NOT NULL,
	[DiscountName] [nvarchar](100) NULL,
	[DiscountDescription] [nvarchar](max) NULL,
	[DiscountRate] [int] NULL,
	[Banner] [nvarchar](max) NULL,
	[CategoryID] [int] NULL,
	[StartSale] [nvarchar](100) NULL,
	[EndSale] [nvarchar](100) NULL,
	[DisableDiscount] [bit] NULL,
 CONSTRAINT [PK__Discount__E43F6DF64C427A65] PRIMARY KEY CLUSTERED 
(
	[DiscountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FashionCollection]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FashionCollection](
	[CollectionID] [int] IDENTITY(1,1) NOT NULL,
	[CollectionName] [nvarchar](200) NULL,
	[CollectionDes] [nvarchar](1000) NULL,
	[CollectionThumbnail] [nvarchar](max) NULL,
	[CollectionDisable] [bit] NULL,
 CONSTRAINT [PK__FashionC__7DE6BC24DB11F19B] PRIMARY KEY CLUSTERED 
(
	[CollectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gender]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gender](
	[GenderId] [int] IDENTITY(1,1) NOT NULL,
	[GenderName] [nvarchar](10) NULL,
 CONSTRAINT [PK__Gender__4E24E9F76ECA1771] PRIMARY KEY CLUSTERED 
(
	[GenderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MostSoldProduct]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MostSoldProduct](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[Sold] [int] NOT NULL,
	[isReset] [bit] NOT NULL,
 CONSTRAINT [PK__MostSold__3213E83FEA609FDC] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[OrderNumber] [nvarchar](100) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[PaymentID] [int] NOT NULL,
	[ShipDate] [datetime] NOT NULL,
	[TransacID] [int] NOT NULL,
	[isPaid] [bit] NOT NULL,
	[PaymentDate] [datetime] NULL,
	[OrderDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[TotalPrice] [int] NULL,
 CONSTRAINT [PK__Orders__C3905BAFA84BBBE7] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdersDetail]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersDetail](
	[OdersDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [int] NOT NULL,
	[Discount] [int] NOT NULL,
	[Size] [nvarchar](10) NOT NULL,
	[Color] [nvarchar](10) NOT NULL,
	[ProductPrice] [int] NULL,
 CONSTRAINT [PK__OrdersDe__E2D303FC3CF16DC8] PRIMARY KEY CLUSTERED 
(
	[OdersDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentName] [nvarchar](50) NOT NULL,
	[Allowed] [bit] NOT NULL,
 CONSTRAINT [PK__Payment__9B556A58480D1846] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[ProductDescription] [nvarchar](max) NOT NULL,
	[Price] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[ProductImage] [nvarchar](100) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Discount] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[ProductGallery] [nvarchar](max) NULL,
	[Color] [nvarchar](max) NULL,
	[Size] [nvarchar](max) NULL,
	[DiscountID] [int] NULL,
	[StarRate] [nvarchar](10) NULL,
	[ShortDes] [nvarchar](200) NULL,
	[CollectionID] [int] NULL,
	[ProductDiscontinue] [bit] NULL,
 CONSTRAINT [PK__Products__B40CC6EDC2DD9614] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Size]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Size](
	[SizeID] [int] IDENTITY(1,1) NOT NULL,
	[SizeName] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK__Size__83BD095A1DE77859] PRIMARY KEY CLUSTERED 
(
	[SizeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionStatus]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionStatus](
	[TransacID] [int] IDENTITY(1,1) NOT NULL,
	[TransacName] [nvarchar](100) NOT NULL,
	[TransacDes] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK__Transact__FDD99D59D72AD473] PRIMARY KEY CLUSTERED 
(
	[TransacID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK__UserRole__8AFACE3A692F83BF] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserStatus]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserStatus](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](20) NULL,
 CONSTRAINT [PK__UserStat__C8EE20633FBC5A4A] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 8/29/2023 1:54:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wishlist](
	[WishlistID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[ProductID] [int] NULL,
	[Color] [nvarchar](10) NULL,
	[Size] [nvarchar](10) NULL,
	[Quantity] [int] NULL,
 CONSTRAINT [PK__Wishlist__233189CB6ED5908A] PRIMARY KEY CLUSTERED 
(
	[WishlistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_AccountsCustomers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomersID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_AccountsCustomers]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_AccountsUserRole] FOREIGN KEY([RoleID])
REFERENCES [dbo].[UserRole] ([RoleID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_AccountsUserRole]
GO
ALTER TABLE [dbo].[Blog]  WITH CHECK ADD  CONSTRAINT [FK_BlogCustomers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomersID])
GO
ALTER TABLE [dbo].[Blog] CHECK CONSTRAINT [FK_BlogCustomers]
GO
ALTER TABLE [dbo].[BlogComments]  WITH CHECK ADD  CONSTRAINT [FK_BlogComments] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomersID])
GO
ALTER TABLE [dbo].[BlogComments] CHECK CONSTRAINT [FK_BlogComments]
GO
ALTER TABLE [dbo].[BlogComments]  WITH CHECK ADD  CONSTRAINT [FK_BlogCommentsBlog] FOREIGN KEY([BlogId])
REFERENCES [dbo].[Blog] ([BlogId])
GO
ALTER TABLE [dbo].[BlogComments] CHECK CONSTRAINT [FK_BlogCommentsBlog]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_CartCustomers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomersID])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_CartCustomers]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_CartProducts] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_CartProducts]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_CommentsCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomersID])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_CommentsCustomer]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_CommentsProducts] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_CommentsProducts]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_CustomersGender] FOREIGN KEY([GenderId])
REFERENCES [dbo].[Gender] ([GenderId])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_CustomersGender]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_CustomersUserStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[UserStatus] ([StatusId])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_CustomersUserStatus]
GO
ALTER TABLE [dbo].[DiscountBanners]  WITH CHECK ADD  CONSTRAINT [FK_DiscountBannersCategory] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[DiscountBanners] CHECK CONSTRAINT [FK_DiscountBannersCategory]
GO
ALTER TABLE [dbo].[MostSoldProduct]  WITH CHECK ADD  CONSTRAINT [FK_MostSoldProductProduct] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[MostSoldProduct] CHECK CONSTRAINT [FK_MostSoldProductProduct]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_OrdersCustomers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomersID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_OrdersCustomers]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_OrdersPayment] FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payment] ([PaymentID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_OrdersPayment]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_OrdersTransactionStatus] FOREIGN KEY([TransacID])
REFERENCES [dbo].[TransactionStatus] ([TransacID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_OrdersTransactionStatus]
GO
ALTER TABLE [dbo].[OrdersDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrdersDetailOrders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrdersDetail] CHECK CONSTRAINT [FK_OrdersDetailOrders]
GO
ALTER TABLE [dbo].[OrdersDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrdersDetailProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[OrdersDetail] CHECK CONSTRAINT [FK_OrdersDetailProducts]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_ProductsCategories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_ProductsCategories]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_ProductsDiscountBanner] FOREIGN KEY([DiscountID])
REFERENCES [dbo].[DiscountBanners] ([DiscountID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_ProductsDiscountBanner]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_ProductsFashionCollection] FOREIGN KEY([CollectionID])
REFERENCES [dbo].[FashionCollection] ([CollectionID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_ProductsFashionCollection]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_WishlistCustomers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomersID])
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_WishlistCustomers]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_WishlistProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_WishlistProducts]
GO
USE [master]
GO
ALTER DATABASE [AristinoDB] SET  READ_WRITE 
GO
