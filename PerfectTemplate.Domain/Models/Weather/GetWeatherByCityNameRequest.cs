﻿using System.ComponentModel.DataAnnotations;

namespace PerfectTemplate.Domain.Models.Weather
{
    public class GetWeatherByCityNameRequest
    {
        [Required]
        [MinLength(12), MaxLength(50)]
        public string CityName { get; set; }
    }
}
