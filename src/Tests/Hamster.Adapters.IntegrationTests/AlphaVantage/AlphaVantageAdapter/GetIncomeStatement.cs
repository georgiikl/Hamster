using System;
using System.Linq;
using System.Threading;
using Autofac;
using Xunit;

using Hamster.Adapters.IntegrationTests.Arrange;
using Hamster.Adapters.Interfaces.AlphaVantage;

namespace Hamster.Adapters.IntegrationTests.AlphaVantage.AlphaVantageAdapter
{
    public class GetIncomeStatement
    {
        [Fact]
        public async void Should_returns_income_statement()
        {
            // Arrange
            var adapter = Arranger.BuildContainer().Resolve<IAlphaVantageAdapter>();
            
            // Act
            const string ticker = "PYPL";
            var dto = await adapter.GetIncomeStatement(ticker, CancellationToken.None);
            
            // Assert
            Assert.NotNull(dto);
            Assert.Equal(ticker, dto.Symbol);
            Assert.NotNull(dto.AnnualReports);
            Assert.NotNull(dto.QuarterlyReports);
            void ItemInspector(IncomeStatementItemDto item)
            {
                Assert.Equal("USD", item.ReportedCurrency);
                var nextDayOfMonth = item.FiscalDateEnding.AddDays(1).Day;
                Assert.Equal(1, nextDayOfMonth);
            }
            var annualReportsInspectors = Enumerable.Repeat<Action<IncomeStatementItemDto>>(ItemInspector, 5).ToArray();
            Assert.Collection(dto.AnnualReports, annualReportsInspectors);
            var quarterReportsInspectors = Enumerable.Repeat<Action<IncomeStatementItemDto>>(ItemInspector, 20).ToArray();
            Assert.Collection(dto.QuarterlyReports, quarterReportsInspectors);
            
            Assert.Equal(16933000000, dto.AnnualReports[0].GrossProfit);
            Assert.Equal(21454000000, dto.AnnualReports[0].TotalRevenue);
            Assert.Null(dto.AnnualReports[0].InvestmentIncomeNet);
        }
    }
}