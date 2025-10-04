using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_projekat.Models;
using WEB_projekat.Services;

namespace WEB_projekat.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult BusDepartures()
        {
            if (Session["User"] == null || (string)Session["Role"] != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            var buses = EmployeeService.GetAllBusDepartures();
            return View("busDepartures", buses);
        }

        public ActionResult CreateDeparture()
        {
            if (Session["User"] == null || (string)Session["Role"] != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }
            return View("createDeparture");
        }

        [HttpPost]
        public ActionResult CreateDeparture(BusDeparture model)
        {
            if (Session["User"] == null || (string)Session["Role"] != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Popunite sva polja ispravno.";
                return View("createDeparture", model);
            }

            DateTime depDate, arrDate;

            if (!DateTime.TryParseExact(Request["DepartureTime"], "yyyy-MM-ddTHH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out depDate))
            {
                ViewBag.Error = "Datum i vreme polaska moraju biti u formatu dd/MM/yyyy HH:mm.";
                return View("createDeparture", model);
            }

            if (!DateTime.TryParseExact(Request["ArrivalTime"], "yyyy-MM-ddTHH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out arrDate))
            {
                ViewBag.Error = "Datum i vreme dolaska moraju biti u formatu dd/MM/yyyy HH:mm.";
                return View("createDeparture", model);
            }

            model.DepartureTime = depDate.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            model.ArrivalTime = arrDate.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            if (EmployeeService.CreateBusDeparture(model, out string errorMessage))
            {
                TempData["Success"] = "Polazak uspešno dodat.";
                return RedirectToAction("BusDepartures");
            }
            else
            {
                ViewBag.Error = errorMessage;
                return View("createDeparture", model);
            }
        }

        public ActionResult DeleteDeparture(Guid id)
        {
            if (Session["User"] == null || (string)Session["Role"] != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            if (EmployeeService.DeleteBusDeparture(id, out string errorMessage))
            {
                TempData["Success"] = "Polazak uspešno obrisan.";
            }
            else
            {
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("BusDepartures");
        }

        public ActionResult Reservations(Guid busId)
        {
            if (Session["User"] == null || (string)Session["Role"] != "Employee")
            {
                return RedirectToAction("Login", "Account");
            }

            var reservations = EmployeeService.GetActiveReservationsForBus(busId);
            ViewBag.BusId = busId;
            return View("reservations", reservations);
        }
    }
}