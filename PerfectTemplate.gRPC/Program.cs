using Microsoft.Extensions.Diagnostics.HealthChecks;
using PerfectTemplate.Application.Interfaces;
using PerfectTemplate.Application.Services;
using PerfectTemplate.gRPC.Automapper;
using PerfectTemplate.gRPC.Interceptors;
using PerfectTemplate.gRPC.Services;
using Polly;
using Polly.Extensions.Http;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

// Vault configuration settings
//try
//{
//    VaultSharp.V1.AuthMethods.IAuthMethodInfo authMethod = new VaultSharp.V1.AuthMethods.Token.TokenAuthMethodInfo(config["Vault:Secret"]);

//    var vaultClientSettings = new VaultClientSettings(config["Vault:Address"], authMethod);

//    var vaultClient = new VaultClient(vaultClientSettings);

//    var secrets = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
//        path: config["Vault:SecretPath"],
//        mountPoint: config["Vault:MountPoint"]);

//    builder.Configuration.AddInMemoryCollection(secrets.Data.Data.ToDictionary(x => "ConnectionStrings:" + x.Key, x => x.Value.ToString()));
//}
//catch (Exception e) { Console.WriteLine("HashiCorp Vault error: " + e.Message); }

builder.Services.AddHttpClient<IWeather, WeatherService>("Weather", client =>
{
    client.BaseAddress = new Uri("http://api.openweathermap.org");
})
  .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
  {
      ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } // It disables SSL verification
  })
  .SetHandlerLifetime(TimeSpan.FromMinutes(5))
  .AddPolicyHandler(GetRetryPolicy());

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                    retryAttempt)));
}

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<RequestLoggerInterceptor>();
});
builder.Services.AddGrpcReflection();
builder.Services.AddGrpcHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(BaseMappingProfile));

builder.Services.AddTransient<RequestLoggerInterceptor>();

builder.Services.AddScoped<IWeather, WeatherService>();

builder.Host.UseSerilog((ctx, config) =>
{
    config
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<PerfectTemplateService>();

IWebHostEnvironment env = app.Environment;

if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}
app.MapGrpcHealthChecksService();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();