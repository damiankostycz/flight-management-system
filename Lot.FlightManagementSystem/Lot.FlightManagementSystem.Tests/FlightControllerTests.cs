using Lot.FlightManagementSystem.WebApi;
using Lot.FlightManagementSystem.WebApi.Controllers;
using Lot.FlightManagementSystem.WebApi.Data;
using Lot.FlightManagementSystem.WebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;


namespace Lot.FlightManagementSystem.Tests
{
    public class FlightControllerTests
    {

        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }




        [Fact]
        public async Task GetAllFlights_ReturnAllFlights()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                dbContext.Flights.Add(new Flight("ABC123", DateTime.Now, "Location1", "Location2", "Type1"));
                dbContext.Flights.Add(new Flight("DEF456", DateTime.Now, "Location3", "Location4", "Type2"));

                await dbContext.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new FlightController(context);

                // Act
                var result = await controller.GetAllFlights();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var flights = Assert.IsType<List<Flight>>(okResult.Value);
                Assert.Equal(2, flights.Count);
            }
        }


        [Fact]
        public async Task GetFlightById_WithValidId_ReturnCorrectFlight()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                dbContext.Flights.Add(new Flight("ABC123", DateTime.Now, "Location1", "Location2", "Type1"));
                dbContext.Flights.Add(new Flight("ABC123", DateTime.Now, "Location1", "Location2", "Type1"));
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);

                // Act
                var result = await controller.GetFlightById(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var flight = Assert.IsType<Flight>(okResult.Value);
                Assert.Equal("ABC123", flight.FlightNumber);
            }
        }

        [Fact]
        public async Task GetFlightById_WithInvalidId_ReturnBadRequest()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                dbContext.Flights.Add(new Flight("ABC123", DateTime.Now, "Location1", "Location2", "Type1"));
                dbContext.Flights.Add(new Flight("ABC123", DateTime.Now, "Location1", "Location2", "Type1"));
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);

                // Act
                var result = await controller.GetFlightById(-1);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
                Assert.Equal("Invalid index", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task GetFlightById_WithNotExistingId_ReturnNotFoundRequest()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                dbContext.Flights.Add(new Flight("ABC123", DateTime.Now, "Location1", "Location2", "Type1"));
                dbContext.Flights.Add(new Flight("ABC123", DateTime.Now, "Location1", "Location2", "Type1"));
                await dbContext.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new FlightController(context);

                // Act
                var result = await controller.GetFlightById(4);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
                Assert.Equal("Flight not found with given id", notFoundResult.Value);
            }
        }
        [Fact]
        public async Task AddFlight_WithValidFlight_ReturnAddedFlight()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);
                var param = new AddFlightParams
                {
                    FlightNumber = "XYZ789",
                    DepartureDate = DateTime.Now,
                    DepartureLocation = "Location1",
                    ArrivalLocation = "Location2",
                    PlaneType = "Type1"
                };

                // Act
                var result = await controller.AddFlight(param);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var flight = Assert.IsType<Flight>(okResult.Value);
                Assert.Equal(param.FlightNumber, flight.FlightNumber);
            }
        }

        [Fact]
        public async Task AddFlight_WithNullFlight_ReturnBadRequest()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);

                // Act
                var result = await controller.AddFlight(null);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
                Assert.Equal("Flight to add was not given", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task DeleteFlight_WithValidId_ReturnOkStatus()
        {   //Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);
                var newFlight = new Flight("XYZ789", DateTime.Now, "Location1", "Location2", "Type3");
                dbContext.Flights.Add(newFlight);
                await dbContext.SaveChangesAsync();

                // Act
                var result = await controller.DeleteFlight(newFlight.FlightId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal("Flight succesfully removed", okResult.Value);

                var deletedFlight = await dbContext.Flights.FindAsync(newFlight.FlightId);
                Assert.Null(deletedFlight);

            }
        }

        [Fact]
        public async Task DeleteFlight_WithInvalidId_ReturnBadRquest()
        {
            //Arrange
            var options = GetDbContextOptions();
            using(var  dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);

                //Act
                var result = await controller.DeleteFlight(-1);

                //Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid index", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task DeleteFlight_WithNotExistingd_ReturnNotFoundRequest()
        {
            //Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);
                var newFlight = new Flight("XYZ789", DateTime.Now, "Location1", "Location2", "Type3");
                await dbContext.SaveChangesAsync();

                //Act
                var result = await controller.DeleteFlight(999);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("Flight not found with given id", notFoundResult.Value);
            }
        }

        [Fact]
        public async Task ModifyFlight_WithValidFlight_ReturnModifiedFlight()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);
                var existingFlight = new Flight("XYZ789", DateTime.Now, "Location1", "Location2", "Type3");
                dbContext.Flights.Add(existingFlight);
                await dbContext.SaveChangesAsync();
                var modifiedFlight = await dbContext.Flights.FindAsync(existingFlight.FlightId);
                modifiedFlight.FlightNumber = "ABC123";
                modifiedFlight.DepartureDate = DateTime.Now.AddDays(1);
                modifiedFlight.DepartureLocation = "NewLcoation2";
                modifiedFlight.ArrivalLocation = "NewLocation1";
                modifiedFlight.PlaneType = "NewType";

                // Act
                var result = await controller.ModifyFlight(modifiedFlight);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var flight = Assert.IsType<Flight>(okResult.Value);
                Assert.Equal(modifiedFlight.FlightId, modifiedFlight.FlightId);
                Assert.Equal(modifiedFlight.FlightNumber, modifiedFlight.FlightNumber);
                Assert.Equal(modifiedFlight.ArrivalLocation, modifiedFlight.ArrivalLocation);
                Assert.Equal(modifiedFlight.DepartureLocation, modifiedFlight.DepartureLocation);
                Assert.Equal(modifiedFlight.DepartureDate, modifiedFlight.DepartureDate);
                Assert.Equal(modifiedFlight.PlaneType, modifiedFlight.PlaneType);
            }
        }

        [Fact]
        public async Task ModifyFlight_WithNullFlight_ReturnBadRequest()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);

                // Act
                var result = await controller.ModifyFlight(null);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
                Assert.Equal("Flight to modify was not given", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task ModifyFlight_WithFlightNotInDb_ReturnNotFoundRequest()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var dbContext = new AppDbContext(options))
            {
                var controller = new FlightController(dbContext);
                var flight = new Flight("ABC123", DateTime.Now, "Location1", "Location2", "Type1");
                dbContext.Flights.Add(flight);
                await dbContext.SaveChangesAsync();
                //this flight will have index different index im db than initial flight
                var modifiedFlight = new Flight("XYZ789", DateTime.Now.AddDays(1), "Location3", "Location4", "Type2");
                // Act
                var result = await controller.ModifyFlight(modifiedFlight);

                // Assert
                var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result.Result);
                Assert.Equal("Flight you want to modify does not exists in database", notFoundRequest.Value);
            }
        }

    }
}