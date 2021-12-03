using System.Threading;
using Xunit;

using Hamster.Controllers.UnitTests.Arrange;

namespace Hamster.Controllers.UnitTests.StockController
{
    public class Fundamental
    {
        [Fact]
        public async void All_fundamental_indicators_should_not_be_null()
        {
            // Arrange
            using var container = Arranger.BuildMockContainer();
            var controller = container.Create<Controllers.StockController>();
            
            // Act
            const string ticker = "PYPL";
            var dto = await controller.Fundamental(ticker, CancellationToken.None);
            
            // Assert
            Assert.NotNull(dto);
            Assert.Equal(34, dto.RevenueGrowth);
            Assert.Equal(38, dto.OperatingIncomeGrowth);
            Assert.Equal(50, dto.NetIncomeGrowth);
            /*Assert.NotNull(dto.GrossMargin);
            Assert.NotNull(dto.OperatingMargin);
            Assert.NotNull(dto.NetProfitMargin);
            Assert.NotNull(dto.Roe);
            Assert.NotNull(dto.Pe);
            Assert.NotNull(dto.Ps);
            Assert.NotNull(dto.EvEbitda);*/
        }
    }
}