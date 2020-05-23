using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public static class GlobalVariables
    {      
        public static int indexer = 0;
        public static int WedstrijdId { get; set; }
        public static List<Wedstrijd_Speler> wedstrijd_Spelers = new List<Wedstrijd_Speler>();
        public static Wedstrijd_Speler actieveSpeler;    
    }
}
