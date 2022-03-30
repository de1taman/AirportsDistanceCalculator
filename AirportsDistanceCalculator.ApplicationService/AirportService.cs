using AirportsDistanceCalculator.Core;
using AirportsDistanceCalculator.Infrastructure;
using System.Device.Location;
using System.Threading.Tasks;

namespace AirportsDistanceCalculator.ApplicationService
{
    public class AirportService : IAirportService
    {
        private readonly AirportsServiceClient airportsServiceClient;

        public AirportService(AirportsServiceClient airportsServiceClient)
        {
            this.airportsServiceClient = airportsServiceClient;
        }
        public async Task<object> GetDistanceAsync(string departureIata, string arrivalIata)
        {
            AirportInfo departureAirport = await airportsServiceClient.GetAirportInfoByIataCode(departureIata.ToUpper());
            AirportInfo arrivalAirport = await airportsServiceClient.GetAirportInfoByIataCode(arrivalIata.ToUpper());
            if (departureAirport == null || arrivalAirport == null)
            {
                return null;
            }
            var depatrureCoodr = new GeoCoordinate(departureAirport.Latitude, departureAirport.Longitude);
            var arrivalCoodr = new GeoCoordinate(arrivalAirport.Latitude, arrivalAirport.Longitude);

            if (depatrureCoodr!=null && arrivalCoodr != null)
            {
                var distance = depatrureCoodr.GetDistanceTo(arrivalCoodr);
                return new { departure = $"{departureAirport.Country}:{departureAirport.IATA}", arrival = $"{arrivalAirport.Country}:{arrivalAirport.IATA}", distance = distance };
            }
            return null;
        }
    }
}
