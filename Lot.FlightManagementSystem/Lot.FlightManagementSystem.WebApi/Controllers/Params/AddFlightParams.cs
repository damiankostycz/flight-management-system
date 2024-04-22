using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lot.FlightManagementSystem.WebApi.Data
{
    public class AddFlightParams
    {
        public string FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public string PlaneType { get; set; }
    }
}
