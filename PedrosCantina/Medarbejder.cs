using System;
using System.Collections.Generic;
using System.Text;

namespace PedrosCantina
{
    public class Medarbejder
    {
        public int MedarbejderId { get; set; }
        public string Navn { get; set; } = "";
        public string TelefonNummer { get; set; } = "";

        public override string ToString()
        {
            return $"Id: {MedarbejderId}, Navn: {Navn}, Telefon: {TelefonNummer}";
        }
    }
}
