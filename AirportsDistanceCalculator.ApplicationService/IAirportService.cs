using AirportsDistanceCalculator.Core;
using System.Threading.Tasks;

namespace AirportsDistanceCalculator.ApplicationService
{
    public interface IAirportService
    {
        Task<AirportsWithDistance> GetDistanceAsync(string departureIata, string arrivalIata);
    }
}
