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
        #warning по код\нейминг стайлам приватные поля обычно именуются начиная с нижнего подчёркивания _ 
        private readonly IAirportService airportService;

        public V1AirportDistanceController(IAirportService airportService)
        {
            this.airportService = airportService;
        }
        [HttpGet("get-distance")]
        /*
         * смотря какая ситуация получается: тут, в контроллере, у тебя описано что возвращаешь ты ActionResult<V1GetDistanceResponse>
         * клиент, обратившись сюда, получит респонс, тело ответа которого попробует привести к  V1GetDistanceResponse
         * но airportService.GetDistanceAsync у тебя возвращает просто object, а т.к. Ok() принимает на вход ЛЮБОЙ объект, вот ты любой объект туда
         * и можешь передать; короче говоря, ты клиента обманываешь.
         * 
         */
        public async Task<ActionResult<V1GetDistanceResponse>> GetDistance([FromBody] V1GetDistanceRequest v1GetDistanceRequest)
        {
            var result = await airportService.GetDistanceAsync(v1GetDistanceRequest.DepartureIata, v1GetDistanceRequest.ArrivalIata);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Not found");
        }
    }
}
