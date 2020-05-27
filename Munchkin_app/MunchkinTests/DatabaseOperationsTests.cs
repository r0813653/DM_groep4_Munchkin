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
    }
}
