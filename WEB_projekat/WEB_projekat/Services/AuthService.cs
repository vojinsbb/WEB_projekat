using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEB_projekat.Models;

namespace WEB_projekat.Services
{
    public static class AuthService
    {
        private const string PassengersFile = "users.json";
        private const string EmployeesFile = "employees.json";
        private const string AdminsFile = "admins.json";

        public static bool RegisterPassenger(Passenger newPassenger, out string errorMessage)
        {
            errorMessage = string.Empty;
            var passengers = DataService.LoadData<Passenger>(PassengersFile);

            if (passengers.Any(p => p.Username == newPassenger.Username))
            {
                errorMessage = "Korisničko ime već postoji.";
                return false;
            }
            if (passengers.Any(p => p.JMBG == newPassenger.JMBG))
            {
                errorMessage = "JMBG već postoji.";
                return false;
            }
            if (passengers.Any(p => p.Email == newPassenger.Email))
            {
                errorMessage = "E-mail već postoji.";
                return false;
            }

            passengers.Add(newPassenger);
            DataService.SaveData(PassengersFile, passengers);
            return true;
        }

        public static object Login(string username, string password, out string role)
        {
            role = string.Empty;

            var passengers = DataService.LoadData<Passenger>(PassengersFile);
            var passenger = passengers.FirstOrDefault(p => p.Username == username && p.Password == password);
            if (passenger != null)
            {
                role = "Passenger";
                return passenger;
            }

            var employees = DataService.LoadData<Employee>(EmployeesFile);
            var employee = employees.FirstOrDefault(e => e.Username == username && e.Password == password);
            if (employee != null)
            {
                if (employee.IsBlocked)
                {
                    role = "BlockedEmployee";
                    return null;
                }

                role = "Employee";
                return employee;
            }

            var admins = DataService.LoadData<Admin>(AdminsFile);
            var admin = admins.FirstOrDefault(a => a.Username == username && a.Password == password);
            if (admin != null)
            {
                role = "Admin";
                return admin;
            }

            return null;
        }
    }
}