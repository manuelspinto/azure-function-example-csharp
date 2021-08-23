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
    public class GetOpenStockPriceForSymbol
    {
        private readonly IStockDataProvider _stockDataProvider;
        private readonly IHttpHelper _httpHelper;

        public GetOpenStockPriceForSymbol(
                IStockDataProvider stockDataProvider,
                IHttpHelper httpHelper){
                    _stockDataProvider = stockDataProvider;
                    _httpHelper = httpHelper;
        }
        
        [Function("GetOpenStockPriceForSymbol")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get", 
                Route = "stock-price/open/{symbol}"
            )] HttpRequestData req,
            string symbol,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetOpenStockPriceForSymbol");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var openPrice = GetOpenStockPriceForSymbolAsync(symbol).Result;

            HttpResponseData response = await _httpHelper.CreateHttpResponse(req, openPrice);
            return response;
        }

        private async Task<decimal> GetOpenStockPriceForSymbolAsync(string symbol){
            var stockData = await _stockDataProvider.GetStockDataForSymbolAsync(symbol);
            var openPrice = stockData.Open;

            return openPrice;
        }
    }
}
