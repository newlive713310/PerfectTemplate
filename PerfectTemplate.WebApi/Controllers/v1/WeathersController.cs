using Microsoft.AspNetCore.Cors;
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
        private readonly ILogger<WeathersController> _logger;
        private readonly IWeather _weather;
        public WeathersController(
            ILogger<WeathersController> logger,
            IWeather weather
            )
        {
            _logger = logger;
            _weather = weather;
        }
        /// <summary>
        /// Get weather info by city name
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> GetWeatherByCityName([FromBody] GetWeatherByCityNameRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _weather.GetWeatherByCityName(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in {nameof(this.GetWeatherByCityName)}. Message: {ex.Message}");

                return new StatusCodeResult(408);
            }
            finally { }
        }
        /// <summary>
        /// Get weather info by coordinates
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> GetWeatherByCoordinates(GetWeatherByCoordinatesRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _weather.GetWeatherByCoordinates(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in {nameof(this.GetWeatherByCoordinates)}. Message: {ex.Message}");

                return new StatusCodeResult(408);
            }
            finally { }
        }
    }
}
