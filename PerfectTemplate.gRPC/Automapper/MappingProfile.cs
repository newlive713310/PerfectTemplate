using AutoMapper;
using PerfectTemplate.gRPC.Protos;

namespace PerfectTemplate.gRPC.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Weather
            CreateMap<Domain.Models.Weather.GetWeatherByCityNameRequest, GetWeatherByCityNameRequest>().ReverseMap();
            CreateMap<Domain.Models.Weather.GetWeatherByCityNameReply, GetWeatherByCityNameReply>().ReverseMap();
            CreateMap<Domain.Models.Weather.GetWeatherByCoordinatesRequest, GetWeatherByCoordinatesRequest>().ReverseMap();
            CreateMap<Domain.Models.Weather.GetWeatherByCoordinatesReply, GetWeatherByCoordinatesReply>().ReverseMap();
        }
    }
}