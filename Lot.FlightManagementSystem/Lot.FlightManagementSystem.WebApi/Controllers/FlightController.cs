using Lot.FlightManagementSystem.WebApi.Data;
using Lot.FlightManagementSystem.WebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lot.FlightManagementSystem.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private const string SecretKey = "dnaskjdnajksdnjksj12nejn12jdd12jn12kdj";
        private readonly SymmetricSecurityKey _securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public FlightController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("getAllFlights"), Authorize]
        public async Task<ActionResult<List<Flight>>> GetAllFlights()
        {
            return Ok(await _appDbContext.Flights.ToListAsync());
        }

        [HttpGet("getFlightById")]
        public async Task<ActionResult<Flight>> GetFlightById(int id)
        {
            if (id <= 0) { return BadRequest("Invalid index"); }
            var flight = await _appDbContext.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound("Flight not found with given id");
            }
            return (Ok(flight));

        }


        [HttpPost("addFlight"), Authorize]
        public async Task<ActionResult<Flight>> AddFlight(AddFlightParams requestParams)
        {
            if(requestParams == null)
            {
                return BadRequest("Flight to add was not given");
            }
            //adjusting date to match proper timezone

            var flight = new Flight(requestParams.FlightNumber, requestParams.DepartureDate.AddHours(5), requestParams.DepartureLocation, requestParams.ArrivalLocation, requestParams.PlaneType);
            _appDbContext.Flights.Add(flight);

            await _appDbContext.SaveChangesAsync();

            return (Ok(flight)); ;
        }

        [HttpDelete("deleteFlight"), Authorize]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            if (id <= 0) { return BadRequest("Invalid index"); }
            var flight = await _appDbContext.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound("Flight not found with given id");
            }
            _appDbContext.Flights.Remove(flight);
            await _appDbContext.SaveChangesAsync();
            return (Ok("Flight succesfully removed"));
        }

        [HttpPut("modifyFlight"), Authorize]
        public async Task<ActionResult<Flight>> ModifyFlight(Flight flight)
        {
            if (flight == null) return BadRequest("Flight to modify was not given");

            var flightToModify = await _appDbContext.Flights.FindAsync(flight.FlightId);
            if (flightToModify == null)
            {
                return NotFound("Flight you want to modify does not exists in database");
            }

            flightToModify.FlightNumber = flight.FlightNumber;
            flightToModify.ArrivalLocation = flight.ArrivalLocation;
            flightToModify.DepartureLocation = flight.DepartureLocation;
            //adjusting date to match proper timezone
            flightToModify.DepartureDate = flight.DepartureDate.AddHours(5);
            flightToModify.PlaneType = flight.PlaneType;
            await _appDbContext.SaveChangesAsync();
            return Ok(flightToModify);
        }

    }
}
