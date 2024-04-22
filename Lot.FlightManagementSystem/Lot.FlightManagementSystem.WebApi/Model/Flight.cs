using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lot.FlightManagementSystem.WebApi.Model
{
    public class Flight
    {
        public Flight(string flightNumber, DateTime departureDate, string departureLocation, string arrivalLocation, string planeType)
        {
            FlightNumber = flightNumber;
            DepartureDate = departureDate;
            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;
            PlaneType = planeType;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public string PlaneType { get; set; }
    }
}
