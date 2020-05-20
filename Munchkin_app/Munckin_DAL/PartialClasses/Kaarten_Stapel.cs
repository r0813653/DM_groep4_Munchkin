using Munchkin_MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munckin_DAL
{
    public partial class Kaarten_Stapel : BasisKlasse
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Kaart_Id" && Kaart_Id <= 0)
                {
                    return "Kaart_Id moet een positief getal zijn!";
                }
                if (columnName == "Stapel_Id" && Stapel_Id <= 0)
                {
                    return "Stapel_Id moet een positief getal zijn!";
                }
                return "";
            }
        }
    }
}
