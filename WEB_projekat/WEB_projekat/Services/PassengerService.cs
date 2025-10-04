using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WEB_projekat.Models;

namespace WEB_projekat.Services
{
    public static class PassengerService
    {
        private const string ReservationsFile = "reservations.json";
        private const string BusesFile = "buses.json";
        public static bool CreateReservation(Passenger passenger, Guid busDepartureId, out string errorMessage)
        {
            errorMessage = string.Empty;

            var reservations = DataService.LoadData<Reservation>(ReservationsFile);
            var buses = DataService.LoadData<BusDeparture>(BusesFile);

            var bus = buses.FirstOrDefault(b => b.Id == busDepartureId);
            if (bus == null)
            {
                errorMessage = "Polazak ne postoji.";
                return false;
            }

            if (bus.AvailableSeats <= 0)
            {
                errorMessage = "Nema slobodnih mesta za ovaj polazak.";
                return false;
            }

            // provera da li vec postoji aktivna rezervacija za isti bus
            var existing = reservations.FirstOrDefault(r =>
                r.Passenger.Username == passenger.Username &&
                r.BusDepartureId == busDepartureId &&
                r.Status == ReservationStatus.Active);

            if (existing != null)
            {
                errorMessage = "Već imate aktivnu rezervaciju za ovaj polazak.";
                return false;
            }

            var newReservation = new Reservation
            {
                Passenger = passenger,
                BusDepartureId = busDepartureId,
                Status = ReservationStatus.Active,
                CreatedAt = DateTime.Now
            };

            reservations.Add(newReservation);

            bus.AvailableSeats--;
            DataService.SaveData(BusesFile, buses);
            DataService.SaveData(ReservationsFile, reservations);

            return true;
        }

        public static bool CancelReservation(Guid reservationId, Passenger passenger, out string errorMessage)
        {
            errorMessage = string.Empty;

            var reservations = DataService.LoadData<Reservation>(ReservationsFile);
            var buses = DataService.LoadData<BusDeparture>(BusesFile);

            foreach (var r in reservations.Where(r => r.Passenger == null))
            {
                r.Passenger = DataService.LoadData<Passenger>("users.json")
                                         .FirstOrDefault(p => p.Username == passenger.Username);
            }

            var reservation = reservations.FirstOrDefault(r => r.Id == reservationId && r.Passenger != null && r.Passenger.Username == passenger.Username);
            if (reservation == null)
            {
                errorMessage = "Rezervacija nije pronađena.";
                return false;
            }

            if (reservation.Status == ReservationStatus.Cancelled)
            {
                errorMessage = "Rezervacija je već otkazana.";
                return false;
            }

            var bus = buses.FirstOrDefault(b => b.Id == reservation.BusDepartureId);
            if (bus == null)
            {
                errorMessage = "Polazak za ovu rezervaciju više ne postoji.";
                return false;
            }

            DateTime busDepDate;
            if (!DateTime.TryParseExact(bus.DepartureTime, "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out busDepDate))
            {
                errorMessage = "Datum polaska nije validan.";
                return false;
            }

            var diff = busDepDate - DateTime.Now;

            if (diff.TotalHours < 24)
            {
                errorMessage = "Rezervaciju možete otkazati najkasnije 24h pre polaska.";
                return false;
            }

            if (diff.TotalMinutes < 24 * 60)
            {
                errorMessage = "Rezervaciju možete otkazati najkasnije 24h pre polaska.";
                return false;
            }

            reservation.Status = ReservationStatus.Cancelled;
            bus.AvailableSeats++;

            DataService.SaveData(BusesFile, buses);
            DataService.SaveData(ReservationsFile, reservations);

            return true;
        }

        public static List<Reservation> GetPassengerReservations(Passenger passenger)
        {
            //var reservations = DataService.LoadData<Reservation>(ReservationsFile);
            //return reservations.Where(r => r.Passenger.Username == passenger.Username).ToList();

            var reservations = DataService.LoadData<Reservation>(ReservationsFile);

            // fallback – ako Passenger nije serijalizovan
            foreach (var r in reservations.Where(r => r.Passenger == null))
            {
                r.Passenger = DataService.LoadData<Passenger>("users.json")
                                         .FirstOrDefault(p => p.Username == passenger.Username);
            }

            return reservations.Where(r => r.Passenger.Username == passenger.Username).ToList();
        }
    }
}