/****** extra data toevoegen die nodig zijn voor de query's ******/

/****** Voeg spelers toe ******/

INSERT [Munchkin].[Spelers] ( [Naam],  [Geslacht]) VALUES ( N'Domien', N'M')
INSERT [Munchkin].[Spelers] ( [Naam],  [Geslacht]) VALUES ( N'Jens', N'M')
INSERT [Munchkin].[Spelers] ( [Naam],  [Geslacht]) VALUES ( N'Yoran', N'F')
INSERT [Munchkin].[Spelers] ( [Naam],  [Geslacht]) VALUES ( N'Ludovic', N'M')



/****** Voeg stapels toe ******/


INSERT [Munchkin].[Stapels] ( [Naam]) VALUES (N'Trekstapel_Kerkerkaarten_Wedstrijd1')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES (N'Trekstapel_Schatkaarten_Wedstrijd1')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES (N'Aflegstapel_Kerkerkaarten_Wedstrijd1')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES (N'Aflegstapel_Schatkaarten_Wedstrijd1')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES (N'Handkaarten_Domien')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES (N'Veldkaarten_Domien')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES (N'Handkaarten_Jens')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES (N'Veldkaarten_Jens')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES (N'Trekstapel_Kerkerkaarten_Wedstrijd2')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES ( N'Trekstapel_Schatkaarten_Wedstrijd2')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES ( N'Aflegstapel_Kerkerkaarten_Wedstrijd2')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES ( N'Aflegstapel_Schatkaarten_Wedstrijd2')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES ( N'Handkaarten_Yoran')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES ( N'Veldkaarten_Yoran')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES ( N'Handkaarten_Ludovic')
INSERT [Munchkin].[Stapels] ( [Naam]) VALUES ( N'Veldkaarten_Ludovic')


/****** Voeg Wedstrijden toe ******/


INSERT [Munchkin].[Wedstrijden] ( [Kerkerkaarten_Trekstapel_Id], [Kerkerkaarten_Aflegstapel_Id], [Schatkaarten_Trekstapel_Id], [Schatkaarten_Aflegstapel_Id]) VALUES ( 1, 3, 2, 4)
INSERT [Munchkin].[Wedstrijden] ( [Kerkerkaarten_Trekstapel_Id], [Kerkerkaarten_Aflegstapel_Id], [Schatkaarten_Trekstapel_Id], [Schatkaarten_Aflegstapel_Id]) VALUES ( 9, 11, 10, 12)

/****** Voeg Wedstrijd_Spelers toe ******/

/****** Voeg Wedstrijd_Spelers toe ******/


INSERT [Munchkin].[Wedstrijd_Spelers] ( [Wedstrijd_Id], [Level], [Speler_Id], [Handkaarten_Id], [Veldkaarten_Id]) VALUES ( 1, 6, 1, 5, 6)
INSERT [Munchkin].[Wedstrijd_Spelers] ( [Wedstrijd_Id], [Level], [Speler_Id], [Handkaarten_Id], [Veldkaarten_Id]) VALUES ( 1, 5, 2, 7, 8)
INSERT [Munchkin].[Wedstrijd_Spelers] ( [Wedstrijd_Id], [Level], [Speler_Id], [Handkaarten_Id], [Veldkaarten_Id]) VALUES ( 2, 4, 3, 13, 14)
INSERT [Munchkin].[Wedstrijd_Spelers] ( [Wedstrijd_Id], [Level], [Speler_Id], [Handkaarten_Id], [Veldkaarten_Id]) VALUES ( 2, 2, 4, 15, 16)





/****** Vraag de naam en beschrijving op van handkaarten van speler1 in wedstrijd 1 ******/

SELECT K.Naam, K.Beschrijving
FROM (
		(Munchkin.Wedstrijd_Spelers WS JOIN Munchkin.Stapels ST
		ON WS.Handkaarten_Id = ST.Id)
		JOIN Munchkin.Kaarten_Stapels KS
		ON KS.Stapel_Id = ST.Id)
		JOIN Munchkin.Kaarten K
		ON KS.Kaart_Id = K.Id
WHERE WS.Speler_Id = 1 and WS.Wedstrijd_Id = 1;

/****** Vraag Klassen uit de aflegstapel op ( in geval van de vervloeking verandering van klasse) ******/

SELECT K.*
FROM (((Munchkin.Stapels S JOIN Munchkin.Kaarten_Stapels KS
		ON KS.Stapel_Id = S.Id)
		JOIN Munchkin.Kaarten K
		ON KS.Kaart_Id = K.Id)
		JOIN Munchkin.Types T
		ON K.Type_id = T.Id)
WHERE S.Id = 3 AND T.Id = 2;

/****** Vraag de spelers op van wedstrijd met id 1 ******/

SELECT S.Naam
FROM ((Munchkin.Spelers S JOIN Munchkin.Wedstrijd_Spelers WS
	ON S.Id = WS.Speler_Id)
	JOIN Munchkin.Wedstrijden W
	ON W.Id = WS.Wedstrijd_Id)
	WHERE W.Id = 1;

/****** Tel alle waardes op die de gevechtswaarde van Jens verhogen (Van de Veldkaarten Jens)******/

SELECT SUM(B.Waarde)
FROM ((((
		(Munchkin.Stapels ST JOIN Munchkin.Wedstrijd_Spelers WS
		ON WS.Veldkaarten_Id = ST.Id)
		JOIN Munchkin.Kaarten_Stapels KS
		ON KS.Stapel_Id = ST.Id)
		JOIN Munchkin.Kaarten K
		ON KS.Kaart_Id = K.Id)
		JOIN Munchkin.Kaart_Bonussen KB
		ON KB.Kaart_Id = K.Id)
		JOIN Munchkin.Bonussen B
		ON B.Id = KB.Bonus_Id)
WHERE ST.Id = 8 AND B.Waarop_Effect = 'Gevechtswaarde'


/****** Alle nuttige waarden van een monster opvragen ******/

SELECT K.Naam, K.Beschrijving, KK.Portie_Ellende AS [Portie Ellende], KK.Level, KK.Aantal_schatten AS [Aantal schatten dat je krijgt], KK.Aantal_Levels AS [Aantal levels dat je krijgt], KK.Ondood, B.Waarde AS [Waarde van de bonus], B.Waarop_Effect AS [Op wie effect]
FROM ((( Munchkin.Bonussen B JOIN Munchkin.Kaart_Bonussen KB
ON B.Id = KB.Bonus_Id)
JOIN Munchkin.Kaarten K
ON K.Id = KB.Kaart_Id)
JOIN Munchkin.Kerkerkaarten KK
ON KK.Id = K.Id)
WHERE K.Id = 4
