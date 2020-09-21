using System.Threading.Tasks;
using PassHomework.Models;

namespace PassHomework.Services.CurrencyExchange
{
    public interface ICurrencyExchangeService
    {
        Task<ExchangeRates> GetLatestExchangeRates();

        Task<ExchangeRates> GetExchangeRatesBySelected(string currency);
    }
}
