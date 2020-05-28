using System;
using System.Data.Entity;
using Munckin_DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MunchkinTests
{
    [TestClass]
    public class DatabaseOperationsTests
    {
        [TestMethod]
        public void Wedstrijd_Speler_OphalenWedstrijd_SpelerMetOpgegevenID_OpgehaaldeWedstrijd_SpelerIsGelijkAanAangemaakteWedstrijd_Speler()
        {
            //arrange
            Wedstrijd_Speler wedstrijd_Speler = new Wedstrijd_Speler();

            //act
            wedstrijd_Speler.Id = 1;
            wedstrijd_Speler.Wedstrijd_Id = 1;
            wedstrijd_Speler.Level = 6;
            wedstrijd_Speler.Speler_Id = 1;
            wedstrijd_Speler.Handkaarten_Id = 5;
            wedstrijd_Speler.Veldkaarten_Id = 6;
            wedstrijd_Speler.Ras = "Mens";
            wedstrijd_Speler.Vluchtbonus = 0;
            wedstrijd_Speler.Gevechtsbonus = 0;
            wedstrijd_Speler.Tijdelijke_Bonus = 0;
            Wedstrijd_Speler ophalenWedstrijd_Speler = DatabaseOperations.OphalenWedstrijd_SpelerViaId(1);

            //assert
            Assert.AreEqual(wedstrijd_Speler, ophalenWedstrijd_Speler);

        }

        [TestMethod]
        public void OphalenKaartMetOpgegevenID_OpgehaaldeKaartIsCorrect()
        {
            //arrange
            Kaart kaart = new Kaart();
            //act
            kaart.Id = 1;
            kaart.Naam = "Lamme Goblin";
            kaart.Beschrijving = "+1 voor vluchten";
            kaart.Afbeelding = "images/Lamme_goblin.png";
            kaart.Type_id = 3;
            kaart.Eenmalig = null;
            kaart.Wanneer_Bruikbaar = "Altijd";

            Kaart ophalenKaart = DatabaseOperations.OphalenKaartViaId(1);
            //assert
            //Assert.AreEqual(kaart, ophalenKaart);
            Assert.AreEqual(kaart.Naam, ophalenKaart.Naam);
            Assert.AreEqual(kaart.Id, ophalenKaart.Id);
            Assert.AreEqual(kaart.Afbeelding, ophalenKaart.Afbeelding);
            Assert.AreEqual(kaart.Type_id, ophalenKaart.Type_id);
            Assert.AreEqual(kaart.Eenmalig, ophalenKaart.Eenmalig);
            Assert.AreEqual(kaart.Wanneer_Bruikbaar, ophalenKaart.Wanneer_Bruikbaar);
        }

    }
}
