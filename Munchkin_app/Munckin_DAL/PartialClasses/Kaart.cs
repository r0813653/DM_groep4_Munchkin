using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Kaart : BasisKlasse
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
                if (columnName == "Beschrijving" && string.IsNullOrWhiteSpace(Beschrijving))
                {
                    return "Beschrijving moet ingevuld zijn";
                }
                if (columnName == "Afbeelding" && string.IsNullOrWhiteSpace(Afbeelding))
                {
                    return "Naam moet ingevuld zijn";
                }
                if (columnName == "Type_id" && Type_id <= 0)
                {
                    return "Type_id mag niet kleiner of gelijk zijn aan 0";
                }
                return "";
            }
        }
        public string SpeelKaart(Wedstrijd_Speler gebruiker)
        {

            Type type = DatabaseOperations.OphalenType(Type_id);
            if (type.Soort.ToUpper() == "RAS")
            {
                string ret = SpeelRas(gebruiker);
                return ret;
            }
            else if (type.Soort.ToUpper() == "HOOFDDEKSEL")
            {
                string bericht1 = "je hebt al een hoofddeksel op. Namelijk een ";
                string ret = SpeelHoofddekselOfHarnasOfSchoeisel(gebruiker, type, bericht1);
                return ret;
            }
            else if (type.Soort.ToUpper() == "HARNAS")
            {
                string bericht1 = "je hebt al een harnas aan. Namelijk een ";
                string ret = SpeelHoofddekselOfHarnasOfSchoeisel(gebruiker, type, bericht1);
                return ret;
            }
            else if (type.Soort.ToUpper() == "SCHOEISEL")
            {
                string bericht1 = "je hebt al schoeisel aan. Namelijk ";
                string ret = SpeelHoofddekselOfHarnasOfSchoeisel(gebruiker, type, bericht1);
                return ret;
            }
            else if (type.Soort.ToUpper() == "EXTRA")
            {
                string ret = SpeelExtra(gebruiker);
                return ret;
            }
            else if (type.Soort.ToUpper() == "1HAND")
            {
                string ret = Speel1Hand(gebruiker);
                return ret;
            }
            else if (type.Soort.ToUpper() == "2HANDEN")
            {
                string ret = Speel2Handen(gebruiker);
                return ret;
            }
            else if (type.Soort.ToUpper() == "GEBRUIKSKAARTEN")
            {
                string ret = SpeelGebruikskaart(gebruiker);
                return ret;
            }
            else if (type.Soort.ToUpper() == "VERVLOEKING")
            {
                string ret = SpeelVervloeking(gebruiker);
                return ret;
            }
            else
            {
                return "Je kan deze kaart nu niet gebruiken";
            }
        }
        public string SpeelKaart(Wedstrijd_Speler gebruiker, Wedstrijd_Speler slachtoffer)
        {
            Type type = DatabaseOperations.OphalenType(Type_id);
            if (type.Soort.ToUpper() == "VERVLOEKING")
            {
                string ret = SpeelVervloeking(gebruiker, slachtoffer);
                return ret;
            }
            else if (type.Soort.ToUpper() == "GEBRUIKSKAARTEN")
            {
                string ret = SpeelGebruikskaart(gebruiker, slachtoffer);
                return ret;
            }
            else
            {
                return "Je kan deze kaart nu niet gebruiken";
            }
        }
        public string SpeelKaart(Wedstrijd_Speler gebruiker, Kaart monster)
        {
            Type type = DatabaseOperations.OphalenType(Type_id);
            if (type.Soort.ToUpper() == "GEBRUIKSKAARTEN")
            {
                string ret = SpeelGebruikskaart(gebruiker, monster);
                return ret;
            }
            else
            {
                return "Je kan deze kaart nu niet gebruiken";
            }
        }
        public string VechtMonster(Wedstrijd_Speler vechter)
        {
            string foutmelding = "";
            int spelersterkte;
            //Waars dbOperation
            int monsterSterkte = Kerkerkaart.Level + Kerkerkaart.Tijdelijke_Bonus ?? default;
            List<Bonus> lijstBonusssen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            //speciale gevallen sterkte berekenen
            if (Naam.ToUpper().Contains("VAMPIEREN"))
            {
                //default checken, weet niet of dit werkt
                spelersterkte = vechter.Level ?? default;
            }
            //Normaal geval sterkte berekenen
            else
            {
                spelersterkte = vechter.Level + vechter.Gevechtsbonus + vechter.Tijdelijke_Bonus ?? default;
                foreach (var bonus in lijstBonusssen)
                {
                    if (vechter.Ras.ToUpper() == bonus.Waarop_Effect.ToUpper())
                    {
                        monsterSterkte += bonus.Waarde;
                    }
                }
            }
            //checken of je wint
            if (spelersterkte > monsterSterkte)
            {
                //levels omhoog, waars dboperations
                vechter.Level += Kerkerkaart.Aantal_Levels;
                if (vechter.IsGeldig())
                {
                    int ok2 = DatabaseOperations.AanpassenWedstrijd_Speler(vechter);
                    if (ok2 <= 0)
                    {
                        foutmelding += "Level is niet gestegen\n";
                    }
                }
                else
                {
                    foutmelding += vechter.Error + Environment.NewLine;
                }

                //schatkaarten trekken
                int wedstrijdId = vechter.Wedstrijd_Id ?? default;
                Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
                List<Kaarten_Stapel> trekstapelSchatkaartenKaarten_Stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(wedstrijd.Schatkaarten_Trekstapel_Id);
                Stapel vechterHandkaarten = DatabaseOperations.OphalenStapelViaId(vechter.Handkaarten_Id);
                int aantalSchattenTrekken = Kerkerkaart.Aantal_schatten ?? default;
                if (vechter.Ras.ToUpper().Contains("ELF") && Naam.ToUpper() == "WIETPLANT")
                {
                    aantalSchattenTrekken += 1;
                }
                for (int i = 0; i < aantalSchattenTrekken; i++)
                {
                    foutmelding += trekstapelSchatkaartenKaarten_Stapels[i].KaartVanStapelWisselen(vechterHandkaarten);

                }
                //kaart naar aflegstapel doen
                Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                kaarten_Stapel1.Kaart_Id = Id;
                kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                if (kaarten_Stapel1.IsGeldig())
                {
                    int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                    if (ok <= 0)
                    {
                        foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                    }
                }
                else
                {
                    foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                }
                if (string.IsNullOrEmpty(foutmelding))
                {
                    return $"Je hebt {Naam} verslagen!";
                }
                else
                {
                    return foutmelding;
                }
            }
            else
            {
                return $"Je kan niet winnen tegen {Naam}";
            }
        }
        public string VechtMonster(Wedstrijd_Speler vechter, Wedstrijd_Speler helper)
        {
            string foutmelding = "";
            int spelersterkte;
            //Waars dbOperation
            int monsterSterkte = Kerkerkaart.Level + Kerkerkaart.Tijdelijke_Bonus ?? default;
            List<Bonus> lijstBonusssen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            //speciale gevallen sterkte berekenen
            if (Naam.ToUpper().Contains("VAMPIEREN"))
            {
                //default checken, weet niet of dit werkt
                spelersterkte = vechter.Level + helper.Level ?? default;
            }
            //Normaal geval sterkte berekenen
            else
            {
                spelersterkte = vechter.Level + vechter.Gevechtsbonus + vechter.Tijdelijke_Bonus + helper.Level + helper.Gevechtsbonus + helper.Tijdelijke_Bonus ?? default;
                foreach (var bonus in lijstBonusssen)
                {
                    if (vechter.Ras.ToUpper() == bonus.Waarop_Effect.ToUpper() || helper.Ras.ToUpper() == bonus.Waarop_Effect.ToUpper())
                    {
                        monsterSterkte += bonus.Waarde;
                    }
                }
            }
            //checken of je wint
            if (spelersterkte > monsterSterkte)
            {
                //levels omhoog
                vechter.Level += Kerkerkaart.Aantal_Levels;
                foutmelding += vechter.PasWedstrijd_SpelerAan();

                if (helper.Ras.ToUpper() == "ELF")
                {
                    helper.Level += 1;
                    foutmelding += helper.PasWedstrijd_SpelerAan();
                }
                //schatkaarten trekken
                int wedstrijdId = vechter.Wedstrijd_Id ?? default;
                Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
                List<Kaarten_Stapel> trekstapelSchatkaartenKaarten_Stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(wedstrijd.Schatkaarten_Trekstapel_Id);
                Stapel vechterHandkaarten = DatabaseOperations.OphalenStapelViaId(vechter.Handkaarten_Id);
                Stapel helperHandkaarten = DatabaseOperations.OphalenStapelViaId(helper.Handkaarten_Id);
                int aantalSchattenTrekken = Kerkerkaart.Aantal_schatten ?? default;
                if (vechter.Ras.ToUpper().Contains("ELF") && Naam.ToUpper() == "WIETPLANT")
                {
                    aantalSchattenTrekken += 1;
                }
                else if (helper.Ras.ToUpper().Contains("ELF") && Naam.ToUpper() == "WIETPLANT")
                {
                    aantalSchattenTrekken += 1;
                }
                for (int i = 0; i < aantalSchattenTrekken; i++)
                {
                    if (i % 2 == 0)
                    {
                        foutmelding += trekstapelSchatkaartenKaarten_Stapels[i].KaartVanStapelWisselen(helperHandkaarten);
                    }
                    else
                    {
                        foutmelding += trekstapelSchatkaartenKaarten_Stapels[i].KaartVanStapelWisselen(vechterHandkaarten);
                    }
                }
                //kaart naar aflegstapel doen
                Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                kaarten_Stapel1.Kaart_Id = Id;
                kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                if (kaarten_Stapel1.IsGeldig())
                {
                    int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                    if (ok <= 0)
                    {
                        foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                    }
                }
                else
                {
                    foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                }
                if (string.IsNullOrEmpty(foutmelding))
                {
                    return $"Je hebt {Naam} verslagen!";
                }
                else
                {
                    return foutmelding;
                }
            }
            else
            {
                return $"Je kan niet winnen tegen {Naam}";
            }
        }
        public string VluchtMonster(Wedstrijd_Speler vluchter, int dobbelsteenworp)
        {
            string foutmelding = "";
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(vluchter.Veldkaarten_Id);
            Stapel handkaarten = DatabaseOperations.OphalenStapelViaId(vluchter.Handkaarten_Id);
            int wedstrijdId = vluchter.Wedstrijd_Id ?? default;
            Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
            Stapel kerkerkaartenAflegstapel = DatabaseOperations.OphalenStapelViaId(wedstrijd.Kerkerkaarten_Aflegstapel_Id);
            Stapel schatkaartenAflegstapel = DatabaseOperations.OphalenStapelViaId(wedstrijd.Schatkaarten_Aflegstapel_Id);
            int spelersterkte;
            List<Bonus> lijstBonusssen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            dobbelsteenworp += vluchter.Vluchtbonus ?? default;
            foreach (var bonus in lijstBonusssen)
            {
                if (bonus.Waarop_Effect.ToUpper() == "VLUCHTEN")
                {
                    dobbelsteenworp -= bonus.Waarde;
                }
            }
            int monsterSterkte = Kerkerkaart.Level + Kerkerkaart.Tijdelijke_Bonus ?? default;
            //speciale gevallen sterkte berekenen
            if (Naam.ToUpper().Contains("VAMPIEREN"))
            {
                //default checken, weet niet of dit werkt
                spelersterkte = vluchter.Level ?? default;
            }
            //Normaal geval sterkte berekenen
            else
            {
                spelersterkte = vluchter.Level + vluchter.Gevechtsbonus + vluchter.Tijdelijke_Bonus ?? default;
                foreach (var bonus in lijstBonusssen)
                {
                    if (vluchter.Ras.ToUpper() == bonus.Waarop_Effect.ToUpper())
                    {
                        monsterSterkte += bonus.Waarde;
                    }
                }
            }
            //checken of je wint
            if (spelersterkte <= monsterSterkte)
            {
                if (Naam.ToUpper().Contains("KWELROG") && vluchter.Level <= 4)
                {
                    //kaart naar aflegstapel doen
                    Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                    kaarten_Stapel1.Kaart_Id = Id;
                    kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                    if (kaarten_Stapel1.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                        if (ok <= 0)
                        {
                            foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                        }
                    }
                    else
                    {
                        foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                    }
                    //return
                    if (string.IsNullOrEmpty(foutmelding))
                    {
                        return $"{Naam} achtervolgt je niet!";
                    }
                    else
                    {
                        return foutmelding;
                    }

                }
                else if (Naam.ToUpper().Contains("GEBROEDERS WIGHT") && vluchter.Level <= 3)
                {
                    //kaart naar aflegstapel doen
                    Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                    kaarten_Stapel1.Kaart_Id = Id;
                    kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                    if (kaarten_Stapel1.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                        if (ok <= 0)
                        {
                            foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                        }
                    }
                    else
                    {
                        foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                    }
                    //return
                    if (string.IsNullOrEmpty(foutmelding))
                    {
                        return $"{Naam} achtervolgt je niet!";
                    }
                    else
                    {
                        return foutmelding;
                    }
                }
                else if (Naam.ToUpper().Contains("PLUTONIUM DRAAK") && vluchter.Level <= 5)
                {
                    //kaart naar aflegstapel doen
                    Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                    kaarten_Stapel1.Kaart_Id = Id;
                    kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                    if (kaarten_Stapel1.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                        if (ok <= 0)
                        {
                            foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                        }
                    }
                    else
                    {
                        foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                    }
                    //return
                    if (string.IsNullOrEmpty(foutmelding))
                    {
                        return $"{Naam} achtervolgt je niet!";
                    }
                    else
                    {
                        return foutmelding;
                    }
                }
                else if (Naam.ToUpper().Contains("KONING TOET") && vluchter.Level <= 3)
                {
                    //kaart naar aflegstapel doen
                    Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                    kaarten_Stapel1.Kaart_Id = Id;
                    kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                    if (kaarten_Stapel1.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                        if (ok <= 0)
                        {
                            foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                        }
                    }
                    else
                    {
                        foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                    }
                    //return
                    if (string.IsNullOrEmpty(foutmelding))
                    {
                        return $"{Naam} achtervolgt je niet!";
                    }
                    else
                    {
                        return foutmelding;
                    }
                }
                else if (Naam.ToUpper() == "WIETPLANT")
                {
                    return $"Je bent kunnen vluchten van {Naam}";
                }
                else if (dobbelsteenworp < 5 || Naam.ToUpper() == "SCHAAMLUIS")
                {
                    //portie ellende uitvoeren
                    if (Naam.ToUpper() == "LAMME GOBLIN")
                    {
                        vluchter.Level -= 1;
                    }
                    if (Naam.ToUpper() == "MAGERE HEIN" || Naam.ToUpper() == "ONDOOD PAARD" || Naam.ToUpper() == "PADDOS")
                    {
                        vluchter.Level -= 2;
                    }
                    if (Naam.ToUpper() == "GLITTERENDE VAMPIEREN")
                    {
                        int laagsteLevel = vluchter.Level ?? default;
                        List<Wedstrijd_Speler> spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(vluchter.Wedstrijd_Id ?? default);
                        foreach (Wedstrijd_Speler speler in spelers)
                        {
                            if (speler.Level < laagsteLevel)
                            {
                                laagsteLevel = speler.Level ?? default;
                            }
                        }
                        vluchter.Level = laagsteLevel;
                    }
                    if (Naam.ToUpper() == "SCHAAMLUIS")
                    {
                        foutmelding += veldkaarten.TypeUitStapelHalen(kerkerkaartenAflegstapel, "HARNAS");
                        foutmelding += veldkaarten.TypeUitStapelHalen(kerkerkaartenAflegstapel, "SCHOEISEL");

                    }
                    if (Naam.ToUpper() == "PLATVOET")
                    {
                        foutmelding += veldkaarten.TypeUitStapelHalen(kerkerkaartenAflegstapel, "HOOFDDEKSEL");
                    }
                    if (Naam.ToUpper() == "KWIJLEND SLIJM")
                    {
                        bool schoeiselVerloren = false;
                        List<Kaarten_Stapel> kaartenstapels = new List<Kaarten_Stapel>();
                        foreach (Kaarten_Stapel veldkaart in veldkaarten.Kaarten_Stapels)
                        {
                            kaartenstapels.Add(veldkaart);
                        }
                        for (int i = 0; i < kaartenstapels.Count(); i++)
                        {
                            //check of er al een kaart van dit type zit in de veldkaarten
                            string type = DatabaseOperations.OphalenType(kaartenstapels[i].Kaart.Type_id).Soort.ToUpper();
                            if (type.ToUpper().Contains("SCHOEISEL"))
                            {
                                //Harnas of schoeisel uit veldkaarten halen
                                foutmelding += kaartenstapels[i].KaartVanStapelWisselen(kerkerkaartenAflegstapel);
                                schoeiselVerloren = true;
                            }
                        }
                        if (schoeiselVerloren == false)
                        {
                            vluchter.Level -= 1;
                        }
                    }
                    if (Naam.ToUpper() == "SNOETZUIGER")
                    {
                        bool hoofddekselVerloren = false;
                        List<Kaarten_Stapel> kaartenstapels = new List<Kaarten_Stapel>();
                        foreach (Kaarten_Stapel veldkaart in veldkaarten.Kaarten_Stapels)
                        {
                            kaartenstapels.Add(veldkaart);
                        }
                        for (int i = 0; i < kaartenstapels.Count(); i++)
                        {
                            //check of er al een kaart van dit type zit in de veldkaarten
                            string type = DatabaseOperations.OphalenType(kaartenstapels[i].Kaart.Type_id).Soort.ToUpper();
                            if (type.ToUpper().Contains("HOOFDDEKSEL"))
                            {
                                //Harnas of schoeisel uit veldkaarten halen
                                foutmelding += kaartenstapels[i].KaartVanStapelWisselen(kerkerkaartenAflegstapel);
                                hoofddekselVerloren = true;
                            }
                        }
                        if (hoofddekselVerloren == false)
                        {
                            vluchter.Level -= 1;
                        }
                    }
                    if (Naam.ToUpper() == "ACHTZIJDIGE DRILPUDDING")
                    {
                        List<Kaarten_Stapel> kaartenstapels = new List<Kaarten_Stapel>();
                        foreach (Kaarten_Stapel veldkaart in veldkaarten.Kaarten_Stapels)
                        {
                            kaartenstapels.Add(veldkaart);
                        }
                        for (int i = 0; i < kaartenstapels.Count(); i++)
                        {
                            //check of er al een grote kaart zit in de veldkaarten
                            if (kaartenstapels[i].Kaart.Schatkaart.Is_Groot == true)
                            {
                                //groot voorwerp uit veldkaarten halen
                                foutmelding += kaartenstapels[i].KaartVanStapelWisselen(kerkerkaartenAflegstapel);
                            }
                        }
                    }
                    if (Naam.ToUpper() == "GEBROEDERS WIGHT")
                    {
                        vluchter.Level = 1;
                    }
                    if (Naam.ToUpper() == "KONING TOET")
                    {
                        //Alle hand en veldkaarten wegdoen
                        veldkaarten.StapelLeegmaken(schatkaartenAflegstapel, kerkerkaartenAflegstapel);
                        handkaarten.StapelLeegmaken(schatkaartenAflegstapel, kerkerkaartenAflegstapel);
                    }
                    if (Naam.ToUpper() == "PLUTONIUM DRAAK" || Naam.ToUpper() == "KWELROG")
                    {
                        vluchter.Level = 1;
                        //Alle hand en veldkaarten wegdoen
                        veldkaarten.StapelLeegmaken(schatkaartenAflegstapel, kerkerkaartenAflegstapel);
                        handkaarten.StapelLeegmaken(schatkaartenAflegstapel, kerkerkaartenAflegstapel);
                    }

                    //kaart naar aflegstapel doen
                    Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                    kaarten_Stapel1.Kaart_Id = Id;
                    kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                    if (kaarten_Stapel1.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                        if (ok <= 0)
                        {
                            foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                        }
                    }
                    else
                    {
                        foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                    }
                    //aanpassen speler
                    foutmelding += vluchter.PasWedstrijd_SpelerAan();
                    //return
                    if (string.IsNullOrEmpty(foutmelding))
                    {
                        return $"Je bent niet kunnen vluchten van {Naam}";
                    }
                    else
                    {
                        return foutmelding;
                    }
                }
                else
                {
                    //vluchten succesvol
                    //if statements voor nadelen die je krijgt van bepaalde monsters zelfs als je kan vluchten
                    if (Naam.ToUpper() == "KONING TOET" || Naam.ToUpper() == "GEBROEDERS WIGHT")
                    {
                        vluchter.Level -= 2;
                    }
                    if (Naam.ToUpper() == "MAGERE HEIN")
                    {
                        vluchter.Level -= 1;
                    }
                    //kaart naar aflegstapel doen
                    //kaart naar aflegstapel doen
                    Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                    kaarten_Stapel1.Kaart_Id = Id;
                    kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                    if (kaarten_Stapel1.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                        if (ok <= 0)
                        {
                            foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                        }
                    }
                    else
                    {
                        foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                    }
                    //aanpassen speler
                    foutmelding += vluchter.PasWedstrijd_SpelerAan();
                    //return
                    if (string.IsNullOrEmpty(foutmelding))
                    {
                        return $"Je bent kunnen vluchten van {Naam}";
                    }
                    else
                    {
                        return foutmelding;
                    }
                }
            }
            else
            {
                return $"Je kan winnen tegen {Naam}";
            }
        }
        public string SpeelRas(Wedstrijd_Speler gebruiker)
        {
            string foutmelding = "";
            //check if speler al een ras heeft
            if (gebruiker.Ras.ToUpper() == "MENS")
            {
                // haal huidige kaartenstapel van de kaart op en haal de veldkaarten van de Speler op.
                Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
                Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
                //pas ras wedstrijd_speler aan
                gebruiker.Ras = Naam;
                // herbereken de bonussen van de speler (miss geeft een item nu geen bonus meer door verandering van ras
                gebruiker.HerberekenBonussenVeldkaarten(veldkaarten);
                //Kaart van handkaarten naar veldkaarten wisselen
                foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(veldkaarten);
                //als het wisselen van de stapel goed gelukt is mag de speler aanpassing worden doorgevoerd
                if (foutmelding == "")
                {
                    foutmelding += gebruiker.PasWedstrijd_SpelerAan();
                }
            }
            else
            {
                foutmelding += "Je hebt al een ras, leg deze eerst af indien je een nieuw ras wil gebruiken\n";
            }
            if (string.IsNullOrEmpty(foutmelding))
            {
                return $"Je bent van ras veranderd, je bent nu een {gebruiker.Ras}!";
            }
            else
            {
                return foutmelding;
            }
        }
        public string SpeelHoofddekselOfHarnasOfSchoeisel(Wedstrijd_Speler gebruiker, Type type, string bericht1)
        {
            string foutmelding = "";
            //haal veldkaarten van de speler op en huidige kaartenstapel van de kaart
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //loop daar alle kaartenstapels van veldkaarten
            foreach (var kaarten_Stapel in veldkaarten.Kaarten_Stapels)
            {
                //check of er al een kaart van dit type zit in de veldkaarten
                if (DatabaseOperations.OphalenType(kaarten_Stapel.Kaart.Type_id).Soort.ToUpper().Contains(type.Soort.ToUpper()))
                {
                    return bericht1 + kaarten_Stapel.Kaart.Naam;
                }
            }
            //controleer op grote voorwerpen
            string groot = ControleerGroteVoorwerpen(veldkaarten);
            if (groot != "")
            {
                return "je kan geen 2 grote voorwerpen dragen";
            }
            //kaart van handkaarten naar veldkaarten wisselen
            foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(veldkaarten);
            //Bonussen bij gebruiker optellen
            BonussenItemToevoegen(gebruiker);
            // als alles tot nu toe gelukt is dan wedstrijd speler aanpassen
            if (foutmelding == "")
            {
                foutmelding += gebruiker.PasWedstrijd_SpelerAan();
            }
            if (string.IsNullOrEmpty(foutmelding))
            {
                return $"Je hebt {Naam} gebruikt!";
            }
            else
            {
                return foutmelding;
            }
        }
        public string SpeelExtra(Wedstrijd_Speler gebruiker)
        {
            string foutmelding = "";
            //huidige kaartenstapel en veldkaarten opvragen
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //Controleer op grote voorwerpen
            string groot = ControleerGroteVoorwerpen(veldkaarten);
            if (groot != "")
            {
                return "je kan geen 2 grote voorwerpen dragen";
            }
            //kaart van handkaarten naar veldkaarten wisselen
            foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(veldkaarten);
            //Bonussen bij gebruiker optellen
            BonussenItemToevoegen(gebruiker);
            // als alles tot nu toe gelukt is dan wedstrijd speler aanpassen
            if (foutmelding == "")
            {
                foutmelding += gebruiker.PasWedstrijd_SpelerAan();
            }
            if (string.IsNullOrEmpty(foutmelding))
            {
                return $"Je bent nu uitgerust met een {Naam}";
            }
            else
            {
                return foutmelding;
            }
        }
        public string Speel1Hand(Wedstrijd_Speler gebruiker)
        {
            string foutmelding = "";
            //Huidige kaartenstapel en veldkaarten opvragen
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);

            int handenVol = 0;
            //loop daar alle kaartenstapels van veldkaarten om te kijken of er genoeg handen vrij zijn
            foreach (var kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
            {
                //check of er al een hoofddeksel zit in de veldkaarten
                if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("1HAND"))
                {
                    handenVol += 1;
                }
                if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("2HANDEN"))
                {
                    handenVol += 2;
                }
                if (handenVol >= 2)
                {
                    return "je hebt je 2 handen al vol.";
                }
            }
            //Controleer op grote voorwerpen
            string groot = ControleerGroteVoorwerpen(veldkaarten);
            if (groot != "")
            {
                return "je kan geen 2 grote voorwerpen dragen";
            }
            //kaart van handkaarten naar veldkaarten wisselen
            foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(veldkaarten);
            //Bonussen bij gebruiker optellen
            BonussenItemToevoegen(gebruiker);
            // als alles tot nu toe gelukt is dan wedstrijd speler aanpassen
            if (foutmelding == "")
            {
                foutmelding += gebruiker.PasWedstrijd_SpelerAan();
            }
            if (string.IsNullOrEmpty(foutmelding))
            {
                return $"Je hebt nu een {Naam} in je hand!";
            }
            else
            {
                return foutmelding;
            }
        }
        public string Speel2Handen(Wedstrijd_Speler gebruiker)
        {
            string foutmelding = "";
            //huidige kaartenstapel en veldkaarten ophalen
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //loop daar alle kaartenstapels van veldkaarten
            foreach (var kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
            {
                //check of er al een hoofddeksel zit in de veldkaarten
                if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("1HAND") || DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("2HANDEN"))
                {
                    return "je hebt niet genoeg handen vrij.";
                }
            }
            //Controleer op grote voorwerpen
            string groot = ControleerGroteVoorwerpen(veldkaarten);
            if (groot != "")
            {
                return "je kan geen 2 grote voorwerpen dragen";
            }
            //kaart van handkaarten naar veldkaarten wisselen
            foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(veldkaarten);
            //Bonussen bij gebruiker optellen
            BonussenItemToevoegen(gebruiker);
            // als alles tot nu toe gelukt is dan wedstrijd speler aanpassen
            if (foutmelding == "")
            {
                foutmelding += gebruiker.PasWedstrijd_SpelerAan();
            }
            if (string.IsNullOrEmpty(foutmelding))
            {
                return $"Je hebt nu een {Naam} in je handen!";
            }
            else
            {
                return foutmelding;
            }
        }
        public string SpeelVervloeking(Wedstrijd_Speler gebruiker)
        {
            string ret = SpeelVervloeking(gebruiker, gebruiker);
            return ret;
        }
        public string SpeelVervloeking(Wedstrijd_Speler gebruiker, Wedstrijd_Speler slachtoffer)
        {
            string foutmelding = "";
            //aflegstapels ophalen
            Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(gebruiker.Wedstrijd_Id ?? default);
            Stapel aflegstapelKerkerkaarten = DatabaseOperations.OphalenStapelViaId(wedstrijd.Kerkerkaarten_Aflegstapel_Id);
            Stapel aflegstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(wedstrijd.Schatkaarten_Aflegstapel_Id);
            //kaartenstapel ophalen om later de kaart te verwijderen uit hand
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //veldkaarten slachtoffer voor herberekenen bonussen
            Stapel veldkaartenSlachtoffer = DatabaseOperations.OphalenStapelViaId(slachtoffer.Veldkaarten_Id);
            //bonus kaart ophalen
            List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            //checken waarop (negatieve)bonus en juiste actie doen
            foreach (var bonus in lijstBonussen)
            {
                if (bonus.Waarop_Effect.ToUpper() == "LEVEL")
                {
                    //level aftrekken
                    slachtoffer.Level += bonus.Waarde;
                    if (slachtoffer.Level <= 0)
                    {
                        slachtoffer.Level = 1;
                    }
                }
                //voorlopig alleen negatieve bonussen op level
            }
            //checken of je iets moet verliezen
            if (Naam.ToUpper().Contains("VERLIES"))
            {

                //Ras verliezen
                if (Naam.ToUpper().Contains("RAS"))
                {
                    slachtoffer.Ras = "Mens";
                    //raskaart uit veldkaarten halen
                    foreach (var kaarten_Stapel1 in veldkaartenSlachtoffer.Kaarten_Stapels)
                    {
                        int rasweg = 0;
                        //check of er een raskaart in veldkaarten zit
                        if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("RAS"))
                        {
                            //Als er een ras in veldkaarten zit, die eruit halen en in aflegstapel kerkerkaarten steken
                            foutmelding += kaarten_Stapel1.KaartVanStapelWisselen(aflegstapelKerkerkaarten);
                            if (foutmelding == "")
                            {
                                rasweg += 1;
                            }
                        }
                        if (rasweg > 0)
                        {
                            break;
                        }
                    }
                }
                //Als je harnas moet verliezen
                else if (Naam.ToUpper().Contains("HARNAS"))
                {
                    foreach (var kaarten_Stapel1 in veldkaartenSlachtoffer.Kaarten_Stapels)
                    {
                        int harnasweg = 0;
                        //check of er al een kaart van dit type in veldkaarten zit
                        if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("HARNAS"))
                        {
                            //Als er een harnas in veldkaarten zit, die eruit halen en in aflegstapel schatkaarten steken
                            foutmelding += kaarten_Stapel1.KaartVanStapelWisselen(aflegstapelSchatkaarten);
                            if (foutmelding == "")
                            {
                                harnasweg += 1;
                            }
                        }
                        if (harnasweg > 0)
                        {
                            break;
                        }
                    }
                }
                //Als je hoofddeksel moet verliezen
                else if (Naam.ToUpper().Contains("HOOFDDEKSEL"))
                {
                    foreach (var kaarten_Stapel1 in veldkaartenSlachtoffer.Kaarten_Stapels)
                    {
                        int hoofddekselweg = 0;
                        //check of er al een kaart van dit type in veldkaarten zit
                        if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("HOOFDDEKSEL"))
                        {
                            //Als er een hoofddeksel in veldkaarten zit, die eruit halen en in aflegstapel schatkaarten steken
                            foutmelding += kaarten_Stapel1.KaartVanStapelWisselen(aflegstapelSchatkaarten);
                            if (foutmelding == "")
                            {
                                hoofddekselweg += 1;
                            }
                        }
                        if (hoofddekselweg > 0)
                        {
                            break;
                        }
                    }
                }
                //als je je schoeisel moet verliezen
                else if (Naam.ToUpper().Contains("SCHOEISEL"))
                {
                    foreach (var kaarten_Stapel1 in veldkaartenSlachtoffer.Kaarten_Stapels)
                    {
                        int schoeiselweg = 0;
                        //check of er al een kaart van dit type in veldkaarten zit
                        if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("SCHOEISEL"))
                        {
                            //Als er schoenen in veldkaarten zitten, die eruit halen en in aflegstapel schatkaarten steken
                            foutmelding += kaarten_Stapel1.KaartVanStapelWisselen(aflegstapelSchatkaarten);
                            if (foutmelding == "")
                            {
                                schoeiselweg += 1;
                            }
                        }
                        if (schoeiselweg > 0)
                        {
                            break;
                        }
                    }
                }
            }
            //Checken of alle bonussen nog gelden en gevechtswaarde herberekenen
            slachtoffer.HerberekenBonussenVeldkaarten(veldkaartenSlachtoffer);
            //kaart naar aflegstapel doen
            foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(aflegstapelKerkerkaarten);
            // als alles tot nu toe gelukt is dan wedstrijd speler aanpassen
            if (foutmelding == "")
            {
                foutmelding += slachtoffer.PasWedstrijd_SpelerAan();
            }
            if (string.IsNullOrEmpty(foutmelding))
            {
                return $"Je hebt {Naam} gebruikt!";
            }
            else
            {
                return foutmelding;
            }
        }
        public string SpeelGebruikskaart(Wedstrijd_Speler gebruiker)
        {
            string foutmelding = "";
            Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(gebruiker.Wedstrijd_Id ?? default);
            Stapel aflegstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(wedstrijd.Schatkaarten_Aflegstapel_Id);
            //kaartenstapel ophalen om later de kaart te verwijderen uit hand
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //bonus kaart ophalen
            List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            //loopen door de bonus(sen)
            foreach (var bonus in lijstBonussen)
            {
                if (bonus.Waarop_Effect.ToUpper() == "MONSTER")
                {
                    return "Je kan deze kaart enkel op een monster gebruiken!";
                }
                if (bonus.Waarop_Effect.ToUpper() == "LEVEL")
                {
                    if (Naam.ToUpper().Contains("ZEUREN"))
                    {
                        List<Wedstrijd_Speler> wedstrijd_Spelers = DatabaseOperations.OphalenWedstrijd_SpelersViaWedstrijdId(gebruiker.Wedstrijd_Id ?? default);
                        bool hoogste = true;
                        foreach (Wedstrijd_Speler speler in wedstrijd_Spelers)
                        {
                            if (speler.Level > gebruiker.Level)
                            {
                                hoogste = false;
                            }
                        }
                        if (hoogste == false)
                        {
                            if (gebruiker.Level < 9)
                            {
                                gebruiker.Level += bonus.Waarde;
                            }
                            else
                            {
                                return $"Je kan {Naam} niet gebruiken om level 10 te worden";
                            }
                        }
                        else
                        {
                            return $"Je kan {Naam} nu niet gebruiken, je hebt het hoogste level";
                        }
                    }
                    else
                    {
                        //level optellen
                        if (gebruiker.Level < 9)
                        {
                            gebruiker.Level += bonus.Waarde;
                        }
                        else
                        {
                            return $"Je kan {Naam} niet gebruiken om level 10 te worden";
                        }
                    }
                }
                if (bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
                {
                    gebruiker.Tijdelijke_Bonus += bonus.Waarde;
                }
            }
            //kaart naar aflegstapel doen
            foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(aflegstapelSchatkaarten);
            // als alles tot nu toe gelukt is dan wedstrijd speler aanpassen
            if (foutmelding == "")
            {
                foutmelding += gebruiker.PasWedstrijd_SpelerAan();
            }
            if (string.IsNullOrEmpty(foutmelding))
            {
                return $"Je hebt {Naam} gebruikt!";
            }
            else
            {
                return foutmelding;
            }

        }
        public string SpeelGebruikskaart(Wedstrijd_Speler gebruiker, Wedstrijd_Speler slachtoffer)
        {
            string foutmelding = "";
            Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(gebruiker.Wedstrijd_Id ?? default);
            Stapel aflegstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(wedstrijd.Schatkaarten_Aflegstapel_Id);
            //kaartenstapel ophalen om later de kaart te verwijderen uit hand
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //bonus kaart ophalen
            List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            //loopen door de bonus(sen)
            foreach (var bonus in lijstBonussen)
            {
                if (bonus.Waarop_Effect.ToUpper() == "MONSTER")
                {
                    return "Je kan deze kaart enkel op een monster gebruiken!";
                }
                if (bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
                {
                    //Bonus toevoegen aan gevechtswaarde
                    slachtoffer.Tijdelijke_Bonus += bonus.Waarde;
                }
                //voorlopig alleen kaarten met effect op gevechtswaarde
                if (bonus.Waarop_Effect.ToUpper() == "LEVEL")
                {
                    return "Je kan deze kaart enkel op jezelf gebruiken";
                }
            }
            //kaart naar aflegstapel doen
            foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(aflegstapelSchatkaarten);
            // als alles tot nu toe gelukt is dan wedstrijd speler aanpassen
            if (foutmelding == "")
            {
                foutmelding += slachtoffer.PasWedstrijd_SpelerAan();
            }
            if (string.IsNullOrEmpty(foutmelding))
            {
                return $"Je hebt {Naam} gebruikt!";
            }
            else
            {
                return foutmelding;
            }
        }
        public string SpeelGebruikskaart(Wedstrijd_Speler gebruiker, Kaart monster)
        {
            string foutmelding = "";
            Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(gebruiker.Wedstrijd_Id ?? default);
            Stapel aflegstapelSchatkaarten = DatabaseOperations.OphalenStapelViaId(wedstrijd.Schatkaarten_Aflegstapel_Id);
            Stapel aflegstapelKerkerkaarten = DatabaseOperations.OphalenStapelViaId(wedstrijd.Kerkerkaarten_Aflegstapel_Id);
            //kaartenstapel ophalen om later de kaart te verwijderen uit hand
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //bonus kaart ophalen
            List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            //loopen door de bonus(sen)
            foreach (var bonus in lijstBonussen)
            {
                if (bonus.Waarop_Effect.ToUpper() == "MONSTER" || bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
                {
                    //Bonus toevoegen aan gevechtswaarde
                    monster.Kerkerkaart.Tijdelijke_Bonus += bonus.Waarde;
                }
                if (bonus.Waarop_Effect.ToUpper() == "AANTAL_SCHATTEN")
                {
                    //miss ook dboperations toevoegen
                    monster.Kerkerkaart.Aantal_schatten += bonus.Waarde;
                    if (monster.Kerkerkaart.Aantal_schatten < 1)
                    {
                        monster.Kerkerkaart.Aantal_schatten = 1;
                    }
                }
            }
            if (Kerkerkaart == null)
            {
                //kaart naar aflegstapel schatkaarten doen
                foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(aflegstapelSchatkaarten);
            }
            else
            {
                //kaart naar aflegstapel kerkerkaarten doen
                foutmelding += oudeKaartenStapel.KaartVanStapelWisselen(aflegstapelKerkerkaarten);
            }
            if (string.IsNullOrEmpty(foutmelding))
            {
                return $"Je hebt {Naam} gebruikt!";
            }
            else
            {
                return foutmelding;
            }
        }
        public string ControleerGroteVoorwerpen(Stapel veldkaarten)
        {
            foreach (var kaarten_Stapel in veldkaarten.Kaarten_Stapels)
            {
                Kaart kaart = DatabaseOperations.OphalenKaartViaId(kaarten_Stapel.Kaart_Id);
                if (Schatkaart != null)
                {
                    if (Schatkaart.Is_Groot == true && kaart.Schatkaart.Is_Groot == true)
                    {
                        return "je kan geen 2 grote voorwerpen dragen";
                    }
                }
            }
            return "";
        }
        public void BonussenItemToevoegen(Wedstrijd_Speler gebruiker)
        {
            //Bonus van kaart opvragen
            List<Bonus> lijstBonusssen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            foreach (var bonus in lijstBonusssen)
            {
                //Checken of hij dit kan gebruiken (hij equipt het sowieso want dat mag maar hij krijgt de bonus niet
                if (bonus.Bruikbaar_Door.ToUpper() == gebruiker.Ras.ToUpper() || bonus.Bruikbaar_Door.ToUpper() == "IEDEREEN")
                {
                    if (bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
                    {
                        //Bonus toevoegen aan gevechtswaarde
                        gebruiker.Gevechtsbonus += bonus.Waarde;
                    }
                    else if (bonus.Waarop_Effect.ToUpper() == "VLUCHTEN")
                    {
                        //Bonus toevoegen aan vluchtbonus
                        gebruiker.Vluchtbonus += bonus.Waarde;
                    }
                }
            }
        }

        public override string ToString()
        {
            return Naam;
        }
    }
}
