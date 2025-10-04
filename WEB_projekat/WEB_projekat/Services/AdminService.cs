using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEB_projekat.Models;

namespace WEB_projekat.Services
{
    public static class AdminService
    {
        private const string EmployeesFile = "employees.json";

        public static bool CreateEmployee(Employee newEmployee, out string errorMessage)
        {
            errorMessage = string.Empty;
            var employees = DataService.LoadData<Employee>(EmployeesFile);

            if (employees.Any(e => e.Username == newEmployee.Username))
            {
                errorMessage = "Korisničko ime već postoji.";
                return false;
            }
            if (employees.Any(e => e.JMBG == newEmployee.JMBG))
            {
                errorMessage = "JMBG već postoji.";
                return false;
            }
            if (employees.Any(e => e.Email == newEmployee.Email))
            {
                errorMessage = "E-mail već postoji.";
                return false;
            }

            employees.Add(newEmployee);
            DataService.SaveData(EmployeesFile, employees);
            return true;
        }

        public static bool UpdateEmployee(Employee updatedEmployee, out string errorMessage)
        {
            errorMessage = string.Empty;
            var employees = DataService.LoadData<Employee>(EmployeesFile);

            var employee = employees.FirstOrDefault(e => e.Username == updatedEmployee.Username);
            if (employee == null)
            {
                errorMessage = "Službenik nije pronađen.";
                return false;
            }

            if (employees.Any(e => e.Email == updatedEmployee.Email && e.Username != updatedEmployee.Username))
            {
                errorMessage = "E-mail već postoji kod drugog službenika.";
                return false;
            }

            employee.Password = updatedEmployee.Password;
            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.BirthDate = updatedEmployee.BirthDate;
            employee.Email = updatedEmployee.Email;
            employee.IsBlocked = updatedEmployee.IsBlocked;

            DataService.SaveData(EmployeesFile, employees);
            return true;
        }

        public static bool BlockEmployee(string username, out string errorMessage)
        {
            errorMessage = string.Empty;
            var employees = DataService.LoadData<Employee>(EmployeesFile);

            var employee = employees.FirstOrDefault(e => e.Username == username);
            if (employee == null)
            {
                errorMessage = "Službenik nije pronađen.";
                return false;
            }

            employee.IsBlocked = true;
            DataService.SaveData(EmployeesFile, employees);
            return true;
        }
        public static List<Employee> GetAllEmployees()
        {
            return DataService.LoadData<Employee>(EmployeesFile);
        }
    }
}