using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAplikacijaProdavnica.DBModels;
using WebAplikacijaProdavnica.Models;

namespace WebAplikacijaProdavnica.Controllers
{
    public class ProdavnicaController : Controller
    {

        public JsonResult PromijeniJezik(string lang)
        {
            HttpCookie myCookie = new HttpCookie("Jezik");
            DateTime now = DateTime.Now;

            myCookie.Value = lang;

            myCookie.Expires = now.AddMonths(10);

            Response.Cookies.Add(myCookie);

            return Json(new { Success = true });
            
        }

        // GET: Prodavnica
        public ActionResult Index()
        {
            using (var context = new ProdavnicaContext())
            {

                var artikli = context.Artikls.Select(a =>
                new ArtiklViewModel()
                {
                    ArtiklID = a.ArtiklID,
                    Cijena = a.Cijena,
                    DobavljacID = a.DobavljacID,
                    DobavljacNaziv = a.Dobavljac.Naziv,
                    Kolicina = a.Kolicina,
                    Naziv = a.Naziv,
                    Opis = a.Opis,
                    Slika = a.Slika
                }).ToList();
                return View(artikli);
            }
        }

        public ActionResult Create()
        {
            using (var context = new ProdavnicaContext())
            {
                ViewBag.Dobavljaci = context.Dobavljacs.Select(d =>
                new SelectListItem()
                {
                    Text = d.Naziv,
                    Value = "" + d.DobavljacID
                }).ToList();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(ArtiklViewModel artiklViewModel)
        {
            using(var context = new ProdavnicaContext())
            {
                var artikl = new Artikl()
                {
                    Cijena = artiklViewModel.Cijena,
                    DobavljacID = artiklViewModel.DobavljacID,
                    Kolicina = artiklViewModel.Kolicina,
                    Naziv = artiklViewModel.Naziv,
                    Opis = artiklViewModel.Opis,
                    Slika = artiklViewModel.Slika
                };
                context.Artikls.Add(artikl);
                context.SaveChanges();

                return RedirectToAction("Index");

            }
        }

        public ActionResult Edit(int id)
        {
            using(var context = new ProdavnicaContext())
            {
                var a = context.Artikls.Find(id);

                var artiklViewModel = new ArtiklViewModel()
                {
                    ArtiklID = a.ArtiklID,
                    Cijena = a.Cijena,
                    DobavljacID = a.DobavljacID,
                    Kolicina = a.Kolicina,
                    Naziv = a.Naziv,
                    Opis = a.Opis,
                    Slika = a.Slika
                };

                ViewBag.Dobavljaci = context.Dobavljacs.Select(d =>
                new SelectListItem()
                {
                    Text = d.Naziv,
                    Value = "" + d.DobavljacID
                }).ToList();

                return View(artiklViewModel);

            }
        }

        [HttpPost]
        public ActionResult Edit(ArtiklViewModel artiklViewModel)
        {
            using (var context = new ProdavnicaContext())
            {
                var artikl = context.Artikls.Find(artiklViewModel.ArtiklID);

                artikl.ArtiklID = artiklViewModel.ArtiklID;
                artikl.Cijena = artiklViewModel.Cijena;
                artikl.DobavljacID = artiklViewModel.DobavljacID;
                artikl.Kolicina = artiklViewModel.Kolicina;
                artikl.Naziv = artiklViewModel.Naziv;
                artikl.Opis = artiklViewModel.Opis;
                artikl.Slika = artiklViewModel.Slika;

                context.SaveChanges();

                return RedirectToAction("Index");

            }
        }

        public ActionResult Delete(int id)
        {
            using(var context = new ProdavnicaContext())
            {
                var a = context.Artikls.Find(id);

                var artiklViewModel = new ArtiklViewModel()
                {
                    ArtiklID = a.ArtiklID,
                    Cijena = a.Cijena,
                    DobavljacID = a.DobavljacID,
                    DobavljacNaziv = a.Dobavljac.Naziv,
                    Kolicina = a.Kolicina,
                    Naziv = a.Naziv,
                    Opis = a.Opis,
                    Slika = a.Slika
                };

                return View(artiklViewModel);

            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var artiklID = Convert.ToInt32(id);

            using(var context = new ProdavnicaContext())
            {
                context.Artikls.Remove(context.Artikls.Find(artiklID));
                context.SaveChanges();

                return RedirectToAction("Index");
            }

        }

    }
}