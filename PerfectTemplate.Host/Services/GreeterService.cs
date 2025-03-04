using Grpc.Core;
using PerfectTemplate.Host;

namespace PerfectTemplate.Host.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<StartCardReleaseResponse> StartCardReleaseProcess(StartCardReleaseRequest request, ServerCallContext context)
        {
            return Task.FromResult(new StartCardReleaseResponse
            {
                Message = "Hello " + request.CardReleaseId
            });
        }
    }
}
