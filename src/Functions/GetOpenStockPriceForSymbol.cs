using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Function.Domain.Providers;

namespace Example.Function
{
    public class GetOpenStockPriceForSymbol
    {
        private readonly IStockDataProvider _stockDataProvider;

        public GetOpenStockPriceForSymbol(
                IStockDataProvider stockDataProvider){
                    _stockDataProvider = stockDataProvider;
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

            var stockData = await _stockDataProvider.GetStockDataForSymbolAsync(symbol);
            var openPrice = stockData.Open;

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(openPrice);

            return response;
        }
    }
}
