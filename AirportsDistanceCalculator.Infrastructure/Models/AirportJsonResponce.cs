namespace AirportsDistanceCalculator.Infrastructure.Models
{
    internal class AirportJsonResponce
    {
        public string Country { get; set; }
        public string CityIata { get; set; }
        public string IATA { get; set; }
        public string TimezoneRegionName { get; set; }
        public string CountryIata { get; set; }
        public string Rating { get; set; }
        public string Name { get; set; }
        public AirportLocation Location { get; set; }
        public string Type { get; set; }
        internal class AirportLocation
        {
            public double Lon { get; set; }
            public double Lat { get; set; }
        }
    }
}
