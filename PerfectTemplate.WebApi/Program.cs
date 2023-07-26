using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PerfectTemplate.Application.Interfaces;
using PerfectTemplate.Application.Services;
using PerfectTemplate.WebApi.Middleware;
using Polly;
using Polly.Extensions.Http;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Perfect Template API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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

builder.Services.AddScoped<IWeather, WeatherService>();
builder.Services.AddScoped(typeof(IDynamicComparer<>), typeof(ComparerService<>));

builder.Services.AddHealthChecks();

builder.Services.AddApiVersioning(opt =>
{
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Host.UseSerilog((ctx, config) =>
{
    config
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext();
});

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();