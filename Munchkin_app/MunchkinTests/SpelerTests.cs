using System;
using Munckin_DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MunchkinTests
{
    [TestClass]
    public class SpelerTests
    {
        [TestMethod]
        public void Naam_ValueIsNietOpgevuld_FoutmeldingNaamMoetIngevuldZijnGooien()
        {
            //arrange
            Speler speler = new Speler();
            string foutmelding = "Naam moet ingevuld zijn" + Environment.NewLine;

            //act
            speler.Id = 1;
            speler.Geslacht = "M";
            speler.Naam = "";

            //assert
            Assert.AreEqual(speler.Error, foutmelding);
        }
    }
}
