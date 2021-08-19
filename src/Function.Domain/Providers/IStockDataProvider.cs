using Function.Domain.Models;
using System.Threading.Tasks;

namespace Function.Domain.Providers
{
    public interface IStockDataProvider
    {
         public Task<StockData> GetStockDataForSymbolAsync(string symbol);
    }
}