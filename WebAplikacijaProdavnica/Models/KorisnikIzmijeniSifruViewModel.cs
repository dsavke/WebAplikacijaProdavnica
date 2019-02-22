using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAplikacijaProdavnica.Models
{
    public class KorisnikIzmijeniSifruViewModel
    {
        public int KorisnikID { get; set; }
        public string KorisnickoIme { get; set; }
        public string NovaSifra { get; set; }
        [CompareAttribute("NovaSifra")]
        public string NovaSifraPonovljena { get; set; }
        public string StaraSifra { get; set; }
        [CompareAttribute("StaraSifra")]
        public string StaraSifraPonovljena { get; set; }
    }
}