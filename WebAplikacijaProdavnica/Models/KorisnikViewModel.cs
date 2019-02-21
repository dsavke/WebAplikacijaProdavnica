using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAplikacijaProdavnica.Models
{
    public class KorisnikViewModel
    {
        public int KorisnikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Adresa { get; set; }
        public string Pol { get; set; }
        public DateTime DatumRodjenja { get; set; }
    }
}