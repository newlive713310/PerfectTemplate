namespace PerfectTemplate.Domain.Models.Weather
{
    public class GetWeatherByCityNameReply
    {
        public string Temp { get; set; }
        public string Summary { get; set; }
        public string City { get; set; }
        public static GetWeatherByCityNameReply GetEmptyModel()
        {
            return new GetWeatherByCityNameReply();
        }
    }
}
