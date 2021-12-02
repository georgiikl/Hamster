using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Hamster.Adapters.Interfaces.AlphaVantage;

namespace Hamster.Adapters.Implementation.AlphaVantage
{
    public class AlphaVantageAdapter : IAlphaVantageAdapter
    {
        private static readonly HttpClient HttpClient;

        static AlphaVantageAdapter()
        {
            HttpClient = new HttpClient();
        }
        
        public async Task<IncomeStatementDto> GetIncomeStatement(string ticker, CancellationToken cancellationToken)
        {
            // Welcome to Alpha Vantage! Your dedicated access key is: VXSJQII4WWM38YE7
            var requestUri = $"https://www.alphavantage.co/query?function=INCOME_STATEMENT&symbol={ticker}&apikey=VXSJQII4WWM38YE7";
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new NullableLongConverter() }
            };
            var result = await HttpClient.GetFromJsonAsync<IncomeStatementDto>(requestUri, options, cancellationToken);
            return result;
        }
    }
}