using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

using Function.Domain.Providers;

namespace Example.Function
{
    public class GetCloseStockPriceForSymbol
    {
        private readonly IStockDataProvider _stockDataProvider;
        private readonly ILogger<GetOpenStockPriceForSymbol> _logger;

        public GetCloseStockPriceForSymbol(
                    IStockDataProvider stockDataProvider,
                    ILogger<GetOpenStockPriceForSymbol> logger){
                _stockDataProvider = stockDataProvider;
                _logger = logger;
        }
        
        [Function("GetCloseStockPriceForSymbol")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "symbol"})]
        [OpenApiParameter(name: "symbol", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Symbol to get stock data from")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "OK response")]
        public async Task<IActionResult> Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get", 
                Route = "stock-price/symbol/{symbol:alpha}/close"
            )] Microsoft.Azure.Functions.Worker.Http.HttpRequestData req,
            string symbol)
        {
            _logger.LogInformation($"Getting previous close stock price for symbol: {symbol}");

            var closePrice = await GetCloseStockPriceForSymbolAsync(symbol);
            
            return new OkObjectResult(closePrice);
        }

        private async Task<decimal> GetCloseStockPriceForSymbolAsync(string symbol){
            var stockData = await _stockDataProvider.GetStockDataForSymbolAsync(symbol);
            var openPrice = stockData.PreviousClose;

            return openPrice;
        }
    }
}
