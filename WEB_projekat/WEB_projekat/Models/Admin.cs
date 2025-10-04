using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_projekat.Models
{
    public class Admin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}