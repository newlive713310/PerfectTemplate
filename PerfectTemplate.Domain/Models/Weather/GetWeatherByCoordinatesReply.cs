namespace PerfectTemplate.Domain.Models.Weather
{
    public class GetWeatherByCoordinatesReply
    {
        public string Temp { get; set; }
        public string Summary { get; set; }
        public string City { get; set; }
        public static GetWeatherByCoordinatesReply GetEmptyModel()
        {
            return new GetWeatherByCoordinatesReply();
        }
    }
}
