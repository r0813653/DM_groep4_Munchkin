using System;
using System.Collections.Generic;
using System.Linq;
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
                return 0; //entities.SaveChanges();
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

    }

}
