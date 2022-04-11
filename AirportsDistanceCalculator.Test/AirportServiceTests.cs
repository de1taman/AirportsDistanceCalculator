using AirportsDistanceCalculator.ApplicationService;
using AirportsDistanceCalculator.Core;
using AirportsDistanceCalculator.Infrastructure;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AirportsDistanceCalculator.Tests
{
    public class AirportServiceTests
    {
        [Fact]
        public async Task ReturnsCorrectResponceIfServiceClientReturnsCorrectAirportInfo()
        {
            //Arrange
            #region test data
            
            AirportInfo ledAirport = new AirportInfo { Country = "Russian Federation" , Iata = "LED", Latitude = 59.983333, Longitude = 30.6 };

            AirportInfo dmeAirport = new AirportInfo { Country = "Russian Federation" , Iata = "DME", Latitude = 55.414566, Longitude = 37.899494 };

            AirportInfo aaaAirport = new AirportInfo { Country = "French Polynesia", Iata = "AAA", Latitude = -17.355648, Longitude = -145.50913 };

            AirportInfo helAirport = new AirportInfo { Country = "Finland", Iata = "HEL", Latitude = 60.25, Longitude = 25.05 };


            double ledDme = 667793.36;

            double ledAaa = 15277938.30;

            double ledHel = 309077.32;

            double dmeAaa = 15786849.22;

            double dmeHel = 929814.66;

            double aaaHel = 15198816.64;

            double ledLed = 0;

            #endregion

            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            string led = "LED";
            string dme = "DME";
            string hel = "HEL";
            string aaa = "AAA";

            var serviceClient = A.Fake<IServiceClient>();
            A.CallTo(() => serviceClient.GetAirportInfoByIataCodeFromApi(led)).Returns(Task.FromResult(ledAirport));
            A.CallTo(() => serviceClient.GetAirportInfoByIataCodeFromApi(dme)).Returns(Task.FromResult(dmeAirport));
            A.CallTo(() => serviceClient.GetAirportInfoByIataCodeFromApi(aaa)).Returns(Task.FromResult(aaaAirport));
            A.CallTo(() => serviceClient.GetAirportInfoByIataCodeFromApi(hel)).Returns(Task.FromResult(helAirport));

            AirportService service = new AirportService(serviceClient, cache);

            //Act

            var ledDmeResult = await service.GetDistanceAsync(led, dme);
            var ledAaaResult = await service.GetDistanceAsync(led, aaa);
            var ledHelResult = await service.GetDistanceAsync(led, hel);
            var dmeAaaResult = await service.GetDistanceAsync(dme, aaa);
            var dmeHelResult = await service.GetDistanceAsync(dme, hel);
            var aaaHelResult = await service.GetDistanceAsync(aaa, hel);
            var ledLedResult = await service.GetDistanceAsync(led, led);

            //Assert

            Assert.InRange<double>(Math.Abs(ledDmeResult.Disance - ledDme), 0, 1000);
            Assert.Equal(ledDmeResult.Departure, string.Concat(ledAirport.Country, ":",ledAirport.Iata));
            Assert.Equal(ledDmeResult.Arrival, string.Concat(dmeAirport.Country, ":", dmeAirport.Iata));

            Assert.InRange<double>(Math.Abs(ledAaaResult.Disance - ledAaa), 0, 1000);
            Assert.Equal(ledAaaResult.Departure, string.Concat(ledAirport.Country, ":", ledAirport.Iata));
            Assert.Equal(ledAaaResult.Arrival, string.Concat(aaaAirport.Country, ":", aaaAirport.Iata));

            Assert.InRange<double>(Math.Abs(ledHelResult.Disance - ledHel), 0, 1000);
            Assert.Equal(ledHelResult.Departure, string.Concat(ledAirport.Country, ":", ledAirport.Iata));
            Assert.Equal(ledHelResult.Arrival, string.Concat(helAirport.Country, ":", helAirport.Iata));

            Assert.InRange<double>(Math.Abs(dmeAaaResult.Disance - dmeAaa), 0, 1000);
            Assert.Equal(dmeAaaResult.Departure, string.Concat(dmeAirport.Country, ":", dmeAirport.Iata));
            Assert.Equal(dmeAaaResult.Arrival, string.Concat(aaaAirport.Country, ":", aaaAirport.Iata));

            Assert.InRange<double>(Math.Abs(dmeHelResult.Disance - dmeHel), 0, 1000);
            Assert.Equal(dmeHelResult.Departure, string.Concat(dmeAirport.Country, ":", dmeAirport.Iata));
            Assert.Equal(dmeHelResult.Arrival, string.Concat(helAirport.Country, ":", helAirport.Iata));

            Assert.InRange<double>(Math.Abs(aaaHelResult.Disance - aaaHel), 0, 1000);
            Assert.Equal(aaaHelResult.Departure, string.Concat(aaaAirport.Country, ":", aaaAirport.Iata));
            Assert.Equal(aaaHelResult.Arrival, string.Concat(helAirport.Country, ":", helAirport.Iata));

            Assert.Equal(ledLedResult.Disance, ledLed);
            Assert.Equal(ledLedResult.Departure, string.Concat(ledAirport.Country, ":", ledAirport.Iata));
            Assert.Equal(ledLedResult.Arrival, string.Concat(ledAirport.Country, ":", ledAirport.Iata));
        }

        [Fact]
        public async Task ReturnsNullIfServiceClientReturnsNull()
        {
            //Arrange
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            string incorrectIata = "SSS";
            var serviceClient = A.Fake<IServiceClient>();
            AirportInfo nullAirportInfo = null;
            A.CallTo(()=> serviceClient.GetAirportInfoByIataCodeFromApi(incorrectIata)).Returns(Task.FromResult(nullAirportInfo));


            AirportService service = new AirportService(serviceClient, cache);
            //Act

            var result = await service.GetDistanceAsync(incorrectIata, incorrectIata);

            //Assert

            Assert.Null(result);

        }
    }
}
