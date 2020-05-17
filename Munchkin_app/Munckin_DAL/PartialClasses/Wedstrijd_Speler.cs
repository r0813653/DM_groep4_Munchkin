using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Wedstrijd_Speler: BasisKlasse
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Level" && Level <= 0)
                {
                    return "Level moet een positief getal zijn!";
                }
                if (columnName == "Ras" && string.IsNullOrWhiteSpace(Ras))
                {
                    return "Iedereen moet een ras hebben!";
                }
                if (columnName == "Vluchtbonus" && Vluchtbonus < 0)
                {
                    return "Vluchtbonus mag niet negatief zijn!";
                }
                if (columnName == "Gevechtsbonus" && Gevechtsbonus < 0)
                {
                    return "Gevechtsbonus mag niet negatief zijn!";
                }
                return "";
            }
        }
        public void HerberekenBonussenVeldkaarten(Stapel veldkaarten)
        {
            
            Vluchtbonus = 0;
            Gevechtsbonus = 0;
            if (Ras.ToUpper() == "ELF")
            {
                Vluchtbonus += 1;
            }
            //checken of hij kaarten heeft die een bonus geven op een bepaald ras. Als hij dat ras nu niet meer heeft dan gaat de bonus weg
            foreach (Kaarten_Stapel kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
            {
                //bonussen ophalen
                List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(kaarten_Stapel1.Kaart_Id);
                foreach (var bonus in lijstBonussen)
                {
                    //checken of je bonus mag gebruiken
                    if (bonus.Bruikbaar_Door.ToUpper() == Ras.ToUpper() || bonus.Bruikbaar_Door.ToUpper() == "IEDEREEN")
                    {
                        if (bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
                        {
                            //Bonus toevoegen aan gevechtswaarde
                            Gevechtsbonus += bonus.Waarde;
                        }
                        else if (bonus.Waarop_Effect.ToUpper() == "VLUCHTEN")
                        {
                            //Bonus toevoegen aan vluchtbonus
                            Vluchtbonus += bonus.Waarde;
                        }
                    }
                }
            }
        }
    }
}
