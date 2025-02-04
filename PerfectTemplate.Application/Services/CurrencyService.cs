using Newtonsoft.Json.Linq;
using PerfectTemplate.Application.Interfaces;

namespace PerfectTemplate.Application.Services
{
    public class CurrencyService : ICurrency
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            // Замените на свой API ключ и URL
            var apiKey = "9716ddcc83574c6660aa45eb"; // Пример API-ключа от ExchangeRate-API
            var url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{fromCurrency}";

            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);

            if (json["result"].ToString() == "success")
            {
                var rate = json["conversion_rates"][toCurrency].ToObject<decimal>();
                return rate;
            }

            throw new Exception("Failed to get exchange rate.");
        }
    }
}