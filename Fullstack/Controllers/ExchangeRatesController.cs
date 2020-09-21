using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PassHomework.Models;
using PassHomework.Services.CurrencyExchange;

namespace PassHomework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ICurrencyExchangeService _currencyExchangeService;
        private readonly IMemoryCache _memoryCache;

        public ExchangeRatesController(ICurrencyExchangeService currencyExchangeService, IMemoryCache memoryCache)
        {
            _currencyExchangeService = currencyExchangeService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            var exchangeRates = await _currencyExchangeService.GetLatestExchangeRates();

            return exchangeRates == null
                ? (IActionResult)NotFound()
                : Ok(exchangeRates);
        }


        [HttpGet("{currency}")]
        public async Task<IActionResult> GetSelected(string currency)
        {
            ExchangeRates basedExchangeRates;

            if (_memoryCache.TryGetValue(currency, out basedExchangeRates) == false)
            {
                basedExchangeRates = await _currencyExchangeService.GetExchangeRatesBySelected(currency);
                _memoryCache.Set(currency, basedExchangeRates, new MemoryCacheEntryOptions() { AbsoluteExpiration = DateTime.UtcNow.AddHours(1) });
            }

            return basedExchangeRates == null
                ? (IActionResult)NotFound()
                : Ok(basedExchangeRates);
        }
    }
}
