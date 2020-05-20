using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Munckin_DAL
{
    public static class DatabaseOperations
    {
        public static int ToevoegenSpelers(Speler speler)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                entities.Spelers.Add(speler);
                return entities.SaveChanges();
            }
        }
      
        public static List<Kaart> OphalenKaartenViaNaam(string DeelNaam)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Kaarten
                    .Where(x => x.Naam.Contains(DeelNaam))
                    .OrderBy(x => x.Naam);
                return query.ToList();
            }
        }

        public static int AanpassenWedstrijd_Speler(Wedstrijd_Speler wedstrijd_Speler)
        {
            try
            {
                using (MunchkinEntities entities = new MunchkinEntities())
                {
                    entities.Entry(wedstrijd_Speler).State = EntityState.Modified;
                    return entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }
        //Voorlopig niet nodig denk ik - voor de zekerheid ff commentaar ipv weg
        //public static Kaart OphalenKaartViaKaartIdMetBonusEnType(int kaartId)
        //{
        //    using (MunchkinEntities entities = new MunchkinEntities())
        //    {
        //        var query = entities.Kaarten
        //                      .Include(x => x.Kaart_Bonussen.Select(sub => sub.Bonus))
        //                      .Include(x => x.Type)
        //                      .Where(x => x.Id == kaartId);
        //        return query.SingleOrDefault();
        //    }
        //}

        //Voorlopig niet nodig denk ik
        //public static Kaart OphalenKaartViaKaartIdMetType(int kaartId)
        //{
        //    using (MunchkinEntities entities = new MunchkinEntities())
        //    {
        //        var query = entities.Kaarten
        //                      .Include(x => x.Type)
        //                      .Include(x => x.Schatkaart)
        //                      .Where(x => x.Id == kaartId);
        //        return query.SingleOrDefault();
        //    }
        //}

        public static Stapel OphalenStapelViaId(int Id)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Stapels
                            .Include(x => x.Kaarten_Stapels.Select(sub => sub.Kaart))
                            .Where(x => x.Id == Id);
                return query.SingleOrDefault();
            }
        }

        public static Kaart OphalenKaartViaId(int id)
        {
          var query = entities.Kaarten
                               .Include(x=> x.Schatkaart)
                               .Include(x=> x.Kerkerkaart)
                              .Where(x => x.Id == id);
                return query.SingleOrDefault();
            }
        }

        //voorlopig niet nodig, laat nog ff staan voor de zekerheid
        //public static Wedstrijd_Speler OphalenWedstrijd_SpelerViaId(int id)
        //{
        //    using (MunchkinEntities entities = new MunchkinEntities())
        //    {
        //        var query = entities.Wedstrijd_Spelers
        //                      .Where(x => x.Id == id);
        //        return query.SingleOrDefault();
        //    }
        //}

        public static Type OphalenType(int typeId)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Types
                              .Where(x => x.Id == typeId);
                return query.SingleOrDefault();
            }
        }

        public static List<Bonus> OphalenBonussenViaKaartId(int kaartId)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                return entities.Bonussen
                    .Include(x => x.Kaart_Bonussen.Select(y => y.Kaart))
                   .Where(p => p.Kaart_Bonussen.Any(b => b.Kaart_Id == kaartId))

                    .ToList();
            }
        }

        public static int ToevoegenKaarten_Stapel(Kaarten_Stapel kaarten_Stapel)
        {
            try
            {
                using (MunchkinEntities entities = new MunchkinEntities())
                {
                    entities.Kaarten_Stapels.Add(kaarten_Stapel);
                    return entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        public static int VerwijderenKaarten_Stapel(Kaarten_Stapel kaarten_Stapel)
        {
            try
            {
                using (MunchkinEntities entities = new MunchkinEntities())
                {
                    entities.Entry(kaarten_Stapel).State = EntityState.Deleted;
                    return entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }

        public static Kaarten_Stapel OphalenKaarten_StapelViaKaart_IdEnStapelId(int stapelId, int kaartId)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Kaarten_Stapels
                            .Where(x => x.Kaart_Id == kaartId && x.Stapel_Id == stapelId);
                return query.SingleOrDefault();
            }
        }
        //Voorlopig niet meer nodig, voor de zekerheid ff commentaar
        //public static List<Wedstrijd_Speler> OphalenWedstrijd_SpelerViaWedstrijd_Id(int wedstrijdId)
        //{
        //    using (MunchkinEntities entities = new MunchkinEntities())
        //    {
        //        var query = entities.Wedstrijd_Spelers
        //                      .Where(x => x.Wedstrijd_Id == wedstrijdId);
        //        return query.ToList();
        //    }
        //}
        public static int AanpassenKerkerkaart(Kerkerkaart kaart)
        {
            try
            {
                using (MunchkinEntities entities = new MunchkinEntities())
                {
                    entities.Entry(kaart).State = EntityState.Modified;
                    return entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.FoutLoggen(ex);
                return 0;
            }
        }
        public static Wedstrijd OphalenWedstrijdViaId(int Id)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Wedstrijden
                            .Where(x => x.Id == Id);
                return query.SingleOrDefault();
            }
        }
        public static List<Kaarten_Stapel> OphalenKaarten_StapelsViaStapelId(int stapelId)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                return entities.Kaarten_Stapels
                   .Where(p => p.Stapel_Id == stapelId)
                    .ToList();
            }
        }
        public static List<Wedstrijd_Speler> OphalenWedstrijd_SpelersViaWedstrijdId(int wedstrijdId)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                return entities.Wedstrijd_Spelers
                   .Where(p => p.Wedstrijd_Id == wedstrijdId)
                    .ToList();
            }
        }

        //Voorlopig niet nodig, ff in commentaar ipv weg voor de zekerheid
        //public static List<Kaart_Bonus> OphalenKaart_BonusViaKaart_Id(int kaartId)
        //{
        //    using (MunchkinEntities entities = new MunchkinEntities())
        //    {
        //        var query = entities.Kaart_Bonussen
        //                    .Where(x => x.Kaart_Id == kaartId);
        //        return query.ToList();
        //    }
        //}
        public static List<Kerkerkaart> OphalenKerkerkaarten()
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                return  entities.Kerkerkaarten
                            .Include(x=>x.Kaart)
                            .ToList() ;                        
               
            }
        }

        public static List<Schatkaart> OphalenSchatkaarten()
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                return  entities.Schatkaarten
                            .Include(x=>x.Kaart)
                            .ToList();         
               
            }
        }

        public static int ToevoegenStapel(Stapel stapel)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                entities.Stapels.Add(stapel);
                return entities.SaveChanges();
                                 
            }
        }

      
        //Van de Jens, veranderd naar functie die Domien had geschreven, ff in commentaar voor het geval er toch iets anders in de functie is
        //public static int ToevoegenKaartenStapel(Kaarten_Stapel kaarten_Stapel)
        //{
        //    using (MunchkinEntities entities = new MunchkinEntities())
        //    {
        //        entities.Kaarten_Stapels.Add(kaarten_Stapel);

        //        return entities.SaveChanges();
        //    }
        //}

        public static int ToevoegenWedstrijd(Wedstrijd wedstrijd)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                entities.Wedstrijden.Add(wedstrijd);

                return entities.SaveChanges();
            }
        }

        public static int ToevoegenWedstrijdSpelers(Wedstrijd_Speler wedstrijd_Speler)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                entities.Wedstrijd_Spelers.Add(wedstrijd_Speler);

                return entities.SaveChanges();
            }
        }
    }

}
