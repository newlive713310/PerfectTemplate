using AutoMapper;
using Grpc.Core;
using PerfectTemplate.Application.Interfaces;
using PerfectTemplate.Host.Protos;

namespace PerfectTemplate.Host.Services
{
    public class PerfectTemplateService : PerfectTemplateInfo.PerfectTemplateInfoBase
    {
        private readonly IMapper _mapper;
        private readonly IWeather _weather;
        private readonly ILogger<PerfectTemplateService> _logger;
        public PerfectTemplateService(
            IMapper mapper,
            IWeather weather,
            ILogger<PerfectTemplateService> logger
            )
        {
            _mapper = mapper;
            _weather = weather;
            _logger = logger;
        }
        public override async Task<GetWeatherByCityNameReply> GetWeatherByCityName(GetWeatherByCityNameRequest request, ServerCallContext context)
        {
            try
            {
                var req = _mapper.Map<Domain.Models.Weather.GetWeatherByCityNameRequest>(request);

                var response = await _weather.GetWeatherByCityName(req);

                return _mapper.Map<GetWeatherByCityNameReply>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
            finally { }
        }
        public override async Task<GetWeatherByCoordinatesReply> GetWeatherByCoordinates(GetWeatherByCoordinatesRequest request, ServerCallContext context)
        {
            try
            {
                var req = _mapper.Map<Domain.Models.Weather.GetWeatherByCoordinatesRequest>(request);

                var response = await _weather.GetWeatherByCoordinates(req);

                return _mapper.Map<GetWeatherByCoordinatesReply>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
            finally { }
        }
    }
}