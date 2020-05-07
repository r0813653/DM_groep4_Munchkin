using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Kerkerkaart:BasisKlasse
    {
        public override string this[string columnName]
        {
            get {
                    if (columnName=="Id" && Id >=0)
                    {
                         return "Id mag niet kleiner of gelijk zijn aan 0";
                    }
                if (columnName == "Beschrijving_2" && string.IsNullOrWhiteSpace(Beschrijving_2))
                {
                     return "beschrijving is verplicht";
                }
                if (columnName == "Portie_Ellende" && string.IsNullOrWhiteSpace(Portie_Ellende))
                {
                     return "Portie ellende is een verplicht veld";
                }
                if (columnName == "Level" && Level >=0 && Level<10)
                {
                     return "Level mag niet kleiner of gelijk zijn aan 0 en mag niet groter zijn dan 10";
                }
                if (columnName == "Aantal_schatten" && Aantal_schatten <=0)
                {
                     return "aantal schatten mag niet gelijk zijn of kleiner dan 0";
                }
                if (columnName == "Ondood")
                {
                    // return "";
                }
                if (columnName == "Aantal_Levels" && Aantal_Levels<=0)
                {
                     return "Aantal levels moet groter zijn dan 0";
                }
                return "";
                 }
        }
    }
}
