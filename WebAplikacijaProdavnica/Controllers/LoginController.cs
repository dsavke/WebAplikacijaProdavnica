using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebAplikacijaProdavnica.DBModels;
using WebAplikacijaProdavnica.Helpers;
using WebAplikacijaProdavnica.Models;

namespace WebAplikacijaProdavnica.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult UlogujSe()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UlogujSe(LoginViewModel viewModel, string returnUrl)
        {

            using (var context = new ProdavnicaContext())
            {

                var korisnik = context.Korisniks.ToList().FirstOrDefault(k => k.Username == viewModel.Username && k.Password == Encryptor.MD5Hash(viewModel.Password));

                if (korisnik != null)
                {
                    var authTicket = new FormsAuthenticationTicket(
                                                     1,
                                                     viewModel.Username,
                                                     DateTime.Now,
                                                     DateTime.Now.AddMinutes(30),
                                                     false,
                                                     string.Join(",", korisnik.KorisnikUlogas.Select(u => u.Uloga.Naziv).Distinct())
                                                     //Primjer dodavanja uloga za korisnika
                                                     //string.Join(",", korisnik.KorisnikUlogas.Select(o => o.Uloga.Naziv).Distinct())
                                                     );

                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("IzmijeniSifru", "Login");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka");
                    return View();
                }

            }

        }


        public ActionResult IzlogujSe()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("UlogujSe", "Login");
        }

        [Authorize(Roles = "Administrator, Moderator, Promoter, Korisnik")]
        public ActionResult IzmijeniSifru()
        {
            using (var context = new ProdavnicaContext())
            {
                var korisnik = context.Korisniks.FirstOrDefault(k => k.Username == User.Identity.Name);
                var korisnikViewModel = new KorisnikIzmijeniSifruViewModel()
                {
                    KorisnikID = korisnik.KorisnikID,
                    KorisnickoIme = korisnik.Username,
                    StaraSifra = korisnik.Password
                };
                return View(korisnikViewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator, Promoter, Korisnik")]
        public ActionResult IzmijeniSifru(KorisnikIzmijeniSifruViewModel korisnikIzmijeniSifruViewModel)
        {
            using(var context = new ProdavnicaContext())
            {
                if (ModelState.IsValid)
                {
                    var korisnik = context.Korisniks.Find(korisnikIzmijeniSifruViewModel.KorisnikID);
                    if (korisnik.Password != korisnikIzmijeniSifruViewModel.NovaSifra)
                    {
                        korisnik.Password = korisnikIzmijeniSifruViewModel.NovaSifra;
                        context.SaveChanges();
                        return View(korisnikIzmijeniSifruViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Greska pri unosu sifre!");
                        return View(korisnikIzmijeniSifruViewModel);
                    }
                }
                else
                {
                    return View(korisnikIzmijeniSifruViewModel);
                }
            }
        }
    }
}