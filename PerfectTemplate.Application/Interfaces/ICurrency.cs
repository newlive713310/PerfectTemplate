namespace PerfectTemplate.Application.Interfaces
{
    public interface ICurrency
    {
        public Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency);
    }
}
