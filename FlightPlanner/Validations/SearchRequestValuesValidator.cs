using FlightPlanner.Core.Interfaces;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Validations;

public class SearchRequestValuesValidator : IValidateSearch
{
    public bool IsValid(SearchFlightRequest searchFlightRequest)
    {
        return (!string.IsNullOrEmpty(searchFlightRequest.DepartureDate) &&
                 !string.IsNullOrEmpty(searchFlightRequest.From) &&
                 !string.IsNullOrEmpty(searchFlightRequest.To));
    }
}