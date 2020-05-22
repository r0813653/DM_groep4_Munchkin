using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Stapel : BasisKlasse
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Id" && Id <= 0)
                {
                    return "Id mag niet kleiner of gelijk zijn aan 0";
                }
                if (columnName == "Naam" && string.IsNullOrWhiteSpace(Naam))
                {
                    return "Naam moet ingevuld zijn";
                }
                return "";
            }
        }
        public string StapelLeegmaken(Stapel schatkaartenAflegstapel, Stapel kerkerkaartenAflegstapel)
        {
            string foutmelding = "";
            List<Kaarten_Stapel> kaartenstapels = new List<Kaarten_Stapel>();
            foreach (Kaarten_Stapel kaart in Kaarten_Stapels)
            {
                kaartenstapels.Add(kaart);
            }
            for (int i = 0; i < kaartenstapels.Count(); i++)
            {
                //om aan shatkaarten en kerkerkaarten te kunnen
                Kaart kaart = DatabaseOperations.OphalenKaartViaId(kaartenstapels[i].Kaart_Id);
                if (kaart.Kerkerkaart == null)
                {
                    foutmelding += kaartenstapels[i].KaartVanStapelWisselen(schatkaartenAflegstapel);
                }
                else
                {
                    foutmelding += kaartenstapels[i].KaartVanStapelWisselen(kerkerkaartenAflegstapel);
                }
            }
            return foutmelding;
        }
        public string TypeUitStapelHalen(Stapel kerkerkaartenAflegstapel, string typeToUpper)
        {
            string foutmelding = "";
            List<Kaarten_Stapel> kaartenstapels = new List<Kaarten_Stapel>();
            foreach (Kaarten_Stapel kaart in Kaarten_Stapels)
            {
                kaartenstapels.Add(kaart);
            }
            for (int i = 0; i < kaartenstapels.Count(); i++)
            {
                //check of er al een kaart van dit type zit in de veldkaarten
                string type = DatabaseOperations.OphalenType(kaartenstapels[i].Kaart.Type_id).Soort.ToUpper();
                if (type.ToUpper().Contains(typeToUpper))
                {
                    //type uit veldkaarten halen
                    foutmelding += kaartenstapels[i].KaartVanStapelWisselen(kerkerkaartenAflegstapel);
                }
            }
            return foutmelding;
        }
    }
}
