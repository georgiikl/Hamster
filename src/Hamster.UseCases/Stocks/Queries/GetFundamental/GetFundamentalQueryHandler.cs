using System;
using System.Net.Http;
using System.Net.Http.Json;
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
        public string GrossProfit { get; set; }
        public string TotalRevenue { get; set; }
        public string CostOfRevenue { get; set; }
        public string CostofGoodsAndServicesSold { get; set; }
        public string OperatingIncome { get; set; }
        public string SellingGeneralAndAdministrative { get; set; }
        public string ResearchAndDevelopment { get; set; }
        public string OperatingExpenses { get; set; }
        public string InvestmentIncomeNet { get; set; }
        public string NetInterestIncome { get; set; }
        public string InterestIncome { get; set; }
        public string InterestExpense { get; set; }
        public string NonInterestIncome { get; set; }
        public string OtherNonOperatingIncome { get; set; }
        public string Depreciation { get; set; }
        public string DepreciationAndAmortization { get; set; }
        public string IncomeBeforeTax { get; set; }
        public string IncomeTaxExpense { get; set; }
        public string InterestAndDebtExpense { get; set; }
        public string NetIncomeFromContinuingOperations { get; set; }
        public string ComprehensiveIncomeNetOfTax { get; set; }
        public string Ebit { get; set; }
        public string Ebitda { get; set; }
        public string NetIncome { get; set; }      
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
            var result = await HttpClient.GetFromJsonAsync<IncomeStatement>(requestUri, cancellationToken);
            return result;
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
            var dto = new FundamentalDto
            {
                RevenueGrowth = incomeStatement.Symbol.Length
            }; 
            return dto;
        }
    }
}