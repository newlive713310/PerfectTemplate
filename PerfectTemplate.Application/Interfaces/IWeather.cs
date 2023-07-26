using PerfectTemplate.Domain.Models.Weather;

namespace PerfectTemplate.Application.Interfaces
{
    public interface IWeather
    {
        Task<GetWeatherByCityNameReply> GetWeatherByCityName(GetWeatherByCityNameRequest request);
        Task<GetWeatherByCoordinatesReply> GetWeatherByCoordinates(GetWeatherByCoordinatesRequest request);
    }
}