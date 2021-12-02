using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Xunit;

using Hamster.Adapters.Interfaces.AlphaVantage;

namespace Hamster.Controllers.IntegrationTests.StockController
{
    public class Fundamental
    {
        [Fact]
        public async void All_fundamental_indicators_should_not_be_null()
        {
            // Arrange
            var controller = BuildContainer().Resolve<Controllers.StockController>();
            
            // Act
            const string ticker = "PYPL";
            var dto = await controller.Fundamental(ticker, CancellationToken.None);
            
            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(dto.RevenueGrowth);
            Assert.NotNull(dto.OperatingIncomeGrowth);
            Assert.NotNull(dto.NetIncomeGrowth);
            /*Assert.NotNull(dto.GrossMargin);
            Assert.NotNull(dto.OperatingMargin);
            Assert.NotNull(dto.NetProfitMargin);
            Assert.NotNull(dto.Roe);
            Assert.NotNull(dto.Pe);
            Assert.NotNull(dto.Ps);
            Assert.NotNull(dto.EvEbitda);*/
        }

        [Fact]
        public async void Foo()
        {
            // Arrange
            var adapter = BuildContainer().Resolve<IAlphaVantageAdapter>();
            
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
        
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterAssemblyTypes(typeof(Controllers.StockController).GetTypeInfo().Assembly)
                .AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterMediatR(typeof(UseCases.DependencyInjection).GetTypeInfo().Assembly);
            builder.RegisterAssemblyModules(typeof(UseCases.DependencyInjection).Assembly);
            builder.RegisterAssemblyModules(typeof(Adapters.Implementation.DependencyInjection).Assembly);
            var container = builder.Build();
            return container;
        }
    }
}