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
        public void kerkerkaartTrekstapelsChecken()
        {
            Stapel aflegstapelKerkerkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Kerkerkaarten_Aflegstapel_Id);
            List<Kaarten_Stapel> kerkerkaartenStapels = new List<Kaarten_Stapel>();
            if (Kaarten_Stapels.Count() <= 4)
            {
                foreach (Kaarten_Stapel kaarten_Stapel in aflegstapelKerkerkaarten.Kaarten_Stapels)
                {
                    kerkerkaartenStapels.Add(kaarten_Stapel);
                }
                kerkerkaartenStapels.Shuffle();
                for (int i = 0; i < kerkerkaartenStapels.Count(); i++)
                {
                    kerkerkaartenStapels[i].KaartVanStapelWisselen(this);
                }
            }
        }
        public void schatkaartTrekstapelChecken()
        {
            Stapel aflegstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(GlobalVariables.wedstrijd.Schatkaarten_Aflegstapel_Id);
            List<Kaarten_Stapel> schatkaartenStapels = new List<Kaarten_Stapel>();
            if (Kaarten_Stapels.Count() <= 7)
            {
                foreach (Kaarten_Stapel kaarten_Stapel in aflegstapelSchatkaarten.Kaarten_Stapels)
                {
                    schatkaartenStapels.Add(kaarten_Stapel);
                }
                schatkaartenStapels.Shuffle();
                for (int i = 0; i < schatkaartenStapels.Count(); i++)
                {
                    schatkaartenStapels[i].KaartVanStapelWisselen(this);
                }
            }
        }
    }
}
