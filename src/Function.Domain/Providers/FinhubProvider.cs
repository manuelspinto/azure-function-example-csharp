using Function.Domain.Models;
using Function.Domain.Services;
using Function.Domain.Services.HttpClients;
using System.Threading.Tasks;

namespace Function.Domain.Providers
{
    public class FinhubProvider : IStockDataProvider
    {
        private readonly FinhubHttpClient _client;
        private readonly IFinhubDataMapper _stockDataMapper;
        
        public FinhubProvider(
            FinhubHttpClient client,
            IFinhubDataMapper stockDataMapper)
        {
            _client = client;
            _stockDataMapper = stockDataMapper;
        }
        public async Task<StockData> GetStockDataForSymbolAsync(string symbol)
        {
            var stockDataRaw = await _client.GetStockDataForSymbolAsync(symbol);

            return _stockDataMapper.MapToStockData(stockDataRaw);
        }
    }
}