using AutoMapper;
using Grpc.Core;
using PerfectTemplate.Application.Interfaces;
using PerfectTemplate.Domain.Helpers;
using PerfectTemplate.gRPC.Protos;

namespace PerfectTemplate.gRPC.Services
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

                var reply = _mapper.Map<GetWeatherByCityNameReply>(response);

                return await Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), new Metadata { { "X-Correlation-Id", CorrelationIdContext.GetCorrelationId() } });
            }
            finally { }
        }
        public override async Task<GetWeatherByCoordinatesReply> GetWeatherByCoordinates(GetWeatherByCoordinatesRequest request, ServerCallContext context)
        {
            try
            {
                var req = _mapper.Map<Domain.Models.Weather.GetWeatherByCoordinatesRequest>(request);

                var response = await _weather.GetWeatherByCoordinates(req);

                var reply = _mapper.Map<GetWeatherByCoordinatesReply>(response);

                return await Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), new Metadata { { "X-Correlation-Id", CorrelationIdContext.GetCorrelationId() } });
            }
            finally { }
        }
    }
}
