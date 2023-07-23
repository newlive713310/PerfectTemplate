namespace PerfectTemplate.Domain.Models.Weather
{
    public class GetWeatherByCoordinatesRequest
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
