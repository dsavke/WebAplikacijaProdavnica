using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebAplikacijaProdavnica.DBModels;

namespace WebAplikacijaProdavnica.Models
{
    public class KorisnikViewModel:IValidatableObject
    {
        public int KorisnikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Adresa { get; set; }
        public string Pol { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var context = new ProdavnicaContext())
            {

                bool postojiUsernameUBazi = context.Korisniks.Any(k => k.Username == Username);

                if (postojiUsernameUBazi)
                {
                    yield return new ValidationResult("Greska", new[] { nameof(Username) });
                }

            }
        }

    }
}