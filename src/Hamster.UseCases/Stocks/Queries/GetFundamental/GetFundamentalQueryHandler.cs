using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

using Hamster.Adapters.Interfaces.AlphaVantage;

namespace Hamster.UseCases.Stocks.Queries.GetFundamental
{
    internal class GetFundamentalQueryHandler : IRequestHandler<GetFundamentalQuery, FundamentalDto>
    {
        private readonly IAlphaVantageAdapter _alphaVantageAdapter;

        public GetFundamentalQueryHandler(IAlphaVantageAdapter alphaVantageAdapter)
        {
            _alphaVantageAdapter = alphaVantageAdapter;
        }
        public async Task<FundamentalDto> Handle(GetFundamentalQuery request, CancellationToken cancellationToken)
        {
            var incomeStatement = await _alphaVantageAdapter.GetIncomeStatementAsync(request.Ticker, cancellationToken);
            var pair = incomeStatement
                .AnnualReports
                .OrderByDescending(x => x.FiscalDateEnding)
                .Take(2)
                .ToArray();
            var revenueGrowth = GetGrowth(pair, x => x.TotalRevenue);
            var operatingIncomeGrowth = GetGrowth(pair, x => x.OperatingIncome);
            var netIncomeGrowth = GetGrowth(pair, x => x.NetIncome);
            var dto = new FundamentalDto
            {
                RevenueGrowth = revenueGrowth,
                OperatingIncomeGrowth = operatingIncomeGrowth,
                NetIncomeGrowth = netIncomeGrowth
            }; 
            return dto;
        }

        private static int? GetGrowth(IncomeStatementItemDto[] pair, Func<IncomeStatementItemDto, long?> selector)
        {
            if (pair.Length < 2) return null;
            var first = pair[1];
            var last = pair[0];
            var expectedLastDate = first.FiscalDateEnding.AddYears(1);
            if (last.FiscalDateEnding != expectedLastDate) return null;
            var firstValue = selector(first);
            var lastValue = selector(last);
            var growth = GetGrowth(firstValue, lastValue);
            return growth;
        }
        
        private static int? GetGrowth(long? first, long? last)
        {
            if (first == null || last is null or 0) return null;
            return (int)(100 - first.Value * 100 / last.Value);
        }
    }
}