using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_projekat.Models
{
    public abstract class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string JMBG { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
    }
}