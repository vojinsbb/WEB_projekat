using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_projekat.Models;
using WEB_projekat.Services;
using static System.Collections.Specialized.BitVector32;

namespace WEB_projekat.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = AuthService.Login(username, password, out string role);

            if (user == null)
            {
                if (role == "BlockedEmployee")
                {
                    ViewBag.Error = "Ovaj službenik je blokiran i ne može da se prijavi.";
                }
                else
                {
                    ViewBag.Error = "Pogrešno korisničko ime ili lozinka.";
                }
                return View();
            }

            Session["User"] = user;
            Session["Role"] = role;

            switch (role)
            {
                case "Passenger":
                    return RedirectToAction("Reservations", "Passenger");
                case "Employee":
                    return RedirectToAction("BusDepartures", "Employee");
                case "Admin":
                    return RedirectToAction("Employees", "Admin");
                default:
                    return RedirectToAction("Login");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Passenger model)
        {
            if (ModelState.IsValid)
            {
                if (!UserValidationService.IsJmbgFormatValid(model.JMBG))
                {
                    ViewBag.Error = "JMBG mora imati 13 cifara.";
                    return View(model);
                }

                if (!UserValidationService.IsJmbgUnique(model.JMBG))
                {
                    ViewBag.Error = "JMBG već postoji.";
                    return View(model);
                }

                DateTime parsedDate;
                if (!DateTime.TryParseExact(model.BirthDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    ViewBag.Error = "Datum rođenja mora biti u formatu dd/MM/yyyy.";
                    return View(model);
                }

                model.BirthDate = parsedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (AuthService.RegisterPassenger(model, out string errorMessage))
                {
                    ViewBag.Success = "Uspešno ste se registrovali. Možete se prijaviti.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Error = errorMessage;
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}