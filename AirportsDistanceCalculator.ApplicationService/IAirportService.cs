using System.Threading.Tasks;

namespace AirportsDistanceCalculator.ApplicationService
{
    public interface IAirportService
    {
        Task<object> GetDistanceAsync(string departureIata, string arrivalIata);
    }
}
