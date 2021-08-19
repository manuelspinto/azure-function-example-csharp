using Function.Domain.Models;

namespace Function.Domain.Services
{
    public class FinhubDataMapper : IFinhubDataMapper
    {
        public StockData MapToStockData(FinhubStockData finhubStockData){
            return new StockData(){
                Open = finhubStockData.o,
                High = finhubStockData.h,
                Low = finhubStockData.l,
                Current = finhubStockData.c,
                PreviousClose = finhubStockData.pc
            };
        }
    }
}