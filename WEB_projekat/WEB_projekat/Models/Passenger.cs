using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_projekat.Models
{
    public class Passenger : User
    {
        public string PassportNumber { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}