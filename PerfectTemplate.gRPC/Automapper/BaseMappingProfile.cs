using AutoMapper;

namespace PerfectTemplate.gRPC.Automapper
{
    public class BaseMappingProfile : Profile
    {
        public BaseMappingProfile()
        {
            CreateMap<DateTime, Google.Protobuf.WellKnownTypes.Timestamp>().ConvertUsing(x => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(x, DateTimeKind.Utc)));
            CreateMap<Google.Protobuf.WellKnownTypes.Timestamp, DateTime>().ConvertUsing(x => x.ToDateTime());
            CreateMap<string, string>().ConvertUsing(x => x ?? String.Empty);
        }
    }
}