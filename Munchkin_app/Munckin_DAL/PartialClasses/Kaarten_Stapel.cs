using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Kaarten_Stapel : BasisKlasse
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Kaart_Id" && Kaart_Id <= 0)
                {
                    return "Kaart_Id moet een positief getal zijn!";
                }
                if (columnName == "Stapel_Id" && Stapel_Id <= 0)
                {
                    return "Stapel_Id moet een positief getal zijn!";
                }
                return "";
            }
        }

        public string KaartVanStapelWisselen(Stapel doelStapel)
        {
            string foutmelding = "";

            //Voeg kaart toe in doelstapel
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = Kaart_Id;
            nieuweKaartenStapel.Stapel_Id = doelStapel.Id;

            //als de nieuwe kaartenstapel geldig is, gaan we aanpassing doorvoeren in db
            if (nieuweKaartenStapel.IsGeldig())
            {
                //nieuwe kaartenstapel toevoegen
                int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                if (ok > 0)
                {
                    //oude kaartenstapel verwijderen
                    int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(this);
                    if (ok2 <= 0)
                    {
                        foutmelding += "Kaart niet uit stapel kunnen halen\n";
                    }
                }
                else
                {
                    foutmelding += "Kaart niet in nieuwe stapel kunnen plaatsen\n";
                }
            }
            else
            {
                foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
            }
            return foutmelding;
        }
    }
}
