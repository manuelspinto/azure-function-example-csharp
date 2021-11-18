using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;

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
        
        [Function("GetOpenStockPriceForSymbol")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "symbol"})]
        [OpenApiParameter(name: "symbol", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Symbol to get stock data from")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "OK response")]
        public async Task<IActionResult> Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(
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
