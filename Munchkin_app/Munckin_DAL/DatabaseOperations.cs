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

      

        public static int ToevoegenKaartenStapel(Kaarten_Stapel kaarten_Stapel)
        {
            using (MunchkinEntities entities = new MunchkinEntities())
            {
                entities.Kaarten_Stapels.Add(kaarten_Stapel);

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
