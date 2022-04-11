using AirportsDistanceCalculator.ApplicationService;
using AirportsDistanceCalculator.Core;
using AirportsDistanceCalculator.Web.Controllers;
using AirportsDistanceCalculator.Web.Request;
using AirportsDistanceCalculator.Web.Response;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace AirportsDistanceCalculator.Tests
{
    public class V1AirportDistanceControllerTests
    {
        [Fact]
        public async Task ReturnsOkWhenAirportServiceReturnsResult()
        {
            //Arrange
            string correctIata = "Iat";
            AirportsWithDistance resultFromAirportService = new AirportsWithDistance { Arrival = correctIata, Departure = correctIata, Disance = 0.00 };
            
            var request= new V1GetDistanceRequest { ArrivalIata = correctIata, DepartureIata = correctIata };
            var airportService = A.Fake<IAirportService>();
            A.CallTo(() => airportService.GetDistanceAsync(request.DepartureIata, request.ArrivalIata)).Returns(Task.FromResult(resultFromAirportService));
            
            var controller = new V1AirportDistanceController(airportService);

            //Act

            var actionResult = await controller.GetDistance(request);

            //Assert

            Assert.IsType<OkObjectResult>(actionResult.Result);

            var result = actionResult.Result as OkObjectResult;
            var responce = result.Value as V1GetDistanceResponse;

            var dist = responce.Disance;
            Assert.Equal(responce.Disance, resultFromAirportService.Disance);
        }


        [Fact]
        public async Task ReturnsBadRequestWhenAirportServiceReturnsNull()
        {
            //Arrange
            AirportsWithDistance nullResultFromAirportService = null;
            string correctIata = "DEP";
            var requestWithNullResult = new V1GetDistanceRequest { ArrivalIata = correctIata, DepartureIata = correctIata };
            var airportService = A.Fake<IAirportService>();
            A.CallTo(() => airportService.GetDistanceAsync(requestWithNullResult.DepartureIata, requestWithNullResult.ArrivalIata)).Returns(Task.FromResult(nullResultFromAirportService));
            var controller = new V1AirportDistanceController(airportService);

            //Act
            var actionResult = await controller.GetDistance(requestWithNullResult);

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            
        }
    }
}
