using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PassHomework.Models;

namespace PassHomework.Services.CurrencyExchange
{
    /// <summary>
    /// Uses https://exchangeratesapi.io/
    /// </summary>
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CurrencyExchangeService> _logger;

        private const string LatestExchangeRatesEndpoint = "https://api.exchangeratesapi.io/latest";
        private const string BasedExchangeRatesEndpoint = "https://api.exchangeratesapi.io/latest?base={0}";

        private static readonly JsonSerializerOptions CamelCaseOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public CurrencyExchangeService(IHttpClientFactory httpClientFactory, ILogger<CurrencyExchangeService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ExchangeRates> GetLatestExchangeRates()
        {
            return await GetExchangeRates(LatestExchangeRatesEndpoint);
        }

        public async Task<ExchangeRates> GetExchangeRatesBySelected(string currency)
        {
            var basedExchangeRates = string.Format(BasedExchangeRatesEndpoint, currency);

            return await GetExchangeRates(basedExchangeRates);
        }

        private async Task<ExchangeRates> GetExchangeRates(string url)
        {

            using var client = _httpClientFactory.CreateClient();

            using (var response = await client.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error calling '{url}'. StatusCode: {response.StatusCode}");

                    return null;
                }

                var responseStream = await response.Content.ReadAsStreamAsync();


                try
                {
                    return await JsonSerializer.DeserializeAsync<ExchangeRates>(responseStream, CamelCaseOptions);
                }
                catch (JsonException exception)
                {
                    _logger.LogError(exception, $"Error deserializing JSON response of '{url}'");

                    return null;
                }
                finally
                {
                    await responseStream.DisposeAsync();
                }
            }
        }
    }
}
