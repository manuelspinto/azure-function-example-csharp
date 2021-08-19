using Function.Domain.Models;

namespace Function.Domain.Services
{
    public interface IFinhubDataMapper
    {
         public StockData MapToStockData(FinhubStockData finhubStockData);
    }
}