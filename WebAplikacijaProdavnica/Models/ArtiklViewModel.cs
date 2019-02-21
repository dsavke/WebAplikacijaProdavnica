using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAplikacijaProdavnica.Models
{
    public class ArtiklViewModel
    {
        [Display(Name = "ArtiklID", ResourceType = typeof(Resources.Resource))]
        public int ArtiklID { get; set; }
        [Display(Name = "Naziv", ResourceType = typeof(Resources.Resource))]
        [Required]
        public string Naziv { get; set; }
        [Display(Name = "Cijena", ResourceType = typeof(Resources.Resource))]
        public decimal Cijena { get; set; }
        [Display(Name = "Kolicina", ResourceType = typeof(Resources.Resource))]
        public int Kolicina { get; set; }
        [Display(Name = "Opis", ResourceType = typeof(Resources.Resource))]
        public string Opis { get; set; }
        [Display(Name = "DobavljacID", ResourceType = typeof(Resources.Resource))]
        public int DobavljacID { get; set; }
        [Display(Name = "Slika", ResourceType = typeof(Resources.Resource))]
        public string Slika { get; set; }
        [Display(Name = "DobavljacID", ResourceType = typeof(Resources.Resource))]
        public string DobavljacNaziv { get; set; }
    }
}