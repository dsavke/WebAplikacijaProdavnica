using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAplikacijaProdavnica.Models
{
    public class ArtikliNaStanjuViewModel
    {
        public int ArtiklID { get; set; }
        public string Naziv { get; set; }
        public decimal Cijena { get; set; }
        public int Kolicina { get; set; }
    }
}