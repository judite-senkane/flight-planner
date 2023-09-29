﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    public class Airport
    {
        [JsonIgnore]
        public int Id { get; set; }
        [StringLength(150)]
        public string Country { get; set; }

        [StringLength(150)]
        public string City { get; set; }

        [StringLength(150)]
        [JsonPropertyName("airport")]
        public string AirportCode { get; set; }
    }
}
