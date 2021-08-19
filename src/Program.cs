using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Function.Domain.Services;
using Function.Domain.Services.HttpClients;
using Function.Domain.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Function
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    s.AddScoped<IFinhubDataMapper, FinhubDataMapper>();
                    s.AddScoped<IStockDataProvider, FinhubProvider>();
                    s.AddHttpClient<FinhubHttpClient>();
                })
                .Build();

            host.Run();
        }
    }
}