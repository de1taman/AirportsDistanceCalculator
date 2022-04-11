using AirportsDistanceCalculator.Core;
using AirportsDistanceCalculator.Infrastructure.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirportsDistanceCalculator.Infrastructure
{
    public class AirportsServiceClient: IServiceClient
    {
       
        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
        private readonly HttpClient _httpClient;
        public AirportsServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<AirportInfo> GetAirportInfoByIataCodeFromApi(string iata)
        {

            AirportJsonResponce airportJsonData = null;

           
                var url = $"airports/{iata}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    airportJsonData = JsonConvert.DeserializeObject<AirportJsonResponce>(responseBody, jsonSerializerSettings);
                   
                }
            
            if (airportJsonData!=null)
            {
                return new AirportInfo { Iata = airportJsonData.IATA, Latitude = airportJsonData.Location.Lat, Longitude = airportJsonData.Location.Lon , Country= airportJsonData.Country };
            }
            
                return null;
        }
    }
}
