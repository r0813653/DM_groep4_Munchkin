using System;
using Munckin_DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MunchkinTests
{
    [TestClass]
    public class SpelerTests
    {
        [TestMethod]//Domien Van den Heuvel
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
    
    
        [TestMethod]//Yoran Aerts
        public void SpeleBonusToevoegen_SpelerBonusVeranderd()
        {
            //arrange
            Kaart kaart = DatabaseOperations.OphalenKaartViaId(29);//geeft plus 1 op vluchten
            Wedstrijd_Speler speler = DatabaseOperations.OphalenWedstrijd_SpelerViaId(5);


            //act
            kaart.BonussenItemToevoegen(speler);

            
           
            
            //assert
            Assert.AreEqual(speler.Vluchtbonus,1) ;
        }
    }
}
