using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfectTemplate.Application.Interfaces;

namespace PerfectTemplate.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ILogger<CurrenciesController> _logger;
        private readonly ICurrency _currency;
        public CurrenciesController(
            ILogger<CurrenciesController> logger,
            ICurrency currency
            )
        {
            _logger = logger;
            _currency = currency;
        }
        /// <summary>
        /// Get currency info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string fromCurrency, string toCurrency)
        {
            if (string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency))
            {
                return BadRequest("Please provide both fromCurrency and toCurrency.");
            }

            try
            {
                var response = await _currency.GetExchangeRate(fromCurrency, toCurrency);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in {nameof(this.Get)}. Message: {ex.Message}");

                return new StatusCodeResult(408);
            }
            finally { }
        }
    }
}