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
                if (columnName == "Id" && Id >= 0)
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
                if (columnName == "Type_id" && Type_id >= 0)
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
            else
            {
                return "Je kan deze kaart nu niet gebruiken";
            }
        }
        public string SpeelKaart(Wedstrijd_Speler gebruiker, Wedstrijd_Speler slachtoffer)
        {
            Type type = DatabaseOperations.OphalenType(Type_id);
            if (type.Soort.ToUpper() == "Vervloeking")
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
        public string VechtMonster(Wedstrijd_Speler vechter, Kaart monster)
        {
            string foutmelding = "";
            int spelersterkte;
            //Waars dbOperation
            int monsterSterkte = monster.Kerkerkaart.Level + monster.Kerkerkaart.Tijdelijke_Bonus ?? default;
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
                //default checken, weet niet of dit werkt
                int aantalSchattenTrekken = Kerkerkaart.Aantal_schatten ?? default;
                if (vechter.Ras.ToUpper().Contains("ELF") && Naam.ToUpper() == "WIETPLANT")
                {
                    aantalSchattenTrekken += 1;
                }
                for (int i = 0; i < aantalSchattenTrekken; i++)
                {
                    Kaarten_Stapel kaarten_Stapel = new Kaarten_Stapel();
                    kaarten_Stapel.Kaart_Id = trekstapelSchatkaartenKaarten_Stapels[i].Kaart_Id;
                    kaarten_Stapel.Stapel_Id = vechter.Handkaarten_Id;
                    //toevoegen kaartenstapel
                    if (kaarten_Stapel.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel);
                        if (ok <= 0)
                        {
                            foutmelding += "Kaart niet in hand kunnen plaatsen\n";
                        }
                    }
                    else
                    {
                        foutmelding += kaarten_Stapel.Error + Environment.NewLine;
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
        public string VechtMonster(Wedstrijd_Speler vechter, Kaart monster, Wedstrijd_Speler helper)
        {
            string foutmelding = "";
            int spelersterkte;
            //Waars dbOperation
            int monsterSterkte = monster.Kerkerkaart.Level + monster.Kerkerkaart.Tijdelijke_Bonus ?? default;
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
                //levels omhoog, waars dboperations
                vechter.Level += Kerkerkaart.Aantal_Levels;
                if (vechter.IsGeldig())
                {
                    int ok2 = DatabaseOperations.AanpassenWedstrijd_Speler(vechter);
                    if (ok2 <= 0)
                    {
                        foutmelding += "Je level is niet gestegen\n";
                    }
                }
                else
                {
                    foutmelding += vechter.Error + Environment.NewLine;
                }

                if (helper.Ras.ToUpper() == "ELF")
                {
                    helper.Level += 1;
                    if (helper.IsGeldig())
                    {
                        int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(vechter);
                        if (ok3 <= 0)
                        {
                            foutmelding += "level van de helper is niet gestegen\n";
                        }
                    }
                    else
                    {
                        foutmelding += helper.Error + Environment.NewLine;
                    }
                }
                //schatkaarten trekken
                int wedstrijdId = vechter.Wedstrijd_Id ?? default;
                Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
                List<Kaarten_Stapel> trekstapelSchatkaartenKaarten_Stapels = DatabaseOperations.OphalenKaarten_StapelsViaStapelId(wedstrijd.Schatkaarten_Trekstapel_Id);
                //default checken, weet niet of dit werkt
                int aantalSchattenTrekken = Kerkerkaart.Aantal_schatten ?? default;
                if (vechter.Ras.ToUpper().Contains("ELF") && Naam.ToUpper() == "WIETPLANT")
                {
                    aantalSchattenTrekken += 1;
                }
                if (helper.Ras.ToUpper().Contains("ELF") && Naam.ToUpper() == "WIETPLANT")
                {
                    aantalSchattenTrekken += 1;
                }
                for (int i = 0; i < aantalSchattenTrekken; i++)
                {
                    if (i % 2 == 0)
                    {
                        Kaarten_Stapel kaarten_Stapel = new Kaarten_Stapel();
                        kaarten_Stapel.Kaart_Id = trekstapelSchatkaartenKaarten_Stapels[i].Kaart_Id;
                        kaarten_Stapel.Stapel_Id = helper.Handkaarten_Id;
                        //toevoegen kaartenstapel
                        if (kaarten_Stapel.IsGeldig())
                        {
                            int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel);
                            if (ok <= 0)
                            {
                                foutmelding += "Kaart niet in hand kunnen plaatsen\n";
                            }
                        }
                        else
                        {
                            foutmelding += kaarten_Stapel.Error + Environment.NewLine;
                        }

                    }
                    else
                    {
                        Kaarten_Stapel kaarten_Stapel = new Kaarten_Stapel();
                        kaarten_Stapel.Kaart_Id = trekstapelSchatkaartenKaarten_Stapels[i].Kaart_Id;
                        kaarten_Stapel.Stapel_Id = vechter.Handkaarten_Id;
                        //toevoegen kaartenstapel
                        if (kaarten_Stapel.IsGeldig())
                        {
                            int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel);
                            if (ok <= 0)
                            {
                                foutmelding += "Kaart niet in hand kunnen plaatsen\n";
                            }
                        }
                        else
                        {
                            foutmelding += kaarten_Stapel.Error + Environment.NewLine;
                        }

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
        public string VluchtMonster(Wedstrijd_Speler vluchter, Kaart monster, int dobbelsteenworp)
        {
            string foutmelding = "";
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
            //Waars dbOperation
            int monsterSterkte = monster.Kerkerkaart.Level + monster.Kerkerkaart.Tijdelijke_Bonus ?? default;
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
                    int wedstrijdId = vluchter.Wedstrijd_Id ?? default;
                    Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
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
                    return $"{Naam} achtervolgt je niet!";
                }
                else if (Naam.ToUpper().Contains("GEBROEDERS WIGHT") && vluchter.Level <= 3)
                {
                    //kaart naar aflegstapel doen
                    int wedstrijdId = vluchter.Wedstrijd_Id ?? default;
                    Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
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
                    return $"{Naam} achtervolgt je niet!";
                }
                else if (Naam.ToUpper().Contains("PLUTONIUM DRAAK") && vluchter.Level <= 5)
                {
                    //kaart naar aflegstapel doen
                    int wedstrijdId = vluchter.Wedstrijd_Id ?? default;
                    Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
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
                    return $"{Naam} achtervolgt je niet!";
                }
                else if (Naam.ToUpper().Contains("KONING TOET") && vluchter.Level <= 3)
                {
                    //kaart naar aflegstapel doen
                    int wedstrijdId = vluchter.Wedstrijd_Id ?? default;
                    Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
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
                    return $"{Naam} achtervolgt je niet!";
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
                        Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(vluchter.Veldkaarten_Id);
                        foreach (Kaarten_Stapel veldkaart in veldkaarten.Kaarten_Stapels)
                        {
                            //check of er al een kaart van dit type zit in de veldkaarten
                            string type = DatabaseOperations.OphalenType(veldkaart.Kaart.Type_id).Soort.ToUpper();
                            if (type.Contains("HARNAS"))
                            {
                                //Harnas uit veldkaarten halen
                                int ok = DatabaseOperations.VerwijderenKaarten_Stapel(veldkaart);
                                if (ok > 0)
                                {
                                    //kaart in aflegstapel steken
                                    int wedstrijdId1 = vluchter.Wedstrijd_Id ?? default;
                                    Wedstrijd wedstrijd1 = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId1);
                                    Kaarten_Stapel kaarten_Stapel2 = new Kaarten_Stapel();
                                    kaarten_Stapel2.Kaart_Id = Id;
                                    kaarten_Stapel2.Stapel_Id = wedstrijd1.Kerkerkaarten_Aflegstapel_Id;
                                    if (kaarten_Stapel2.IsGeldig())
                                    {
                                        int ok1 = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel2);
                                        if (ok1 <= 0)
                                        {
                                            foutmelding += "Harnas niet in aflegstapel kunnen plaatsen.\n";
                                        }
                                    }
                                    else
                                    {
                                        foutmelding += kaarten_Stapel2.Error + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    foutmelding += "Harnas niet uit veldkaarten kunnen halen";
                                }
                            }
                            if (type.Contains("SCHOEISEL"))
                            {
                                //Harnas uit veldkaarten halen
                                int ok = DatabaseOperations.VerwijderenKaarten_Stapel(veldkaart);
                                if (ok > 0)
                                {
                                    //kaart in aflegstapel steken
                                    int wedstrijdId1 = vluchter.Wedstrijd_Id ?? default;
                                    Wedstrijd wedstrijd1 = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId1);
                                    Kaarten_Stapel kaarten_Stapel2 = new Kaarten_Stapel();
                                    kaarten_Stapel2.Kaart_Id = Id;
                                    kaarten_Stapel2.Stapel_Id = wedstrijd1.Kerkerkaarten_Aflegstapel_Id;
                                    if (kaarten_Stapel2.IsGeldig())
                                    {
                                        int ok1 = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel2);
                                        if (ok1 <= 0)
                                        {
                                            foutmelding += "Schoeisel niet in aflegstapel kunnen plaatsen.\n";
                                        }
                                    }
                                    else
                                    {
                                        foutmelding += kaarten_Stapel2.Error + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    foutmelding += "Schoeisel niet uit veldkaarten kunnen halen";
                                }
                            }
                        }
                    }
                    if (Naam.ToUpper() == "PLATVOET")
                    {
                        Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(vluchter.Veldkaarten_Id);
                        foreach (Kaarten_Stapel veldkaart in veldkaarten.Kaarten_Stapels)
                        {
                            //check of er al een kaart van dit type zit in de veldkaarten
                            string type = DatabaseOperations.OphalenType(veldkaart.Kaart.Type_id).Soort.ToUpper();
                            if (type.Contains("HOOFDDEKSEL"))
                            {
                                //Harnas uit veldkaarten halen
                                int ok = DatabaseOperations.VerwijderenKaarten_Stapel(veldkaart);
                                if (ok > 0)
                                {
                                    //kaart in aflegstapel steken
                                    int wedstrijdId1 = vluchter.Wedstrijd_Id ?? default;
                                    Wedstrijd wedstrijd1 = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId1);
                                    Kaarten_Stapel kaarten_Stapel2 = new Kaarten_Stapel();
                                    kaarten_Stapel2.Kaart_Id = Id;
                                    kaarten_Stapel2.Stapel_Id = wedstrijd1.Kerkerkaarten_Aflegstapel_Id;
                                    if (kaarten_Stapel2.IsGeldig())
                                    {
                                        int ok1 = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel2);
                                        if (ok1 <= 0)
                                        {
                                            foutmelding += "Hoofddeksel niet in aflegstapel kunnen plaatsen.\n";
                                        }
                                    }
                                    else
                                    {
                                        foutmelding += kaarten_Stapel2.Error + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    foutmelding += "Hoofddeksel niet uit veldkaarten kunnen halen";
                                }
                            }
                        }
                    }
                    if (Naam.ToUpper() == "KWIJLEND SLIJM")
                    {
                        bool schoeiselVerloren = false;
                        Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(vluchter.Veldkaarten_Id);
                        foreach (Kaarten_Stapel veldkaart in veldkaarten.Kaarten_Stapels)
                        {
                            //check of er al een kaart van dit type zit in de veldkaarten
                            string type = DatabaseOperations.OphalenType(veldkaart.Kaart.Type_id).Soort.ToUpper();
                            if (type.Contains("SCHOEISEL"))
                            {
                                //Harnas uit veldkaarten halen
                                int ok = DatabaseOperations.VerwijderenKaarten_Stapel(veldkaart);
                                if (ok > 0)
                                {
                                    //kaart in aflegstapel steken
                                    int wedstrijdId1 = vluchter.Wedstrijd_Id ?? default;
                                    Wedstrijd wedstrijd1 = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId1);
                                    Kaarten_Stapel kaarten_Stapel2 = new Kaarten_Stapel();
                                    kaarten_Stapel2.Kaart_Id = Id;
                                    kaarten_Stapel2.Stapel_Id = wedstrijd1.Kerkerkaarten_Aflegstapel_Id;
                                    if (kaarten_Stapel2.IsGeldig())
                                    {
                                        int ok1 = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel2);
                                        if (ok1 <= 0)
                                        {
                                            foutmelding += "Schoeisel niet in aflegstapel kunnen plaatsen.\n";
                                        }
                                    }
                                    else
                                    {
                                        foutmelding += kaarten_Stapel2.Error + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    foutmelding += "Schoeisel niet uit veldkaarten kunnen halen";
                                }
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
                        Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(vluchter.Veldkaarten_Id);
                        foreach (Kaarten_Stapel veldkaart in veldkaarten.Kaarten_Stapels)
                        {
                            //check of er al een kaart van dit type zit in de veldkaarten
                            string type = DatabaseOperations.OphalenType(veldkaart.Kaart.Type_id).Soort.ToUpper();
                            if (type.Contains("HOOFDDEKSEL"))
                            {
                                //Harnas uit veldkaarten halen
                                int ok = DatabaseOperations.VerwijderenKaarten_Stapel(veldkaart);
                                if (ok > 0)
                                {
                                    //kaart in aflegstapel steken
                                    int wedstrijdId1 = vluchter.Wedstrijd_Id ?? default;
                                    Wedstrijd wedstrijd1 = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId1);
                                    Kaarten_Stapel kaarten_Stapel2 = new Kaarten_Stapel();
                                    kaarten_Stapel2.Kaart_Id = Id;
                                    kaarten_Stapel2.Stapel_Id = wedstrijd1.Kerkerkaarten_Aflegstapel_Id;
                                    if (kaarten_Stapel2.IsGeldig())
                                    {
                                        int ok1 = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel2);
                                        if (ok1 <= 0)
                                        {
                                            foutmelding += "Hoofddeksel niet in aflegstapel kunnen plaatsen.\n";
                                        }
                                    }
                                    else
                                    {
                                        foutmelding += kaarten_Stapel2.Error + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    foutmelding += "Hoofddeksel niet uit veldkaarten kunnen halen";
                                }
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
                        Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(vluchter.Veldkaarten_Id);
                        foreach (Kaarten_Stapel veldkaart in veldkaarten.Kaarten_Stapels)
                        {
                            //check of er al een grote kaart zit in de veldkaarten
                            if (veldkaart.Kaart.Schatkaart.Is_Groot == true)
                            {
                                //Harnas uit veldkaarten halen
                                int ok = DatabaseOperations.VerwijderenKaarten_Stapel(veldkaart);
                                if (ok > 0)
                                {
                                    //kaart in aflegstapel steken
                                    int wedstrijdId1 = vluchter.Wedstrijd_Id ?? default;
                                    Wedstrijd wedstrijd1 = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId1);
                                    Kaarten_Stapel kaarten_Stapel2 = new Kaarten_Stapel();
                                    kaarten_Stapel2.Kaart_Id = Id;
                                    kaarten_Stapel2.Stapel_Id = wedstrijd1.Kerkerkaarten_Aflegstapel_Id;
                                    if (kaarten_Stapel2.IsGeldig())
                                    {
                                        int ok1 = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel2);
                                        if (ok1 <= 0)
                                        {
                                            foutmelding += "groot voorwerp niet in aflegstapel kunnen plaatsen.\n";
                                        }
                                    }
                                    else
                                    {
                                        foutmelding += kaarten_Stapel2.Error + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    foutmelding += "groot voorwerp niet uit veldkaarten kunnen halen";
                                }
                            }
                        }
                    }
                    if (Naam.ToUpper() == "GEBROEDERS WIGHT")
                    {
                        vluchter.Level = 1;
                    }
                    if (Naam.ToUpper() == "KONING TOET")
                    {
                        Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(vluchter.Veldkaarten_Id);
                        Stapel handkaarten = DatabaseOperations.OphalenStapelViaId(vluchter.Handkaarten_Id);
                        foreach (Kaarten_Stapel veldkaart in veldkaarten.Kaarten_Stapels)
                        {

                            int ok = DatabaseOperations.VerwijderenKaarten_Stapel(veldkaart);
                            if (ok > 0)
                            {
                                if (true)// checken of het een kerker of schatkaart is om in juiste aflegstapel te doen
                                {
                                    //kaart in aflegstapel steken
                                    int wedstrijdId1 = vluchter.Wedstrijd_Id ?? default;
                                    Wedstrijd wedstrijd1 = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId1);
                                    Kaarten_Stapel kaarten_Stapel2 = new Kaarten_Stapel();
                                    kaarten_Stapel2.Kaart_Id = Id;
                                    kaarten_Stapel2.Stapel_Id = wedstrijd1.Kerkerkaarten_Aflegstapel_Id;
                                    if (kaarten_Stapel2.IsGeldig())
                                    {
                                        int ok1 = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel2);
                                        if (ok1 <= 0)
                                        {
                                            foutmelding += "groot voorwerp niet in aflegstapel kunnen plaatsen.\n";
                                        }
                                    }
                                    else
                                    {
                                        foutmelding += kaarten_Stapel2.Error + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    //anders is andere aflegstapel
                                }
                            }
                            else
                            {
                                foutmelding += "groot voorwerp niet uit veldkaarten kunnen halen";
                            }

                        }
                        //foreach hierboven kopieren voor handkaarten
                    }
                    if (Naam.ToUpper() == "PLUTONIUM DRAAK" || Naam.ToUpper() == "KWELROG")
                    {
                        vluchter.Level = 1;
                        //Alle hand en veldkaarten wegdoen
                    }

                    //kaart naar aflegstapel doen
                    int wedstrijdId = vluchter.Wedstrijd_Id ?? default;
                    Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
                    Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                    kaarten_Stapel1.Kaart_Id = Id;
                    kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                    if (kaarten_Stapel1.IsGeldig() && vluchter.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                        if (ok > 0)
                        {
                            //aanpassen speler
                            int ok2 = DatabaseOperations.AanpassenWedstrijd_Speler(vluchter);
                            if (ok2 <= 0)
                            {
                                foutmelding += "Nadelen zijn niet uitgevoerd\n";
                            }
                        }
                        else
                        {
                            foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                        }
                    }
                    else
                    {
                        foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                        foutmelding += vluchter.Error + Environment.NewLine;
                    }
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
                    if (Naam.ToUpper() == "KONING TOET" || Naam.ToUpper() == "GEBROEDERS WIGHT")
                    {
                        vluchter.Level -= 2;
                    }
                    if (Naam.ToUpper() == "MAGERE HEIN")
                    {
                        vluchter.Level -= 1;
                    }
                    //kaart naar aflegstapel doen
                    int wedstrijdId = vluchter.Wedstrijd_Id ?? default;
                    Wedstrijd wedstrijd = DatabaseOperations.OphalenWedstrijdViaId(wedstrijdId);
                    Kaarten_Stapel kaarten_Stapel1 = new Kaarten_Stapel();
                    kaarten_Stapel1.Kaart_Id = Id;
                    kaarten_Stapel1.Stapel_Id = wedstrijd.Kerkerkaarten_Aflegstapel_Id;
                    if (kaarten_Stapel1.IsGeldig() && vluchter.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel1);
                        if (ok > 0)
                        {
                            //aanpassen speler
                            int ok2 = DatabaseOperations.AanpassenWedstrijd_Speler(vluchter);
                            if (ok2 <= 0)
                            {
                                foutmelding += "Nadelen zijn niet uitgevoerd\n";
                            }
                        }
                        else
                        {
                            foutmelding += "Monster niet in aflegstapel kunnen plaatsen.\n";
                        }
                    }
                    else
                    {
                        foutmelding += kaarten_Stapel1.Error + Environment.NewLine;
                        foutmelding += vluchter.Error + Environment.NewLine;
                    }
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
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
            //check if speler al een ras heeft
            if (gebruiker.Ras.ToUpper() == "MENS")
            {
                Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);

                //Voeg kaart toe in veldkaarten
                Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
                nieuweKaartenStapel.Kaart_Id = Id;
                nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;
                //pas wedstrijd speler aan
                gebruiker.Ras = Naam;

                gebruiker.HerberekenBonussenVeldkaarten(veldkaarten);
                //Functie voor onderstaande code geschreven, ook nodig bij vervloeking. Nog testen.
                //gebruiker.Vluchtbonus = 0;
                //gebruiker.Gevechtsbonus = 0;

                //if (gebruiker.Ras.ToUpper() == "ELF")
                //{
                //    gebruiker.Vluchtbonus += 1;
                //}
                ////checken of hij kaarten heeft die een bonus geven op een bepaald ras. Als hij dat ras nu niet meer heeft dan gaat de bonus weg
                //foreach (Kaarten_Stapel kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
                //{
                //    List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(kaarten_Stapel1.Kaart_Id);
                //    foreach (var bonus in lijstBonussen)
                //    {
                //        if (bonus.Bruikbaar_Door.ToUpper() == gebruiker.Ras.ToUpper() || bonus.Bruikbaar_Door.ToUpper() == "IEDEREEN")
                //        {
                //            if (bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
                //            {
                //                //Bonus toevoegen aan gevechtswaarde
                //                gebruiker.Gevechtsbonus += bonus.Waarde;
                //            }
                //            else if (bonus.Waarop_Effect.ToUpper() == "VLUCHTEN")
                //            {
                //                //Bonus toevoegen aan vluchtbonus
                //                gebruiker.Vluchtbonus += bonus.Waarde;
                //            }
                //        }
                //    }
                //}

                //als de nieuwe kaartenstapel en de wedstrijdspeler geldig zijn, gaan we aanpassing doorvoeren in db
                if (nieuweKaartenStapel.IsGeldig() && gebruiker.IsGeldig())
                {
                    //nieuwe kaartenstapel toevoegen
                    int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                    if (ok > 0)
                    {
                        //oude kaartenstapel verwijderen
                        int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(oudeKaartenStapel);
                        if (ok2 > 0)
                        {
                            //wedstrijd_Speler aanpassen
                            int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(gebruiker);
                            if (ok3 <= 0)
                            {
                                foutmelding += "Bonussen speler niet aangepast\n";
                            }
                        }
                        else
                        {
                            foutmelding += "Kaart niet uit je hand kunnen halen\n";
                        }
                    }
                    else
                    {
                        foutmelding += "Kaart niet op het veld kunnen plaatsen\n";
                    }
                }
                else
                {
                    foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
                    foutmelding += gebruiker.Error + Environment.NewLine;
                }
            }
            else
            {
                foutmelding += "Je hebt al een ras, leg deze eerst af indien je een nieuw ras wil gebruiken";
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
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
            string foutmelding = "";
            //loop daar alle kaartenstapels van veldkaarten
            foreach (var kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
            {
                //check of er al een kaart van dit type zit in de veldkaarten
                if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains(type.Soort.ToUpper()))
                {
                    return bericht1 + kaarten_Stapel1.Kaart.Naam;
                }
                //NOG TE TESTEN!!!!!!!! WERKT NIET MISS MET GETTYPE om te checken of het een schar-tkaart is?
                //if (kaart.Schatkaart.Is_Groot == true && kaarten_Stapel1.Kaart.Schatkaart.Is_Groot == true)
                //{
                //    return "je kan geen 2 grote voorwerpen dragen";
                //}
                //TOT HIER
            }

            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);

            //Voeg kaart toe in veldkaarten
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = Id;
            nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;

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
            //check of de nieuwe kaartenstapel geldig is en de aanpassing in wedstrijdSpeler
            if (nieuweKaartenStapel.IsGeldig() && gebruiker.IsGeldig())
            {
                int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                if (ok > 0)
                {
                    int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(oudeKaartenStapel);
                    if (ok2 > 0)
                    {
                        int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(gebruiker);
                        if (ok3 <= 0)
                        {
                            foutmelding += "Bonussen zijn niet toegevoegd\n";
                        }
                    }
                    else
                    {
                        foutmelding += "Kaart niet uit je hand kunnen halen\n";
                    }
                }
                else
                {
                    foutmelding += "Kaart niet op het veld kunnen plaatsen\n";
                }
            }
            else
            {
                foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
                foutmelding += gebruiker.Error + Environment.NewLine;
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
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
            string foutmelding = "";
            //loop daar alle kaartenstapels van veldkaarten
            foreach (var kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
            {
                //NOG TE TESTEN!!!!!!!! WERKT NIET MISS MET GETTYPE om te checken of het een schar-tkaart is?
                //if (kaart.Schatkaart.Is_Groot == true && kaarten_Stapel1.Kaart.Schatkaart.Is_Groot == true)
                //{
                //    return "je kan geen 2 grote voorwerpen dragen";
                //}
                //TOT HIER
            }

            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);

            //Voeg kaart toe in veldkaarten
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = Id;
            nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;

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
            //check of de nieuwe kaartenstapel geldig is en de aanpassing in wedstrijdSpeler
            if (nieuweKaartenStapel.IsGeldig() && gebruiker.IsGeldig())
            {
                int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                if (ok > 0)
                {
                    int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(oudeKaartenStapel);
                    if (ok2 > 0)
                    {
                        int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(gebruiker);
                        if (ok3 <= 0)
                        {
                            foutmelding += "Bonussen zijn niet toegevoegd\n";
                        }
                    }
                    else
                    {
                        foutmelding += "Kaart niet uit je hand kunnen halen\n";
                    }
                }
                else
                {
                    foutmelding += "Kaart niet op het veld kunnen plaatsen\n";
                }
            }
            else
            {
                foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
                foutmelding += gebruiker.Error + Environment.NewLine;
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
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
            string foutmelding = "";
            int handenVol = 0;
            //loop daar alle kaartenstapels van veldkaarten
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
                //NOG TE TESTEN!!!!!!!! WERKT NIET MISS MET GETTYPE om te checken of het een schar-tkaart is?
                //if (kaart.Schatkaart.Is_Groot == true && kaarten_Stapel1.Kaart.Schatkaart.Is_Groot == true)
                //{
                //    return "je kan geen 2 grote voorwerpen dragen";
                //}
                //TOT HIER
            }

            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);

            //Voeg kaart toe in veldkaarten
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = Id;
            nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;

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
            //check of de nieuwe kaartenstapel geldig is en de aanpassing in wedstrijdSpeler
            if (nieuweKaartenStapel.IsGeldig() && gebruiker.IsGeldig())
            {
                int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                if (ok > 0)
                {
                    int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(oudeKaartenStapel);
                    if (ok2 > 0)
                    {
                        int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(gebruiker);
                        if (ok3 <= 0)
                        {
                            foutmelding += "Bonussen zijn niet toegevoegd\n";
                        }
                    }
                    else
                    {
                        foutmelding += "Kaart niet uit je hand kunnen halen\n";
                    }
                }
                else
                {
                    foutmelding += "Kaart niet op het veld kunnen plaatsen\n";
                }
            }
            else
            {
                foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
                foutmelding += gebruiker.Error + Environment.NewLine;
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
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(gebruiker.Veldkaarten_Id);
            string foutmelding = "";
            //loop daar alle kaartenstapels van veldkaarten
            foreach (var kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
            {
                //check of er al een hoofddeksel zit in de veldkaarten
                if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("1HAND") || DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("2HANDEN"))
                {
                    return "je hebt je 2 handen al vol.";
                }
                //NOG TE TESTEN!!!!!!!! WERKT NIET MISS MET GETTYPE om te checken of het een schar-tkaart is?
                //if (kaart.Schatkaart.Is_Groot == true && kaarten_Stapel1.Kaart.Schatkaart.Is_Groot == true)
                //{
                //    return "je kan geen 2 grote voorwerpen dragen";
                //}
                //TOT HIER
            }

            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);

            //Voeg kaart toe in veldkaarten
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = Id;
            nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;

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
            //check of de nieuwe kaartenstapel geldig is en de aanpassing in wedstrijdSpeler
            if (nieuweKaartenStapel.IsGeldig() && gebruiker.IsGeldig())
            {
                int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                if (ok > 0)
                {
                    int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(oudeKaartenStapel);
                    if (ok2 > 0)
                    {
                        int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(gebruiker);
                        if (ok3 <= 0)
                        {
                            foutmelding += "Bonussen zijn niet toegevoegd\n";
                        }
                    }
                    else
                    {
                        foutmelding += "Kaart niet uit je hand kunnen halen\n";
                    }
                }
                else
                {
                    foutmelding += "Kaart niet op het veld kunnen plaatsen\n";
                }
            }
            else
            {
                foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
                foutmelding += gebruiker.Error + Environment.NewLine;
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
        public string SpeelVervloeking(Wedstrijd_Speler gebruiker, Wedstrijd_Speler slachtoffer)
        {
            string foutmelding = "";
            //kaartenstapel ophalen om later de kaart te verwijderen uit hand
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //voor als slachtoffer een voorwerp verliest
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
            }
            //checken of je iets moet verliezen
            if (Naam.ToUpper().Contains("VERLIES"))
            {
                //Ras verliezen
                if (Naam.ToUpper().Contains("RAS"))
                {
                    slachtoffer.Ras = "Mens";
                    //Checken of alle bonussen nog gelden en gevechtswaarde herberekenen
                    slachtoffer.Vluchtbonus = 0;
                    slachtoffer.Gevechtsbonus = 0;
                    Stapel veldkaarten = DatabaseOperations.OphalenStapelViaId(slachtoffer.Veldkaarten_Id);
                    foreach (Kaarten_Stapel kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
                    {
                        List<Bonus> lijstBonussen2 = DatabaseOperations.OphalenBonussenViaKaartId(kaarten_Stapel1.Kaart_Id);
                        foreach (var bonus in lijstBonussen2)
                        {
                            if (bonus.Bruikbaar_Door.ToUpper() == slachtoffer.Ras.ToUpper() || bonus.Bruikbaar_Door.ToUpper() == "IEDEREEN")
                            {
                                if (bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
                                {
                                    //Bonus toevoegen aan gevechtswaarde
                                    slachtoffer.Gevechtsbonus += bonus.Waarde;
                                }
                                else if (bonus.Waarop_Effect.ToUpper() == "VLUCHTEN")
                                {
                                    //Bonus toevoegen aan vluchtbonus
                                    gebruiker.Vluchtbonus += bonus.Waarde;
                                }
                            }
                        }
                    }
                }
                //Als je harnas moet verliezen
                else if (Naam.ToUpper().Contains("HARNAS"))
                {
                    foreach (var kaarten_Stapel1 in veldkaartenSlachtoffer.Kaarten_Stapels)
                    {
                        //check of er al een kaart van dit type in veldkaarten zit
                        if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("HARNAS"))
                        {
                            //Kaart verwijderen
                            int ok = DatabaseOperations.VerwijderenKaarten_Stapel(kaarten_Stapel1);
                            if (ok > 0)
                            {
                                //bonus van de kaart aftrekken
                                slachtoffer.HerberekenBonussenVeldkaarten(veldkaartenSlachtoffer);

                            }
                            else
                            {
                                foutmelding += "Kaart niet uit de veldkaarten kunnen halen\n";
                            }
                        }
                    }
                }
                //Als je hoofddeksel moet verliezen
                else if (Naam.ToUpper().Contains("HOOFDDEKSEL"))
                {
                    foreach (var kaarten_Stapel1 in veldkaartenSlachtoffer.Kaarten_Stapels)
                    {
                        //check of er al een kaart van dit type in veldkaarten zit
                        if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("HOOFDDEKSEL"))
                        {
                            //Kaart verwijderen
                            int ok = DatabaseOperations.VerwijderenKaarten_Stapel(kaarten_Stapel1);
                            if (ok > 0)
                            {
                                //bonus van de kaart aftrekken
                                slachtoffer.HerberekenBonussenVeldkaarten(veldkaartenSlachtoffer);

                            }
                            else
                            {
                                foutmelding += "Kaart niet uit de veldkaarten kunnen halen\n";
                            }
                        }
                    }
                }
                //als je je schoeisel moet verliezen
                else if (Naam.ToUpper().Contains("SCHOEISEL"))
                {
                    foreach (var kaarten_Stapel1 in veldkaartenSlachtoffer.Kaarten_Stapels)
                    {
                        //check of er al een kaart van dit type in veldkaarten zit
                        if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("SCHOEISEL"))
                        {
                            //Kaart verwijderen
                            int ok = DatabaseOperations.VerwijderenKaarten_Stapel(kaarten_Stapel1);
                            if (ok > 0)
                            {
                                //bonus van de kaart aftrekken
                                slachtoffer.HerberekenBonussenVeldkaarten(veldkaartenSlachtoffer);

                            }
                            else
                            {
                                foutmelding += "Kaart niet uit de veldkaarten kunnen halen\n";
                            }
                        }
                    }
                }
            }
            //kaart naar aflegstapel doen
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = Id;
            //schat dat ik hiervoor een dbOperations functie OphalenWedstrijd ga moeten maken
            nieuweKaartenStapel.Stapel_Id = gebruiker.Wedstrijd.Kerkerkaarten_Aflegstapel_Id;
            //als de nieuwe kaartenstapel en de wedstrijdspeler geldig zijn, gaan we aanpassing doorvoeren in db
            if (nieuweKaartenStapel.IsGeldig() && slachtoffer.IsGeldig())
            {
                //nieuwe kaartenstapel toevoegen
                int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                if (ok > 0)
                {
                    //oude kaartenstapel verwijderen
                    int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(oudeKaartenStapel);
                    if (ok2 > 0)
                    {
                        //wedstrijd_Speler aanpassen
                        int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(slachtoffer);
                        if (ok3 <= 0)
                        {
                            foutmelding += "Bonussen speler niet aangepast\n";
                        }
                    }
                    else
                    {
                        foutmelding += "Kaart niet uit je hand kunnen halen\n";
                    }
                }
                else
                {
                    foutmelding += "Kaart niet in aflegstapel kunnen plaatsen\n";
                }
            }
            else
            {
                foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
                foutmelding += slachtoffer.Error + Environment.NewLine;
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
            //kaartenstapel ophalen om later de kaart te verwijderen uit hand
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //bonus kaart ophalen
            List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            //loopen door de bonus(sen)
            foreach (var bonus in lijstBonussen)
            {
                if (bonus.Waarop_Effect.ToUpper() == "LEVEL")
                {
                    if (Naam.ToUpper().Contains("ZEUREN"))
                    {
                        List<Wedstrijd_Speler> wedstrijd_Spelers = new List<Wedstrijd_Speler>();
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
                            gebruiker.Level += bonus.Waarde;
                        }
                    }
                    else
                    {
                        //level optellen
                        gebruiker.Level += bonus.Waarde;
                    }
                }
            }
            //kaart naar aflegstapel doen
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = Id;
            //schat dat ik hiervoor een dbOperations functie OphalenWedstrijd ga moeten maken
            nieuweKaartenStapel.Stapel_Id = gebruiker.Wedstrijd.Kerkerkaarten_Aflegstapel_Id;
            //als de nieuwe kaartenstapel en de wedstrijdspeler geldig zijn, gaan we aanpassing doorvoeren in db
            if (nieuweKaartenStapel.IsGeldig() && gebruiker.IsGeldig())
            {
                //nieuwe kaartenstapel toevoegen
                int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                if (ok > 0)
                {
                    //oude kaartenstapel verwijderen
                    int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(oudeKaartenStapel);
                    if (ok2 > 0)
                    {
                        //wedstrijd_Speler aanpassen
                        int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(gebruiker);
                        if (ok3 <= 0)
                        {
                            foutmelding += "Level speler niet aangepast\n";
                        }
                    }
                    else
                    {
                        foutmelding += "Kaart niet uit je hand kunnen halen\n";
                    }
                }
                else
                {
                    foutmelding += "Kaart niet in aflegstapel kunnen plaatsen\n";
                }
            }
            else
            {
                foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
                foutmelding += gebruiker.Error + Environment.NewLine;
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
            //kaartenstapel ophalen om later de kaart te verwijderen uit hand
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //bonus kaart ophalen
            List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            //loopen door de bonus(sen)
            foreach (var bonus in lijstBonussen)
            {
                if (bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
                {
                    //Bonus toevoegen aan gevechtswaarde
                    slachtoffer.Tijdelijke_Bonus += bonus.Waarde;
                }
            }
            //kaart naar aflegstapel doen
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = Id;
            //schat dat ik hiervoor een dbOperations functie OphalenWedstrijd ga moeten maken
            nieuweKaartenStapel.Stapel_Id = gebruiker.Wedstrijd.Kerkerkaarten_Aflegstapel_Id;
            //als de nieuwe kaartenstapel en de wedstrijdspeler geldig zijn, gaan we aanpassing doorvoeren in db
            if (nieuweKaartenStapel.IsGeldig() && slachtoffer.IsGeldig())
            {
                //nieuwe kaartenstapel toevoegen
                int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                if (ok > 0)
                {
                    //oude kaartenstapel verwijderen
                    int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(oudeKaartenStapel);
                    if (ok2 > 0)
                    {
                        //wedstrijd_Speler aanpassen
                        int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(slachtoffer);
                        if (ok3 <= 0)
                        {
                            foutmelding += "bonussen speler niet aangepast\n";
                        }
                    }
                    else
                    {
                        foutmelding += "Kaart niet uit je hand kunnen halen\n";
                    }
                }
                else
                {
                    foutmelding += "Kaart niet in aflegstapel kunnen plaatsen\n";
                }
            }
            else
            {
                foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
                foutmelding += slachtoffer.Error + Environment.NewLine;
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
            //kaartenstapel ophalen om later de kaart te verwijderen uit hand
            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, Id);
            //bonus kaart ophalen
            List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(Id);
            //loopen door de bonus(sen)
            foreach (var bonus in lijstBonussen)
            {
                if (bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
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
            //kaart naar aflegstapel doen
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = Id;
            //schat dat ik hiervoor een dbOperations functie OphalenWedstrijd ga moeten maken
            nieuweKaartenStapel.Stapel_Id = gebruiker.Wedstrijd.Kerkerkaarten_Aflegstapel_Id;
            //als de nieuwe kaartenstapel en de wedstrijdspeler geldig zijn, gaan we aanpassing doorvoeren in db
            if (nieuweKaartenStapel.IsGeldig() && monster.IsGeldig())
            {
                //nieuwe kaartenstapel toevoegen
                int ok = DatabaseOperations.ToevoegenKaarten_Stapel(nieuweKaartenStapel);
                if (ok > 0)
                {
                    //oude kaartenstapel verwijderen
                    int ok2 = DatabaseOperations.VerwijderenKaarten_Stapel(oudeKaartenStapel);
                    if (ok2 > 0)
                    {
                        //wedstrijd_Speler aanpassen
                        int ok3 = DatabaseOperations.AanpassenKaart(monster);
                        if (ok3 <= 0)
                        {
                            foutmelding += "bonussen monster niet aangepast\n";
                        }
                    }
                    else
                    {
                        foutmelding += "Kaart niet uit je hand kunnen halen\n";
                    }
                }
                else
                {
                    foutmelding += "Kaart niet in aflegstapel kunnen plaatsen\n";
                }
            }
            else
            {
                foutmelding += nieuweKaartenStapel.Error + Environment.NewLine;
                foutmelding += monster.Error + Environment.NewLine;
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
    }
}
