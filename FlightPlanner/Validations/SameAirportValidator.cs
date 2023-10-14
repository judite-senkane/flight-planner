﻿using FlightPlanner.Core.Interfaces;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Validations;

public class SameAirportValidator : IValidate
{
    public bool IsValid(Flight flight)
    {
        return flight?.From.AirportCode.Trim().ToLower() != 
               flight?.To.AirportCode.Trim().ToLower();
    }
}