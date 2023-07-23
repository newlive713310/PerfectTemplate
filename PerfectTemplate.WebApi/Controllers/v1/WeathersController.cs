using Microsoft.AspNetCore.Mvc;
using PerfectTemplate.Application.Interfaces;
using PerfectTemplate.Domain.Models.Weather;

namespace PerfectTemplate.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class WeathersController : ControllerBase
    {
        private readonly IWeather _weather;
        public WeathersController(
            IWeather weather
            )
        {
            _weather = weather;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> GetWeatherByCityName([FromBody] GetWeatherByCityNameRequest request)
        {
            try
            {
                var response = await _weather.GetWeatherByCityName(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
    }
}
