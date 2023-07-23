using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PerfectTemplate.Application.Interfaces;
using PerfectTemplate.Domain.Models.Weather;
using System.Net;

namespace PerfectTemplate.Application.Services
{
    public class WeatherService : IWeather
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WeatherService> _logger;
        private readonly string _apiKey = String.Empty;
        private readonly string _weatherApi = String.Empty;
        private readonly string _mongoDb = String.Empty;

        public WeatherService(
            IHttpClientFactory httpClientFactory,
            ILogger<WeatherService> logger,
            IConfiguration configuration
            )
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _apiKey = configuration.GetSection("WeatherConfig").GetSection("Api-Key").Value;
            _weatherApi = configuration.GetSection("WeatherConfig").GetSection("WeatherApi").Value;
        }
        public async Task<GetWeatherByCityNameReply> GetWeatherByCityName(GetWeatherByCityNameRequest request)
        {
            // Creating empty returned model
            var reply = GetWeatherByCityNameReply.GetEmptyModel();

            //_logger.LogDebug($"Debug Info | CorrelationId { correlationId }: Querying weather data to external service with parameter {city}");

            using (var clientHttp = new HttpClient())
            {
                try
                {
                    var _httpClient = _httpClientFactory.CreateClient("Weather");

                    // Calling HTTPClient Post method
                    //_logger.LogDebug($"Trying to call HTTPClient Post method with url {_url}");
                    using var response = await _httpClient.GetAsync($"/data/2.5/weather?q={request.CityName}&appid={_apiKey}&units=metric");
                    response.EnsureSuccessStatusCode();
                    //_logger.LogDebug($"Successfully called HTTPClient Post method with url {_url}");

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Getting data from weather external service
                        _logger.LogDebug($"Successfully called HTTPClient. Returned status {response.StatusCode}");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    var rawWeather = JsonConvert.DeserializeObject<WeatherApiResponse>(result);

                    reply = new GetWeatherByCityNameReply()
                    {
                        Temp = rawWeather.Main.Temp,
                        Summary = string.Join(",", rawWeather.Weather.Select(x => x.Main)),
                        City = rawWeather.Name
                    };

                    //clientHttp.BaseAddress = new Uri(_weatherApi);
                    //var response = await clientHttp.GetAsync($"/data/2.5/weather?q={city}&appid={_apiKey}&units=metric");
                    //response.EnsureSuccessStatusCode();

                    //// Getting data from weather external service
                    //var stringResult = await response.Content.ReadAsStringAsync();

                    //_logger.LogTrace($"Trace Info | CorrelationId {correlationId}: Returned weather data from {nameof(GetWeatherByCityName)} method {JsonConvert.SerializeObject(stringResult)}");

                    //var rawWeather = JsonConvert.DeserializeObject<WeatherResponse>(stringResult);
                    //reply = new WeatherReply()
                    //{
                    //    Temp = rawWeather.Main.Temp,
                    //    Summary = string.Join(",", rawWeather.Weather.Select(x => x.Main)),
                    //    City = rawWeather.Name
                    //};

                    // Creating Mongo client
                    //var clientMongo = new MongoClient(_mongoDb);

                    //using (var cursor = await clientMongo.ListDatabasesAsync())
                    //{
                    //    _logger.LogDebug($"Debug Info | CorrelationId {correlationId}: Getting db test in MongoDb");
                    //    var db = clientMongo.GetDatabase("test");

                    //    // Checking collection WeatherResponses exists
                    //    var collectionExists = db.ListCollectionNames().ToList().Contains("WeatherResponses");

                    //    if (!collectionExists)
                    //    {
                    //        _logger.LogDebug($"Debug Info | CorrelationId {correlationId}: Creating collection WeatherResponses in MongoDb");
                    //        await db.CreateCollectionAsync("WeatherResponses");
                    //    }

                    //    var weatherResponses = db.GetCollection<BsonDocument>("WeatherResponses");

                    //    var doc = new BsonDocument
                    //        {
                    //            { "Datetime", DateTime.UtcNow},
                    //            { "CityName", city},
                    //            { "CorrelationId", correlationId},
                    //            { "Temp", reply.Temp },
                    //            { "Summary", reply.Summary },
                    //            { "City", reply.City }
                    //        };

                    //    await weatherResponses.InsertOneAsync(doc);
                    //}
                }
                catch (HttpRequestException httpRequestException) { throw; }
                finally { }

                return await Task.FromResult(reply) ?? GetWeatherByCityNameReply.GetEmptyModel();
            }
        }

        public Task<GetWeatherByCoordinatesReply> GetWeatherByCoordinates(GetWeatherByCoordinatesRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
