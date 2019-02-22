using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAplikacijaProdavnica.DBModels;
using WebAplikacijaProdavnica.Models;
using System.Linq.Dynamic;

namespace WebAplikacijaProdavnica.Controllers
{
    public class KorisnikUlogaController : Controller
    {
        // GET: KorisnikUloga
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult Korisnici()
        {
            try
            {
                using (var context = new ProdavnicaContext())
                {
                    var korisnici = context.Korisniks.Select(k =>
                    new
                    {
                        Value = k.KorisnikID,
                        DisplayText = k.Ime + " " + k.Prezime
                    }).ToList();

                    return Json(new { Result = "OK", Options = korisnici });

                }
            }catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult Uloge()
        {
            try
            {
                using (var context = new ProdavnicaContext())
                {
                    var uloge = context.Ulogas.Select(k =>
                    new
                    {
                        Value = k.UlogaID,
                        DisplayText = k.Naziv
                    }).ToList();

                    return Json(new { Result = "OK", Options = uloge });

                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var context = new ProdavnicaContext())
                {
                    var korisnikUloge = context.KorisnikUlogas
                        .Select(o => new KorisnikUlogaViewModel
                        {
                            KorisnikID = o.KorisnikID,
                            KorisnikUlogaID = o.KorisnikUlogaID,
                            UlogaID = o.UlogaID
                        });
                    var count = korisnikUloge.Count();
                    var records = korisnikUloge.OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize).ToList();
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
        public JsonResult Create(KorisnikUlogaViewModel korisnikUlogaViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                using (var context = new ProdavnicaContext())
                {

                    var nekiKorisnik = context.Korisniks.Find(korisnikUlogaViewModel.KorisnikID);
                    var nekaUloga = context.Ulogas.Find(korisnikUlogaViewModel.UlogaID);

                    var ima = nekiKorisnik.KorisnikUlogas.Any(u => u.UlogaID == nekaUloga.UlogaID);

                    if(ima) return Json(new { Result = "ERROR", Message = "Korisniku je vec dodjeljena ta uloga" });

                    KorisnikUloga korisnikUloga = new KorisnikUloga
                    {

                        KorisnikID = korisnikUlogaViewModel.KorisnikID,
                        UlogaID = korisnikUlogaViewModel.UlogaID
                    };
                    context.KorisnikUlogas.Add(korisnikUloga);
                    context.SaveChanges();

                    korisnikUlogaViewModel.KorisnikUlogaID = korisnikUloga.KorisnikUlogaID;

                    return Json(new { Result = "OK", Record = korisnikUlogaViewModel });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult Edit(KorisnikUlogaViewModel korisnikUlogaViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new ProdavnicaContext())
                {

                    var nekiKorisnik = context.Korisniks.Find(korisnikUlogaViewModel.KorisnikID);
                    var nekaUloga = context.Ulogas.Find(korisnikUlogaViewModel.UlogaID);

                    var ima = nekiKorisnik.KorisnikUlogas.Any(u => u.UlogaID == nekaUloga.UlogaID && korisnikUlogaViewModel.KorisnikUlogaID != u.KorisnikUlogaID);

                    if (ima) return Json(new { Result = "ERROR", Message = "Korisniku je vec dodjeljena ta uloga" });

                    KorisnikUloga korisnikUloga = context.KorisnikUlogas.Find(korisnikUlogaViewModel.KorisnikUlogaID);
                    korisnikUloga.KorisnikID = korisnikUlogaViewModel.KorisnikID;
                    korisnikUloga.UlogaID = korisnikUlogaViewModel.UlogaID;
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
        public JsonResult Delete(int KorisnikUlogaID)
        {
            try
            {
                using (var context = new ProdavnicaContext())
                {
                    KorisnikUloga korisnikUloga = context.KorisnikUlogas.Find(KorisnikUlogaID);
                    context.KorisnikUlogas.Remove(korisnikUloga);
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