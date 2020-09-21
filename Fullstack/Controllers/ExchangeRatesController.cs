using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PassHomework.Services.CurrencyExchange;

namespace PassHomework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ICurrencyExchangeService _currencyExchangeService;

        public ExchangeRatesController(ICurrencyExchangeService currencyExchangeService)
        {
            _currencyExchangeService = currencyExchangeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            var exchangeRates = await _currencyExchangeService.GetLatestExchangeRates();

            return exchangeRates == null
                ? (IActionResult) NotFound()
                : Ok(exchangeRates);
        }

        [HttpGet("{currency}")]
        public async Task<IActionResult> GetSelected(string currency)
        {
            var basedExchangeRates = await _currencyExchangeService.GetExchangeRatesBySelected(currency);

            return basedExchangeRates == null
                ? (IActionResult)NotFound()
                : Ok(basedExchangeRates);
        }
    }
}
