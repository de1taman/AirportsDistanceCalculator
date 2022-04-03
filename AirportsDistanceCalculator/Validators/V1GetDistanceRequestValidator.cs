using AirportsDistanceCalculator.Web.Request;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AirportsDistanceCalculator.Web.Validators
{
    public class V1GetDistanceRequestValidator: AbstractValidator<V1GetDistanceRequest>
    {
        public V1GetDistanceRequestValidator()
        {
            Regex regex = new Regex(@"^[A-Z]{3}$", RegexOptions.IgnoreCase);
            RuleFor(x => x.ArrivalIata).Must(iata => regex.IsMatch(iata));
            RuleFor(x => x.DepartureIata).Must(iata => regex.IsMatch(iata));
        }
    }
}
