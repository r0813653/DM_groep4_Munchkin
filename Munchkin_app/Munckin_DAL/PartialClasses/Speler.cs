using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Speler:BasisKlasse
    {
        public override string this[string columnName]
        {
            get {
               
                if (columnName == "Naam" && string.IsNullOrWhiteSpace(Naam))
                {
                     return "Naam moet ingevuld zijn";
                }


                if (columnName == "Geslacht" && string.IsNullOrWhiteSpace(Geslacht))
                {
                    return "Geslacht is een verplicht veld";
                }
                return ""; 
            }
        }
    }
}
