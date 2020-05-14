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
        public string SpeelKaart(int kaartId, Wedstrijd_Speler wedstrijdSpeler)
        {
            Kaart kaart = DatabaseOperations.OphalenKaartViaKaartIdMetType(kaartId);
            Stapel veldkaarten = DatabaseOperations.OphalenStapelViaVeldkaartenId(wedstrijdSpeler.Veldkaarten_Id);
            if (kaart.Type.Soort.ToUpper() == "RAS")
            {
                if (wedstrijdSpeler.Ras.ToUpper() == "MENS")
                {
                    Kaarten_Stapel kaarten_Stapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(wedstrijdSpeler.Handkaarten_Id, kaart.Id);
                    int ok = DatabaseOperations.VerwijderenKaarten_Stapel(kaarten_Stapel);
                    if (ok > 0)
                    {
                        //Voeg kaart toe in veldkaarten
                        Kaarten_Stapel kaartenstapel2 = new Kaarten_Stapel();
                        kaartenstapel2.Kaart_Id = kaart.Id;
                        kaartenstapel2.Stapel_Id = veldkaarten.Id;
                        //Moet hier een isgeldig over staan?
                        if (kaartenstapel2.IsGeldig())
                        {
                            int ok3 = DatabaseOperations.ToevoegenKaarten_Stapel(kaartenstapel2);
                            if (ok3 > 0)
                            {
                                wedstrijdSpeler.Ras = kaart.Naam;
                                if (wedstrijdSpeler.Ras.ToUpper() == "ELF")
                                {
                                    wedstrijdSpeler.Vluchtbonus += 1;
                                }
                                if (wedstrijdSpeler.IsGeldig())
                                {
                                    int ok2 = DatabaseOperations.AanpassenWedstrijd_Speler(wedstrijdSpeler);
                                    if (ok2 > 0)
                                    {
                                        return $"Je bent van ras veranderd, je bent nu een {wedstrijdSpeler.Ras}!";
                                    }
                                    else
                                    {
                                        return "Er is iets mis gelopen, je ras is niet aangepast";
                                    }
                                }
                                else
                                {
                                    return wedstrijdSpeler.Error;
                                }
                            }
                            else
                            {
                                DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel);
                                return "Er is iets fout gelopen, gelieve opnieuw te proberen";
                            }
                            
                        }
                        else
                        {
                            return kaartenstapel2.Error;
                        }
                        
                    }
                    else
                    {
                        return "Je ras is niet aangepast, gelieve opnieuw te proberen";
                    }
                    
                }
                else
                {
                    return "Je hebt al een ras, leg deze eerst af indien je een nieuw ras wil gebruiken";
                }
            }
            else if (DatabaseOperations.OphalenType(kaart.Type_id).Soort.ToUpper() == "HOOFDDEKSEL")
            {
                //loop daar alle kaartenstapels van veldkaarten
                //foreach probleem met return, moet er hier een for lus gebruikt worden?
                foreach (var kaarten_Stapel1 in veldkaarten.Kaarten_Stapels)
                {
                    //check of er al een hoofddeksel zit in de veldkaarten
                    if (DatabaseOperations.OphalenType(kaarten_Stapel1.Kaart.Type_id).Soort.ToUpper().Contains("HOOFDDEKSEL"))
                    {
                        return $"je hebt al een hoofddeksel op. Namelijk een {kaarten_Stapel1.Kaart.Naam}";
                    }
                }
                //Verwijder equipment uit handkaarten
                Kaarten_Stapel kaarten_Stapel = DatabaseOperations.OphalenKaarten_StapelViaKaart_IdEnStapelId(wedstrijdSpeler.Handkaarten_Id, kaart.Id);
                int ok = DatabaseOperations.VerwijderenKaarten_Stapel(kaarten_Stapel);
                //als het verwijderen gelukt is mag je verder gaan
                if (ok > 0)
                {
                    //Voeg kaart toe in veldkaarten
                    Kaarten_Stapel kaartenstapel2 = new Kaarten_Stapel();
                    kaartenstapel2.Kaart_Id = kaart.Id;
                    kaartenstapel2.Stapel_Id = veldkaarten.Id;

                    if (kaartenstapel2.IsGeldig())
                    {
                        int ok2 = DatabaseOperations.ToevoegenKaarten_Stapel(kaartenstapel2);
                        //check of toevoegen gelukt is
                        if (ok2 > 0)
                        {

                            //Bonus van kaart opvragen
                            List<Bonus> lijstBonusssen = DatabaseOperations.OphalenBonussenViaKaartId(kaart.Id);
                            foreach (var bonus in lijstBonusssen)
                            {
                                if (bonus.Waarop_Effect.ToUpper() == "GEVECHTSWAARDE")
                                {
                                    //Bonus toevoegen aan gevechtswaarde
                                    wedstrijdSpeler.Gevechtsbonus += bonus.Waarde;
                                }
                                else if (bonus.Waarop_Effect.ToUpper() == "VLUCHTEN")
                                {
                                    //Bonus toevoegen aan vluchtbonus
                                    wedstrijdSpeler.Vluchtbonus += bonus.Waarde;
                                }
                            }
                            int ok3 = DatabaseOperations.AanpassenWedstrijd_Speler(wedstrijdSpeler);
                            if (ok3 > 0)
                            {
                                return $"Je hebt {kaart.Naam} op gezet!";
                            }
                            else
                            {
                                return "Er zijn geen bonussen toegevoegd";
                            }

                        }
                        else
                        {
                            //kaart terug in hand zetten indien het niet gelukt is
                            DatabaseOperations.ToevoegenKaarten_Stapel(kaarten_Stapel);
                            return "Er is iets fout gelopen, gelieve opnieuw te proberen";
                        }
                    }
                    else
                    {
                        return kaartenstapel2.Error;
                    }
                }
                else
                {
                    return "Er is iets fout gelopen, gelieve opnieuw te proberen";
                }

            }
            else if (true)
            {
                //Voor harnas of dergelijke
                return "";
            }

        }

    }
}
