using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAplikacijaProdavnica.DBModels;
using WebAplikacijaProdavnica.Models;

namespace WebAplikacijaProdavnica.Controllers
{
    public class RacunController : Controller
    {
        // GET: Racun
        [Authorize(Roles = "Administrator, Moderator, Promoter, Korisnik")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator, Promoter, Korisnik")]
        public ActionResult Create()
        {
            using (var context = new ProdavnicaContext())
            {
                var artikli = context.Artikls.Where(a => a.Kolicina > 0).Select(a =>
                    new ArtikliNaStanjuViewModel()
                    {
                        ArtiklID = a.ArtiklID,
                        Cijena = a.Cijena,
                        Kolicina = a.Kolicina,
                        Naziv = a.Naziv
                    }).ToList();

                return View(artikli);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator, Promoter, Korisnik")]
        public JsonResult Create(List<KupljeniArtikliViewModel> kupljeneStavke)
        {
            using (var context = new ProdavnicaContext())
            {

                if (!ModelState.IsValid)
                {
                    return Json(new { Success = false, Message = "Pokusaj ponovo!" });
                }

                if (kupljeneStavke == null)
                {
                    return Json(new { Success = false, Message = "Pokusaj ponovo!" });
                }

                if (kupljeneStavke.Any(k => k.Kolicina == 0 ||  k.ArtiklID == 0))
                {
                    return Json(new { Success = false, Message = "Pokusaj ponovo!" });
                }

                if (kupljeneStavke.Any(k => k.Kolicina > context.Artikls.Find(k.ArtiklID).Kolicina))
                {
                    return Json(new { Success = false, Message = "Pokusaj ponovo!" });
                }


                var racun = new Racun() { DatumIzdavanja = DateTime.Today };

                var korisnik = context.Korisniks.FirstOrDefault(k => k.Username == User.Identity.Name);
                racun.KorisnikID = korisnik.KorisnikID;
                var zadnjiRacun = context.Racuns.Where(r => r.DatumIzdavanja == racun.DatumIzdavanja).Count();
                racun.BrojRacuna = zadnjiRacun + 1;

                var ukupanIznos = kupljeneStavke.Sum(k => k.Kolicina * context.Artikls.Find(k.ArtiklID).Cijena);
                racun.UkupanIznos = ukupanIznos;

                foreach (var item in kupljeneStavke)
                {
                    Stavka stavka = new Stavka();
                    stavka.ArtikalID = item.ArtiklID;
                    stavka.Kolicina = item.Kolicina;
                    stavka.Cijena = context.Artikls.Find(item.ArtiklID).Cijena;
                    racun.Stavkas.Add(stavka);

                    context.Artikls.Find(item.ArtiklID).Kolicina -= (int)stavka.Kolicina;

                }

                context.Racuns.Add(racun);

                context.SaveChanges();

                return Json(new { Success = true });
            }
        }

    }
}