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

        public static Stapel OphalenStapelViaId(int Id)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Stapels
                            //Gaat 2de select?
                            .Include(x => x.Kaarten_Stapels.Select(sub => sub.Kaart).Select(b => b.Schatkaart))
                            .Where(x => x.Id == Id);
                return query.SingleOrDefault();
            }
        }

        public static Kaart OphalenKaartViaId(int id)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Kaarten
                               .Include(x => x.Schatkaart)
                               .Include(x => x.Kerkerkaart)
                              .Where(x => x.Id == id);
                return query.SingleOrDefault();
            }
        }

        public static Wedstrijd_Speler OphalenWedstrijd_SpelerViaId(int id)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Wedstrijd_Spelers
                            .Include(x => x.Speler)
                            .Include(x => x.Stapel_Handkaarten)
                            .Where(x => x.Id == id);
                return query.SingleOrDefault();
            }
        }

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
        
        public static Wedstrijd OphalenWedstrijdViaId(int Id)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Wedstrijden
                            .Where(x => x.Id == Id)
                            .Include(a => a.Kerkerkaarten_Aflegstapel)
                            .Include(b => b.Schatkaarten_Aflegstapel)
                            .Include(c => c.Schatkaarten_Aflegstapel.Kaarten_Stapels)
                            .Include(d => d.Schatkaarten_Trekstapel)
                            .Include(e => e.Kerkerkaarten_Trekstapel);

                return query.SingleOrDefault();
            }
        }
        public static List<Kaarten_Stapel> OphalenKaarten_StapelsViaStapelId(int stapelId)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                return entities.Kaarten_Stapels
                    .Include(a=>a.Kaart.Schatkaart)
                    .Include(a=>a.Stapel)
                    .Include(x => x.Kaart)
                    .Include(x => x.Kaart.Kerkerkaart)
                    .Include(x => x.Kaart.Schatkaart)
                    .Include(x => x.Kaart.Type)
                    .Where(p => p.Stapel_Id == stapelId)
                    .ToList();
            }
        }
        public static List<Wedstrijd_Speler> OphalenWedstrijd_SpelersViaWedstrijdId(int wedstrijdId)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                return entities.Wedstrijd_Spelers
                    .Include(a=>a.Stapel_Handkaarten)
                    .Include(b=>b.Stapel_Veldkaarten)
                    .Include(x => x.Speler)
                   .Where(p => p.Wedstrijd_Id == wedstrijdId)
                    .ToList();
            }
        }

        public static List<Kerkerkaart> OphalenKerkerkaarten()
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                return entities.Kerkerkaarten
                            .Include(x => x.Kaart)
                            .ToList();

            }
        }

        public static List<Schatkaart> OphalenSchatkaarten()
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                return entities.Schatkaarten
                            .Include(x => x.Kaart)
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
