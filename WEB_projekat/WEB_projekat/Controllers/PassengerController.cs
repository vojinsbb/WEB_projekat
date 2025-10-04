using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_projekat.Models;
using WEB_projekat.Services;

namespace WEB_projekat.Controllers
{
    public class PassengerController : Controller
    {
        public ActionResult Reservations()
        {
            if (Session["User"] == null || (string)Session["Role"] != "Passenger")
            {
                return RedirectToAction("Login", "Account");
            }

            var passenger = (Passenger)Session["User"];
            var reservations = PassengerService.GetPassengerReservations(passenger);

            return View("reservations", reservations);
        }

        public ActionResult AvailableDepartures()
        {
            if (Session["User"] == null || (string)Session["Role"] != "Passenger")
            {
                return RedirectToAction("Login", "Account");
            }

            var buses = EmployeeService.GetAllBusDepartures();
            return View("availableDepartures", buses); // fajl malim slovima
        }

        public ActionResult CreateReservation(Guid busId)
        {
            if (Session["User"] == null || (string)Session["Role"] != "Passenger")
            {
                return RedirectToAction("Login", "Account");
            }

            var passenger = (Passenger)Session["User"];
            if (PassengerService.CreateReservation(passenger, busId, out string errorMessage))
            {
                TempData["Success"] = "Rezervacija je uspešno napravljena.";
            }
            else
            {
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("reservations");
        }

        public ActionResult CancelReservation(Guid reservationId)
        {
            if (Session["User"] == null || (string)Session["Role"] != "Passenger")
            {
                return RedirectToAction("Login", "Account");
            }

            var passenger = (Passenger)Session["User"];
            if (PassengerService.CancelReservation(reservationId, passenger, out string errorMessage))
            {
                TempData["Success"] = "Rezervacija je uspešno otkazana.";
            }
            else
            {
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("reservations");
        }
    }
}