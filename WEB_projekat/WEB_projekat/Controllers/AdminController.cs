using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WEB_projekat.Models;
using WEB_projekat.Services;

namespace WEB_projekat.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Employees()
        {
            if (Session["User"] == null || (string)Session["Role"] != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var employees = AdminService.GetAllEmployees();
            return View("employees", employees);
        }

        public ActionResult CreateEmployee()
        {
            if (Session["User"] == null || (string)Session["Role"] != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View("createEmployee");
        }

        [HttpPost]
        public ActionResult CreateEmployee(Employee model)
        {
            if (Session["User"] == null || (string)Session["Role"] != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!UserValidationService.IsJmbgFormatValid(model.JMBG))
            {
                ViewBag.Error = "JMBG mora imati 13 cifara.";
                return View(model);
            }

            if (!UserValidationService.IsJmbgUnique(model.JMBG))
            {
                ViewBag.Error = "JMBG je već u upotrebi (među putnicima ili službenicima).";
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

            if (AdminService.CreateEmployee(model, out string errorMessage))
            {
                TempData["Success"] = "Službenik uspešno kreiran.";
                return RedirectToAction("Employees");
            }
            else
            {
                ViewBag.Error = errorMessage;
            }

            return View("createEmployee", model);
        }

        public ActionResult EditEmployee(string username)
        {
            if (Session["User"] == null || (string)Session["Role"] != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var employees = AdminService.GetAllEmployees();
            var employee = employees.Find(e => e.Username == username);

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View("editEmployee", employee);
        }

        [HttpPost]
        public ActionResult EditEmployee(Employee updatedEmployee)
        {
            if (Session["User"] == null || (string)Session["Role"] != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            DateTime parsedDate;
            if (!DateTime.TryParseExact(updatedEmployee.BirthDate, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                ViewBag.Error = "Datum rođenja mora biti u formatu dd/MM/yyyy.";
                return View(updatedEmployee);
            }

            updatedEmployee.BirthDate = parsedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (AdminService.UpdateEmployee(updatedEmployee, out string errorMessage))
            {
                TempData["Success"] = "Službenik uspešno ažuriran.";
                return RedirectToAction("Employees");
            }
            else
            {
                ViewBag.Error = errorMessage;
                return View("editEmployee", updatedEmployee);
            }
        }

        public ActionResult BlockEmployee(string username)
        {
            if (Session["User"] == null || (string)Session["Role"] != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (AdminService.BlockEmployee(username, out string errorMessage))
            {
                TempData["Success"] = "Službenik blokiran.";
            }
            else
            {
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("Employees");
        }
    }
}