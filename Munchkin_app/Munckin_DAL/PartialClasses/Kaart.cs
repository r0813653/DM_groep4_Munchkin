using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Kaart
    {
        public string SpeelKaart(int kaartId, Wedstrijd_Speler gebruiker)
        {
            Kaart kaart = DatabaseOperations.OphalenKaartViaKaartIdMetType(kaartId);
            if (kaart.Type.Soort.ToUpper() == "RAS")
            {
                string ret = SpeelRas(kaart, gebruiker);
                return ret;
            }
            else if (kaart.Type.Soort.ToUpper() == "HOOFDDEKSEL")
            {
                string type = "HOOFDDEKSEL";
                string bericht1 = "je hebt al een hoofddeksel op. Namelijk een ";
                string ret = SpeelHoofddekselOfHarnasOfSchoeisel(kaart, gebruiker, type, bericht1);
                return ret;
            }
            else if (kaart.Type.Soort.ToUpper() == "HARNAS")
            {
                string type = "HARNAS";
                string bericht1 = "je hebt al een harnas aan. Namelijk een ";
                string ret = SpeelHoofddekselOfHarnasOfSchoeisel(kaart, gebruiker, type, bericht1);
                return ret;
            }
            else if (kaart.Type.Soort.ToUpper() == "SCHOEISEL")
            {
                string type = "SCHOEISEL";
                string bericht1 = "je hebt al schoeisel aan. Namelijk ";
                string ret = SpeelHoofddekselOfHarnasOfSchoeisel(kaart, gebruiker, type, bericht1);
                return ret;
            }
            else if (kaart.Type.Soort.ToUpper() == "EXTRA")
            {
                string ret = SpeelExtra(kaart, gebruiker);
                return ret;
            }
            else if (kaart.Type.Soort.ToUpper() == "1HAND")
            {
                string ret = Speel1Hand(kaart, gebruiker);
                return ret;
            }
            else if (kaart.Type.Soort.ToUpper() == "2HANDEN")
            {
                string ret = Speel2Handen(kaart, gebruiker);
                return ret;
            }
            else
            {
                return "Je kan deze kaart nu niet gebruiken";
            }
        }

        public string SpeelRas(Kaart kaart, Wedstrijd_Speler gebruiker)
        {
            string foutmelding = "";
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaVeldkaartenId(gebruiker.Veldkaarten_Id);
            //check if speler al een ras heeft
            if (gebruiker.Ras.ToUpper() == "MENS")
            {
                Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, kaart.Id);

                //Voeg kaart toe in veldkaarten
                Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
                nieuweKaartenStapel.Kaart_Id = kaart.Id;
                nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;
                //pas wedstrijd speler aan
                //eerst bonussen op 0 zetten, we berekenen alle bonussen van veldkaarten terug opnieuw omdat er met de verrandering miss sommige wegvallen
                gebruiker.Vluchtbonus = 0;
                gebruiker.Gevechtsbonus = 0;
                gebruiker.Ras = kaart.Naam;
                if (gebruiker.Ras.ToUpper() == "ELF")
                {
                    gebruiker.Vluchtbonus += 1;
                }
                //checken of hij kaarten heeft die een bonus geven op een bepaald ras. Als hij dat ras nu niet meer heeft dan gaat de bonus weg
                foreach (Kaarten_Stapel kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
                {
                    List<Bonus> lijstBonussen = DatabaseOperations.OphalenBonussenViaKaartId(kaarten_Stapel1.Kaart_Id);
                    foreach (var bonus in lijstBonussen)
                    {
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
        public string SpeelHoofddekselOfHarnasOfSchoeisel(Kaart kaart, Wedstrijd_Speler gebruiker, string type, string bericht1)
        {
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaVeldkaartenId(gebruiker.Veldkaarten_Id);
            string foutmelding = "";
            //loop daar alle kaartenstapels van veldkaarten
            foreach (var kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
            {
                //check of er al een kaart van dit type zit in de veldkaarten
                if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains(type.ToUpper()))
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

            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, kaart.Id);

            //Voeg kaart toe in veldkaarten
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = kaart.Id;
            nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;

            //Bonus van kaart opvragen
            List<Bonus> lijstBonusssen = DatabaseOperations.OphalenBonussenViaKaartId(kaart.Id);
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
                return $"Je hebt {kaart.Naam} gebruikt!";
            }
            else
            {
                return foutmelding;
            }
        }
        public string SpeelExtra(Kaart kaart, Wedstrijd_Speler gebruiker)
        {
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaVeldkaartenId(gebruiker.Veldkaarten_Id);
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

            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, kaart.Id);

            //Voeg kaart toe in veldkaarten
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = kaart.Id;
            nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;

            //Bonus van kaart opvragen
            List<Bonus> lijstBonusssen = DatabaseOperations.OphalenBonussenViaKaartId(kaart.Id);
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
                return $"Je bent nu uitgerust met een {kaart.Naam}";
            }
            else
            {
                return foutmelding;
            }
        }
        public string Speel1Hand(Kaart kaart, Wedstrijd_Speler gebruiker)
        {
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaVeldkaartenId(gebruiker.Veldkaarten_Id);
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

            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, kaart.Id);

            //Voeg kaart toe in veldkaarten
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = kaart.Id;
            nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;

            //Bonus van kaart opvragen
            List<Bonus> lijstBonusssen = DatabaseOperations.OphalenBonussenViaKaartId(kaart.Id);
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
                return $"Je hebt nu een {kaart.Naam} in je hand!";
            }
            else
            {
                return foutmelding;
            }
        }
        public string Speel2Handen(Kaart kaart, Wedstrijd_Speler gebruiker)
        {
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaVeldkaartenId(gebruiker.Veldkaarten_Id);
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

            Kaarten_Stapel oudeKaartenStapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(gebruiker.Handkaarten_Id, kaart.Id);

            //Voeg kaart toe in veldkaarten
            Kaarten_Stapel nieuweKaartenStapel = new Kaarten_Stapel();
            nieuweKaartenStapel.Kaart_Id = kaart.Id;
            nieuweKaartenStapel.Stapel_Id = veldkaarten.Id;

            //Bonus van kaart opvragen
            List<Bonus> lijstBonusssen = DatabaseOperations.OphalenBonussenViaKaartId(kaart.Id);
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
                return $"Je hebt nu een {kaart.Naam} in je handen!";
            }
            else
            {
                return foutmelding;
            }
        }

    }
}
