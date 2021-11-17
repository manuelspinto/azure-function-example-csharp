using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

using Function.Domain.Providers;

namespace Example.Function
{
    public class GetOpenStockPriceForSymbol
    {
        private readonly IStockDataProvider _stockDataProvider;
        private readonly ILogger<GetOpenStockPriceForSymbol> _logger;

        public GetOpenStockPriceForSymbol(
                    IStockDataProvider stockDataProvider,
                    ILogger<GetOpenStockPriceForSymbol> logger){
                _stockDataProvider = stockDataProvider;
                _logger = logger;
        }
        
        [FunctionName("GetOpenStockPriceForSymbol")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name"})]
        [OpenApiParameter(name: "symbol", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Symbol to get stock data from")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get", 
                Route = "stock-price/open/{symbol}"
            )] HttpRequest req,
            string symbol)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var openPrice = await GetOpenStockPriceForSymbolAsync(symbol);

            return new OkObjectResult(openPrice);
        }

        private async Task<decimal> GetOpenStockPriceForSymbolAsync(string symbol){
            var stockData = await _stockDataProvider.GetStockDataForSymbolAsync(symbol);
            var openPrice = stockData.Open;

            return openPrice;
        }
    }
}
