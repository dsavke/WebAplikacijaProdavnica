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
    public class DobavljacController : Controller
    {
        // GET: Dobavljac
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
                    var dobavljaci = context.Dobavljacs.Select(o => new
                    {
                        o.DobavljacID,
                        o.Naziv
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
        public JsonResult Create(DobavljacViewModel dobavljac)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                using (var context = new ProdavnicaContext())
                {
                    Dobavljac d = new Dobavljac
                    {

                        Naziv = dobavljac.Naziv
                    };
                    context.Dobavljacs.Add(d);
                    context.SaveChanges();

                    return Json(new { Result = "OK", Record = d });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Update(DobavljacViewModel dobavljac)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new ProdavnicaContext())
                {
                    Dobavljac d = context.Dobavljacs.Find(dobavljac.DobavljacID);
                    d.Naziv = dobavljac.Naziv;
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
        public JsonResult Delete(int DobavljacId)
        {
            try
            {
                using (var context = new ProdavnicaContext())
                {
                    Dobavljac d = context.Dobavljacs.Find(DobavljacId);
                    context.Dobavljacs.Remove(d);
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