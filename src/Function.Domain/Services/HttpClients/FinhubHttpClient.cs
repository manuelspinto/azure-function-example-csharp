using System.Net.Http.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Function.Domain.Models;

namespace Function.Domain.Services.HttpClients
{
    public class FinhubHttpClient
    {
        public HttpClient _client { get; }

        public FinhubHttpClient(HttpClient client){
            client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("finhub_api_baseUrl"));
            client.DefaultRequestHeaders.Add("X-Finnhub-Token", Environment.GetEnvironmentVariable("finhub_api_token"));

            _client = client;
        }

        public async Task<FinhubStockData> GetStockDataForSymbolAsync(string symbol){
            var quotePath = $"quote?symbol={symbol}";

            var response = await _client.GetAsync(quotePath);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<FinhubStockData>();
        }
    }
}