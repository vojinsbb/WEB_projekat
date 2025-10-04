using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEB_projekat.Models;

namespace WEB_projekat.Services
{
    public static class EmployeeService
    {
        private const string BusesFile = "buses.json";
        private const string ReservationsFile = "reservations.json";

        public static bool CreateBusDeparture(BusDeparture newBus, out string errorMessage)
        {
            errorMessage = string.Empty;
            var buses = DataService.LoadData<BusDeparture>(BusesFile);

            newBus.AvailableSeats = newBus.TotalSeats;

            buses.Add(newBus);
            DataService.SaveData(BusesFile, buses);
            return true;
        }

        public static bool DeleteBusDeparture(Guid busDepartureId, out string errorMessage)
        {
            errorMessage = string.Empty;
            var buses = DataService.LoadData<BusDeparture>(BusesFile);
            var reservations = DataService.LoadData<Reservation>(ReservationsFile);

            var bus = buses.FirstOrDefault(b => b.Id == busDepartureId);
            if (bus == null)
            {
                errorMessage = "Polazak ne postoji.";
                return false;
            }

            buses.Remove(bus);

            foreach (var res in reservations.Where(r => r.BusDepartureId == busDepartureId && r.Status == ReservationStatus.Active))
            {
                res.Status = ReservationStatus.Cancelled;
            }

            DataService.SaveData(BusesFile, buses);
            DataService.SaveData(ReservationsFile, reservations);

            return true;
        }

        public static List<Reservation> GetActiveReservationsForBus(Guid busDepartureId)
        {
            var reservations = DataService.LoadData<Reservation>(ReservationsFile);
            return reservations
                .Where(r => r.BusDepartureId == busDepartureId && r.Status == ReservationStatus.Active)
                .ToList();
        }

        public static List<BusDeparture> GetAllBusDepartures()
        {
            return DataService.LoadData<BusDeparture>(BusesFile);
        }
    }
}