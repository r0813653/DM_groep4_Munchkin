using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munckin_DAL;

namespace MunchkinTests
{
    [TestClass]
    public class KaartTest
    {
        [TestMethod] //jens
        public void ControleerGroteVoorwerpen_GebruikerRasIsDwerg_ReturnedStringIsEmpty()
        {
            //arrange
            Kaart kaart = DatabaseOperations.OphalenKaartViaId(15);
            Wedstrijd_Speler gebruiker = DatabaseOperations.OphalenWedstrijd_SpelerViaId(5);
            gebruiker.Veldkaarten_Id = 19;
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);            
           
            
            gebruiker.Ras = "dwerg";
           
            //act            
            string result = kaart.ControleerGroteVoorwerpen(veldkaarten, gebruiker);
            //assert
            Assert.AreEqual(result,string.Empty);
        }
    }
}
