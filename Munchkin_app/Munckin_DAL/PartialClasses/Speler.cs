﻿using Munchkin_MODELS;
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
                if (columnName=="Id" && Id <=0 )
                {
                    return "Id moet groter dan 0 zijn.";
                }
                if (columnName == "Naam" && string.IsNullOrWhiteSpace(Naam))
                {
                     return "Naam moet ingevuld zijn";
                }
                if (columnName == "Level" && Level<0 && Level>10 )
                {
                     return "Level moet gelijk of groter zijn dan 0 en kleiner dan 10";
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
