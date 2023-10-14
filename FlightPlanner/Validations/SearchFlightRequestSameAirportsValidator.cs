using FlightPlanner.Core.Interfaces;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Validations;

public class SearchFlightRequestSameAirportsValidator : IValidateSearch
{
    public bool IsValid(SearchFlightRequest searchFlightRequest)
    {
        return searchFlightRequest.From.Trim().ToLower() != searchFlightRequest.To.Trim().ToLower();
    }
}