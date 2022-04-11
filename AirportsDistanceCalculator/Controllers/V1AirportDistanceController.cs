using AirportsDistanceCalculator.ApplicationService;
using AirportsDistanceCalculator.Web.Request;
using AirportsDistanceCalculator.Web.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AirportsDistanceCalculator.Web.Controllers
{
    [ApiController]
    [Route("api/v1") ]
    public class V1AirportDistanceController : Controller
    {
        private readonly IAirportService _airportService;

        public V1AirportDistanceController(IAirportService airportService)
        {
            this._airportService = airportService;
        }
        [HttpPost("get-distance")]
        public async Task<ActionResult<V1GetDistanceResponse>> GetDistance([FromBody] V1GetDistanceRequest v1GetDistanceRequest)
        {
            var result = await _airportService.GetDistanceAsync(v1GetDistanceRequest.DepartureIata, v1GetDistanceRequest.ArrivalIata);
            if (result != null)
            {
                return Ok( new V1GetDistanceResponse { Disance = result.Disance });
            }
            
            return BadRequest("Not found");
        }
    }
}
