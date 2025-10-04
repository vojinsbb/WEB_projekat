using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEB_projekat.Models
{
    public class BusDeparture
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        public string StartLocation { get; set; }
        public string EndLocation { get; set; }

        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}