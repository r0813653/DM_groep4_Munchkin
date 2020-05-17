using System;
using System.Collections.Generic;
using System.Linq;
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
                return 0; //entities.SaveChanges();
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

        public static Stapel OphalenStapelViaVeldkaartenId(int veldKaartenId)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Stapels
                            .Include(x => x.Kaarten_Stapels.Select(sub => sub.Kaart))
                            .Where(x => x.Id == veldKaartenId);
                return query.SingleOrDefault();
            }
        }

        public static Kaart OphalenKaartViaId(int id)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Kaarten
                              .Where(x => x.Id == id);
                return query.SingleOrDefault();
            }
        }

        public static Wedstrijd_Speler OphalenWedstrijd_SpelerViaId(int id)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                var query = entities.Wedstrijd_Spelers
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

    }
}
