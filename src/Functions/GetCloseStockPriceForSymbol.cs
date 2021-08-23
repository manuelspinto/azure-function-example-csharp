using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Function.Domain.Providers;
using Function.Domain.Helpers;

namespace Example.Function
{
    public class GetCloseStockPriceForSymbol
    {
        private readonly IStockDataProvider _stockDataProvider;
        private readonly IHttpHelper _httpHelper;

        public GetCloseStockPriceForSymbol(
                IStockDataProvider stockDataProvider,
                IHttpHelper httpHelper){
                    _stockDataProvider = stockDataProvider;
                    _httpHelper = httpHelper;
        }
        
        [Function("GetCloseStockPriceForSymbol")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get", 
                Route = "stock-price/close/{symbol}"
            )] HttpRequestData req,
            string symbol,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetCloseStockPriceForSymbol");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var closePrice = GetCloseStockPriceForSymbolAsync(symbol).Result;

            HttpResponseData response = await _httpHelper.CreateHttpResponse(req, closePrice);
            return response;
        }

        private async Task<decimal> GetCloseStockPriceForSymbolAsync(string symbol){
            var stockData = await _stockDataProvider.GetStockDataForSymbolAsync(symbol);
            var closePrice = stockData.PreviousClose;

            return closePrice;
        }
    }
}
