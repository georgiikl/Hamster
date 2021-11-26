using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Xunit;

using Hamster.UseCases.Stocks.Queries.GetFundamental;

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
            Assert.NotNull(dto.OperatingProfitGrowth);
            Assert.NotNull(dto.NetProfitGrowth);
            Assert.NotNull(dto.GrossMargin);
            Assert.NotNull(dto.OperatingMargin);
            Assert.NotNull(dto.NetProfitMargin);
            Assert.NotNull(dto.Roe);
            Assert.NotNull(dto.Pe);
            Assert.NotNull(dto.Ps);
            Assert.NotNull(dto.EvEbitda);
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
            void ItemInspector(IncomeStatementItem item)
            {
                Assert.Equal("USD", item.ReportedCurrency);
                var nextDayOfMonth = item.FiscalDateEnding.AddDays(1).Day;
                Assert.Equal(1, nextDayOfMonth);
            }
            var annualReportsInspectors = Enumerable.Repeat<Action<IncomeStatementItem>>(ItemInspector, 5).ToArray();
            Assert.Collection(dto.AnnualReports, annualReportsInspectors);
            var quarterReportsInspectors = Enumerable.Repeat<Action<IncomeStatementItem>>(ItemInspector, 20).ToArray();
            Assert.Collection(dto.QuarterlyReports, quarterReportsInspectors);
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
            var container = builder.Build();
            return container;
        }
    }
}