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
        [OpenApiParameter(name: "symbol", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Symbol to get stock data from")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "OK response")]
        public async Task<IActionResult> Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get", 
                Route = "stock-price/symbol/{symbol:alpha}/open"
            )] Microsoft.Azure.Functions.Worker.Http.HttpRequestData req,
            string symbol)
        {
            _logger.LogInformation($"Getting previous close stock price for symbol: {symbol}");

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
