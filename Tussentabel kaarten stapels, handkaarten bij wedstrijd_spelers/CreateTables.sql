/****** Create schema Munchkin ******/
CREATE SCHEMA "Munchkin"

/****** Create table kaarten ******/

CREATE TABLE [Munchkin].[Kaarten](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](255) NOT NULL,
	[Beschrijving] [nvarchar](500) NOT NULL,
	[Afbeelding] [nvarchar](255) NOT NULL,
	[Type_id] [int] NOT NULL,
	[Eenmalig] [bit] NULL,
	[Wanneer_Bruikbaar] [nvarchar](255) NULL,
 CONSTRAINT [PK_Kaarten] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table kaarten_Stapels ******/

CREATE TABLE [Munchkin].[Kaarten_Stapels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Kaart_Id] [int] NOT NULL,
	[Stapel_Id] [int] NOT NULL,
 CONSTRAINT [PK_Kaarten_Stapels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Create table Kerkerkaarten ******/

CREATE TABLE [Munchkin].[Kerkerkaarten](
	[Id] [int] NOT NULL,
	[Beschrijving_2] [nvarchar](255) NULL,
	[Portie_Ellende] [nvarchar](255) NULL,
	[Level] [int] NULL,
	[Aantal_schatten] [int] NULL,
	[Ondood] [bit] NULL,
	[Aantal_Levels] [int] NULL,
	[Tijdelijke_Bonus] [int] DEFAULT 0,
 CONSTRAINT [PK_Kerkerkaarten] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table Schatkaarten ******/

CREATE TABLE [Munchkin].[Schatkaarten](
	[Id] [int] NOT NULL,
	[Schatkaart_Waarde] [int] NULL,
	[Is_Groot] [bit] NULL,
 CONSTRAINT [PK_Schatkaarten] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table Types ******/

CREATE TABLE [Munchkin].[Types](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Soort] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Types] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table Kaart_Bonussen ******/

CREATE TABLE [Munchkin].[Kaart_Bonussen](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Kaart_Id] [int] NOT NULL,
	[Bonus_Id] [int] NOT NULL,
 CONSTRAINT [PK_Kaart_Bonussen] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table Bonussen ******/

CREATE TABLE [Munchkin].[Bonussen](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Niet_Bruikbaar_Door] [nvarchar](255) NOT NULL,
	[Bruikbaar_Door] [nvarchar](255) NOT NULL,
	[Waarde] [int] NOT NULL,
	[Waarop_Effect] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Bonussen] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table Stapels ******/

CREATE TABLE [Munchkin].[Stapels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Stapels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table Spelers ******/

CREATE TABLE [Munchkin].[Spelers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](255) NOT NULL,
	[Geslacht] [char](1) NOT NULL,
 CONSTRAINT [PK_Spelers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table Wedstrijd_Spelers ******/

CREATE TABLE [Munchkin].[Wedstrijd_Spelers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Speler_Id] [int] NOT NULL,
	[Wedstrijd_Id] [int] NULL,
	[Handkaarten_Id] [int] NOT NULL,
	[Veldkaarten_Id] [int] NOT NULL,
	[Level] [int] DEFAULT 1,
	[Ras] [nvarchar](50) DEFAULT 'Mens',
	[Vluchtbonus] [int] DEFAULT 0,
	[Gevechtsbonus] [int] DEFAULT 0,
	[Tijdelijke_Bonus] [int] DEFAULT 0,
 CONSTRAINT [PK_Wedstrijd_Spelers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table Wedstrijden ******/

CREATE TABLE [Munchkin].[Wedstrijden](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Winnaar_Id] [int] NULL,
	[Toernooi_Id] [int] NULL,
	[Kerkerkaarten_Trekstapel_Id] [int] NOT NULL,
	[Kerkerkaarten_Aflegstapel_Id] [int] NOT NULL,
	[Schatkaarten_Trekstapel_Id] [int] NOT NULL,
	[Schatkaarten_Aflegstapel_Id] [int] NOT NULL,
 CONSTRAINT [PK_Wedstrijden] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Create table Toernooien ******/

CREATE TABLE [Munchkin].[Toernooien](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](255) NOT NULL,
	[Datum] [datetime] NULL,
	[Winnaar_Id] [int] NOT NULL,
 CONSTRAINT [PK_Toernooien] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** foreign key van Kaarten naar Kaart_Bonussen ******/

ALTER TABLE [Munchkin].[Kaart_Bonussen]  WITH CHECK ADD  CONSTRAINT [FK_Kaart_Id] FOREIGN KEY([Kaart_Id])
REFERENCES [Munchkin].[Kaarten] ([Id])
GO

ALTER TABLE [Munchkin].[Kaart_Bonussen] CHECK CONSTRAINT [FK_Kaart_Id]
GO


/****** foreign key van Bonussen naar Kaart_Bonussen ******/

ALTER TABLE [Munchkin].[Kaart_Bonussen]  WITH CHECK ADD  CONSTRAINT [FK_Bonus_Id] FOREIGN KEY([Bonus_Id])
REFERENCES [Munchkin].[Bonussen] ([Id])
GO

ALTER TABLE [Munchkin].[Kaart_Bonussen] CHECK CONSTRAINT [FK_Bonus_Id]
GO

/****** foreign key van Types naar Kaarten ******/

ALTER TABLE [Munchkin].[Kaarten]  WITH CHECK ADD  CONSTRAINT [FK_Type_Id] FOREIGN KEY([Type_Id])
REFERENCES [Munchkin].[Types] ([Id])
GO

ALTER TABLE [Munchkin].[Kaarten] CHECK CONSTRAINT [FK_Type_Id]
GO

/****** foreign key van kaarten naar Kerkerkaarten ******/

ALTER TABLE [Munchkin].[Kerkerkaarten]  WITH CHECK ADD  CONSTRAINT [FK_Kerkerkaart_Id] FOREIGN KEY([Id])
REFERENCES [Munchkin].[Kaarten] ([Id])
GO

ALTER TABLE [Munchkin].[Kerkerkaarten] CHECK CONSTRAINT [FK_Kerkerkaart_Id]
GO

/****** foreign key van kaarten naar Schatkaarten ******/

ALTER TABLE [Munchkin].[Schatkaarten]  WITH CHECK ADD  CONSTRAINT [FK_Schatkaart_Id] FOREIGN KEY([Id])
REFERENCES [Munchkin].[Kaarten] ([Id])
GO

ALTER TABLE [Munchkin].[Schatkaarten] CHECK CONSTRAINT [FK_Schatkaart_Id]
GO

/****** foreign key van Stapels naar Kaarten_Stapels ******/

ALTER TABLE [Munchkin].[Kaarten_Stapels]  WITH CHECK ADD  CONSTRAINT [FK_Stapel_Id_Kaarten_Stapels] FOREIGN KEY([Stapel_Id])
REFERENCES [Munchkin].[Stapels] ([Id])
GO

ALTER TABLE [Munchkin].[Kaarten_Stapels] CHECK CONSTRAINT [FK_Stapel_Id_Kaarten_Stapels]
GO

/****** foreign key van Kaarten naar Kaarten_Stapels ******/

ALTER TABLE [Munchkin].[Kaarten_Stapels]  WITH CHECK ADD  CONSTRAINT [FK_Kaart_Id_Kaarten_Stapels] FOREIGN KEY([Kaart_Id])
REFERENCES [Munchkin].[Kaarten] ([Id])
GO

ALTER TABLE [Munchkin].[Kaarten_Stapels] CHECK CONSTRAINT [FK_Stapel_Id_Kaarten_Stapels]
GO

/****** foreign key van Spelers naar Wedstrijd_Spelers ******/

ALTER TABLE [Munchkin].[Wedstrijd_Spelers]  WITH CHECK ADD  CONSTRAINT [FK_Speler_Id] FOREIGN KEY([Speler_Id])
REFERENCES [Munchkin].[Spelers] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijd_Spelers] CHECK CONSTRAINT [FK_Speler_Id]
GO

/****** foreign key van Wedstrijden naar Wedstrijd_Spelers ******/

ALTER TABLE [Munchkin].[Wedstrijd_Spelers]  WITH CHECK ADD  CONSTRAINT [FK_Wedstrijd_Id] FOREIGN KEY([Wedstrijd_Id])
REFERENCES [Munchkin].[Wedstrijden] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijd_Spelers] CHECK CONSTRAINT [FK_Wedstrijd_Id]
GO

/****** foreign key van Spelers naar Wedstrijden ******/

ALTER TABLE [Munchkin].[Wedstrijden]  WITH CHECK ADD  CONSTRAINT [FK_Winnaar_Id_Wedstrijd] FOREIGN KEY([Winnaar_Id])
REFERENCES [Munchkin].[Spelers] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijden] CHECK CONSTRAINT [FK_Winnaar_Id_Wedstrijd]
GO

/****** foreign key van Spelers naar Toernooien ******/

ALTER TABLE [Munchkin].[Toernooien]  WITH CHECK ADD  CONSTRAINT [FK_Winnaar_Id_Toernooi] FOREIGN KEY([Winnaar_Id])
REFERENCES [Munchkin].[Spelers] ([Id])
GO

ALTER TABLE [Munchkin].[Toernooien] CHECK CONSTRAINT [FK_Winnaar_Id_Toernooi]
GO

/****** foreign key van Toernooien naar Wedstrijden ******/

ALTER TABLE [Munchkin].[Wedstrijden]  WITH CHECK ADD  CONSTRAINT [FK_Toernooi_Id] FOREIGN KEY([Toernooi_Id])
REFERENCES [Munchkin].[Toernooien] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijden] CHECK CONSTRAINT [FK_Toernooi_Id]
GO

/****** foreign key van stapels naar wedstrijd_spelers (handkaarten) ******/

ALTER TABLE [Munchkin].[Wedstrijd_Spelers]  WITH CHECK ADD  CONSTRAINT [FK_Handkaarten_Id] FOREIGN KEY([Handkaarten_Id])
REFERENCES [Munchkin].[Stapels] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijd_Spelers] CHECK CONSTRAINT [FK_Handkaarten_Id]
GO

/****** foreign key van stapels naar wedstrijd_spelers (veldkaarten) ******/

ALTER TABLE [Munchkin].[Wedstrijd_Spelers]  WITH CHECK ADD  CONSTRAINT [FK_Veldkaarten_Id] FOREIGN KEY([Veldkaarten_Id])
REFERENCES [Munchkin].[Stapels] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijd_Spelers] CHECK CONSTRAINT [FK_Veldkaarten_Id]
GO


/****** foreign key van stapels naar wedstrijden (Kerkerkaarten_Trekstapel_Id) ******/

ALTER TABLE [Munchkin].[Wedstrijden]  WITH CHECK ADD  CONSTRAINT [FK_Kerkerkaarten_Trekstapel_Id] FOREIGN KEY([Kerkerkaarten_Trekstapel_Id])
REFERENCES [Munchkin].[Stapels] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijden] CHECK CONSTRAINT [FK_Kerkerkaarten_Trekstapel_Id]
GO

/****** foreign key van stapels naar wedstrijden (Kerkerkaarten_Aflegstapel_Id) ******/

ALTER TABLE [Munchkin].[Wedstrijden]  WITH CHECK ADD  CONSTRAINT [FK_Kerkerkaarten_Aflegstapel_Id] FOREIGN KEY([Kerkerkaarten_Aflegstapel_Id])
REFERENCES [Munchkin].[Stapels] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijden] CHECK CONSTRAINT [FK_Kerkerkaarten_Aflegstapel_Id]
GO

/****** foreign key van stapels naar wedstrijden (Schatkaarten_Trekstapel_Id) ******/

ALTER TABLE [Munchkin].[Wedstrijden]  WITH CHECK ADD  CONSTRAINT [FK_Schatkaarten_Trekstapel_Id] FOREIGN KEY([Schatkaarten_Trekstapel_Id])
REFERENCES [Munchkin].[Stapels] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijden] CHECK CONSTRAINT [FK_Schatkaarten_Trekstapel_Id]
GO

/****** foreign key van stapels naar wedstrijden (Schatkaarten_Aflegstapel_Id) ******/

ALTER TABLE [Munchkin].[Wedstrijden]  WITH CHECK ADD  CONSTRAINT [FK_Schatkaarten_Aflegstapel_Id] FOREIGN KEY([Schatkaarten_Aflegstapel_Id])
REFERENCES [Munchkin].[Stapels] ([Id])
GO

ALTER TABLE [Munchkin].[Wedstrijden] CHECK CONSTRAINT [FK_Schatkaarten_Aflegstapel_Id]
GO


/****** Insert data types ******/


SET IDENTITY_INSERT [Munchkin].[Types] ON 

INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (1, N'Ras')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (2, N'Klasse')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (3, N'Monster')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (4, N'Vervloeking')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (5, N'Gebruikskaarten')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (6, N'Hoofddeksel')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (7, N'1Hand')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (8, N'2Handen')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (9, N'Schoeisel')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (10, N'Harnas')
INSERT [Munchkin].[Types] ([Id], [Soort]) VALUES (11, N'Extra')
SET IDENTITY_INSERT [Munchkin].[Types] OFF

/****** Insert data Kaarten ******/

SET IDENTITY_INSERT [Munchkin].[Kaarten] ON 

INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (1, N'Lamme Goblin', N'+1 voor vluchten', N'images/Lamme_goblin.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (4, N'Ondood Paard', N'+5 tegen dwergen;', N'images/Ondood_paard.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (6, N'Wietplant', N'Elven trekken een extra schatkaart na het verslaan.', N'images/Wietplant.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (8, N'Paddos', N'-1 voor vluchten', N'images/Paddos.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (11, N'Plutonium Draak', N'Achtervolgt niemand van level 5 of lager', N'images/Plutonium_draak.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (12, N'Magere Hein', N'Als je moet vluchten verlies je 1 level, ook als je ontsnapt.', N'images/Magere_hein.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (15, N'Koning Toet', N'Achtervolgt niemand van level 3 of lager. Karakters van hogere levels verliezen 2 levels, zelfs als ze ontsnappen;', N'images/Koning_toet.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (20, N'Schaamluis', N'Kun je niet voor vluchten;', N'images/Schaamluis.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (23, N'Kwijlend Slijm', N'Glibberig slijm!  +4 tegen elven', N'images/Kwijlend_slijm.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (24, N'Glitterende Vampieren', N'Voorwerpen of andere bonussen helpen hier niet tegen. Bevecht ze alleen met je persoonlijke level', N'images/Glitterende_vampieren.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (25, N'Platvoet', N'+3 tegen dwergen en halflings.', N'images/Platvoet.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (28, N'Kwelrog', N'Achtervolgt niemand van level 4 of lager.', N'images/Kwelrog.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (29, N'Achtzijdige Drilpudding', N'+1 voor vluchten.', N'images/Achtzijdige_drilpudding.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (33, N'Gebroeders Wight', N'Achtervolgen niemand van level 3 of lager. Karakters van hogere levels verliezen 2 levels, zelfs als ze ontsnappen.', N'images/Gebroeders_wight.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (35, N'Snoetzuiger', N'Het is gruwelijk! +6 tegen Elven.', N'images/Snoetzuiger.png', 3, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (43, N'Verlies 1 level', N'Verlies 1 level', N'images/Verlies_1_level_2.png', 4, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (45, N'Pekingeend', N'Je zou beter moeten weten dan een eend in een kerker op te pakken. Je verliest 2 levels.', N'images/Pekingeend.png', 4, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (46, N'Verlies het hoofddeksel dat je draagt', N'Verlies het hoofddeksel dat je draagt.', N'images/Verlies_het_hoofddeksel_dat_je_draagt.png', 4, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (47, N'Verlies_1_level', N'Verlies 1 level.', N'images/Verlies_1_level_1.png', 4, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (48, N'Verlies het harnas dat je draagt', N'Verlies het harnas dat je draagt.', N'images/Verlies_het_harnas_dat_je_draagt.png', 4, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (52, N'Verlies je ras', N'Leg je raskaart(en) af en word mens.', N'images/Verlies_je_ras.png', 4, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (54, N'Verlies het schoeisel dat je draagt', N'Verlies het voorwerp dat je draagt.', N'images/Verlies_het_schoeisel_dat_je_draagt.png', 4, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (58, N'Dwerg', N'Je kunt een onbeperkt aantal grote voorwerpen dragen.', N'images/Dwerg_1.png', 1, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (60, N'Dwerg', N'Je kunt een onbeperkt aantal grote voorwerpen dragen.', N'images/Dwerg_2.png', 1, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (61, N'Dwerg', N'Je kunt een onbeperkt aantal grote voorwerpen dragen.', N'images/Dwerg_3.png', 1, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (62, N'Elf', N'+1 voor vluchten.', N'images/Elf_1.png', 1, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (63, N'Elf', N'+1 voor vluchten', N'images/Elf_2.png', 1, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (64, N'Elf', N'+1 voor vluchten.', N'images/Elf_3.png', 1, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (65, N'Halfling', N'Je mag elke beurt 1 voorwerp verkopen voor de dubbele prijs (extra voorwerpen gaan voor de normale prijs).', N'images/Halfling_1.png', 1, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (66, N'Halfling', N'Je mag elke beurt 1 voorwerp verkopen voor de dubbele prijs (extra voorwerpen gaan voor de normale prijs).', N'images/Halfling_2.png', 1, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (67, N'Halfling', N'Je mag elke beurt 1 voorwerp verkopen voor de dubbele prijs (extra voorwerpen gaan voor de normale prijs).', N'images/Halfling_3.png', 1, NULL, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (86, N'Oeroud', N'Speel tijdens een gevecht. Als het monster wordt verslagen, trek dan 2 extra schatkaarten.', N'images/Oeroud.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (95, N'Intelligent', N'Speel tijdens een gevecht. Als het monster wordt verslagen, trek dan 1 extra schatkaart.', N'images/Intelligent.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (96, N'Laaiend', N'Speel tijdens een gevecht. Als het monster wordt verslagen, trek dan 1 extra schatkaart.', N'images/Laaiend.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (97, N'Reusachtig', N'Speel tijdens een gevecht. Als het monster wordt verslagen, trek dan 2 extra schatkaarten.', N'images/Reusachtig.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (98, N'Baby', N'Speel tijdens een gevecht. Als het monster wordt verslagen, trek dan 1 schatkaart minder (minimum is 1).', N'images/Baby.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (99, N'Koop De Spelleider Om Met Snacks', N'Stijg een Level', N'images/Koop_De_Spelleider_Om_Met_Snacks.png', 5, 1, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (101, N'Regelneuker', N'Stijg een Level', N'images/Regelneuker.png', 5, 1, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (102, N'Kook een Mierenhoop', N'Stijg een Level', N'images/Kook_Een_Mierenhoop.png', 5, 1, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (103, N'Hete Hengsten Drank', N'Stijg een Level', N'images/Hete_Hengsten_Drank.png', 5, 1, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (106, N'Creatief Boekhouden', N'Stijg een Level', N'images/Creatief_Boekhouden.png', 5, 1, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (107, N'Zeuren bij de Spelleider', N'Je mag dit niet gebruiken als je op dit moment de speler met het hoogste level bent of gezamenlijk het hoogste bent. Stijg een Level', N'images/Zeuren_Bij_De_Spelleider.png', 5, 1, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (110, N'1000 Goudstukken', N'Stijg een Level', N'images/1000_Goudstukken.png', 5, 1, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (112, N'Elektrisch Radioactief Zuur Drankje', N'Gebruik in een gevecht. +5 voor één van beide kanten. Eenmalig bruikbaar.', N'images/Elektrisch_Radioactief_Zuur_Drankje.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (115, N'Waanzinnig Dapperheidsdrankje', N'Gebruik in een gevecht. +2 voor één van beide kanten. Eenmalig bruikbaar.', N'images/Waanzinnig_Dapperheidsdrankje.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (116, N'Slaapdrankje', N'Gebruik in een gevecht. +2 voor één van beide kanten. Eenmalig bruikbaar.', N'images/Slaapdrankje.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (117, N'Ijzingwekkend Explosief Drankje', N'Gebruik in een gevecht. +3 voor één van beide kanten. Eenmalig bruikbaar.', N'images/Ijzingwekkend_Explosief_Drankje.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (121, N'Walgelijk Sportdrankje', N'Gebruik in een gevecht. +2 voor één van beide kanten. Eenmalig bruikbaar.', N'images/Walgelijk_Sportdrankje.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (128, N'Vrankje van Derwarring', N'Gebruik in een gevecht. +3 voor één van beide kanten. Eenmalig bruikbaar.', N'images/Vrankje_Van_Derwarring.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (130, N'Schattige Balonnen', N'Gebruik in een gevecht als afleiding. +5 voor één van beide kanten. Eenmalig bruikbaar', N'images/Schattige_Ballonnen.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (131, N'Vlammend Giftig Drankje', N'Gebruik in een gevecht. +3 voor één van beide kanten. Eenmalig bruikbaar.', N'images/Vlammend_Giftig_Drankje.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (132, N'Magische Scud', N'Gebruik in een gevecht. +5 voor één van beide kanten. Eenmalig bruikbaar', N'images/Magische_Scud.png', 5, 1, N'Gevecht')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (138, N'KeukenTrapje', N'+3 Bonus. Alleen te gebruiken door een Halfling', N'images/Keukentrapje.png', 11, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (140, N'Erg Indrukwekkende Titel', N'+3 Bonus.', N'images/Erg_Indrukwekkende_Titel.png', 11, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (142, N'Kniepunten', N'+1 Bonus.', N'images/Kniepunten.png', 11, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (143, N'Broodje Ei met Haring', N'+3 Bonus. Alleen te gebruiken door een Halfling.', N'images/Broodje_Ei_Met_Haring.png', 11, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (145, N'Stoere Zweetdoek', N'+3 Bonus. Alleen te gebruiken door een Mens.', N'images/Stoere_Zweetdoek.png', 6, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (149, N'Dappere Dodohelm', N'+1 Bonus.', N'images/Dappere_Dodohelm.png', 6, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (152, N'Vurig Harnas', N'+2 Bonus.', N'images/Vurig_Harnas.png', 10, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (153, N'Slijmerig Harnas', N'+1 Bonus.', N'images/Slijmerig_Harnas.png', 10, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (155, N'Kort Breed Harnas', N'+3 Bonus. Alleen te gebruiken door een Dwerg.', N'images/Kort_Breed_Harnas.png', 10, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (156, N'Lederen Harnas', N'+1 Bonus.', N'images/Lederen_Harnas.png', 10, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (158, N'Hamertje Tik', N'+4 Bonus. Alleen te gebruiken door een dwerg. En weer een knieschijf minder', N'images/Hamertje_Tik.png', 7, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (165, N'Modieus Schildje', N'+2 Bonus.', N'images/Modieus_Schildje.png', 7, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (166, N'Oneerlijke Degen', N'+3 Bonus. Alleen te gebruiken door een Elf.', N'images/Oneerlijke_Degen.png', 7, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (170, N'Gluiperig Zwaard', N'+2 Bonus.', N'images/Gluiperig_Zwaard.png', 7, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (171, N'Immens Rotsblok', N'+3 Bonus.', N'images/Immens_Rotsblok.png', 8, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (172, N'Multifunctionele Padvinders Hellebaard', N'+4 Bonus. Alleen te gebruiken door een Mens.', N'images/Multifunctionele_Padvinders_Helebaard.png', 8, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (173, N'Gestrikte Boog', N'+4 Bonus. Alleen te gebruiken door een Elf.', N'images/Gestrikte_Boog.png', 8, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (174, N'Grote Paal', N'+1 Bonus. Te gebruiken door iedereen die van grote palen houdt.', N'images/Grote_Paal.png', 8, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (175, N'Pathologische Kettingszaag', N'+3 Bonus.', N'images/Pathologische_Kettingzaag.png', 8, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (176, N'Supersnelle Schoenen', N'Geeft je +2 bij Vluchten', N'images/Supersnelle_Schoenen.png', 9, 0, N'Altijd')
INSERT [Munchkin].[Kaarten] ([Id], [Naam], [Beschrijving], [Afbeelding], [Type_id], [Eenmalig], [Wanneer_Bruikbaar]) VALUES (178, N'Holtrapper Laarzen', N'+2 Bonus.', N'images/Holtrapper_Laarzen.png', 9, 0, N'Altijd')
SET IDENTITY_INSERT [Munchkin].[Kaarten] OFF

/****** Insert data Kerkerkaarten ******/

INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (1, NULL, N'Hij slaat je met zijn kruk. Verlies 1 level.', 1, 1, 0, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (4, NULL, N'Schopt, bijt en stinkt een uur in de wind. Verlies 2 levels.', 4, 2, 1, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (6, NULL, N'Geen. Ontsnappen gaat vanzelf', 1, 1, 0, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (8, NULL, N'Ze bijten! Verlies 2 levels.', 2, 1, 0, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (11, NULL, N'Je wordt levend geroosterd en opgegeten.', 20, 5, 0, 2)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (12, NULL, N'Zijn botte behandeling kost je 2 levels.', 2, 1, 1, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (15, NULL, N'Verlies al je voorwerpen en alle kaarten in je hand.', 16, 4, 1, 2)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (20, NULL, N'Leg je harnas af en alle kledingstukken die je onder je taille draagt.', 1, 1, 0, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (23, NULL, N'Leg het schoeisel af dat je draagt. Verlies 1 level als je geen schoeisel hebt.', 1, 1, 0, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (24, NULL, N'Jouw level wordt gelijk aan het laagste level in het spel.', 8, 2, 0, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (25, NULL, N'Loopt je onder de voet en eet je hoed. Verlies de Hoofdeksel(s) die je draagt.', 12, 3, 0, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (28, NULL, N'Je wordt afgeranseld tot de dood.', 18, 5, 0, 2)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (29, NULL, N'Als je er niet in slaagt om te vluchten, moet je al je grote voorwerpen afleggen.', 2, 1, 0, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (33, NULL, N'Je valt terug op level 1.', 16, 4, 1, 2)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (35, NULL, N'Als het je gezicht wegzuigt, gaat je hoofddeksel ook weg. Leg al je hoofddeksels af en verlies 1 level.', 8, 2, 0, 1)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (43, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (45, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (46, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (47, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (48, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (52, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (54, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (58, N'Je mag maximaal 6 kaarten in je hand houden.', NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (60, N'Je mag maximaal 6 kaarten in je hand houden.', NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (61, N'Je mag maximaal 6 kaarten in je hand houden.', NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (62, N'Je gaat 1 level omhoog elke keer dat je een ander helpt een monster te doden.', NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (63, N'Je gaat 1 level omhoog elke keer dat je een ander helpt een monster te doden.', NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (64, N'Je gaat 1 level omhoog elke keer dat je een ander helpt een monster te doden.', NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (65, N'Als je eerste vluchtpoging mislukt, dan mag je een kaart afleggen  en het nogmaals proberen.', NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (66, N'Als je eerste vluchtpoging mislukt, dan mag je een kaart afleggen  en het nogmaals proberen.', NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (67, N'Als je eerste vluchtpoging mislukt, dan mag je een kaart afleggen  en het nogmaals proberen.', NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (86, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (95, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (96, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (97, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Munchkin].[Kerkerkaarten] ([Id], [Beschrijving_2], [Portie_Ellende], [Level], [Aantal_schatten], [Ondood], [Aantal_Levels]) VALUES (98, NULL, NULL, NULL, NULL, NULL, NULL)

/****** Insert data Schatkaarten ******/

INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (99, NULL, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (101, NULL, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (102, NULL, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (103, NULL, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (106, NULL, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (107, NULL, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (110, NULL, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (112, 200, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (115, 100, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (116, 100, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (117, 100, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (121, 200, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (128, 100, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (130, 0, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (131, 100, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (132, 300, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (138, 400, 1)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (140, NULL, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (142, 200, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (143, 400, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (145, 400, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (149, 200, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (152, 400, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (153, 200, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (155, 400, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (156, 200, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (158, 600, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (165, 400, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (166, 600, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (170, 400, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (171, 0, 1)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (172, 600, 1)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (173, 800, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (174, 200, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (175, 600, 1)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (176, 400, 0)
INSERT [Munchkin].[Schatkaarten] ([Id], [Schatkaart_Waarde], [Is_Groot]) VALUES (178, 400, 0)


/****** Insert data Bonussen ******/

SET IDENTITY_INSERT [Munchkin].[Bonussen] ON 

INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (1, N'Niemand', N'Iedereen', 1, N'Vluchten')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (2, N'Niemand', N'Iedereen', 5, N'Dwerg')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (3, N'Niemand', N'Iedereen', 5, N'Elf')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (4, N'Niemand', N'Iedereen', -1, N'Vluchten')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (5, N'Niemand', N'Iedereen', 4, N'Priester')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (6, N'Niemand', N'Iedereen', 4, N'Krijger')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (7, N'Niemand', N'Iedereen', 6, N'Magiër')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (8, N'Niemand', N'Iedereen', 3, N'Priester')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (9, N'Niemand', N'Iedereen', 6, N'Krijger')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (10, N'Niemand', N'Iedereen', 4, N'Elf')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (11, N'Niemand', N'Iedereen', 3, N'Dwerg')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (12, N'Niemand', N'Iedereen', 3, N'Halfling')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (13, N'Niemand', N'Iedereen', -2, N'Vluchten')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (14, N'Niemand', N'Iedereen', 6, N'Dwerg')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (15, N'Niemand', N'Iedereen', 6, N'Elf')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (16, N'Niemand', N'Iedereen', 5, N'magiër')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (17, N'Niemand', N'Iedereen', -1, N'Dobbelsteenworp')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (18, N'Niemand', N'Iedereen', -1, N'Level')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (19, N'Niemand', N'Iedereen', -2, N'Level')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (20, N'Niemand', N'Iedereen', -5, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (21, N'Niemand', N'Iedereen', 10, N'Monster')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (22, N'Niemand', N'Iedereen', 5, N'Monster')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (23, N'Niemand', N'Iedereen', -5, N'Monster')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (24, N'Niemand', N'Iedereen', 1, N'Level')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (25, N'Niemand', N'Iedereen', 5, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (26, N'Niemand', N'Iedereen', 2, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (27, N'Niemand', N'Iedereen', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (28, N'Niemand', N'Elf', 2, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (31, N'Priester', N'Iedereen', 0, N'Andere')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (32, N'Dief', N'Iedereen', 2, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (33, N'Niemand', N'Halfling', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (34, N'Niemand', N'Iedereen', 1, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (35, N'Krijger', N'Iedereen', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (37, N'Niemand', N'Dief', 4, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (38, N'Elf', N'Iedereen', 1, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (39, N'Niemand', N'Elf', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (40, N'Niemand', N'Mens', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (41, N'Niemand', N'Magiër', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (42, N'Magiër', N'Iedereen', 3, N'Gevechswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (43, N'Niemand', N'Dwerg', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (44, N'Niemand', N'Mannen', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (45, N'Niemand', N'Dwerg', 4, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (46, N'Niemand', N'Magiër', 5, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (47, N'Niemand', N'Priester', 4, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (48, N'Niemand', N'Vrouwen', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (49, N'Niemand', N'Priester', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (50, N'Niemand', N'Krijger', 4, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (51, N'Niemand', N'Dief', 3, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (52, N'Niemand', N'Iedereen', 3, N'Vluchten')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (53, N'Niemand', N'Mens', 4, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (54, N'Niemand', N'Elf', 4, N'Gevechtswaarde')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (55, N'Niemand', N'Iedereen', 2, N'Vluchten')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (56, N'Niemand', N'Iedereen', 1, N'Aantal_Schatten')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (57, N'Niemand', N'Iedereen', -1, N'Aantal_Schatten')
INSERT [Munchkin].[Bonussen] ([Id], [Niet_Bruikbaar_Door], [Bruikbaar_Door], [Waarde], [Waarop_Effect]) VALUES (58, N'Niemand', N'Iedereen', 2, N'Aantal_Schatten')
SET IDENTITY_INSERT [Munchkin].[Bonussen] OFF

/****** Insert data Kaart_Bonussen ******/

SET IDENTITY_INSERT [Munchkin].[Kaart_Bonussen] ON 

INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (1, 1, 1)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (2, 29, 1)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (3, 4, 2)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (5, 8, 4)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (11, 23, 10)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (12, 25, 11)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (13, 25, 12)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (16, 35, 15)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (19, 43, 18)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (20, 47, 18)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (21, 45, 19)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (23, 86, 21)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (24, 97, 21)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (25, 95, 22)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (26, 96, 22)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (27, 98, 23)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (28, 99, 24)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (29, 101, 24)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (30, 102, 24)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (32, 103, 24)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (34, 106, 24)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (35, 107, 24)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (39, 110, 24)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (40, 112, 25)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (42, 115, 26)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (43, 116, 26)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (44, 117, 27)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (45, 121, 26)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (46, 128, 27)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (47, 130, 25)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (48, 131, 27)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (49, 132, 25)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (53, 138, 33)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (56, 140, 27)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (58, 142, 34)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (59, 143, 33)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (61, 145, 40)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (65, 149, 34)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (67, 152, 26)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (68, 153, 34)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (69, 155, 43)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (70, 156, 34)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (72, 158, 45)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (78, 165, 26)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (79, 166, 39)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (83, 170, 26)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (84, 171, 27)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (85, 172, 53)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (86, 173, 54)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (87, 174, 34)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (88, 175, 27)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (89, 176, 55)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (90, 178, 26)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (91, 98, 57)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (92, 95, 56)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (93, 96, 56)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (94, 97, 58)
INSERT [Munchkin].[Kaart_Bonussen] ([Id], [Kaart_Id], [Bonus_Id]) VALUES (95, 86, 58)
SET IDENTITY_INSERT [Munchkin].[Kaart_Bonussen] OFF