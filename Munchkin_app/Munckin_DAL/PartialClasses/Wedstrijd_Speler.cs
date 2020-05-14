using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Wedstrijd_Speler: BasisKlasse
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Level" && Level <= 0)
                {
                    return "Level moet een positief getal zijn!";
                }
                if (columnName == "Ras" && string.IsNullOrWhiteSpace(Ras))
                {
                    return "Iedereen moet een ras hebben!";
                }
                if (columnName == "Vluchtbonus" && Vluchtbonus < 0)
                {
                    return "Vluchtbonus mag niet negatief zijn!";
                }
                if (columnName == "Gevechtsbonus" && Gevechtsbonus < 0)
                {
                    return "Gevechtsbonus mag niet negatief zijn!";
                }
                return "";
            }
        }
    }
}
