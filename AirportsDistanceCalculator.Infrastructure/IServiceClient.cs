using AirportsDistanceCalculator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportsDistanceCalculator.Infrastructure
{
    public interface IServiceClient
    {
        public  Task<AirportInfo> GetAirportInfoByIataCode(string iata);
    }
}
