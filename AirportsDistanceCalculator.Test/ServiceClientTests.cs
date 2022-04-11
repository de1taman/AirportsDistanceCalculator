using AirportsDistanceCalculator.Infrastructure;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AirportsDistanceCalculator.Tests
{
    public class ServiceClientTests
    {
        [Fact]
        public async Task ReturnsAirportInfoObjectForValidIataCode()
        {
            //Arrange
            #region test data
            string led = "LED";
            string dme = "DME";

            string dmeWebResponce = "{\"country\":\"Russian Federation\",\"city_iata\":\"MOW\",\"iata\":\"DME\",\"city\":\"Moscow\",\"timezone_region_name\":\"Europe / Moscow\",\"country_iata\":\"RU\",\"rating\":3,\"name\":\"Domodedovo\",\"location\":{\"lon\":37.899494,\"lat\":55.414566},\"type\":\"airport\",\"hubs\":9}";
            string ledWebResponce = "{\"country\":\"Russian Federation\",\"iata\":\"LED\",\"timezone_region_name\":\"Europe / Moscow\",\"country_iata\":\"RU\",\"rating\":3,\"name\":\"Saint Petersburg\",\"location\":{\"lon\":30.6,\"lat\":59.983333},\"type\":\"city\"}";
           

            HttpResponseMessage ledResponce = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(ledWebResponce) };
            HttpResponseMessage dmeResponce = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(dmeWebResponce) };
           
            HttpRequestMessage ledRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://places-dev.cteleport.com/airports/{led}");
            HttpRequestMessage dmeRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://places-dev.cteleport.com/airports/{dme}");
           
            #endregion

            #region HttpMessageHandler mock
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(p => p.RequestUri == ledRequestMessage.RequestUri),
            ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(ledWebResponce)
                    })
                .Verifiable();


            handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(p => p.RequestUri == dmeRequestMessage.RequestUri),
            ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(dmeWebResponce)
                    })
                .Verifiable();

            #endregion

            var fakeHttp = new HttpClient(handlerMock.Object);
            fakeHttp.BaseAddress = new Uri("https://places-dev.cteleport.com/");

            var client = new AirportsServiceClient(fakeHttp);

            //Act
            
            var responceLed = await client.GetAirportInfoByIataCodeFromApi(led);
            var responceDme = await client.GetAirportInfoByIataCodeFromApi(dme);
            
            //Assert

            Assert.Equal("Russian Federation", responceLed.Country);
            Assert.Equal("LED", responceLed.Iata);
            Assert.Equal(59.983333, responceLed.Latitude);
            Assert.Equal(30.6, responceLed.Longitude);

            Assert.Equal("Russian Federation", responceDme.Country);
            Assert.Equal("DME", responceDme.Iata);
            Assert.Equal(55.414566, responceDme.Latitude);
            Assert.Equal(37.899494, responceDme.Longitude);

        }

        [Fact]
        public async Task ReturnsNullForNotExistingIata()
        {
            //Arrange
            #region test data
            string eey = "EEY";
         
            HttpResponseMessage eeyResponce = new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound };

            HttpRequestMessage eeyRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://places-dev.cteleport.com/airports/{eey}");

            #endregion

            #region HttpMessageHandler mock
            var handlerMock = new Mock<HttpMessageHandler>();
           
            handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(p => p.RequestUri == eeyRequestMessage.RequestUri),
            ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                })
                .Verifiable();
            #endregion

            var fakeHttp = new HttpClient(handlerMock.Object);
            fakeHttp.BaseAddress = new Uri("https://places-dev.cteleport.com/");

            var client = new AirportsServiceClient(fakeHttp);

            //Act

            var responceEey = await client.GetAirportInfoByIataCodeFromApi(eey);

            //Assert

            Assert.Null(responceEey);
        }


        [Fact]
        public async Task ReturnsNullForInvalidIata()
        {
            //Arrange
            #region test data
            string tooLong = "EEYY";

            HttpResponseMessage tooLongResponce = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };

            HttpRequestMessage tooLongRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://places-dev.cteleport.com/airports/{tooLong}");

            #endregion

            #region HttpMessageHandler mock
            var handlerMock = new Mock<HttpMessageHandler>();
           

            handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(p => p.RequestUri == tooLongRequestMessage.RequestUri),
            ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                })
                .Verifiable();
            #endregion


            var fakeHttp = new HttpClient(handlerMock.Object);
            fakeHttp.BaseAddress = new Uri("https://places-dev.cteleport.com/");

            var client = new AirportsServiceClient(fakeHttp);

            //Act

            var responceTooLong = await client.GetAirportInfoByIataCodeFromApi(tooLong);

            //Assert

            Assert.Null(responceTooLong);
        }

    }
}
