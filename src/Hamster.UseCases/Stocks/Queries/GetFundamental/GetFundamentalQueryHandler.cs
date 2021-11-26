using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hamster.UseCases.Stocks.Queries.GetFundamental
{
    public class IncomeStatement
    {
        public string Symbol { get; set; }
        public IncomeStatementItem[] AnnualReports { get; set; }
        public IncomeStatementItem[] QuarterlyReports { get; set; }
    }
    
    public class IncomeStatementItem
    {
        public DateTime FiscalDateEnding { get; set; }
        public string ReportedCurrency { get; set; }
        public long? GrossProfit { get; set; }
        
        /// <summary>
        /// Выручка
        /// </summary>
        public long? TotalRevenue { get; set; }
        
        public long? CostOfRevenue { get; set; }
        public long? CostofGoodsAndServicesSold { get; set; }
        
        /// <summary>
        /// Операционная прибыль
        /// </summary>
        public long? OperatingIncome { get; set; }
        
        public long? SellingGeneralAndAdministrative { get; set; }
        public long? ResearchAndDevelopment { get; set; }
        public long? OperatingExpenses { get; set; }
        public long? InvestmentIncomeNet { get; set; }
        public long? NetInterestIncome { get; set; }
        public long? InterestIncome { get; set; }
        public long? InterestExpense { get; set; }
        public long? NonInterestIncome { get; set; }
        public long? OtherNonOperatingIncome { get; set; }
        public long? Depreciation { get; set; }
        public long? DepreciationAndAmortization { get; set; }
        public long? IncomeBeforeTax { get; set; }
        public long? IncomeTaxExpense { get; set; }
        public long? InterestAndDebtExpense { get; set; }
        public long? NetIncomeFromContinuingOperations { get; set; }
        public long? ComprehensiveIncomeNetOfTax { get; set; }
        public long? Ebit { get; set; }
        public long? Ebitda { get; set; }
        
        /// <summary>
        /// Чистая прибыль
        /// </summary>
        public long? NetIncome { get; set; }      
    }
    
    public interface IAlphaVantageAdapter
    {
        Task<IncomeStatement> GetIncomeStatement(string ticker, CancellationToken cancellationToken);
    }

    public class AlphaVantageAdapter : IAlphaVantageAdapter
    {
        private static readonly HttpClient HttpClient;

        static AlphaVantageAdapter()
        {
            HttpClient = new HttpClient();
        }
        
        public async Task<IncomeStatement> GetIncomeStatement(string ticker, CancellationToken cancellationToken)
        {
            // Welcome to Alpha Vantage! Your dedicated access key is: VXSJQII4WWM38YE7
            var requestUri = $"https://www.alphavantage.co/query?function=INCOME_STATEMENT&symbol={ticker}&apikey=VXSJQII4WWM38YE7";
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new NullableLongConverter() }
            };
            var result = await HttpClient.GetFromJsonAsync<IncomeStatement>(requestUri, options, cancellationToken);
            return result;
        }
    }

    public class NullableLongConverter : JsonConverter<long?>
    {
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            if (s is null or "None") return null;
            return long.TryParse(s, out var x) ? x : null;
        }

        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            var s = value?.ToString();
            writer.WriteStringValue(s);
        }
    }
    
    internal class GetFundamentalQueryHandler : IRequestHandler<GetFundamentalQuery, FundamentalDto>
    {
        private readonly IAlphaVantageAdapter _alphaVantageAdapter;

        public GetFundamentalQueryHandler(IAlphaVantageAdapter alphaVantageAdapter)
        {
            _alphaVantageAdapter = alphaVantageAdapter;
        }
        public async Task<FundamentalDto> Handle(GetFundamentalQuery request, CancellationToken cancellationToken)
        {
            var incomeStatement = await _alphaVantageAdapter.GetIncomeStatement(request.Ticker, cancellationToken);
            var pair = incomeStatement
                .AnnualReports
                .OrderByDescending(x => x.FiscalDateEnding)
                .Take(2)
                .ToArray();
            var revenueGrowth = GetGrowth(pair[1].TotalRevenue, pair[0].TotalRevenue);
            var operatingIncomeGrowth = GetGrowth(pair[1].OperatingIncome, pair[0].OperatingIncome);
            var netIncomeGrowth = GetGrowth(pair[1].NetIncome, pair[0].NetIncome);
            var dto = new FundamentalDto
            {
                RevenueGrowth = revenueGrowth,
                OperatingIncomeGrowth = operatingIncomeGrowth,
                NetIncomeGrowth = netIncomeGrowth
            }; 
            return dto;
        }

        private static int? GetGrowth(long? first, long? last)
        {
            if (first == null || last == null) return null;
            return (int)(100 - first.Value * 100 / last.Value);
        }
    }
}