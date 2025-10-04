using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WEB_projekat.Models;

namespace WEB_projekat.Services
{
    public static class UserValidationService
    {
        public static bool IsJmbgFormatValid(string jmbg)
        {
            return !string.IsNullOrWhiteSpace(jmbg) && Regex.IsMatch(jmbg, @"^\d{13}$");
        }

        // proverava jedinstvenost JMBG među zaposlenima i putnicima
        public static bool IsJmbgUnique(string jmbg, string excludingUsername = null)
        {
            if (string.IsNullOrWhiteSpace(jmbg)) return false;

            var employees = DataService.LoadData<Employee>("employees.json");
            var passengers = DataService.LoadData<Passenger>("users.json");

            bool inEmployees = employees.Any(e => e.JMBG == jmbg && e.Username != excludingUsername);
            bool inPassengers = passengers.Any(p => p.JMBG == jmbg && p.Username != excludingUsername);

            return !(inEmployees || inPassengers);
        }
    }
}