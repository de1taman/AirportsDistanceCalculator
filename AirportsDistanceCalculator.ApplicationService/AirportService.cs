using AirportsDistanceCalculator.Core;
using AirportsDistanceCalculator.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Device.Location;
using System.Threading.Tasks;

namespace AirportsDistanceCalculator.ApplicationService
{
    public class AirportService : IAirportService
    {
        private readonly IServiceClient _airportsServiceClient;
        private IMemoryCache _cache;
        public AirportService(IServiceClient airportsServiceClient, IMemoryCache memoryCache)
        {
            this._airportsServiceClient = airportsServiceClient;
            _cache = memoryCache;
        }
        public async Task<AirportsWithDistance> GetDistanceAsync(string departureIata, string arrivalIata)
        {
            AirportInfo departureAirport = await GetAirportInfoByIataCode(departureIata.ToUpper());
            AirportInfo arrivalAirport = await GetAirportInfoByIataCode(arrivalIata.ToUpper());

            if (departureAirport == null || arrivalAirport == null)
            {
                return null;
            }
            var departureCoordinate = new GeoCoordinate(departureAirport.Latitude, departureAirport.Longitude);
            var arrivalCoordinate = new GeoCoordinate(arrivalAirport.Latitude, arrivalAirport.Longitude);
            var distance = departureCoordinate.GetDistanceTo(arrivalCoordinate);
            return new AirportsWithDistance { Arrival = $"{arrivalAirport.Country}:{arrivalAirport.Iata}", Departure = $"{departureAirport.Country}:{departureAirport.Iata}", Disance = distance };
        }
        
        private async Task< AirportInfo> GetAirportInfoByIataCode(string iata)
        {
            AirportInfo airportInfo;
            if (!_cache.TryGetValue(iata, out airportInfo))
            {
                airportInfo = await _airportsServiceClient.GetAirportInfoByIataCodeFromApi(iata);
                if (airportInfo != null)
                {
                    _cache.Set(iata, airportInfo, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return airportInfo;
        }
    }
}
