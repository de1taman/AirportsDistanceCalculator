using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportsDistanceCalculator.Web.Request
{
   
    public class V1GetDistanceRequest
    {
        public string DepartureIata { get; set; }
        public string ArrivalIata { get; set; }
    }
}
