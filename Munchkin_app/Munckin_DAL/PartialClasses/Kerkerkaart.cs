using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Kerkerkaart :BasisKlasse
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Id" && Id <= 0)
                {
                    return "Id mag niet kleiner of gelijk zijn aan 0";
                }
                return "";
            }
        }

        public int Tijdelijke_Bonus { get; set; } = 0;
    }
}
