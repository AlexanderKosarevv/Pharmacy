USE [KosarevKursovikApteka]
GO
/****** Object:  Table [dbo].[Drug]    Script Date: 22.12.2023 3:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Drug](
	[IDDrug] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Price] [decimal](10, 2) NULL,
	[Manufacturer] [nvarchar](max) NULL,
	[IDRecipe] [int] NULL,
	[IDType] [int] NULL,
 CONSTRAINT [PK_Drug] PRIMARY KEY CLUSTERED 
(
	[IDDrug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 22.12.2023 3:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[IDEmployee] [int] IDENTITY(1,1) NOT NULL,
	[Surname] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Patronymic] [nvarchar](50) NULL,
	[IDSex] [int] NULL,
	[Birthdate] [date] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[IDEmployee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipe]    Script Date: 22.12.2023 3:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipe](
	[IDRecipe] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Recipe] PRIMARY KEY CLUSTERED 
(
	[IDRecipe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 22.12.2023 3:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RollID] [int] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RollID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sale]    Script Date: 22.12.2023 3:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sale](
	[IDSale] [int] IDENTITY(1,1) NOT NULL,
	[IDDrug] [int] NULL,
	[Quantity] [int] NULL,
	[IDEmployee] [int] NULL,
 CONSTRAINT [PK_Sale] PRIMARY KEY CLUSTERED 
(
	[IDSale] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sexes]    Script Date: 22.12.2023 3:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sexes](
	[IDSex] [int] NOT NULL,
	[Name] [nvarchar](10) NULL,
 CONSTRAINT [PK_Sexes] PRIMARY KEY CLUSTERED 
(
	[IDSex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Type]    Script Date: 22.12.2023 3:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Type](
	[IDType] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED 
(
	[IDType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 22.12.2023 3:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[RollID] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Drug] ON 

INSERT [dbo].[Drug] ([IDDrug], [Name], [Price], [Manufacturer], [IDRecipe], [IDType]) VALUES (1, N'Аскорбиновая кислота', CAST(46.00 AS Decimal(10, 2)), N'Фармстандарт-УфаВИТА, Россия', 2, 2)
INSERT [dbo].[Drug] ([IDDrug], [Name], [Price], [Manufacturer], [IDRecipe], [IDType]) VALUES (2, N'Эргоферон', CAST(522.00 AS Decimal(10, 2)), N'Материа Медика Холдинг, Россия', 2, 1)
INSERT [dbo].[Drug] ([IDDrug], [Name], [Price], [Manufacturer], [IDRecipe], [IDType]) VALUES (3, N'Сульпирид', CAST(86.00 AS Decimal(10, 2)), N'ОРГАНИКА, АО, Россия', 1, 1)
INSERT [dbo].[Drug] ([IDDrug], [Name], [Price], [Manufacturer], [IDRecipe], [IDType]) VALUES (4, N'Мезапам', CAST(1195.00 AS Decimal(10, 2)), N'ОРГАНИКА, АО, Россия', 1, 1)
INSERT [dbo].[Drug] ([IDDrug], [Name], [Price], [Manufacturer], [IDRecipe], [IDType]) VALUES (5, N'Налоксон', CAST(237.00 AS Decimal(10, 2)), N'Синтез, Польша', 1, 6)
SET IDENTITY_INSERT [dbo].[Drug] OFF
GO
SET IDENTITY_INSERT [dbo].[Employee] ON 

INSERT [dbo].[Employee] ([IDEmployee], [Surname], [Name], [Patronymic], [IDSex], [Birthdate]) VALUES (1, N'Косарев', N'Александр', N'Юрьевич', 1, CAST(N'2003-05-14' AS Date))
INSERT [dbo].[Employee] ([IDEmployee], [Surname], [Name], [Patronymic], [IDSex], [Birthdate]) VALUES (2, N'Литвинова', N'Эмилия ', N'Егоровна', 2, CAST(N'1986-07-22' AS Date))
INSERT [dbo].[Employee] ([IDEmployee], [Surname], [Name], [Patronymic], [IDSex], [Birthdate]) VALUES (3, N'Орлов', N'Егор', N'Глебович', 1, CAST(N'1990-07-04' AS Date))
INSERT [dbo].[Employee] ([IDEmployee], [Surname], [Name], [Patronymic], [IDSex], [Birthdate]) VALUES (4, N'Пономарева', N'Анна', N'Александровна', 2, CAST(N'2000-09-21' AS Date))
SET IDENTITY_INSERT [dbo].[Employee] OFF
GO
SET IDENTITY_INSERT [dbo].[Recipe] ON 

INSERT [dbo].[Recipe] ([IDRecipe], [Name]) VALUES (1, N'Да')
INSERT [dbo].[Recipe] ([IDRecipe], [Name]) VALUES (2, N'Нет')
SET IDENTITY_INSERT [dbo].[Recipe] OFF
GO
INSERT [dbo].[Roles] ([RollID], [RoleName]) VALUES (0, N'Администратор')
INSERT [dbo].[Roles] ([RollID], [RoleName]) VALUES (1, N'Пользователь')
INSERT [dbo].[Roles] ([RollID], [RoleName]) VALUES (2, N'Гость')
GO
SET IDENTITY_INSERT [dbo].[Sale] ON 

INSERT [dbo].[Sale] ([IDSale], [IDDrug], [Quantity], [IDEmployee]) VALUES (-1, NULL, NULL, NULL)
INSERT [dbo].[Sale] ([IDSale], [IDDrug], [Quantity], [IDEmployee]) VALUES (1, 1, 3, 1)
INSERT [dbo].[Sale] ([IDSale], [IDDrug], [Quantity], [IDEmployee]) VALUES (2, 2, 4, 2)
INSERT [dbo].[Sale] ([IDSale], [IDDrug], [Quantity], [IDEmployee]) VALUES (3, 5, 5, 4)
INSERT [dbo].[Sale] ([IDSale], [IDDrug], [Quantity], [IDEmployee]) VALUES (4, 3, 12, 1)
INSERT [dbo].[Sale] ([IDSale], [IDDrug], [Quantity], [IDEmployee]) VALUES (5, 4, 1, 3)
SET IDENTITY_INSERT [dbo].[Sale] OFF
GO
INSERT [dbo].[Sexes] ([IDSex], [Name]) VALUES (-1, N'...')
INSERT [dbo].[Sexes] ([IDSex], [Name]) VALUES (1, N'Мужчина')
INSERT [dbo].[Sexes] ([IDSex], [Name]) VALUES (2, N'Женщина')
GO
SET IDENTITY_INSERT [dbo].[Type] ON 

INSERT [dbo].[Type] ([IDType], [Name]) VALUES (1, N'Таблетка')
INSERT [dbo].[Type] ([IDType], [Name]) VALUES (2, N'Порошок')
INSERT [dbo].[Type] ([IDType], [Name]) VALUES (3, N'Капсула')
INSERT [dbo].[Type] ([IDType], [Name]) VALUES (4, N'Мазь')
INSERT [dbo].[Type] ([IDType], [Name]) VALUES (5, N'Крем')
INSERT [dbo].[Type] ([IDType], [Name]) VALUES (6, N'Раствор')
INSERT [dbo].[Type] ([IDType], [Name]) VALUES (7, N'Сироп')
INSERT [dbo].[Type] ([IDType], [Name]) VALUES (8, N'Капли')
INSERT [dbo].[Type] ([IDType], [Name]) VALUES (9, N'Аэрозоль')
SET IDENTITY_INSERT [dbo].[Type] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Login], [Password], [RollID]) VALUES (0, N'admin', N'admin', 0)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [RollID]) VALUES (1, N'test1', N'test1', 1)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [RollID]) VALUES (2, N'admin1', N'admin1', 2)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [RollID]) VALUES (3, N'dantess', N'danyadanya', 1)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [RollID]) VALUES (4, N'123', N'123', 1)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [RollID]) VALUES (5, N'zaza', N'zaza', 1)
INSERT [dbo].[Users] ([UserID], [Login], [Password], [RollID]) VALUES (6, N'sasa', N'sasa', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Drug]  WITH CHECK ADD  CONSTRAINT [FK_Drug_Recipe] FOREIGN KEY([IDRecipe])
REFERENCES [dbo].[Recipe] ([IDRecipe])
GO
ALTER TABLE [dbo].[Drug] CHECK CONSTRAINT [FK_Drug_Recipe]
GO
ALTER TABLE [dbo].[Drug]  WITH CHECK ADD  CONSTRAINT [FK_Drug_Type] FOREIGN KEY([IDType])
REFERENCES [dbo].[Type] ([IDType])
GO
ALTER TABLE [dbo].[Drug] CHECK CONSTRAINT [FK_Drug_Type]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Sexes] FOREIGN KEY([IDSex])
REFERENCES [dbo].[Sexes] ([IDSex])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Sexes]
GO
ALTER TABLE [dbo].[Sale]  WITH CHECK ADD  CONSTRAINT [FK_Sale_Drug] FOREIGN KEY([IDDrug])
REFERENCES [dbo].[Drug] ([IDDrug])
GO
ALTER TABLE [dbo].[Sale] CHECK CONSTRAINT [FK_Sale_Drug]
GO
ALTER TABLE [dbo].[Sale]  WITH CHECK ADD  CONSTRAINT [FK_Sale_Employee] FOREIGN KEY([IDEmployee])
REFERENCES [dbo].[Employee] ([IDEmployee])
GO
ALTER TABLE [dbo].[Sale] CHECK CONSTRAINT [FK_Sale_Employee]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RollID])
REFERENCES [dbo].[Roles] ([RollID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
