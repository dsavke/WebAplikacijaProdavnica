using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAplikacijaProdavnica.DBModels;
using WebAplikacijaProdavnica.Models;
using System.Linq.Dynamic;
using WebAplikacijaProdavnica.Helpers;

namespace WebAplikacijaProdavnica.Controllers
{
    public class KorisnikController : Controller
    {
        // GET: Korisnik
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var context = new ProdavnicaContext())
                {
                    var dobavljaci = context.Korisniks
                        .Select(o => new KorisnikViewModel
                        {
                            KorisnikID = o.KorisnikID,
                            Adresa = o.Adresa,
                            DatumRodjenja = o.DatumRodjenja,
                            Prezime = o.Prezime,
                            Ime = o.Ime,
                            Pol = o.Pol,
                            Password = o.Password,
                            Username = o.Username
                        });
                    var count = dobavljaci.Count();
                    var records = dobavljaci.OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new
                    {
                        Result = "OK",
                        Records = records,
                        TotalRecordCount = count
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult Create(KorisnikViewModel korisnikViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                using (var context = new ProdavnicaContext())
                {
                    Korisnik korisnik = new Korisnik
                    {
                        Prezime = korisnikViewModel.Prezime,
                        Ime = korisnikViewModel.Ime,
                        Pol = korisnikViewModel.Pol,
                        Adresa = korisnikViewModel.Adresa,
                        DatumRodjenja = korisnikViewModel.DatumRodjenja,
                        KorisnikID = korisnikViewModel.KorisnikID,
                        Username = korisnikViewModel.Username,
                        Password = korisnikViewModel.Password
                    };
                    context.Korisniks.Add(korisnik);
                    context.SaveChanges();

                    return Json(new { Result = "OK", Record = korisnik });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult Edit(KorisnikViewModel korisnikViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new ProdavnicaContext())
                {
                    Korisnik korisnik = context.Korisniks.Find(korisnikViewModel.KorisnikID);
                    korisnik.Ime = korisnikViewModel.Ime;
                    korisnik.Prezime = korisnikViewModel.Prezime;
                    korisnik.Adresa = korisnikViewModel.Adresa;
                    korisnik.Pol = korisnikViewModel.Pol;
                    korisnik.DatumRodjenja = korisnikViewModel.DatumRodjenja;
                    korisnik.Username = korisnikViewModel.Username;
                    korisnik.Password = korisnikViewModel.Password;
                    context.SaveChanges();
                }

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult Delete(int KorisnikID)
        {
            try
            {
                using (var context = new ProdavnicaContext())
                {
                    Korisnik korisnik = context.Korisniks.Find(KorisnikID);
                    context.Korisniks.Remove(korisnik);
                    context.SaveChanges();
                    return Json(new { Result = "OK" });
                }


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


    }

}