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
    public class UlogaController : Controller
    {
        // GET: Uloga
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var context = new ProdavnicaContext())
                {
                    var uloge = context.Ulogas
                        .Select(o => new UlogaViewModel
                        {
                            UlogaID = o.UlogaID,
                            Naziv = o.Naziv
                        });
                    var count = uloge.Count();
                    var records = uloge.OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize).ToList();
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
        public JsonResult Create(UlogaViewModel ulogaViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                using (var context = new ProdavnicaContext())
                {
                    Uloga uloga = new Uloga
                    {
                        UlogaID = ulogaViewModel.UlogaID,
                        Naziv = ulogaViewModel.Naziv
                    };
                    context.Ulogas.Add(uloga);
                    context.SaveChanges();

                    return Json(new { Result = "OK", Record = uloga });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Edit(UlogaViewModel ulogaViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new ProdavnicaContext())
                {
                    Uloga uloga = context.Ulogas.Find(ulogaViewModel.UlogaID);
                    uloga.Naziv = ulogaViewModel.Naziv;
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
        public JsonResult Delete(int UlogaID)
        {
            try
            {
                using (var context = new ProdavnicaContext())
                {
                    Uloga uloga = context.Ulogas.Find(UlogaID);
                    context.Ulogas.Remove(uloga);
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