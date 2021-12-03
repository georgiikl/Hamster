using System;
using System.Threading;
using Moq;

using Hamster.Adapters.Interfaces.AlphaVantage;

namespace Hamster.Controllers.UnitTests.Arrange
{
    public static class AlphaVantageAdapter
    {
        public static Mock<IAlphaVantageAdapter> GetMock()
        {
            var mock = new Mock<IAlphaVantageAdapter>();
            mock
                .Setup(x => x.GetIncomeStatementAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new IncomeStatementDto
                {
                    AnnualReports = new [] { FirstItem, SecondItem }
                });
            return mock;
        }

        private static readonly IncomeStatementItemDto FirstItem = new IncomeStatementItemDto
        {
            FiscalDateEnding = new DateTime(2019, 12, 31),
            TotalRevenue = 1000,
            OperatingIncome = 500,
            NetIncome = 200
        };
        private static readonly IncomeStatementItemDto SecondItem = new IncomeStatementItemDto
        {
            FiscalDateEnding = new DateTime(2020, 12, 31),
            TotalRevenue = 1500,
            OperatingIncome = 800,
            NetIncome = 400
        };
    }
}