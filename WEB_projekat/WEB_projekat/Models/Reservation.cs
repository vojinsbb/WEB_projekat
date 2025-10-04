using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_projekat.Models
{
    public enum ReservationStatus
    {
        Active,
        Cancelled
    }

    public class Reservation
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Passenger Passenger { get; set; }
        public Guid BusDepartureId { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Active;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}